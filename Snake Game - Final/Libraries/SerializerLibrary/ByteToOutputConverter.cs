using Contracts.DataContracts;
using System;
using System.Collections.Generic;

namespace SerializerLibrary
{
 



    public class InputToByteConverter
    {
        public byte[] ConvertToBytes(Input input)
        {
            byte[] bytes = CustomInputSerializer.SerializeInput(input);
            byte[] bytesWithLength = LengthPrepender.PrependLength(bytes);

            return bytesWithLength;
        }
    }


    public class ByteToInputConverter
    {
        private List<byte> buffer = new List<byte>();
        private List<Input> deserializedInputs = new List<Input>();

        // Receive data in chunks and attempt deserialization
        public void ReceiveData(byte[] data)
        {
            buffer.AddRange(data);
            DeserializeBuffer();
        }

        // Returns all deserialized inputs and optionally clears them from the list
        public List<Input> GetDeserializedInputs(bool clearList)
        {
            List<Input> inputs = new List<Input>(deserializedInputs);
            if (clearList)
            {
                deserializedInputs.Clear();
            }
            return inputs;
        }

        // Deserialize complete packets from buffer
        private void DeserializeBuffer()
        {
            while (buffer.Count >= 4) // Assuming the length field is 4 bytes
            {
                // Ensure big-endian byte order when interpreting the length
                byte[] lengthBytes = buffer.GetRange(0, 4).ToArray();
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(lengthBytes);

                int length = BitConverter.ToInt32(lengthBytes, 0);

                if (buffer.Count >= 4 + length)
                {
                    byte[] payload = buffer.GetRange(4, length).ToArray();
                    Input input = CustomInputDeserializer.DeserializeInput(payload);
                    deserializedInputs.Add(input);
                    buffer.RemoveRange(0, 4 + length);
                }
                else
                {
                    break;
                }
            }
        }
    }


    public class OutputToByteConverter
    {
        public byte[] ConvertToBytes(Output output)
        {
            byte[] bytes = CustomOutputSerializer.SerializeOutput(output);
            byte[] bytesWithLength = LengthPrepender.PrependLength(bytes);

            return bytesWithLength;
        }
    }

    public class ByteToOutputConverter
    {
        private List<byte> buffer = new List<byte>();
        private List<Output> deserializedOutputs = new List<Output>();

        // Receive data in chunks and attempt deserialization
        public void ReceiveData(byte[] data)
        {
            buffer.AddRange(data);
            DeserializeBuffer();
        }

        // Returns all deserialized outputs and optionally clears them from the list
        public List<Output> GetDeserializedOutputs(bool clearList)
        {
            List<Output> outputs = new List<Output>(deserializedOutputs);
            if (clearList)
            {
                deserializedOutputs.Clear();
            }
            return outputs;
        }

        // Deserialize complete packets from buffer
        private void DeserializeBuffer()
        {
            while (buffer.Count >= 4) // Assuming the length field is 4 bytes
            {
                // Ensure big-endian byte order when interpreting the length
                byte[] lengthBytes = buffer.GetRange(0, 4).ToArray();
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(lengthBytes);

                int length = BitConverter.ToInt32(lengthBytes, 0);

                if (buffer.Count >= 4 + length)
                {
                    byte[] payload = buffer.GetRange(4, length).ToArray();
                    Output output = CustomOutputDeserializer.DeserializeOutput(payload);
                    deserializedOutputs.Add(output);
                    buffer.RemoveRange(0, 4 + length);
                }
                else
                {
                    break;
                }
            }
        }
    }

    public class LengthPrepender
    {
        public static byte[] PrependLength(byte[] data)
        {
            // Convert length to byte array (4 bytes for int)
            int length = data.Length;
            byte[] lengthBytes = BitConverter.GetBytes(length);

            // Ensure big-endian byte order (if needed)
            if (BitConverter.IsLittleEndian)
                Array.Reverse(lengthBytes);

            // Create new array and copy length and data
            byte[] result = new byte[lengthBytes.Length + data.Length];
            lengthBytes.CopyTo(result, 0);
            data.CopyTo(result, lengthBytes.Length);

            return result;
        }
    }
}
