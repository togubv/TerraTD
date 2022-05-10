using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
    
public class TowerBuilder : MonoBehaviour
{
    [SerializeField] private GameScene gameScene;
    [SerializeField] private Bank bank;
    [Header("Game objects")]
    [SerializeField] private GameObject dragging_go;

    private GameObject[] goButtonLevelPool;
    private GameObject[] goUpgradeButton;
    private GameObject[] prefabTower;
    [SerializeField] private TowerCard[] cardTower;
    [SerializeField] private int[] pool;
    [SerializeField] private int[] grid;
    private GameObject[] goCell;

    private int draggingTowerID;
    [SerializeField] private int neededElementTowerID;
    [SerializeField] private int targetedCell;
    private bool isDragTower;
    private bool isUpgrade;
    private GameObject pressedButton;
    [SerializeField] private GameObject[] builtTower;

    public delegate void StartTowerCooldown(GameObject go, float duration);
    public event StartTowerCooldown StartTowerCooldownEvent;

    public delegate void ClickToBuiltTowerHandler(GameObject go, int[] countUpgrades);
    public event ClickToBuiltTowerHandler ClickToBuiltTowerHandlerEvent;

    public GameObject[] GoButtonLevelPool => goButtonLevelPool;
    public TowerCard[] CardTower => cardTower;
    public int[] Pool => pool;

    private void Awake()
    {
        goCell = gameScene.GoCell;
        grid = new int[goCell.Length];
        goButtonLevelPool = gameScene.GoButtonLevelPool;
        goUpgradeButton = gameScene.GoUpgradeButton;
        prefabTower = gameScene.PrefabTower;
        cardTower = gameScene.CardTower;
        builtTower = new GameObject[grid.Length];   
    }

    private void Update()
    {
        if (!isDragTower && !isUpgrade && Input.GetMouseButtonDown(0))
        {
            Vector2 cursor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(cursor, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject.CompareTag("CellNone") && grid[TakegoCell(hit.collider.gameObject)] != 0)
            {
                isUpgrade = true;
                targetedCell = TakegoCell(hit.collider.gameObject);
                ClickToBuiltTowerHandlerEvent?.Invoke(hit.collider.gameObject, TakeCountOfUpgradesTower(hit.collider.gameObject));
            }
            else
            {
                HideUpgradeWindow();
            }
        }

        if (isDragTower)
        {
            HideUpgradeWindow();
            IsDragTower();
        }

        if (Input.GetMouseButtonDown(1))
        {
            isDragTower = false;
            HideUpgradeWindow();
        }
    }

    private void IsDragTower()
    {
        Vector2 cursor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(cursor, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject.CompareTag("CellNone") && grid[TakegoCell(hit.collider.gameObject)] == 0)
        {
            dragging_go.transform.position = hit.collider.transform.position;
            if (Input.GetMouseButtonDown(0))
            {
                int id = TakegoCell(hit.collider.gameObject);
                if (grid[id] == 0)
                {
                    SetNewTower(draggingTowerID, id);
                    isDragTower = false;
                }
            }
        }
        else
        {
            dragging_go.transform.position = cursor;
            return;
        }
    }

    public void StartDragging(GameObject go)
    {
        int buttonID = TakeButtonNumber(go);
        Debug.Log("buttonID: " + buttonID);
        if (pool[buttonID] == 0) return;
        pressedButton = go;
        TowerType type = cardTower[pool[buttonID]].type;
        StartTowerDragging(buttonID);
    }

    public void StartTowerDragging(int buttonID)
    {
        if (cardTower[pool[buttonID]].cost <= bank.count)
        {
            draggingTowerID = pool[buttonID];
            StartCoroutine(DragTower());
        }
        else
        {
            BreakDrag();
            Debug.Log("Gold is not enough");
        }
    }

    private IEnumerator DragTower()
    {
        StartDrag();
        yield return new WaitUntil(() => isDragTower == false);
        BreakDrag();
    }

    private void StartDrag()
    {
        dragging_go.SetActive(true);
        isDragTower = true;
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

        StartTowerCooldownEvent?.Invoke(pressedButton, prefabTower[tower_id].GetComponent<TowerConfuguration>().Card.build_cooldown);
        if (prefabTower[tower_id].GetComponent<TowerConfuguration>().Card.income > 0) bank.IncreaseIncome(this, 1);
    }

    private int TakegoCell(GameObject go)
    {
        for (int i = 0; i < goCell.Length; i++)
        {
            if (go == goCell[i]) return i;
        }
        Debug.Log("NOT FOUND GO NUMBER IN ARRAY");
        isDragTower = false;
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

    private void RemoveTowerFromGame(int cellID, int cashBack)
    {
        Destroy(builtTower[cellID]);
        RemoveTowerFromGrid(cellID);
        if (cashBack > 0) bank.IncreaseBank(this, cashBack);
    }

    public void RemoveTowerFromGrid(int cellID)
    {
        grid[cellID] = 0;
        builtTower[cellID] = null;
    }

    private void HideUpgradeWindow()
    {
        isUpgrade = false;
        targetedCell = 0;
        ClickToBuiltTowerHandlerEvent?.Invoke(null, null);
    }

    private int[] TakeCountOfUpgradesTower(GameObject go)
    {
        return cardTower[grid[TakegoCell(go)]].upperTower;
    }

    public void UpgradeCurrentTower(int buttonID)
    {
        TowerCard card = cardTower[grid[targetedCell]];

        if (CheckAndDecreaseElementCostAndCount(card, buttonID))
        {
            int towerID = grid[targetedCell];
            Debug.Log("towerID: " + towerID);
            RemoveTowerFromGame(targetedCell, 0);
            int upgradeID = cardTower[towerID].upperTower[buttonID];
            Debug.Log("upgradeID: " + upgradeID);
            SetNewTower(upgradeID, targetedCell);
            HideUpgradeWindow();
        }
        else
        {
            Debug.Log("Element is not enough");
        }
    }

    private bool CheckAndDecreaseElementCostAndCount(TowerCard card, int elementID)
    {
        TowerCard upperCard = cardTower[card.upperTower[elementID]];
        Debug.Log("upperCard: " + upperCard.name);
        switch (elementID)
        {
            case 0:
                if (bank.countFire > 0 && upperCard.costFire <= bank.countFire)
                {
                    bank.DecreaseElementCount(this, elementID, upperCard.costFire);
                    return true;
                }
                else
                    return false;
            case 1:
                if (bank.countWater > 0 && upperCard.costWater <= bank.countWater)
                {
                    bank.DecreaseElementCount(this, elementID, upperCard.costWater);
                    return true;
                }
                else
                    return false;
            case 2:
                if (bank.countEarth > 0 && upperCard.costEarth <= bank.countEarth)
                {
                    bank.DecreaseElementCount(this, elementID, upperCard.costEarth);
                    return true;
                }
                else
                    return false;
            case 3:
                if (bank.countAir > 0 && upperCard.costAir <= bank.countAir)
                {
                    bank.DecreaseElementCount(this, elementID, upperCard.costAir);
                    return true;
                }
                else
                    return false;
        }
        return false;
    }
}
