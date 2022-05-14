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
    [SerializeField] private int[] pool;

    public GameObject[] GoButtonLevelPool => goButtonLevelPool;
    public TowerCard[] CardTower => cardTower;
    public int[] Pool => pool;

    private BuildBehaviour buildBehaviour;
    private GameObject[] goButtonLevelPool;
    private GameObject[] prefabTower;
    private TowerCard[] cardTower;
    private int[] grid;
    private GameObject[] goCell;
    private int draggingTowerID;
    private int targetedCell;
    private GameObject pressedButton;
    [SerializeField] private GameObject[] builtTower;

    public delegate void StartTowerCooldown(GameObject go, float duration);
    public event StartTowerCooldown StartTowerCooldownEvent;
    public delegate void ClickToBuiltTowerHandler(GameObject go, int[] countUpgrades);
    public event ClickToBuiltTowerHandler ClickToBuiltTowerHandlerEvent;

    private void Awake()
    {
        goCell = gameScene.GoCell;
        grid = new int[goCell.Length];
        goButtonLevelPool = gameScene.GoButtonLevelPool;
        prefabTower = gameScene.PrefabTower;
        cardTower = gameScene.CardTower;
        builtTower = new GameObject[grid.Length];
        SwitchBuilderBehaivour(BuildBehaviour.Game);
    }

    private void Update()
    {
        Debug.Log(buildBehaviour.ToString());
        if (Input.GetMouseButtonDown(0))
        {
            switch(((int)buildBehaviour))
            {
                case 0:
                    ClickToTower();
                    //Debug.Log("GAME");
                    break;
                case 1:
                    ClickToCellForBuild();
                    //Debug.Log("BUILD");
                    break;
                case 2:
                    //Debug.Log("UPGRADE");
                    break;
                case 3:
                    RemoveClickedTower();
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
        Vector2 cursor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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

    private void ClickToTower()
    {
        Vector2 cursor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(cursor, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject.CompareTag("CellNone") && grid[TakegoCell(hit.collider.gameObject)] != 0)
        {
            SwitchBuilderBehaivour(BuildBehaviour.Upgrade);
            targetedCell = TakegoCell(hit.collider.gameObject);
            ClickToBuiltTowerHandlerEvent?.Invoke(hit.collider.gameObject, TakeCountOfUpgradesTower(hit.collider.gameObject));
        }
        else
        {
            HideUpgradeWindow();
        }
    }

    private void ClickToCellForBuild()
    {
        Vector2 cursor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(cursor, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject.CompareTag("CellNone"))
        {
            int id = TakegoCell(hit.collider.gameObject);
            if (grid[id] == 0)
            {
                SetNewTower(draggingTowerID, id);
                buildBehaviour = BuildBehaviour.Game;
                SwitchBuilderBehaivour(BuildBehaviour.Game);
            }
        }
        
    }

    private void RemoveClickedTower()
    {
        Vector2 cursor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(cursor, Vector2.zero);

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
        Debug.Log("buttonID: " + buttonID);
        if (pool[buttonID] == 0) return;
        pressedButton = go;
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

        StartTowerCooldownEvent?.Invoke(pressedButton, prefabTower[tower_id].GetComponent<TowerConfuguration>().Card.build_cooldown);
        if (prefabTower[tower_id].GetComponent<TowerConfuguration>().Card.income > 0) bank.IncreaseIncome(this, cardTower[tower_id].income);
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
        targetedCell = 0;
        ClickToBuiltTowerHandlerEvent?.Invoke(null, null);
        SwitchBuilderBehaivour(BuildBehaviour.Game);
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
