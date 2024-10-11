using Contracts.DataContracts;
using Microsoft.Xna.Framework;

public static class CustomOutputSerializer
{
    public static byte[] SerializeOutput(Output output)
    {
        using (var ms = new MemoryStream())
        {
            WriteList(ms, output.addSnakeDatas, WriteAddSnakeData);
            WriteList(ms, output.moveSnakeDatas, WriteMoveSnakeData);
            WriteList(ms, output.foodStateDatas, WriteFoodStateData);
            WriteList(ms, output.foodSpawnDatas, WriteFoodSpawnData);
            WriteList(ms, output.foodDeleteDatas, WriteFoodDeleteData);
            WriteList(ms, output.connectedSignalDatas, WriteConnectedSignalData);
            WriteList(ms, output.removeSnakeDatas, WriteRemoveSnakeData);
            WriteList(ms, output.reviveSnakeDatas, WriteReviveSnakeData);
            WriteList(ms, output.killSnakeDatas, WriteKillSnakeData);
            WriteList(ms, output.changeSnakeAngleDatas, WriteChangeSnakeAngleData);
            WriteList(ms, output.snakeSystemStatesDatas, WriteSnakeSystemStateData);
            WriteList(ms, output.expandSnakeDatas, WriteExpandSnakeData);
            WriteList(ms, output.makeSnakeNotInvincibleDatas, WriteMakeSnakeNotInvincibleData);

            return ms.ToArray();
        }
    }

    // Example methods for writing data of different classes
    private static void WriteAddSnakeData(MemoryStream ms, AddSnakeData data)
    {
        WriteInt(ms, data.clientID);
        WriteString(ms, data.playerName);
        WriteList(ms, data.segmentPositions, WriteVector2);
    }

    private static void WriteMoveSnakeData(MemoryStream ms, MoveSnakeData data)
    {
        WriteInt(ms, data.clientID);
        WriteList(ms, data.segmentPositions, WriteVector2);
    }

    private static void WriteFoodStateData(MemoryStream ms, FoodStateData data)
    {
        WriteInt(ms, data.clientID);
        WriteList(ms, data.foodIDs, WriteInt);
        WriteList(ms, data.foodPositions_X, WriteFloat);
        WriteList(ms, data.foodPositions_Y, WriteFloat);
    }

    private static void WriteFoodSpawnData(MemoryStream ms, FoodSpawnData data)
    {
        WriteFloat(ms, data.positionX);
        WriteFloat(ms, data.positionY);
        WriteInt(ms, data.foodID);
    }

    private static void WriteFoodDeleteData(MemoryStream ms, FoodDeleteData data)
    {
        WriteInt(ms, data.foodID);
    }

    private static void WriteConnectedSignalData(MemoryStream ms, ConnectedSignalData data)
    {
        WriteInt(ms, data.clientID);
    }

    private static void WriteRemoveSnakeData(MemoryStream ms, RemoveSnakeData data)
    {
        WriteInt(ms, data.clientID);
    }

    private static void WriteReviveSnakeData(MemoryStream ms, ReviveSnakeData data)
    {
        WriteInt(ms, data.clientID);
        WriteVector2(ms, data.position);
    }

    private static void WriteKillSnakeData(MemoryStream ms, KillSnakeData data)
    {
        WriteInt(ms, data.clientID);
    }

    private static void WriteMakeSnakeNotInvincibleData(MemoryStream ms, MakeSnakeNotInvincibleData data)
    {
        WriteInt(ms, data.clientID);
    }

    private static void WriteChangeSnakeAngleData(MemoryStream ms, ChangeSnakeAngleData data)
    {
        WriteInt(ms, data.clientID);
        WriteFloat(ms, data.radianAngle);
    }

    private static void WriteExpandSnakeData(MemoryStream ms, ExpandSnakeData data)
    {
        WriteInt(ms, data.clientID);
        WriteInt(ms, data.numberOfSegments);
    }

    // Methods to write basic types and collections
    private static void WriteVector2(MemoryStream ms, Vector2 vector)
    {
        WriteFloat(ms, vector.X);
        WriteFloat(ms, vector.Y);
    }

    private static void WriteSnakeSystemStateData(MemoryStream ms, SnakeSystemStateData data)
    {
        WriteInt(ms, data.sendToClientID);
        WriteList(ms, data.clientIDs, WriteInt);
        WriteList(ms, data.playerNames, WriteString);
        WriteList(ms, data.segmentPositions, (stream, list) => WriteList(stream, list, WriteVector2));
        WriteList(ms, data.activeStates, WriteBool);
        WriteList(ms, data.invincibleFlags, WriteBool); // New line to handle List<bool> for invincibility
    }


