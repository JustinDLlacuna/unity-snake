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

    #region Managers
    private static UIManager instance;

    private ScoreKeeper scoreKeeper;

    private Settings settings;
    #endregion

    private delegate void Method();

    [SerializeField] private new Camera camera;

    [SerializeField] private TMP_Dropdown dropdown;

    [SerializeField] private Image dropdownImage;

    [Header("Text")]
    [SerializeField] private Text currentScoreText;
    [SerializeField] private Text pauseButtonText;
    [SerializeField] private Text highScoreText;
    [SerializeField] private Text averageScoreText;
    [SerializeField] private Text ticksPerSecondText;
    [SerializeField] private Text hValueText;
    [SerializeField] private Text sValueText;
    [SerializeField] private Text vValueText;
    [SerializeField] private Text versionText;
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

    [Header("Toggles")]
    [SerializeField] private Toggle beepToggle;
    [SerializeField] private Toggle musicToggle;
    [Space(10)]

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
    }

    public bool GetBeepToggleValue()
    {
        return beepToggle.isOn;
    }

    public void ToggleBeep()
    {
        settings.UseBeep = beepToggle.isOn;
    }

    public void ToggleMusic()
    {
        settings.UseMusic = musicToggle.isOn;
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

    #region Show Panels
    //Deactivates game panel's current score, pause button, and stop button.
    //Shows start panel's high score, ticks per second, toggle beep, and start button.
    public void ShowStartPanel()
    {
        highScoreText.text = $"HIGH SCORE: {scoreKeeper.GetHighScore((int)settings.TicksPerSecond)}";
        averageScoreText.text = $"AVERAGE SCORE: {scoreKeeper.GetAvgScore((int)settings.TicksPerSecond)}";

        //Loading ticks per second value from settings data.
        ticksPerSecondText.text = $"TICKS PER SECOND: {settings.TicksPerSecond}";
        ticksPerSecondSlider.value = settings.TicksPerSecond;

        //Loading color values to sliders from settings data.
        ChangeColor();

        camera.backgroundColor = settings.BackgroundHue.toRGB;
        versionText.color = new Color(1f - settings.BackgroundHue.toRGB.r, 1f - settings.BackgroundHue.toRGB.g, 1f - settings.BackgroundHue.toRGB.b);

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
        averageScoreText.text = $"AVERAGE SCORE: {scoreKeeper.GetAvgScore((int)value)}";
    }

    public void ChangeColor()
    {
        Hue newHue = settings.PrimaryHue;       
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
                versionText.color = new Color(1f - newHue.toRGB.r, 1f - newHue.toRGB.g, 1f - newHue.toRGB.b);
                break;
        }

        dropdownImage.color = newHue.toRGB;

        HSlider.value = newHue.H;
        SSlider.value = newHue.S;
        VSlider.value = newHue.V;

        hValueText.text = HSlider.value.ToString(DECIMAL_PLACES);
        sValueText.text = SSlider.value.ToString(DECIMAL_PLACES);
        vValueText.text = VSlider.value.ToString(DECIMAL_PLACES);

        dropdownTextGUI.color = new Color(1f - newHue.toRGB.r, 1f - newHue.toRGB.g, 1f - newHue.toRGB.b); 
    }

    public void UpdateHueFromSliders()
    {
        float h = HSlider.value;
        float s = SSlider.value;
        float v = VSlider.value;

        Hue newHue = new Hue(h, s, v);

        switch (currentHueChange)
        {
            case HueType.P:
                settings.PrimaryHue = newHue;
                break;

            case HueType.S:
                settings.SecondaryHue = newHue; 
                break;

            case HueType.F:
                settings.FruitHue = newHue;
                break;

            case HueType.G:
                settings.GridHue = newHue;
                break;

            case HueType.B:
                settings.BackgroundHue = newHue;
                camera.backgroundColor = newHue.toRGB;
                versionText.color = new Color(1f - newHue.toRGB.r, 1f - newHue.toRGB.g, 1f - newHue.toRGB.b);
                break;
        }

        dropdownImage.color = newHue.toRGB;

        hValueText.text = HSlider.value.ToString(DECIMAL_PLACES);
        sValueText.text = SSlider.value.ToString(DECIMAL_PLACES);
        vValueText.text = VSlider.value.ToString(DECIMAL_PLACES);

        dropdownTextGUI.color = new Color(1f - newHue.toRGB.r, 1f - newHue.toRGB.g, 1f - newHue.toRGB.b);
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
}