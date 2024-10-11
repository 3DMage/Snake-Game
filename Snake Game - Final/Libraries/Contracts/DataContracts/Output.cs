using Microsoft.Xna.Framework;

namespace Contracts.DataContracts
{
    
    public class Output
    {
        public List<SnakeSystemStateData> snakeSystemStatesDatas { get; set; }
        public List<AddSnakeData> addSnakeDatas { get; set; }
        public List<MoveSnakeData> moveSnakeDatas { get; set; }
        public List<FoodStateData> foodStateDatas { get; set; }
        public List<FoodSpawnData> foodSpawnDatas { get; set; }
        public List<FoodDeleteData> foodDeleteDatas { get; set; }
        public List<ConnectedSignalData> connectedSignalDatas { get; set; }
        public List<RemoveSnakeData> removeSnakeDatas { get; set; }
        public List<ReviveSnakeData> reviveSnakeDatas { get; set; }
        public List<KillSnakeData> killSnakeDatas { get; set; }
        public List<ExpandSnakeData> expandSnakeDatas { get; set; }
        public List<ChangeSnakeAngleData> changeSnakeAngleDatas { get; set; }
        public List<MakeSnakeNotInvincibleData> makeSnakeNotInvincibleDatas { get; set; }

        public Output()
        {
            snakeSystemStatesDatas = new List<SnakeSystemStateData>();
            addSnakeDatas = new List<AddSnakeData>();
            removeSnakeDatas = new List<RemoveSnakeData>();
            moveSnakeDatas = new List<MoveSnakeData>();
            reviveSnakeDatas = new List<ReviveSnakeData>();
            killSnakeDatas = new List<KillSnakeData>();
            expandSnakeDatas = new List<ExpandSnakeData>();
            changeSnakeAngleDatas = new List<ChangeSnakeAngleData>();
            makeSnakeNotInvincibleDatas = new List<MakeSnakeNotInvincibleData>();

            foodSpawnDatas = new List<FoodSpawnData>();
            foodDeleteDatas = new List<FoodDeleteData>();
            foodStateDatas = new List<FoodStateData>();

            connectedSignalDatas = new List<ConnectedSignalData>();
        }
    }

      //? SPECIFIC CLIENT
    public class FoodStateData
    {
        public int clientID;
        public List<int> foodIDs;
        public List<float> foodPositions_X;
        public List<float> foodPositions_Y;

        public FoodStateData(int clientID, List<int> foodIDs, List<float> foodPositions_X, List<float> foodPositions_Y)
        {
            this.clientID = clientID;
            this.foodIDs = foodIDs;
            this.foodPositions_X = foodPositions_X;
            this.foodPositions_Y = foodPositions_Y;
        }
    }

    
    public class FoodSpawnData
    {
        public float positionX;
        public float positionY;
        public int foodID;

        public FoodSpawnData(float positionX, float positionY, int foodID)
        {
            this.positionX = positionX;
            this.positionY = positionY;
            this.foodID = foodID;
        }
    }

    
    public class FoodDeleteData
    {
        public int foodID;

        public FoodDeleteData(int foodID)
        {
            this.foodID = foodID;
        }
    }

    
    public class AddSnakeData
    {
        public int clientID { get; private set; }
        public string playerName { get; private set; }
        public List<Vector2> segmentPositions { get; private set; }

        public AddSnakeData(int clientID, string playerName, List<Vector2> segmentPositions)
        {
            this.clientID = clientID;
            this.segmentPositions = segmentPositions;
            this.playerName = playerName;
        }
    }

    
    public class MoveSnakeData
    {
        public int clientID { get; private set; }
        public List<Vector2> segmentPositions { get; private set; }

        public MoveSnakeData(int snakeID, List<Vector2> segmentPositions)
        {
            clientID = snakeID;
            this.segmentPositions = segmentPositions;
        }
    }

     //? SPECIFIC CLIENT
    public class SnakeSystemStateData
    {
        public int sendToClientID;
        public List<int> clientIDs { get; private set; }
        public List<string> playerNames { get; private set; }
        public List<List<Vector2>> segmentPositions { get; private set; }
        public List<bool> activeStates { get; private set; }
        public List<bool> invincibleFlags { get; private set; }


        public SnakeSystemStateData(int sendToClientID, List<int> clientIDs, List<string> playerNames, List<List<Vector2>> segmentPositions, List<bool> activeStates, List<bool> invincibleFlags)
        {
            this.sendToClientID = sendToClientID;
            this.clientIDs = clientIDs;
            this.segmentPositions = segmentPositions;
            this.playerNames = playerNames;
            this.activeStates = activeStates;
            this.invincibleFlags = invincibleFlags;
        }
    }

     //? SPECIFIC CLIENT
    public class ConnectedSignalData
    {
        public int clientID { get; private set; }

        public ConnectedSignalData(int clientID)
        {
            this.clientID = clientID;
        }
    }

    
    public class RemoveSnakeData
    {
        public int clientID { get; private set; }

        public RemoveSnakeData(int clientID)
        {
            this.clientID = clientID;
        }
    }

    
    public class ReviveSnakeData
    {
        public int clientID;
        public Vector2 position;

        public ReviveSnakeData(int clientID, Vector2 position)
        {
            this.clientID = clientID;
            this.position = position;
        }
    }

    
    public class KillSnakeData
    {
        public int clientID { get; private set; }

        public KillSnakeData(int clientID)
        {
            this.clientID = clientID;
        }
    }

    public class MakeSnakeNotInvincibleData
    {
        public int clientID { get; private set; }

        public MakeSnakeNotInvincibleData(int clientID)
        {
            this.clientID = clientID;
        }
    }

    public class ExpandSnakeData
    {
        public int clientID { get; private set; }
        public int numberOfSegments { get; private set; }

        public ExpandSnakeData(int clientID, int numberOfSegments)
        {
            this.clientID = clientID;
            this.numberOfSegments = numberOfSegments;
        }
    }


    public class ChangeSnakeAngleData
    {
        public int clientID;
        public float radianAngle;

        public ChangeSnakeAngleData(int clientID, float radianAngle)
        {
            this.clientID = clientID;
            this.radianAngle = radianAngle;
        }
    }
}