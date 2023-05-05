using System;
using System.Collections.Generic;

public class ScoreKeeper
{
    private static ScoreKeeper instance;

    private const string SAVE_FILE_NAME = "snake_score.json";

    private int currentScore;

    private Dictionary<int, List<int>> allScores;

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
        ResetCurrentScore();
        LoadScores();
    }
   
    public int GetHighScore(int ticks)
    {
        //Return 0 if highscore for tick not recorded.
        if (!allScores.ContainsKey(ticks))
        {
            return 0;
        }

        int highScore = 0;

        foreach(int score in allScores[ticks])
        {
            if (score > highScore)
                highScore = score;
        }

        return highScore;
    }

    public int GetAvgScore(int ticks)
    {
        if (!allScores.ContainsKey(ticks) || allScores[ticks].Count <= 0)
        {
            return 0;
        }

        float sum = 0f;

        foreach (int score in allScores[ticks])
        {
            sum += score;
        }

        return (int)(sum / allScores[ticks].Count);
    }

    public void IncrementCurrentScore()
    {
        currentScore++;
    }

    public void ResetCurrentScore()
    {
        currentScore = 0;
    }

    public void UpdateScores(int ticks)
    {
        if (!allScores.ContainsKey(ticks))
            allScores.Add(ticks, new List<int>());

        allScores[ticks].Add(currentScore);
    }

    public void SaveScores()
    {
        SaveData saveData = new SaveData();
        saveData.scrData = new List<ScoreData>();

        foreach(int tick in allScores.Keys)
        {
            foreach(int score in allScores[tick])
            {
                saveData.scrData.Add(new ScoreData(tick, score));
            }
        }
 
        SaveLoader.SaveData(saveData, SAVE_FILE_NAME);
    }     
    

    public void LoadScores()
    {
        allScores = new Dictionary<int, List<int>>();

        SaveData saveData = SaveLoader.LoadData<SaveData>(SAVE_FILE_NAME);

        foreach(ScoreData scoreData in saveData.scrData)
        {
            if (!allScores.ContainsKey(scoreData.tick))
                allScores.Add(scoreData.tick, new List<int>());

            allScores[scoreData.tick].Add(scoreData.score);
        }
    }

    [Serializable]
    private class SaveData
    {
        public List<ScoreData> scrData;

        public SaveData()
        {
            scrData = new List<ScoreData>();
        }
    }

    [Serializable]
    private class ScoreData
    {
        public int tick;
        public int score;

        public ScoreData()
        {
            tick = 0;
            score = 0;
        }

        public ScoreData(int t, int s)
        {
            tick = t;
            score = s;
        }
    }
}