    private static void WriteInt(MemoryStream ms, int value)
    {
        ms.Write(BitConverter.GetBytes(value), 0, 4);
    }

    private static void WriteFloat(MemoryStream ms, float value)
    {
        ms.Write(BitConverter.GetBytes(value), 0, 4);
    }

    private static void WriteString(MemoryStream ms, string value)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(value);
        WriteInt(ms, bytes.Length); // Length prefix
        ms.Write(bytes, 0, bytes.Length);
    }

    private static void WriteBool(MemoryStream ms, bool value)
    {
        ms.Write(BitConverter.GetBytes(value), 0, 1);  // Write the boolean as one byte
    }

    private static void WriteList<T>(MemoryStream ms, List<T> list, Action<MemoryStream, T> writeMethod)
    {
        WriteInt(ms, list.Count);
        foreach (var item in list)
        {
            writeMethod(ms, item);
        }
    }
}

public static class CustomOutputDeserializer
{
    public static Output DeserializeOutput(byte[] data)
    {
        using (var ms = new MemoryStream(data))
        {
            var output = new Output
            {
                addSnakeDatas = ReadList(ms, ReadAddSnakeData),
                moveSnakeDatas = ReadList(ms, ReadMoveSnakeData),
                foodStateDatas = ReadList(ms, ReadFoodStateData),
                foodSpawnDatas = ReadList(ms, ReadFoodSpawnData),
                foodDeleteDatas = ReadList(ms, ReadFoodDeleteData),
                connectedSignalDatas = ReadList(ms, ReadConnectedSignalData),
                removeSnakeDatas = ReadList(ms, ReadRemoveSnakeData),
                reviveSnakeDatas = ReadList(ms, ReadReviveSnakeData),
                killSnakeDatas = ReadList(ms, ReadKillSnakeData),
                changeSnakeAngleDatas = ReadList(ms, ReadChangeSnakeAngleData),
                snakeSystemStatesDatas = ReadList(ms, ReadSnakeSystemStateData),
                expandSnakeDatas = ReadList(ms, ReadExpandSnakeData),
                makeSnakeNotInvincibleDatas = ReadList(ms, ReadMakeSnakeNotInvincibleData)
            };

            return output;
        }
    }

    private static AddSnakeData ReadAddSnakeData(MemoryStream ms)
    {
        var clientID = ReadInt(ms);
        var playerName = ReadString(ms);
        var segmentPositions = ReadList(ms, ReadVector2);
        return new AddSnakeData(clientID, playerName, segmentPositions);
    }

    private static MoveSnakeData ReadMoveSnakeData(MemoryStream ms)
    {
        var snakeID = ReadInt(ms);
        var segmentPositions = ReadList(ms, ReadVector2);
        return new MoveSnakeData(snakeID, segmentPositions);
    }

    private static FoodStateData ReadFoodStateData(MemoryStream ms)
    {
        var clientID = ReadInt(ms);
        var foodIDs = ReadList(ms, ReadInt);
        var foodPositions_X = ReadList(ms, ReadFloat);
        var foodPositions_Y = ReadList(ms, ReadFloat);
        return new FoodStateData(clientID, foodIDs, foodPositions_X, foodPositions_Y);
    }

    private static FoodSpawnData ReadFoodSpawnData(MemoryStream ms)
    {
        var positionX = ReadFloat(ms);
        var positionY = ReadFloat(ms);
        var foodID = ReadInt(ms);
        return new FoodSpawnData(positionX, positionY, foodID);
    }

    private static FoodDeleteData ReadFoodDeleteData(MemoryStream ms)
    {
        var foodID = ReadInt(ms);
        return new FoodDeleteData(foodID);
    }

    private static ConnectedSignalData ReadConnectedSignalData(MemoryStream ms)
    {
        var clientID = ReadInt(ms);
        return new ConnectedSignalData(clientID);
    }

    private static RemoveSnakeData ReadRemoveSnakeData(MemoryStream ms)
    {
        var clientID = ReadInt(ms);
        return new RemoveSnakeData(clientID);
    }

    private static ReviveSnakeData ReadReviveSnakeData(MemoryStream ms)
    {
        var clientID = ReadInt(ms);
        var position = ReadVector2(ms);
        return new ReviveSnakeData(clientID, position);
    }

    private static KillSnakeData ReadKillSnakeData(MemoryStream ms)
    {
        var clientID = ReadInt(ms);
        return new KillSnakeData(clientID);
    }

    private static MakeSnakeNotInvincibleData ReadMakeSnakeNotInvincibleData(MemoryStream ms)
    {
        var clientID = ReadInt(ms);
        return new MakeSnakeNotInvincibleData(clientID);
    }

