using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TowerBuilderUI : MonoBehaviour
    {
        [SerializeField] private GameScene gameScene;
        [SerializeField] private TowerBuilder towerBuilder;
        [Header("Game Objects")]
        [SerializeField] private GameObject draggingObject;
        [SerializeField] private GameObject canvasUpgrade;
        [SerializeField] private Transform transformUpgradeButtons;
        [SerializeField] private GameObject[] goButtonUpgrade;

        private Text[][] textButtonCost;
        private Text[][] textButtonUpgradeCost;
        private SpriteRenderer sR_DraggingObject;
        private GameObject[] goButtonLevelPool;
        private Image[] imageButtonLevelPool;
        private Image[] imageButtonUpgrade;
        private Button[] buttonButtonLevelPool;
        private Button[] buttonButtonUpgrade;

        private void OnEnable()
        {
            towerBuilder.StartDraggingTowerHandlerEvent += EnableDraggingObject;
            towerBuilder.StartTowerCooldownEvent += StartBuildButtonCooldownAnimation;
            towerBuilder.ClickToBuiltTowerHandlerEvent += ShowTowerUpdradeWindow;
            towerBuilder.UpdateButtonCostBehaviourEvent += UpdateTowerButtonsColor;

            sR_DraggingObject = draggingObject.GetComponent<SpriteRenderer>();

            goButtonLevelPool = gameScene.GoButtonLevelPool;
            imageButtonLevelPool = new Image[goButtonLevelPool.Length];
            imageButtonUpgrade = new Image[goButtonUpgrade.Length];
            buttonButtonLevelPool = new Button[goButtonLevelPool.Length];
            buttonButtonUpgrade = new Button[goButtonUpgrade.Length];

            for (int i = 1; i < goButtonLevelPool.Length; i++)
            {
                imageButtonLevelPool[i] = goButtonLevelPool[i].GetComponent<Image>();
                buttonButtonLevelPool[i] = goButtonLevelPool[i].GetComponent<Button>();
            }

            for (int i = 0; i < goButtonUpgrade.Length; i++)
            {
                imageButtonUpgrade[i] = goButtonUpgrade[i].GetComponent<Image>();
                buttonButtonUpgrade[i] = goButtonUpgrade[i].GetComponent<Button>();
            }

            textButtonCost = new Text[goButtonLevelPool.Length - 1][];
            textButtonUpgradeCost = new Text[goButtonUpgrade.Length][];

            for (int i = 0; i < textButtonCost.Length; i++)
            {
                textButtonCost[i] = goButtonLevelPool[i + 1].GetComponentsInChildren<Text>();
            }

            for (int i = 0; i < textButtonUpgradeCost.Length; i++)
            {
                textButtonUpgradeCost[i] = goButtonUpgrade[i].GetComponentsInChildren<Text>();
            }

            SetButtonSprite();
        }

        private void SetButtonSprite()
        {
            for (int i = 1; i < imageButtonLevelPool.Length; i++)
            {
                if (i >= towerBuilder.Pool.Length)
                    return;
                if (towerBuilder.Pool[i] != 0)
                    imageButtonLevelPool[i].sprite = towerBuilder.CardTower[towerBuilder.Pool[i]].sprite;
            }
        }

        private void EnableDraggingObject(TowerCard card)
        {
            sR_DraggingObject.sprite = card.sprite;
            draggingObject.SetActive(true);
        }

        private void StartBuildButtonCooldownAnimation(GameObject go, float duration)
        {
            Animator anim = go.GetComponent<Animator>();
            Button button = go.GetComponent<Button>();
            anim.speed = 1 / duration;
            anim.SetTrigger("CooldownAnimation");
            StartCoroutine(DisableButtonForDuration(button, duration));
        }

        private IEnumerator DisableButtonForDuration(Button button, float duration)
        {
            button.enabled = false;
            yield return new WaitForSeconds(duration);
            button.enabled = true;
        }

        private void ShowTowerUpdradeWindow(GameObject clickedGO, TowerCard[] upperCards)
        {
            if (clickedGO == null)
            {
                canvasUpgrade.SetActive(false);
                return;
            }

            for (int i = 0; i < goButtonUpgrade.Length; i++)
            {
                if (upperCards[i] == null)
                {
                    goButtonUpgrade[i].SetActive(false);
                    continue;
                }
                imageButtonUpgrade[i].sprite = upperCards[i].sprite;
                goButtonUpgrade[i].SetActive(true);
                ShowCostIndicatorsForUpgradeButton(i, upperCards[i], textButtonUpgradeCost);
            }
            canvasUpgrade.SetActive(true);
            transformUpgradeButtons.position = clickedGO.transform.position;
        }

        private void ShowCostIndicatorsForUpgradeButton(int j, TowerCard card, Text[][] txt)
        {
            if (card.cost > 0)
            {
                TextUpdate(txt[j][0], card.cost.ToString());
                txt[j][0].gameObject.SetActive(true);
            }
            else
            {
                TextUpdate(txt[j][0], null);
                txt[j][0].gameObject.SetActive(false);
            }

            for (int i = 0; i < card.costElement.Length; i++)
            {
                if (card.costElement[i] > 0)
                {
                    TextUpdate(txt[j][i + 1], card.costElement[i].ToString());
                    txt[j][i + 1].gameObject.SetActive(true);
                    continue;
                }
                TextUpdate(txt[j][i + 1], null);
                txt[j][i + 1].gameObject.SetActive(false);
            }
        }

        private void UpdateTowerButtonsColor(bool[] buttons, bool[] updateButtons)
        {
            for (int i = 1; i < buttons.Length; i++)
            {
                if (buttons[i] == true)
                {
                    imageButtonLevelPool[i].color = Color.white;
                    buttonButtonLevelPool[i].enabled = true;
                    continue;
                }
                imageButtonLevelPool[i].color = Color.gray;
                buttonButtonLevelPool[i].enabled = false;
            }

            for (int i = 0; i < updateButtons.Length; i++)
            {
                if (updateButtons[i] == true)
                {
                    imageButtonUpgrade[i].color = Color.white;
                    continue;
                }
                imageButtonUpgrade[i].color = Color.gray;
            }
        }

        //private void UpdateElementButtonsColor(int[] updateButton)
        //{
        //    upgradeButtonID = updateButton;

        //    for (int i = 0; i < imageButtonUpgrade.Length; i++)
        //    {
        //        if (updateButton[i] != 0 && CheckElementsCost(updateButton[i]))
        //        {
        //            imageButtonUpgrade[i].color = Color.white;
        //            continue;
        //        }

        //        imageButtonUpgrade[i].color = Color.gray;
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

        private void TextUpdate(Text text, string str)
        {
            text.text = str;
        }
    }
}