using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private const float COLOR_INC = .01f;
    private const int TICK_INC = 1;
    private const string DECIMAL_PLACES = "F2";

    private enum HueType
    {
        P,
        S,
        F,
        G,
        B
    };

    private HueType currentHueChange = HueType.P;

    private static UIManager instance;

    private ScoreKeeper scoreKeeper;
    private Settings settings;

    private delegate void Method();

    [SerializeField] private new Camera camera;
    [SerializeField] private TMP_Dropdown dropdown;

    [SerializeField] private Image dropdownImage;

    [Header("Text")]
    [SerializeField] private Text currentScoreText;
    [SerializeField] private Text pauseButtonText;
    [SerializeField] private Text highScoreText;
    [SerializeField] private Text ticksPerSecondText;
    [SerializeField] private Text hValueText;
    [SerializeField] private Text sValueText;
    [SerializeField] private Text vValueText;
    [Space(10)]

    [SerializeField] private TextMeshProUGUI dropdownTextGUI;

    [Header("Buttons")]
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button stopButton;
    [Space(10)]


    [SerializeField] private Slider ticksPerSecondSlider;

    [Header("Sliders")]
    [SerializeField] private Slider HSlider;
    [SerializeField] private Slider SSlider;
    [SerializeField] private Slider VSlider;
    [Space(10)]

    [SerializeField] private Toggle beepToggle;
    [SerializeField] private Toggle musicToggle;

    [Header("Panels")]
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject gridPanel;

    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();

                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(UIManager).Name;
                    instance = obj.AddComponent<UIManager>();
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        scoreKeeper = ScoreKeeper.Instance;
        settings = Settings.Instance;

        camera.backgroundColor = settings.BackgroundColor;
    }

    public bool GetBeepToggleValue()
    {
        return beepToggle.isOn;
    }

    public void ShowResumeButton()
    {
        pauseButtonText.text = "RESUME";
    }

    public void ShowPauseButton()
    {
        pauseButtonText.text = "PAUSE";
    }

    private void UpdateCurrentScoreText()
    {
        currentScoreText.text = $"SCORE: {scoreKeeper.CurrentScore}";
    }

    public void ToggleBeep()
    {
        settings.UseBeep = beepToggle.isOn;
    }

    public void ToggleMusic()
    {
        settings.UseMusic = musicToggle.isOn;
    }


    #region Show Panels
    //Deactivates game panel's current score, pause button, and stop button.
    //Shows start panel's high score, ticks per second, toggle beep, and start button.
    public void ShowStartPanel()
    {
        highScoreText.text = $"HIGH SCORE: {scoreKeeper.GetHighScore((int)settings.TicksPerSecond)}";

        //Loading ticks per second value from settings data.
        ticksPerSecondText.text = $"TICKS PER SECOND: {settings.TicksPerSecond}";
        ticksPerSecondSlider.value = settings.TicksPerSecond;

        //Loading color values to sliders from settings data.
        ChangeColor();
        
        
        beepToggle.isOn = settings.UseBeep;
        musicToggle.isOn = settings.UseMusic;
        startPanel.SetActive(true);
        gridPanel.SetActive(true);
        currentScoreText.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(false);
        stopButton.gameObject.SetActive(false);
    }

    //Shows game panel's current score, pause button, and stop button.
    //Deactivates start panel's high score, ticks per second, toggle beep, and start button.
    public void ShowGamePanel()
    {
        UpdateCurrentScoreText();
        startPanel.SetActive(false);
        gridPanel.SetActive(true);
        currentScoreText.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(true);
        stopButton.gameObject.SetActive(true);
    }
    #endregion

    #region Updates
    //Ticks per second is updated via slider.
    public void UpdateTicksPerSecond()
    {
        float value = ticksPerSecondSlider.value;
        settings.TicksPerSecond = value;
        ticksPerSecondText.text = $"TICKS PER SECOND: {value}";
        highScoreText.text = $"HIGH SCORE: {scoreKeeper.GetHighScore((int)value)}";
    }

    public void ChangeColor()
    {
        Hue newHue = settings.PrimaryHue;
        Color newColor = Color.HSVToRGB(newHue.h, newHue.s, newHue.v);
        currentHueChange = (HueType)dropdown.value;
       
        switch (currentHueChange)
        {
            case HueType.S:
                newHue = settings.SecondaryHue;
                break;

            case HueType.F:
                newHue = settings.FruitHue;
                break;

            case HueType.G:
                newHue = settings.GridHue;
                break;

            case HueType.B:
                newHue = settings.BackgroundHue;
                break;
        }

        dropdownImage.color = newColor;

        HSlider.value = newHue.h;
        SSlider.value = newHue.s;
        VSlider.value = newHue.v;

        hValueText.text = HSlider.value.ToString(DECIMAL_PLACES);
        sValueText.text = SSlider.value.ToString(DECIMAL_PLACES);
        vValueText.text = VSlider.value.ToString(DECIMAL_PLACES);

        dropdownTextGUI.color = new Color(1f - newColor.r, 1f - newColor.g, 1f - newColor.b);

    }

    public void UpdateHueFromSliders()
    {
        float h = HSlider.value;
        float s = SSlider.value;
        float v = VSlider.value;

        Color newColor = Color.HSVToRGB(h, s, v);

        switch (currentHueChange)
        {
            case HueType.P:
                settings.PrimaryHue = new Hue(h, s, v);
                break;

            case HueType.S:
                settings.SecondaryHue = new Hue(h, s, v); 
                break;

            case HueType.F:
                settings.FruitHue = new Hue(h, s, v);
                break;

            case HueType.G:
                settings.GridHue = new Hue(h, s, v);
                break;

            case HueType.B:
                settings.BackgroundHue = new Hue(h, s, v);
                camera.backgroundColor = newColor;
                break;
        }

        dropdownImage.color = newColor;

        hValueText.text = HSlider.value.ToString(DECIMAL_PLACES);
        sValueText.text = SSlider.value.ToString(DECIMAL_PLACES);
        vValueText.text = VSlider.value.ToString(DECIMAL_PLACES);

        dropdownTextGUI.color = new Color(1f - newColor.r, 1f - newColor.g, 1f - newColor.b);
    }
    #endregion

    #region Increments & Decrements
    public void IncrementCurrentScore()
    {
        scoreKeeper.IncrementCurrentScore();
        UpdateCurrentScoreText();
    }

    public void IncrementTicksPerSecond()
    {
        IncrementDriver(ticksPerSecondSlider, UpdateTicksPerSecond, TICK_INC);
    }

    public void DecrementTicksPerSecond()
    {
        DecrementDriver(ticksPerSecondSlider, UpdateTicksPerSecond, TICK_INC);
    }

    public void IncrementH()
    {
        IncrementDriver(HSlider, UpdateHueFromSliders, COLOR_INC);
    }

    public void DecrementH()
    {
        DecrementDriver(HSlider, UpdateHueFromSliders, COLOR_INC);
    }

    public void IncrementS()
    {
        IncrementDriver(SSlider, UpdateHueFromSliders, COLOR_INC);
    }

    public void DecrementS()
    {
        DecrementDriver(SSlider, UpdateHueFromSliders, COLOR_INC);
    }

    public void IncrementV()
    {
        IncrementDriver(VSlider, UpdateHueFromSliders, COLOR_INC);
    }

    public void DecrementV()
    {
        DecrementDriver(VSlider, UpdateHueFromSliders, COLOR_INC);
    }

    #endregion

    #region Incremnt & Decrement Drivers
    private void IncrementDriver(Slider slider, Method method, float increment)
    {
        slider.value += increment;
        method();
    }

    private void IncrementDriver(Slider slider, Method method, int increment)
    {
        slider.value += increment;
        method();
    }

    private void DecrementDriver(Slider slider, Method method, float increment)
    {
        slider.value -= increment;
        method();
    }

    private void DecrementDriver(Slider slider, Method method, int increment)
    {
        slider.value -= increment;
        method();
    }
    #endregion

    #region Update Color Driver
    private void UpdateColorDriver(Slider slider, HueType huetype)
    {
        float value = slider.value;
        slider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.HSVToRGB(value, 1f, 1f);
        slider.gameObject.transform.Find("Background").GetComponent<Image>().color = Color.HSVToRGB(value, 1f, 1f);

        switch(huetype)
        {
            case HueType.P:
                //settings.PrimaryHue = value;
                break;

            case HueType.S:
                //settings.SecondaryHue = value;
                break;

            case HueType.F:
               // settings.FruitHue = value;
                break;
        }
    }
    #endregion
}