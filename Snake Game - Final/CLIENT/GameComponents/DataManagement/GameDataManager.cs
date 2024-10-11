namespace GameComponents
{
    // This class manages the score and session data for the game.  Also utilizes IO to save and load score data.
    public static class GameDataManager
    {
        // Stores the data for the current round of gameplay.
        //public static LunarLnader_SessionData sessionData { get; private set; } = new LunarLnader_SessionData();

        // Stores a list of the high scores.
        public static HighScoresList highScoresList { get; set; } = null;

        // Manages persistent storage of high scores and other relevant data.
        public static ScoreData_IO scoreData_IO { get; private set; }

        public static string playerName = "";
        public static int clientID = -1;
        public static int playerScore = 0;



        public static void Initialize()
        {

        }

        // This attempts to load score data from a file.  If it can't, it initializes a new instance of the score data.
        public static void TryLoadingScoresList()
        {
            scoreData_IO = new ScoreData_IO();

            // Attempt loading the scores list.
            scoreData_IO.LoadScoresList();

            // If nothing loaded, initialize new data instance.
            if (highScoresList == null)
            {
                highScoresList = new HighScoresList();
                highScoresList.InitializeDefaultData();
                scoreData_IO.SaveScoresList();
            }
        }
    }
}
