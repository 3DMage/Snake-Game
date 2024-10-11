namespace SerializerLibrary.SerializerComponents
{
    public enum DataPacketType : ushort
    {
        // Server to client
        ConnectedSignal,
        AddSnake,
        RemoveSnake,
        KillSnake,
        MoveSnake,
        ExpandSnake,
        ReviveSnake,
        SnakeSystemState,
        PlayerSnakeID,
        FoodSpawn,
        FoodDelete,
        FoodSystemState,



        // Client to server
        Join,
        Disconnect,
        DirectionChangeRequest
    }
}
