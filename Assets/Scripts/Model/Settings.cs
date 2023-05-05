using System;

public class Settings
{
    private static Settings instance;

    private const string SAVE_FILE_NAME = "settings.json";

    private bool useBeep;
    private float ticksPerSecond;
    private float primaryHue;
    private float secondaryHue;
    private float fruitHue;

    public bool UseBeep
    {
        get
        {
            return useBeep;
        }

        set
        {
            useBeep = value;
        }
    }

    public float TicksPerSecond 
    {
        get
        {
            return ticksPerSecond;
        }

        set
        {
            ticksPerSecond = value;
        }
    }

    public float PrimaryColor
    {
        get
        {
            return primaryHue;
        }

        set
        {
            primaryHue = value;
        }
    }

    public float SecondaryColor
    {
        get
        {
            return secondaryHue;
        }

        set
        {
            secondaryHue = value;
        }
    }

    public float FruitColor
    {
        get
        {
            return fruitHue;
        }

        set
        {
            fruitHue = value;
        }
    }

    public static Settings Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Settings();
            }

            return instance;
        }
    }

    private Settings()
    {
        LoadSettings();
    }

    public void SaveSettings()
    {
        SettingsData saveData = new SettingsData();
        saveData.tps = ticksPerSecond;
        saveData.uB = useBeep;
        saveData.pHue = primaryHue;
        saveData.sHue = secondaryHue;
        saveData.fHue = fruitHue;

        SaveLoader.SaveData(saveData, SAVE_FILE_NAME);
    }

    public void LoadSettings()
    {
        SettingsData saveData = SaveLoader.LoadData<SettingsData>(SAVE_FILE_NAME);

        ticksPerSecond = saveData.tps;   
        
        useBeep = saveData.uB;

        primaryHue = saveData.pHue;
        secondaryHue = saveData.sHue;
        fruitHue = saveData.fHue;
    }

    [Serializable]
    private class SettingsData
    {
        public float tps;
        public float pHue;
        public float sHue;
        public float fHue;
        public bool uB;

        public SettingsData()
        {
            tps = 10f;
            uB = true;
            pHue = 120f / 360f;
            sHue = 60f / 360f;
            fHue = 0f;
        }
    }
}
