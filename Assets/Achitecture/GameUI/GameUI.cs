using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private TowerBuilder towerBuilder;
        [SerializeField] private GameScene gameScene;
        [Header("Game objects")]
        [SerializeField] private GameObject goDragging;
        [SerializeField] private Slider sliderIncome;
        [SerializeField] private Text textGoldCounter, textPlayerHPCounter, textIncomeCounter;
        [SerializeField] private Text[] textElement;
        [SerializeField] private GameObject canvasUpgrade;
        [SerializeField] private Transform transformUpgradeButtons;
        [SerializeField] private GameObject[] upgradeButton;
        [SerializeField] private RectTransform rt_UpgradePanelBG;

        private Bank bank;
        private Slider[] sliderButtonLevelPool;
        private GameObject[] goButtonLevelPool;
        private Button[] buttonButtonLevelPool;
        private Image[] imageButtonLevelPool;
        [SerializeField] private Image[] imageUpgradeButton;
        private Text[][] textButtonLevelPool;
        private Text[][] textUpgradeButton;
        private SpriteRenderer sprR_dragging_tower;
        private int buttonSize;
        private int[] pool;
        private int[] upgradeButtonID;
        private Sprite[] towerSprite;
        private TowerCard[] cardTower;

        private void OnEnable()
        {
            bank = levelManager.gameObject.GetComponent<Bank>();

            bank.BankUpdateHandlerEvent += UpdateGoldCounter;
            bank.IncomeUpdateHandlerEvent += UpdateIncomeCounter;
            bank.ElementCountUpdateHandlerEvent += UpdateElementCounter;
            levelManager.PlayerHPHandlerEvent += UpdatePlayerHPCounter;
            levelManager.SliderIncomeIndicatorEvent += UpdateSliderIndicator;

            OnEnableUpdateCounters();
        }

        private void OnEnableUpdateCounters()
        {
            TextUpdate(textGoldCounter, bank.count.ToString());
            TextUpdate(textPlayerHPCounter, levelManager.PlayerHp.ToString());

            for (int i = 0; i < textElement.Length; i++)
            {
                TextUpdate(textElement[i], bank.countElement[i].ToString());
            }

            UpdateSliderIndicator(levelManager.IncomeTimer, levelManager.IncomeInterval);
        }

        private void TextUpdate(Text text, string str)
        {
            text.text = str;
        }

        private void UpdateSliderIndicator(int newValue, int maxValue)
        {
            if (sliderIncome.maxValue != maxValue)
                sliderIncome.maxValue = maxValue;
            sliderIncome.value = newValue;
        }

        private void UpdateGoldCounter(object sender, int oldCount, int newCount)
        {
            TextUpdate(textGoldCounter, newCount.ToString());
        }

        private void UpdateIncomeCounter(object sender, int oldIncome, int newIncome)
        {
            TextUpdate(textIncomeCounter, newIncome.ToString());
        }

        private void UpdateElementCounter(object sender, int elementID, int oldCount, int newCount)
        {
            TextUpdate(textElement[elementID], newCount.ToString());
        }

        private void UpdatePlayerHPCounter(object sender, int oldCount, int newCount)
        {
            TextUpdate(textPlayerHPCounter, newCount.ToString());
        }

        private void OnDisable()
        {
            bank.BankUpdateHandlerEvent -= UpdateGoldCounter;
            bank.IncomeUpdateHandlerEvent -= UpdateIncomeCounter;
            bank.ElementCountUpdateHandlerEvent -= UpdateElementCounter;
            levelManager.PlayerHPHandlerEvent -= UpdatePlayerHPCounter;
            levelManager.SliderIncomeIndicatorEvent -= UpdateSliderIndicator;
        }

        //private void Start()
        //{
        //    pool = towerBuilder.Pool;
        //    goButtonLevelPool = gameScene.GoButtonLevelPool;
        //    cardTower = gameScene.CardTower;
        //    buttonSize = goButtonLevelPool.Length;
        //    imageButtonLevelPool = new Image[buttonSize];
        //    buttonButtonLevelPool = new Button[buttonSize];
        //    sliderButtonLevelPool = new Slider[buttonSize];

        //    for (int i = 1; i < buttonSize; i++)
        //    {
        //        imageButtonLevelPool[i] = goButtonLevelPool[i].GetComponent<Image>();
        //        buttonButtonLevelPool[i] = goButtonLevelPool[i].GetComponent<Button>();
        //        sliderButtonLevelPool[i] = goButtonLevelPool[i].GetComponentInChildren<Slider>();
        //    }

        //    for (int i = 1; i < buttonSize; i++)
        //    {
        //        buttonButtonLevelPool[i] = goButtonLevelPool[i].GetComponent<Button>();
        //    }

        //    towerSprite = new Sprite[cardTower.Length];

        //    for (int i = 1; i < cardTower.Length; i++)
        //    {
        //        towerSprite[i] = cardTower[i].sprite;
        //    }


        //    textUpgradeButton = new Text[upgradeButton.Length][];
        //    Debug.Log("upgradeButton.Length: " + upgradeButton.Length);

        //    for (int i = 1; i < upgradeButton.Length; i++)
        //    {
        //        textUpgradeButton[i] = upgradeButton[i].GetComponentsInChildren<Text>();
        //        Debug.Log(textUpgradeButton[i][0]);
        //        Debug.Log(textUpgradeButton[i][1]);
        //        Debug.Log(textUpgradeButton[i][2]);
        //        Debug.Log(textUpgradeButton[i][3]);
        //        Debug.Log(textUpgradeButton[i][4]);
        //    }

        //    textButtonLevelPool = new Text[buttonSize][];
        //    Debug.Log("textButtonLevelPool.Length: " + textButtonLevelPool.Length);

        //    for (int i = 1; i < 3; i++)
        //    {
        //        textButtonLevelPool[i] = goButtonLevelPool[i].GetComponentsInChildren<Text>();
        //        Debug.Log(textButtonLevelPool[i][0]);
        //        Debug.Log(textButtonLevelPool[i][1]);
        //        Debug.Log(textButtonLevelPool[i][2]);
        //        Debug.Log(textButtonLevelPool[i][3]);
        //        Debug.Log(textButtonLevelPool[i][4]);
        //    }

        //    for (int i = 1; i < imageButtonLevelPool.Length; i++)
        //    {
        //        imageButtonLevelPool[i].sprite = towerSprite[pool[i]];
        //        if (pool[i] != 0) ShowCostIndicatorsForUpgradeButton(i, cardTower[pool[i]], textButtonLevelPool);//textButtonLevelPool[i].text = cardTower[pool[i]].cost.ToString();
        //    }

        //    imageUpgradeButton = new Image[upgradeButton.Length];
        //    upgradeButtonID = new int[upgradeButton.Length];
        //    for (int i = 1; i < upgradeButton.Length; i++)
        //    {
        //        imageUpgradeButton[i] = upgradeButton[i].GetComponent<Image>();
        //    }

        //    bank = levelManager.GetComponent<Bank>();
        //    levelManager = levelManager.GetComponent<LevelManager>();
        //    sprR_dragging_tower = goDragging.GetComponent<SpriteRenderer>();

        //    bank.BankUpdateHandlerEvent += UpdateTowerButtons;
        //    bank.IncomeUpdateHandlerEvent += UpdateIncomeUI;
        //    bank.ElementCountUpdateHandlerEvent += UpdateElements;
        //    levelManager.PlayerHPHandlerEvent += UpdatePlayerHPUI;
        //    towerBuilder.StartTowerCooldownEvent += StartboolButtonCooldownAnimation;
        //    towerBuilder.ClickToBuiltTowerHandlerEvent += ShowUpgradeWindow;

        //    StartCoroutine(SliderIncomeValue());
        //}

        //public void EnableDraggingButton(GameObject go)
        //{
        //    sprR_dragging_tower.sprite = towerSprite[pool[TakeButtonID(go)]];
        //}

        //private int TakeButtonID(GameObject go)
        //{
        //    for (int i = 1; i < goButtonLevelPool.Length; i++)
        //    {
        //        if (go == goButtonLevelPool[i]) return i;
        //    }
        //    Debug.Log("GAMEUI: NOT FOUND BUTTON");
        //    return 0;
        //}

        //private void UpdateTowerButtons(object sender, int oldCount, int newCount)
        //{
        //    UpdateCurrentGoldCountUI(newCount);
        //    //Debug.Log($"Get {newCount} from {sender.GetType()} reward");
        //    UpdateTowerButtonsColor(newCount);
        //    UpdateElementButtonsColor(upgradeButtonID);
        //}

        //private void UpdateCurrentGoldCountUI(int count)
        //{
        //    textBank.text = count.ToString();
        //}

        //private void UpdateTowerButtonsColor(int bankCount)
        //{
        //    for (int i = 1; i < goButtonLevelPool.Length; i++)
        //    {
        //        if (pool[i] != 0 && cardTower[pool[i]].cost <= bankCount)
        //        {
        //            imageButtonLevelPool[i].color = Color.white;
        //        }
        //        else
        //        {
        //            imageButtonLevelPool[i].color = Color.gray;
        //        }
        //    }
        //}

        //private void UpdateElementButtonsColor(int[] updateButton)
        //{
        //    upgradeButtonID = updateButton;

        //    for (int i = 0; i < imageUpgradeButton.Length; i++)
        //    {
        //        if (updateButton[i] != 0 && CheckElementsCost(updateButton[i]))
        //        {
        //            imageUpgradeButton[i].color = Color.white;
        //            continue;
        //        }

        //        imageUpgradeButton[i].color = Color.gray;
        //    }
        //}

        //private bool CheckElementsCost(int buttonID)
        //{
        //    TowerCard card = cardTower[buttonID];

        //    if (card.cost > bank.count) 
        //        return false;

        //    for (int i = 0; i < card.costElement.Length; i++)
        //    {
        //        if (card.costElement[i] > bank.countElement[i])
        //            return false;
        //    }

        //    return true;
        //}

        //private void UpdatePlayerHPUI(object sender, int oldHP, int newHP)
        //{
        //    textPlayerHP.text = newHP.ToString();
        //}

        //private void UpdateIncomeUI(object sender, int oldIncome, int newIncome)
        //{
        //    textIncome.text = newIncome.ToString();
        //}

        //private void StartboolButtonCooldownAnimation(GameObject go, float cooldown)
        //{
        //    int i = TakeButtonID(go);
        //    if (i != 0)
        //    {
        //        go.GetComponent<Animator>().speed = 1 / cooldown;
        //        go.GetComponent<Animator>().SetTrigger("CooldownAnimation");
        //        StartCoroutine(Cooldown(i, cooldown));
        //    }
        //}

        //private IEnumerator Cooldown(int i, float cooldown)
        //{
        //    buttonButtonLevelPool[i].enabled = false;
        //    yield return new WaitForSeconds(cooldown);
        //    buttonButtonLevelPool[i].enabled = true;
        //}

        //private IEnumerator SliderIncomeValue()
        //{
        //    sliderIncome.maxValue = levelManager.IncomeInterval;
        //    while (sliderIncome)
        //    {
        //        if (sliderIncome.value < sliderIncome.maxValue) sliderIncome.value += 1.0f;
        //        else sliderIncome.value = 1.0f;
        //        yield return new WaitForSeconds(1.0f);
        //    }
        //}

        //private void UpdateElements(object sender, int elementID, int oldCount, int newCount)
        //{
        //    switch (elementID)
        //    {
        //        case 0:
        //            textFire.text = newCount.ToString();
        //            break;
        //        case 1:
        //            textWater.text = newCount.ToString();
        //            break;
        //        case 2:
        //            textEarth.text = newCount.ToString();
        //            break;
        //        case 3:
        //            textAir.text = newCount.ToString();
        //            break;
        //    }
        //    UpdateElementButtonsColor(upgradeButtonID);
        //}

        //public void ShowUpgradeWindow(GameObject go, TowerCard card)
        //{
        //    if (go != null)
        //    {
        //        int[] upperTower = card.upperTower;
        //        UpdateElementButtonsColor(upperTower);
        //        for (int i = 1; i < upgradeButton.Length; i++)
        //        {
        //            if (upperTower[i] == 0)
        //            {
        //                upgradeButton[i].SetActive(false);
        //                continue;
        //            }

        //            imageUpgradeButton[i].sprite = towerSprite[upperTower[i]];
        //            upgradeButton[i].SetActive(true);
        //            ShowCostIndicatorsForUpgradeButton(i, cardTower[upperTower[i]], textUpgradeButton);
        //        }
        //        canvasUpgrade.SetActive(true);
        //        transformUpgradeButtons.position = go.transform.position;
        //    }
        //    else
        //    {
        //        canvasUpgrade.SetActive(false);
        //    }
        //}

        //private void ShowCostIndicatorsForUpgradeButton(int j, TowerCard card, Text[][] txt)
        //{
        //    if (card.cost > 0)
        //    {
        //        //textTowerCosts[j * 5].text = card.cost.ToString();
        //        //textTowerCosts[j * 5].gameObject.SetActive(true);
        //        txt[j][0].text = card.cost.ToString();
        //        txt[j][0].gameObject.SetActive(true);
        //    }
        //    else
        //    {
        //        //textTowerCosts[j * 5].text = null;
        //        //textTowerCosts[j * 5].gameObject.SetActive(false);
        //        txt[j][0].text = null;
        //        txt[j][0].gameObject.SetActive(false);
        //    }

        //    for (int i = 0; i < card.costElement.Length; i++)
        //    {
        //        if (card.costElement[i] > 0)
        //        {
        //            //textTowerCosts[j * 5 + i + 1].text = card.costElement[i].ToString();
        //            //textTowerCosts[j * 5 + i + 1].gameObject.SetActive(true);
        //            txt[j][i + 1].text = card.costElement[i].ToString();
        //            txt[j][i + 1].gameObject.SetActive(true);
        //            continue;
        //        }
        //        //textTowerCosts[j * 5 + i + 1].text = null;
        //        //textTowerCosts[j * 5 + i + 1].gameObject.SetActive(false);
        //        txt[j][i + 1].text = null;
        //        txt[j][i + 1].gameObject.SetActive(false);
        //    }
        //}
    }
}
