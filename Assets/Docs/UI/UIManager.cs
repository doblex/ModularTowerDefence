using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public delegate void OnTurretSelected(TowerTemplate tower);

    public OnTurretSelected onTurretSelected;

    VisualElement root;
    VisualElement templateContainer;

    Label waveTimer;
    Label waveNumber;

    Label currency;

    VisualElement lifeFillBar;
    Label lifePercent;

    [SerializeField] UIDocument pause;
    VisualElement pauseMenu;
    Button returnButton;

    [SerializeField] UIDocument gameOver;
    VisualElement gameOverMenu;
    Button gameOverReturnButton;
    Button gameOverRestartButton;
    Label gameOverEndText;

    [SerializeField] VisualTreeAsset turretTemplate;
    [SerializeField] VisualTreeAsset powerUpTemplate;
    [SerializeField] VisualTreeAsset effectTemplate;

    List<TowerTemplate> towerTemplates = new List<TowerTemplate>();
    private bool isGamePaused;

    private void Awake()
    {
        GameManager.UIManagerInstance = this;

        root = GetComponent<UIDocument>().rootVisualElement;

        templateContainer = root.Q<VisualElement>("TurretContainer");

        waveTimer = root.Q<Label>("Timer");
        waveNumber = root.Q<Label>("Wave");

        currency = root.Q<Label>("Currency");

        lifeFillBar = root.Q<VisualElement>("FillBar");
        lifePercent = root.Q<Label>("LifePercent");

        pauseMenu = pause.rootVisualElement;
        pauseMenu.style.display = DisplayStyle.None;

        returnButton = pauseMenu.Q<Button>("Return");
        returnButton.clicked += () =>
        {
            isGamePaused = false;
            Time.timeScale = 1;
            UnRegisterTemplates();
            SceneManager.LoadScene("Title");
        };

        gameOverMenu = gameOver.rootVisualElement;
        gameOverMenu.style.display = DisplayStyle.None;
        gameOverReturnButton = gameOverMenu.Q<Button>("Return");
        gameOverReturnButton.clicked += () =>
        {
            isGamePaused = false;
            Time.timeScale = 1;
            UnRegisterTemplates();
            SceneManager.LoadScene("Title");
        };

        gameOverRestartButton = gameOverMenu.Q<Button>("Retry");
        gameOverRestartButton.clicked += () =>
        {
            isGamePaused = false;
            Time.timeScale = 1;
            UnRegisterTemplates();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        };

        gameOverEndText = gameOverMenu.Q<Label>("EndText");
    }

    private void Start()
    {
        GameManager.Instance.LifeChanged += UpdateLifeBar;
        GameManager.Instance.CurrencyChanged += UpdateCurrency;
        RoundManager.Instance.onWaveChanged += UpdateWaveNumber;
        WaveSpawner.Instance.onWaveTimerChanged += UpdateWaveTimer;
        GameManager.Instance.GameFinished += (win) =>
        {
            isGamePaused = true;
            Time.timeScale = 0;
            gameOverMenu.style.display = DisplayStyle.Flex;
            gameOverEndText.text = win ? "You Win!" : "You Lose!";
        };

        towerTemplates.AddRange(GameManager.Instance.GetTemplates());

        PaintUITemplates(true);
    }

    private void Update()
    {
        Pause();
        PaintUITemplates();
    }

    private void UnRegisterTemplates()
    {
        foreach (TowerTemplate template in towerTemplates)
        {
            VisualElement element = null;

            if (template.container == null)
            {
                switch (template.placementType)
                {
                    case TowerType.powerUp:
                        element = DoPowerUpTemplate((PowerUpTemplate)template);
                        break;
                    case TowerType.Turret:
                        element = DoTurretTemplate((TurretTemplate)template);
                        break;
                }

                element.UnregisterCallback<PointerDownEvent>(ev => OnTemplateClicked(element));
                template.isSelected = false;
                template.container.RemoveFromClassList("selected");
            }
        }
    }

    private void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isGamePaused = !isGamePaused;


            if (isGamePaused)
            {
                Time.timeScale = 0;
                pauseMenu.style.display = DisplayStyle.Flex;
            }
            else
            {
                Time.timeScale = 1;
                pauseMenu.style.display = DisplayStyle.None;
            }
        }
    }

    private void PaintUITemplates(bool firstTime = false)
    {
        foreach (TowerTemplate template in towerTemplates)
        {
            VisualElement element = null;

            if (template.container == null || firstTime)
            {
                switch (template.placementType)
                {
                    case TowerType.powerUp:
                        element = DoPowerUpTemplate((PowerUpTemplate)template);
                        break;
                    case TowerType.Turret:
                        element = DoTurretTemplate((TurretTemplate)template);
                        break;
                }

                element.RegisterCallback<PointerDownEvent>(ev => OnTemplateClicked(element));

                template.SetContainer(element);
            }
            else
            {
                element = template.container;
            }

            template.SetSelected(template.isSelected);

            template.SetPurchasable();

            templateContainer.Add(element);
        }
    }

    private VisualElement DoPowerUpTemplate(PowerUpTemplate template)
    {
        VisualElement element = powerUpTemplate.CloneTree().Children().ElementAt(0);
        element.Q<Label>("Title").text = template.objectName;
        element.Q<Label>("Cost").text = template.cost.ToString() + "$";
        element.Q<Label>("UpgradeCost").text = template.towerPoint.ToString();

        ScrollView scroll = element.Q<ScrollView>("effects");

        if (template.projectilePrefab != null)
        { 
            VisualElement projectile = effectTemplate.CloneTree().Children().ElementAt(0);
            projectile.Q<Label>("effect").text = template.projectileName;
            scroll.Add(projectile);
        }

        foreach (var item in template.effects)
        {
            VisualElement effect = effectTemplate.CloneTree().Children().ElementAt(0);
            effect.Q<Label>("effect").text = item.value.ToString() + " " +  item.powerUpType.ToString();
            scroll.Add(effect);
        }

        return element;
    }

    private VisualElement DoTurretTemplate(TurretTemplate template)
    {
        VisualElement element = turretTemplate.CloneTree().Children().ElementAt(0);
        element.Q<Label>("Title").text = template.objectName;
        element.Q<Label>("Cost").text = template.cost.ToString() + "$";
        element.Q<Label>("UpgradeCost").text = template.towerPoint.ToString();
        element.Q<Label>("FireRate").text = template.fireRate.ToString() + "/s";
        element.Q<Label>("Damage").text = template.damage.ToString();

        return element;
    }

    private void OnTemplateClicked(VisualElement element)
    {
        TowerTemplate selectedTemplate = null;

        foreach (TowerTemplate template in towerTemplates)
        {
            if (template.container != element)
            {
                template.isSelected = false;
                template.container.RemoveFromClassList("selected");
            }
            else
            {
                if (template.isSelected || !template.isPurchasable)
                {
                    template.isSelected = false;
                    template.container.RemoveFromClassList("selected");

                    selectedTemplate = null;
                }
                else
                { 
                    template.isSelected = true;
                    template.container.AddToClassList("selected");

                    selectedTemplate = template;
                }
            }
        }

        onTurretSelected?.Invoke(selectedTemplate);
    }

    public void UpdateWaveTimer(float time)
    {
        waveTimer.text = time.ToString("0.0") + " s to next Wave";
    }

    public void UpdateWaveNumber(float wave, float round)
    {
        waveNumber.text = (wave + 1).ToString() + " Wave " + (round + 1).ToString() + " Round";
    }

    public void UpdateCurrency(int amount)
    {
        currency.text = amount.ToString() + "$";
    }

    public void UpdateLifeBar(float value, float maxValue)
    {
        float i = Mathf.Lerp(0, 100, value / maxValue);

        lifeFillBar.style.width = Length.Percent(i);
        lifeFillBar.style.backgroundColor = Color.Lerp(Color.red, Color.green, value / maxValue);
        lifePercent.text = i.ToString("0") + "%";
    }
}
