using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GameComponents
{
    // The list of high scores achieved in the game.
    [DataContract(Name = "HighScoreList")]
    public class HighScoresList
    {
        // Stores the last scored achieved from previous round.
        [DataMember()]
        public int latestScore;

        // Stores the index of latest achieved high score.  Utilized by UI for typing player name.
        public int latestHighScoreInputIndex = -1;

        // The list of high scores.
        [DataMember()]
        public List<ScoreEntry> scores;

        // Constructor.
        public HighScoresList()
        {
            scores = new List<ScoreEntry>();

            for (int i = 0; i < 10; i++)
            {
                scores.Add(new ScoreEntry(0, ""));
            }
        }

        // Adds a new score to the high score list.  Updates positions and removes lowest entries below 10th place.  Returns true if score successfully added, else false if not.
        public bool AddScore(ScoreEntry scoreEntry)
        {
            // Check if the list is empty or the score is less than the last element and the list is full.
            if (scores.Count == 0 || scores.Count == 10 && scoreEntry.score <= scores[scores.Count - 1].score)
            {
                if (scores.Count == 10)
                {
                    // The list is full and the new score is lower than the lowest score in the list, do nothing.
                    return false;
                }
                else
                {
                    // The list is not full, just add the score at the end.
                    scores.Add(scoreEntry);
                    return true;
                }
            }

            // Find the correct position to insert the new score.  Index 0 is highest score.
            for (int i = 0; i < scores.Count; i++)
            {
                if (scoreEntry.score > scores[i].score)
                {
                    // Higher score found at current position.  Insert score at position and bump everything below down.
                    scores.Insert(i, scoreEntry);
                    latestHighScoreInputIndex = i;
                    break;
                }
            }

            // Ensure only 10 entries exist.
            if (scores.Count > 10)
            {
                scores.RemoveRange(10, scores.Count - 10);
            }

            return true;
        }

        // Initializes high score list with default data.
        public void InitializeDefaultData()
        {
            scores[0] = new ScoreEntry(0, "");
            scores[1] = new ScoreEntry(0, "");
            scores[2] = new ScoreEntry(0, "");
            scores[3] = new ScoreEntry(0, "");
            scores[4] = new ScoreEntry(0, "");
            scores[5] = new ScoreEntry(0, "");
            scores[6] = new ScoreEntry(0, "");
            scores[7] = new ScoreEntry(0, "");
            scores[8] = new ScoreEntry(0, "");
            scores[9] = new ScoreEntry(0, "");
        }
    }
}

