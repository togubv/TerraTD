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
    [SerializeField] private Text textBank, textPlayerHP, textIncome;

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


    //[SerializeField] private TowerBuilder tower_builder;
    //[SerializeField] private LevelManager go_levelManager;

    //[SerializeField] private GameObject goDragging;
    //[SerializeField] private Text text_bank, text_playerHP;
    //[SerializeField] private GameObject[] goButtonLevelPool;
    //[SerializeField] private TowerCard[] tower_card;

    //private Bank bank;
    //private LevelManager levelManager;

    //private Button[] buttonButtonLevelPool;
    //private Slider[] sliderButtonLevelPool;
    //private SpriteRenderer sprR_dragging_tower;
    //public Image[] imageButtonLevelPool;
    //[SerializeField] private Sprite[] towerSprite;
    //private int[] pool;
    //private bool[] boolButtonCooldown;

    //public Color colorDisabled;

    private void Start()
    {
        Initialize();

        for (int i = 0; i < buttonSize; i++)
        {
            buttonButtonLevelPool[i] = goButtonLevelPool[i].GetComponent<Button>();
        }

        towerSprite = new Sprite[cardTower.Length + 1];

        for (int i = 0; i < cardTower.Length; i++)
        {
            towerSprite[i + 1] = cardTower[i].sprite;
            imageButtonLevelPool[i].sprite = towerSprite[pool[i]];
        }

        bank = levelManager.GetComponent<Bank>();
        levelManager = levelManager.GetComponent<LevelManager>();
        sprR_dragging_tower = goDragging.GetComponent<SpriteRenderer>();
        bank.BankUpdateHandlerEvent += UpdateTowerButtons;
        bank.IncomeUpdateHandlerEvent += UpdateIncomeUI;
        levelManager.PlayerHPHandlerEvent += UpdatePlayerHPUI;
        towerBuilder.StartTowerCooldownEvent += StartboolButtonCooldownAnimation;
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

        for (int i = 0; i < buttonSize; i++)
        {
            imageButtonLevelPool[i] = goButtonLevelPool[i].GetComponent<Image>();
            //imageButtonLevelPool[i].sprite = towerSprite[pool[i]];
            buttonButtonLevelPool[i] = goButtonLevelPool[i].GetComponent<Button>();
            sliderButtonLevelPool[i] = goButtonLevelPool[i].GetComponentInChildren<Slider>();
            if (pool[i] == 0) goButtonLevelPool[i].SetActive(false);
        }
    }

    public void EnableDraggingButton(GameObject go)
    {
        sprR_dragging_tower.sprite = towerSprite[pool[TakeButtonID(go)]];
    }

    private int TakeButtonID(GameObject go)
    {
        for (int i = 0; i < goButtonLevelPool.Length; i++)
        {
            if (go == goButtonLevelPool[i]) return i;
        }
        Debug.Log("GAMEUI: NOT FOUND BUTTON");
        return -1;
    }

    private void UpdateTowerButtons(object sender, int oldCount, int newCount)
    {
        UpdateCurrentGoldCountUI(newCount);
        Debug.Log($"Get {newCount} from {sender.GetType()} reward");
        UpdateButtonsColor(newCount);
    }

    private void UpdateCurrentGoldCountUI(int count)
    {
        textBank.text = count.ToString();
    }

    private void UpdateButtonsColor(int bankCount)
    {
        for (int i = 0; i < goButtonLevelPool.Length; i++)
        {
            if (pool[i] != 0 && cardTower[pool[i] - 1].cost <= bankCount)
            {
                imageButtonLevelPool[i].color = Color.white;
            }
            else
            {
                imageButtonLevelPool[i].color = Color.gray;
            }
        }
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
}
