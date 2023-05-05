using System;
using UnityEngine;

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

    private Color primaryColor;
    private Color secondaryColor;
    private Color fruitColor;
    private Color gridColor;
    private Color backgroundColor;

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
            primaryColor = Color.HSVToRGB(primaryHue.h, primaryHue.s, primaryHue.v);
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
            secondaryColor = Color.HSVToRGB(secondaryHue.h, secondaryHue.s, secondaryHue.v);
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
            fruitColor = Color.HSVToRGB(fruitHue.h, fruitHue.s, fruitHue.v);
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
            gridColor = Color.HSVToRGB(gridHue.h, gridHue.s, gridHue.v);
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
            backgroundColor = Color.HSVToRGB(backgroundHue.h, backgroundHue.s, backgroundHue.v);
        }
    }

    public Color PrimaryColor
    {
        get
        {
            return primaryColor;
        }
    }

    public Color SecondaryColor
    {
        get
        {
            return secondaryColor;
        }
    }

    public Color FruitColor
    {
        get
        {
            return fruitColor;
        }
    }

    public Color GridColor
    {
        get
        {
            return gridColor;
        }
    }

    public Color BackgroundColor
    {
        get
        {
            return backgroundColor;
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
        saveData.uM = useMusic;

        saveData.pHue = primaryHue;
        saveData.sHue = secondaryHue;
        saveData.fHue = fruitHue;
        saveData.gHue = gridHue;
        saveData.bHue = backgroundHue;

        SaveLoader.SaveData(saveData, SAVE_FILE_NAME);
    }

    public void LoadSettings()
    {
        SettingsData saveData = SaveLoader.LoadData<SettingsData>(SAVE_FILE_NAME);

        ticksPerSecond = saveData.tps;   
        
        useBeep = saveData.uB;
        useMusic = saveData.uM;

        PrimaryHue = saveData.pHue;
        SecondaryHue = saveData.sHue;
        FruitHue = saveData.fHue;
        GridHue = saveData.gHue;
        BackgroundHue = saveData.bHue;
    }

    [Serializable]
    private class SettingsData
    {
        public float tps;
       
        public bool uB;
        public bool uM;

        public Hue pHue;
        public Hue sHue;
        public Hue fHue;
        public Hue gHue;
        public Hue bHue;

        public SettingsData()
        {
            tps = 10f;
            uB = true;
            uM = true;
            pHue = new Hue(120f/360f, 1f, 1f);
            sHue = new Hue(60f/360f, 1f, 1f);
            fHue = new Hue(0f, 1f, 1f);
            gHue = new Hue(0f, 0f, 60f/100f);
            bHue = new Hue(0f, 0f, 0f);
        }
    }
}
