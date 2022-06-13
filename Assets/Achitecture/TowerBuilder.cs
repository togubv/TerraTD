using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
    
public class TowerBuilder : MonoBehaviour
{
    [SerializeField] private GameScene gameScene;
    [SerializeField] private Bank bank;
    [Header("Game objects")]
    [SerializeField] private GameObject dragging_go;
    [SerializeField] private int[] pool;

    public GameObject[] GoButtonLevelPool => goButtonLevelPool;
    public TowerCard[] CardTower => cardTower;
    public int[] Pool => pool;

    private BuildBehaviour buildBehaviour;
    private GameObject[] goButtonLevelPool;
    private GameObject[] prefabTower;
    [SerializeField] private TowerCard[] cardTower;
    private int[] grid;
    private GameObject[] goCell;
    private int draggingTowerID;
    private int targetedCell;
    private GameObject pressedButton;
    private Camera mainCamera;
    private bool[][] isButtonEnabled;
    [SerializeField] private GameObject[] builtTower;

    public delegate void StartTowerCooldown(GameObject go, float duration);
    public event StartTowerCooldown StartTowerCooldownEvent;
    public delegate void ClickToBuiltTowerHandler(GameObject go, TowerCard[] upperCards);
    public event ClickToBuiltTowerHandler ClickToBuiltTowerHandlerEvent;
    public delegate void StartDraggingTowerHandler(TowerCard card);
    public event StartDraggingTowerHandler StartDraggingTowerHandlerEvent;
    public delegate void UpdateButtonCostBehaviour(bool[] buttons, bool[] updateButton);
    public event UpdateButtonCostBehaviour UpdateButtonCostBehaviourEvent;

    private void Awake()
    {
        mainCamera = Camera.main;
        goCell = gameScene.GoCell;
        grid = new int[goCell.Length];
        goButtonLevelPool = gameScene.GoButtonLevelPool;
        prefabTower = gameScene.PrefabTower;
        cardTower = gameScene.ListTowerCard.TowerCard;
        builtTower = new GameObject[grid.Length];
        isButtonEnabled = new bool[2][];
        isButtonEnabled[0] = new bool[goButtonLevelPool.Length];
        isButtonEnabled[1] = new bool[4];
        bank.BankUpdateHandlerEvent += UpdateEnabledButtonsBank;
        bank.ElementCountUpdateHandlerEvent += UpdateEnabledButtonsElement;
        SwitchBuilderBehaivour(BuildBehaviour.Game);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 cursor = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(cursor, Vector2.zero);

            switch (((int)buildBehaviour))
            {
                case 0:
                    ClickToTower(hit);
                    //Debug.Log("GAME");
                    break;
                case 1:
                    ClickToCellForBuild(hit);
                    //Debug.Log("BUILD");
                    break;
                case 2:
                    if (EventSystem.current.IsPointerOverGameObject() == false) HideUpgradeWindow();
                    //Debug.Log("UPGRADE");
                    break;
                case 3:
                    RemoveClickedTower(hit);
                    //Debug.Log("REMOVE");
                    break;
            }
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            BackToGameBehaviour();
        }

