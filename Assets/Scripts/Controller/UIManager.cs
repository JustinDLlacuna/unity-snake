using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    private ScoreKeeper scoreKeeper;
    private Settings settings;

    [SerializeField] private Text currentScoreText;
    [SerializeField] private Text pauseButtonText;
    [SerializeField] private Text highScoreText;
    [SerializeField] private Text ticksPerSecondText;

    [SerializeField] private Button pauseButton;
    [SerializeField] private Button stopButton;

    [SerializeField] private Slider ticksPerSecondSlider;
    [SerializeField] private Slider primaryColorSlider;
    [SerializeField] private Slider secondaryColorSlider;
    [SerializeField] private Slider fruitColorSlider;

    [SerializeField] private Toggle beepToggle;

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

    public void IncrementCurrentScore()
    {
        scoreKeeper.IncrementCurrentScore();
        UpdateCurrentScoreText();
    }

    //Deactivates game panel's current score, pause button, and stop button.
    //Shows start panel's high score, ticks per second, toggle beep, and start button.
    public void ShowStartPanel()
    {
        highScoreText.text = $"HIGH SCORE: {scoreKeeper.GetHighScore((int)settings.TicksPerSecond)}";

        //Loading ticks per second value from settings data.
        ticksPerSecondText.text = $"TICKS PER SECOND: {settings.TicksPerSecond}";
        ticksPerSecondSlider.value = settings.TicksPerSecond;

        //Loading color values to sliders from settings data.
        primaryColorSlider.value = settings.PrimaryColor;
        secondaryColorSlider.value = settings.SecondaryColor;
        fruitColorSlider.value = settings.FruitColor;

        beepToggle.isOn = settings.UseBeep;
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

    //Ticks per second is updated via slider.
    public void UpdateTicksPerSecond()
    {
        float value = ticksPerSecondSlider.value;
        settings.TicksPerSecond = value;
        ticksPerSecondText.text = $"TICKS PER SECOND: {value}";
        highScoreText.text = $"HIGH SCORE: {scoreKeeper.GetHighScore((int)value)}";
    }

    public void UpdatePColor()
    {
        float value = primaryColorSlider.value;
        primaryColorSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.HSVToRGB(value, 1f, 1f);
        primaryColorSlider.gameObject.transform.Find("Background").GetComponent<Image>().color = Color.HSVToRGB(value, 1f, 1f);
        settings.PrimaryColor = value;
    }

    public void UpdateSColor()
    {
        float value = secondaryColorSlider.value;
        secondaryColorSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.HSVToRGB(value, 1f, 1f);
        secondaryColorSlider.gameObject.transform.Find("Background").GetComponent<Image>().color = Color.HSVToRGB(value, 1f, 1f);
        settings.SecondaryColor = value;
    }

    public void UpdateFColor()
    {
        float value = fruitColorSlider.value;
        fruitColorSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.HSVToRGB(value, 1f, 1f);
        fruitColorSlider.gameObject.transform.Find("Background").GetComponent<Image>().color = Color.HSVToRGB(value, 1f, 1f);
        settings.FruitColor = value;
    }

    public void IncrementTicksPerSecond()
    {
        ticksPerSecondSlider.value++;
        UpdateTicksPerSecond();
    }

    public void DecrementTicksPerSecond()
    {
        ticksPerSecondSlider.value--;
        UpdateTicksPerSecond();
    }

    public void IncrementPColor()
    {
        primaryColorSlider.value += .01f;
        UpdatePColor();
    }

    public void DecrementPColor()
    {
        primaryColorSlider.value -= .01f;
        UpdatePColor();
    }

    public void IncrementSColor()
    {
        secondaryColorSlider.value += .01f;
        UpdateSColor();
    }

    public void DecrementSColor()
    {
        secondaryColorSlider.value -= .01f;
        UpdateSColor();
    }

    public void IncrementFColor()
    {
        fruitColorSlider.value += .01f;
        UpdateFColor();
    }

    public void DecrementFColor()
    {
        fruitColorSlider.value -= .01f;
        UpdateFColor();
    }

    public void ToggleBeep()
    {
        settings.UseBeep = beepToggle.isOn;
    }
}