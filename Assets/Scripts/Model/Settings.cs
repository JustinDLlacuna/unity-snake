using System;

public class Settings
{
    private static Settings instance;

    private const string SAVE_FILE_NAME = "settings.json";

    private bool useBeep;
    private bool useMusic;

    private float ticksPerSecond;

    private Hue primaryHue;
    private Hue secondaryHue;
    private Hue fruitHue;
    private Hue gridHue;
    private Hue backgroundHue;

    #region Properties
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

    public bool UseMusic
    {
        get
        {
            return useMusic;
        }

        set
        {
            useMusic = value;
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

    public Hue PrimaryHue
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
   
    public Hue SecondaryHue
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

    public Hue FruitHue
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

    public Hue GridHue
    {
        get
        {
            return gridHue;
        }

        set
        {
            gridHue = value;
        }
    }

    public Hue BackgroundHue
    {
        get
        {
            return backgroundHue;
        }

        set
        {
            backgroundHue = value;
        }
    }
    #endregion

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
        SaveLoader.SaveData(new SettingsData(useBeep, useMusic, ticksPerSecond, 
            primaryHue, secondaryHue, fruitHue, gridHue, backgroundHue), SAVE_FILE_NAME);
    }

    public void LoadSettings()
    {
        SettingsData saveData = SaveLoader.LoadData<SettingsData>(SAVE_FILE_NAME);

        ticksPerSecond = saveData.tps;   
        
        useBeep = saveData.uB;
        useMusic = saveData.uM;

        PrimaryHue = new Hue(saveData.pHue);
        SecondaryHue = new Hue(saveData.sHue);
        FruitHue = new Hue(saveData.fHue);
        GridHue = new Hue(saveData.gHue);
        BackgroundHue = new Hue(saveData.bHue);
    }

    [Serializable]
    private class SettingsData
    {      
        public bool uB;
        public bool uM;

        public float tps;

        public Hue pHue;
        public Hue sHue;
        public Hue fHue;
        public Hue gHue;
        public Hue bHue;

        public SettingsData()
        {            
            uB = true;
            uM = true;

            tps = 10f;

            pHue = new Hue(120f/360f, 1f, 1f);
            sHue = new Hue(60f/360f, 1f, 1f);
            fHue = new Hue(0f, 1f, 1f);
            gHue = new Hue(0f, 0f, 0f);
            bHue = new Hue(0f, 0f, 60f / 100f);
        }

        public SettingsData(bool uB, bool uM, float tps, Hue pHue, Hue sHue, Hue fHue, Hue gHue, Hue bHue)
        {
            this.uB = uB;
            this.uM = uM;

            this.tps = tps;

            this.pHue = pHue;
            this.sHue = sHue;
            this.fHue = fHue;
            this.gHue = gHue;
            this.bHue = bHue;
        }
    }
}
