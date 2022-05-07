using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private TowerBuilder towerBuilder;
    //[SerializeField] private GameScene gameScene;
    [Header("Game objects")]
    [SerializeField] private GameObject goDragging;
    [SerializeField] private Slider sliderIncome;
    [SerializeField] private Text textBank, textPlayerHP, textIncome;
    [SerializeField] private Text textFire, textWater, textEarth, textAir;

    private Bank bank;
    private Slider[] sliderButtonLevelPool;
    private GameObject[] goButtonLevelPool;
    private Button[] buttonButtonLevelPool;
    private Image[] imageButtonLevelPool;
    private SpriteRenderer sprR_dragging_tower;
    private int buttonSize;
    //private bool[] boolButtonCooldown;
    private int[] pool;
    private Sprite[] towerSprite;
    private TowerCard[] cardTower;
    private bool isGame;

    private void Start()
    {
        Initialize();

        for (int i = 1; i < buttonSize; i++)
        {
            buttonButtonLevelPool[i] = goButtonLevelPool[i].GetComponent<Button>();
        }

        towerSprite = new Sprite[cardTower.Length];

        for (int i = 1; i < cardTower.Length; i++)
        {
            towerSprite[i] = cardTower[i].sprite;
            imageButtonLevelPool[i].sprite = towerSprite[pool[i]];
        }

        bank = levelManager.GetComponent<Bank>();
        levelManager = levelManager.GetComponent<LevelManager>();
        sprR_dragging_tower = goDragging.GetComponent<SpriteRenderer>();
        bank.BankUpdateHandlerEvent += UpdateTowerButtons;
        bank.IncomeUpdateHandlerEvent += UpdateIncomeUI;
        bank.ElementCountUpdateHandlerEvent += UpdateElements;
        levelManager.PlayerHPHandlerEvent += UpdatePlayerHPUI;
        towerBuilder.StartTowerCooldownEvent += StartboolButtonCooldownAnimation;
        StartCoroutine(SliderIncomeValue());
    }

    private void Initialize()
    {
        pool = towerBuilder.Pool;
        goButtonLevelPool = towerBuilder.GoButtonLevelPool;
        cardTower = towerBuilder.CardTower;
        //goButtonLevelPool = towerBuilder.GoButtonLevelPool;
        buttonSize = goButtonLevelPool.Length;
        imageButtonLevelPool = new Image[buttonSize];
        buttonButtonLevelPool = new Button[buttonSize];
        sliderButtonLevelPool = new Slider[buttonSize];
        //towerSprite = tower_builder.towerSprite;
        //boolButtonCooldown = new bool[buttonSize];

        for (int i = 1; i < buttonSize; i++)
        {
            imageButtonLevelPool[i] = goButtonLevelPool[i].GetComponent<Image>();
            //imageButtonLevelPool[i].sprite = towerSprite[pool[i]];
            buttonButtonLevelPool[i] = goButtonLevelPool[i].GetComponent<Button>();
            sliderButtonLevelPool[i] = goButtonLevelPool[i].GetComponentInChildren<Slider>();
            if (i > 2 && i < 7)
            {
                buttonButtonLevelPool[i].enabled = false;
                imageButtonLevelPool[i].enabled = false;
            }
        }
    }

    public void EnableDraggingButton(GameObject go)
    {
        sprR_dragging_tower.sprite = towerSprite[pool[TakeButtonID(go)]];
    }

    private int TakeButtonID(GameObject go)
    {
        for (int i = 1; i < goButtonLevelPool.Length; i++)
        {
            if (go == goButtonLevelPool[i]) return i;
        }
        Debug.Log("GAMEUI: NOT FOUND BUTTON");
        return 0;
    }

    private void UpdateTowerButtons(object sender, int oldCount, int newCount)
    {
        UpdateCurrentGoldCountUI(newCount);
        //Debug.Log($"Get {newCount} from {sender.GetType()} reward");
        UpdateTowerButtonsColor(newCount);
    }

    private void UpdateCurrentGoldCountUI(int count)
    {
        textBank.text = count.ToString();
    }

    private void UpdateTowerButtonsColor(int bankCount)
    {
        for (int i = 1; i < goButtonLevelPool.Length - 4; i++)
        {
            if (pool[i] != 0 && cardTower[pool[i]].cost <= bankCount)
            {
                imageButtonLevelPool[i].color = Color.white;
            }
            else
            {
                imageButtonLevelPool[i].color = Color.gray;
            }
        }
    }

    private void UpdateElementButtonsColor()
    {
        for (int i = 7; i < goButtonLevelPool.Length; i++)
        {
            if (pool[i] != 0 && CheckElementsCost(i))
            {
                imageButtonLevelPool[i].color = Color.white;
            }
            else
            {
                imageButtonLevelPool[i].color = Color.gray;
            }
        }
    }

    private bool CheckElementsCost(int buttonID)
    {
        if (cardTower[buttonID].costFire > bank.countFire) return false;
        if (cardTower[buttonID].costWater > bank.countWater) return false;
        if (cardTower[buttonID].costEarth > bank.countEarth) return false;
        if (cardTower[buttonID].costAir > bank.countAir) return false;
        return true;
    }

    private void UpdatePlayerHPUI(object sender, int oldHP, int newHP)
    {
        textPlayerHP.text = newHP.ToString();
    }

    private void UpdateIncomeUI(object sender, int oldIncome, int newIncome)
    {
        textIncome.text = newIncome.ToString();
    }

    private void StartboolButtonCooldownAnimation(GameObject go, float cooldown)
    {
        int i = TakeButtonID(go);
        //boolButtonCooldown[i] = true;
        go.GetComponent<Animator>().speed = 1 / cooldown;
        go.GetComponent<Animator>().SetTrigger("CooldownAnimation");
        StartCoroutine(Cooldown(i, cooldown));
    }

    private IEnumerator Cooldown(int i, float cooldown)
    {
        buttonButtonLevelPool[i].enabled = false;
        yield return new WaitForSeconds(cooldown);
        //boolButtonCooldown[i] = false;
        buttonButtonLevelPool[i].enabled = true;
    }

    private IEnumerator SliderIncomeValue()
    {
        sliderIncome.maxValue = levelManager.IncomeInterval;
        while (sliderIncome)
        {
            if (sliderIncome.value < sliderIncome.maxValue) sliderIncome.value += 1.0f;
            else sliderIncome.value = 1.0f;
            yield return new WaitForSeconds(1.0f);
        }
    }

    private void UpdateElements(object sender, int elementID, int oldCount, int newCount)
    {
        switch(elementID)
        {
            case 1:
                textFire.text = newCount.ToString();
                break;
            case 2:
                textWater.text = newCount.ToString();
                break;
            case 3:
                textEarth.text = newCount.ToString();
                break;
            case 4:
                textAir.text = newCount.ToString();
                break;
        }
        UpdateElementButtonsColor();
    }
}
