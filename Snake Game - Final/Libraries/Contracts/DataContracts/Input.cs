namespace Contracts.DataContracts
{
    
    public class Input
    {
        public TimeSpan elapsedTime { get; set; }

        public List<ClientJoinRequestData> clientJoinRequestDatas { get; set; }
        public List<DirectionChangeRequestData> directionChangeRequestDatas { get; set; }
        public List<ReviveSnakeRequestData> reviveSnakeRequestDatas { get; set; }


        public Input()
        {
            this.elapsedTime = new TimeSpan();
            clientJoinRequestDatas = new List<ClientJoinRequestData>();
            directionChangeRequestDatas = new List<DirectionChangeRequestData>();
            reviveSnakeRequestDatas = new List<ReviveSnakeRequestData>();
        }

        public Input(TimeSpan elapsedTime)
        {
            this.elapsedTime = elapsedTime;
            clientJoinRequestDatas = new List<ClientJoinRequestData>();
            directionChangeRequestDatas = new List<DirectionChangeRequestData>();
            reviveSnakeRequestDatas = new List<ReviveSnakeRequestData>();
        }
    }

    
    public class ClientJoinRequestData
    {
        public int clientID { get; set; }
        public string playerName { get; set; }

        public ClientJoinRequestData(int clientID, string playerName)
        {
            this.clientID = clientID;
            this.playerName = playerName;
        }
    }

    
    public class DirectionChangeRequestData
    {
        public int clientID { get; set; }
        public float radianAngle { get; set; }

        public DirectionChangeRequestData(int clientID, float radianAngle)
        {
            this.clientID = clientID;
            this.radianAngle = radianAngle;
        }
    }

    public class ReviveSnakeRequestData
    {
        public int clientID { get; set; }

        public ReviveSnakeRequestData(int clientID)
        {
            this.clientID = clientID;
        }
    }
}
