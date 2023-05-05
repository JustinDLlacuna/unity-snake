using System;
using System.Collections.Generic;

public class ScoreKeeper
{
    private static ScoreKeeper instance;

    private const string SAVE_FILE_NAME = "snake_score.json";

    private int currentScore;

    private Dictionary<int, int> highScores;

    public int CurrentScore => currentScore;

    public static ScoreKeeper Instance
    {
        get
        {
            if ((instance == null))
            {
                instance = new ScoreKeeper();
            }

            return instance;
        }
    }

    private ScoreKeeper()
    {
        currentScore = 0;
        LoadHighScore();
    }
   
    public int GetHighScore(int ticks)
    {
        //Return 0 if highscore for tick not recorded.
        if (!highScores.ContainsKey(ticks))
        {
            return 0;
        }

        return highScores[ticks];
    }

    public void IncrementCurrentScore()
    {
        currentScore++;
    }

    public void ResetCurrentScore()
    {
        currentScore = 0;
    }

    public void UpdateHighScore(int ticks)
    {
        //Record highscore of tick if not recorded.
        if (!highScores.ContainsKey(ticks))
        {
            highScores.Add(ticks, currentScore);
            return;
        }

        //Update highscore of tick if recorded.
        if (currentScore > highScores[ticks])
        {
            highScores[ticks] = currentScore;
        }
    }

    public void SaveHighScore()
    {
        SaveData saveData = new SaveData();
        saveData.ticks = new List<int>(highScores.Keys);
        saveData.scores = new List<int>(highScores.Values);

        SaveLoader.SaveData(saveData, SAVE_FILE_NAME);
    }

    public void LoadHighScore()
    {
        highScores = new Dictionary<int, int>();
        SaveData saveData = SaveLoader.LoadData<SaveData>(SAVE_FILE_NAME);
        
        for (int i = 0; i < saveData.ticks.Count; i++)
        {
            highScores.Add(saveData.ticks[i], saveData.scores[i]);
        }      
    }

    [Serializable]
    private class SaveData
    {
        public List<int> ticks;
        public List<int> scores;

        public SaveData()
        {
            ticks = new List<int>();
            scores = new List<int>();
        }
    }
}