    private static ChangeSnakeAngleData ReadChangeSnakeAngleData(MemoryStream ms)
    {
        var clientID = ReadInt(ms);
        var radianAngle = ReadFloat(ms);
        return new ChangeSnakeAngleData(clientID, radianAngle);
    }

    private static SnakeSystemStateData ReadSnakeSystemStateData(MemoryStream ms)
    {
        var sendToClientID = ReadInt(ms);
        var clientIDs = ReadList(ms, ReadInt);
        var playerNames = ReadList(ms, ReadString);
        var segmentPositions = ReadList(ms, subStream => ReadList(subStream, ReadVector2));
        var activeStates = ReadList(ms, ReadBool);
        var invincibleFlags = ReadList(ms, ReadBool); // New line to read List<bool> for invincibility

        return new SnakeSystemStateData(sendToClientID, clientIDs, playerNames, segmentPositions, activeStates, invincibleFlags);
    }

    private static ExpandSnakeData ReadExpandSnakeData(MemoryStream ms)
    {
        var clientID = ReadInt(ms);
        var numberOfSegments = ReadInt(ms);
        return new ExpandSnakeData(clientID, numberOfSegments);
    }


    // Helper methods to read basic types and collections from a stream
    private static Vector2 ReadVector2(MemoryStream ms)
    {
        var x = ReadFloat(ms);
        var y = ReadFloat(ms);
        return new Vector2(x, y);
    }

    private static bool ReadBool(MemoryStream ms)
    {
        var buffer = new byte[1];
        ms.Read(buffer, 0, 1);
        return BitConverter.ToBoolean(buffer, 0);  // Convert the byte back to a bool
    }

    private static int ReadInt(MemoryStream ms)
    {
        var buffer = new byte[4];
        ms.Read(buffer, 0, 4);
        return BitConverter.ToInt32(buffer, 0);
    }

    private static float ReadFloat(MemoryStream ms)
    {
        var buffer = new byte[4];
        ms.Read(buffer, 0, 4);
        return BitConverter.ToSingle(buffer, 0);
    }

    private static string ReadString(MemoryStream ms)
    {
        var length = ReadInt(ms);
        var buffer = new byte[length];
        ms.Read(buffer, 0, length);
        return System.Text.Encoding.UTF8.GetString(buffer);
    }

    private static List<T> ReadList<T>(MemoryStream ms, Func<MemoryStream, T> readMethod)
    {
        var count = ReadInt(ms);
        var list = new List<T>(count);
        for (int i = 0; i < count; i++)
        {
            list.Add(readMethod(ms));
        }
        return list;
    }
}

public static class CustomInputSerializer
{
    public static byte[] SerializeInput(Input input)
    {
        using (var ms = new MemoryStream())
        {
            // Serialize TimeSpan
            WriteTimeSpan(ms, input.elapsedTime);

            // Serialize lists
            WriteList(ms, input.clientJoinRequestDatas, WriteClientJoinRequestData);
            WriteList(ms, input.directionChangeRequestDatas, WriteDirectionChangeRequestData);
            WriteList(ms, input.reviveSnakeRequestDatas, WriteReviveSnakeRequestData);

            return ms.ToArray();
        }
    }

    private static void WriteClientJoinRequestData(MemoryStream ms, ClientJoinRequestData data)
    {
        WriteInt(ms, data.clientID);
        WriteString(ms, data.playerName);
    }

    private static void WriteDirectionChangeRequestData(MemoryStream ms, DirectionChangeRequestData data)
    {
        WriteInt(ms, data.clientID);
        WriteFloat(ms, data.radianAngle);
    }

    private static void WriteTimeSpan(MemoryStream ms, TimeSpan timeSpan)
    {
        WriteLong(ms, timeSpan.Ticks);
    }

    private static void WriteLong(MemoryStream ms, long value)
    {
        ms.Write(BitConverter.GetBytes(value), 0, 8);
    }

    // Methods to write basic types and collections
    private static void WriteReviveSnakeRequestData(MemoryStream ms, ReviveSnakeRequestData data)
    {
        WriteInt(ms, data.clientID);
    }

    private static void WriteInt(MemoryStream ms, int value)
    {
        ms.Write(BitConverter.GetBytes(value), 0, 4);
    }

    private static void WriteFloat(MemoryStream ms, float value)
    {
        ms.Write(BitConverter.GetBytes(value), 0, 4);
    }

    private static void WriteString(MemoryStream ms, string value)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(value);
        WriteInt(ms, bytes.Length); // Length prefix
        ms.Write(bytes, 0, bytes.Length);
    }

    private static void WriteList<T>(MemoryStream ms, List<T> list, Action<MemoryStream, T> writeMethod)
    {
        WriteInt(ms, list.Count);
        foreach (var item in list)
        {
            writeMethod(ms, item);
        }
    }
}