        if (buildBehaviour == BuildBehaviour.Build)
        {
            IsDragTower();
        }        
    }

    private void IsDragTower()
    {
        Vector2 cursor = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(cursor, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject.CompareTag("CellNone") && grid[TakegoCell(hit.collider.gameObject)] == 0)
        {
            dragging_go.transform.position = hit.collider.transform.position;
        }
        else
        {
            dragging_go.transform.position = cursor;
        }
    }

    private void BackToGameBehaviour()
    {
        buildBehaviour = BuildBehaviour.Game;
        HideUpgradeWindow();
    }

    private void ClickToTower(RaycastHit2D hit)
    {
        if (hit.collider != null && hit.collider.gameObject.CompareTag("CellNone") && grid[TakegoCell(hit.collider.gameObject)] != 0)
        {
            SwitchBuilderBehaivour(BuildBehaviour.Upgrade);
            targetedCell = TakegoCell(hit.collider.gameObject);
            TowerCard card = cardTower[grid[targetedCell]];
            TowerCard[] upperCards = new TowerCard[card.upperTower.Length];
            int summUpperCards = 0;
            for (int i = 0; i < upperCards.Length; i++)
            {
                if (card.upperTower[i] != 0)
                {
                    upperCards[i] = cardTower[card.upperTower[i]];
                    summUpperCards += 1;
                }
            }
            if (summUpperCards < 1)
            {
                HideUpgradeWindow();
                return;
            }
            ClickToBuiltTowerHandlerEvent?.Invoke(hit.collider.gameObject, upperCards);
            return;
        }
        HideUpgradeWindow();
    }

    private void ClickToCellForBuild(RaycastHit2D hit)
    {
        if (hit.collider != null && hit.collider.gameObject.CompareTag("CellNone"))
        {
            int id = TakegoCell(hit.collider.gameObject);
            if (grid[id] == 0)
            {
                SetNewTower(draggingTowerID, id);
                StartTowerCooldownEvent?.Invoke(pressedButton, prefabTower[grid[id]].GetComponent<TowerConfuguration>().Card.build_cooldown);
                buildBehaviour = BuildBehaviour.Game;
                SwitchBuilderBehaivour(BuildBehaviour.Game);
            }
        }
        BreakDrag();
    }

    private void RemoveClickedTower(RaycastHit2D hit)
    {
        if (hit.collider != null && hit.collider.gameObject.CompareTag("CellNone"))
        {
            int id = TakegoCell(hit.collider.gameObject);
            if (grid[id] != 0)
            {
                RemoveTowerFromGame(id, 0);
            }
        }
        SwitchBuilderBehaivour(BuildBehaviour.Game);
    }

    public void StartRemovingTower()
    {
        if (buildBehaviour == BuildBehaviour.Remove) SwitchBuilderBehaivour(BuildBehaviour.Game);
        else SwitchBuilderBehaivour(BuildBehaviour.Remove);
    }

    public void StartDragging(GameObject go)
    {
        int buttonID = TakeButtonNumber(go);

        if (pool[buttonID] == 0) 
            return;
        
        pressedButton = go;
        StartTowerDragging(buttonID);
        StartDraggingTowerHandlerEvent?.Invoke(cardTower[pool[buttonID]]);
    }

    public void StartTowerDragging(int buttonID)
    {
        TowerCard card = cardTower[pool[buttonID]];
        
        if (CompareForTowerCostAndBank(card, true))
        {
            draggingTowerID = pool[buttonID];
            StartCoroutine(DragTower());
            return;
        }

        BreakDrag();
    }

    private bool CompareForTowerCostAndBank(TowerCard card, bool debugToggle)
    {
        switch(card.requiredResources)
        {
            case RequiredResources.Gold:
                if (CompareGold())
                    return true;
                return false;

            case RequiredResources.Element:
                if (CompareElement())
                    return true;
                return false;

            case RequiredResources.All:
                if (CompareGold() && CompareElement())
                    return true;
                return false;

            default:
                return true;
        }
        
        bool CompareGold()
        {
            if (card.cost > bank.count)
            {
                if (debugToggle)
                    Debug.Log("Gold is not enough");
                return false;
            }
            return true;
        }

        bool CompareElement()
        {
            for (int i = 0; i < card.costElement.Length; i++)
            {
                if (card.costElement[i] > bank.countElement[i])
                {
                    if (debugToggle)
                        Debug.Log("Element is not enough");
                    return false;
                }
            }
            return true;
        }
    }

    private IEnumerator DragTower()
    {
        StartDrag();
        yield return new WaitUntil(() => buildBehaviour != BuildBehaviour.Build);
        BreakDrag();
    }

    private void StartDrag()
    {
        HideUpgradeWindow();
        dragging_go.SetActive(true);
        buildBehaviour = BuildBehaviour.Build;
        SwitchBuilderBehaivour(BuildBehaviour.Build);
    }

    private void BreakDrag()
    {
        dragging_go.SetActive(false);
        draggingTowerID = 0;
        pressedButton = null;
    }

    private void SetNewTower(int tower_id, int cell_id)
    {
        grid[cell_id] = tower_id;
        GameObject new_tower = Instantiate(prefabTower[tower_id], goCell[cell_id].transform.position, Quaternion.identity, transform);
        new_tower.GetComponent<TowerConfuguration>().SetNumber(cell_id);
        bank.DecreaseBank(this, prefabTower[tower_id].GetComponent<TowerConfuguration>().Card.cost);
        builtTower[cell_id] = new_tower;

        if (prefabTower[tower_id].GetComponent<TowerConfuguration>().Card.income > 0) 
            bank.IncreaseIncome(this, cardTower[tower_id].income);
    }

    private int TakegoCell(GameObject go)
    {
        for (int i = 0; i < goCell.Length; i++)
        {
            if (go == goCell[i]) return i;
        }
        Debug.Log("NOT FOUND GO NUMBER IN ARRAY");
        buildBehaviour = BuildBehaviour.Game;
        return 0;
    }    

    private int TakeButtonNumber(GameObject go)
    {
        for (int i = 1; i < goButtonLevelPool.Length; i++)
        {
            if (go == goButtonLevelPool[i]) return i;
        }
        Debug.Log("NOT FOUND BUTTON");
        return 0;
    }

    public void RemoveTowerFromGame(int cellID, int cashBack)
    {
        TowerCard card = cardTower[grid[cellID]];
        Debug.Log("Card name: " + card.name);
        Destroy(builtTower[cellID]);
        RemoveTowerFromGrid(cellID);
        if (card.income > 0) bank.DecreaseIncome(this, card.income);
        if (cashBack > 0) bank.IncreaseBank(this, cashBack);
    }

    private void RemoveTowerFromGrid(int cellID)
    {
        grid[cellID] = 0;
        builtTower[cellID] = null;
    }

    private void HideUpgradeWindow()
    { 
        targetedCell = 0;
        ClickToBuiltTowerHandlerEvent?.Invoke(null, null);
        SwitchBuilderBehaivour(BuildBehaviour.Game);
    }

    public void UpgradeCurrentTower(int buttonID)
    {
        TowerCard card = cardTower[grid[targetedCell]];

        if (DecreaseCountElementOrGoldInBank(card, buttonID))
        {
            int towerID = grid[targetedCell];
            Debug.Log("towerID: " + towerID);
            RemoveTowerFromGame(targetedCell, 0);
            int upgradeID = cardTower[towerID].upperTower[buttonID];
            Debug.Log("upgradeID: " + upgradeID);
            SetNewTower(upgradeID, targetedCell);
            HideUpgradeWindow();
        }
    }

    private bool DecreaseCountElementOrGoldInBank(TowerCard card, int elementID)
    {
        TowerCard upperCard = cardTower[card.upperTower[elementID]];

        switch (upperCard.requiredResources)
        {
            case RequiredResources.Gold:
                if (DecreaseGold())
                    return true;
                return false;

            case RequiredResources.Element:
                if (DecreaseElements())
                    return true;
                return false;

            case RequiredResources.All:
                if (DecreaseGold() && DecreaseElements())
                    return true;
                return false;

            default:
                return true;
        }

        bool DecreaseGold()
        {
            if (CompareForTowerCostAndBank(upperCard, true))
            {
                bank.DecreaseBank(this, upperCard.cost);
                return true;
            }
            return false;
        }

        bool DecreaseElements()
        {
            if (CompareForTowerCostAndBank(upperCard, true))
            {
                for (int i = 0; i < upperCard.costElement.Length; i++)
                {
                    if (upperCard.costElement[i] > 0) bank.DecreaseElementCount(this, i, upperCard.costElement[i]);
                }
                return true;
            }
            return false;
        }
    }

    private void UpdateEnabledButtonsBank(object sender, int oldCount, int newCount)
    {
        for (int i = 1; i < goButtonLevelPool.Length; i++)
        {
            if (i >= pool.Length)
                continue;
            if (pool[i] != 0 && CompareForTowerCostAndBank(cardTower[pool[i]], false))
            {
                isButtonEnabled[0][i] = true;
                Debug.Log($"isButtonEnabled[0][{i}] = true");
                continue;
            }
            isButtonEnabled[0][i] = false;
            Debug.Log($"isButtonEnabled[0][{i}] = false");
        }

        UpdateButtonCostBehaviourEvent?.Invoke(isButtonEnabled[0], isButtonEnabled[1]);
    }

    private void UpdateEnabledButtonsElement(object sender, int elementID, int oldCount, int newCount)
    {
        if (draggingTowerID == 0)
            return;
        TowerCard card = cardTower[draggingTowerID];
        for (int i = 0; i < isButtonEnabled.Length; i++)
        {
            if (CompareForTowerCostAndBank(cardTower[card.upperTower[i]], false))
            {
                isButtonEnabled[1][i] = true;
                Debug.Log($"isButtonEnabled[1][{i}] = true");
                continue;
            }
            isButtonEnabled[1][i] = false;
            Debug.Log($"isButtonEnabled[1][{i}] = false");
        }

        UpdateButtonCostBehaviourEvent?.Invoke(isButtonEnabled[0], isButtonEnabled[1]);
    }

    private void SwitchBuilderBehaivour(BuildBehaviour behaviour)
    {
        buildBehaviour = behaviour;
    }
}

public enum BuildBehaviour
{
    Game,
    Build,
    Upgrade,
    Remove
}