public static class CustomInputDeserializer
{
    public static Input DeserializeInput(byte[] data)
    {
        using (var ms = new MemoryStream(data))
        {
            var elapsedTime = ReadTimeSpan(ms);

            var input = new Input(elapsedTime)
            {
                clientJoinRequestDatas = ReadList(ms, ReadClientJoinRequestData),
                directionChangeRequestDatas = ReadList(ms, ReadDirectionChangeRequestData),
                reviveSnakeRequestDatas = ReadList(ms, ReadReviveSnakeRequestData),
            };

            return input;
        }
    }

    private static ClientJoinRequestData ReadClientJoinRequestData(MemoryStream ms)
    {
        var clientID = ReadInt(ms);
        var playerName = ReadString(ms);
        return new ClientJoinRequestData(clientID, playerName);
    }

    private static DirectionChangeRequestData ReadDirectionChangeRequestData(MemoryStream ms)
    {
        var clientID = ReadInt(ms);
        var radianAngle = ReadFloat(ms);
        return new DirectionChangeRequestData(clientID, radianAngle);
    }

    private static TimeSpan ReadTimeSpan(MemoryStream ms)
    {
        var ticks = ReadLong(ms);
        return new TimeSpan(ticks);
    }

    private static long ReadLong(MemoryStream ms)
    {
        var buffer = new byte[8];
        ms.Read(buffer, 0, 8);
        return BitConverter.ToInt64(buffer, 0);
    }

    private static ReviveSnakeRequestData ReadReviveSnakeRequestData(MemoryStream ms)
    {
        int clientID = ReadInt(ms);
        return new ReviveSnakeRequestData(clientID);
    }

    private static int ReadInt(MemoryStream ms)
    {
        var buffer = new byte[4];
        ms.Read(buffer, 0, 4);
        return BitConverter.ToInt32(buffer, 0);
    }

    private static float ReadFloat(MemoryStream ms)
    {
        var buffer = new byte[4];
        ms.Read(buffer, 0, 4);
        return BitConverter.ToSingle(buffer, 0);
    }

    private static string ReadString(MemoryStream ms)
    {
        var length = ReadInt(ms);
        var buffer = new byte[length];
        ms.Read(buffer, 0, length);
        return System.Text.Encoding.UTF8.GetString(buffer);
    }

    private static List<T> ReadList<T>(MemoryStream ms, Func<MemoryStream, T> readMethod)
    {
        var count = ReadInt(ms);
        var list = new List<T>(count);
        for (int i = 0; i < count; i++)
        {
            list.Add(readMethod(ms));
        }
        return list;
    }
}

public static class Prepender
{
    public static byte[] PrependByteCount(byte[] message)
    {
        // Get the length of the original message
        int messageLength = message.Length;

        // Convert the length to a byte array (4 bytes)
        byte[] lengthBytes = BitConverter.GetBytes(messageLength);

        // Ensure little-endian byte order if necessary
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(lengthBytes);
        }

        // Create a new array to hold the length and the message
        byte[] result = new byte[lengthBytes.Length + message.Length];

        // Copy the length and the message into the new array
        Array.Copy(lengthBytes, 0, result, 0, lengthBytes.Length);
        Array.Copy(message, 0, result, lengthBytes.Length, message.Length);

        return result;
    }
}

public static class OutputChecker
{
    public static bool AreAllListsEmptyOrNull(Output output)
    {
        if (output == null)
            return true; // Assuming null to be treated as empty.

        // Check each list in the output object whether it is null or empty.
        return (output.snakeSystemStatesDatas == null || !output.snakeSystemStatesDatas.Any()) &&
               (output.addSnakeDatas == null || !output.addSnakeDatas.Any()) &&
               (output.moveSnakeDatas == null || !output.moveSnakeDatas.Any()) &&
               (output.foodStateDatas == null || !output.foodStateDatas.Any()) &&
               (output.foodSpawnDatas == null || !output.foodSpawnDatas.Any()) &&
               (output.foodDeleteDatas == null || !output.foodDeleteDatas.Any()) &&
               (output.connectedSignalDatas == null || !output.connectedSignalDatas.Any()) &&
               (output.removeSnakeDatas == null || !output.removeSnakeDatas.Any()) &&
               (output.reviveSnakeDatas == null || !output.reviveSnakeDatas.Any()) &&
               (output.killSnakeDatas == null || !output.killSnakeDatas.Any()) &&
               (output.snakeSystemStatesDatas == null || !output.snakeSystemStatesDatas.Any()) &&
               (output.expandSnakeDatas == null || !output.expandSnakeDatas.Any()) &&
               (output.changeSnakeAngleDatas == null || !output.changeSnakeAngleDatas.Any());
    }
}
