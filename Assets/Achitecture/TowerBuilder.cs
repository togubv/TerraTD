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
    private GameObject[] prefabTower;
    [SerializeField] private TowerCard[] cardTower;
    [SerializeField] private int[] pool;
    [SerializeField] private int[] grid;
    private GameObject[] goCell;

    private int draggingTowerID;
    [SerializeField] private int neededElementTowerID;
    private bool isDragTower;
    private bool isDragElement;
    private GameObject pressedButton;
    [SerializeField] private GameObject[] builtTower;

    public delegate void StartTowerCooldown(GameObject go, float duration);
    public event StartTowerCooldown StartTowerCooldownEvent;

    public GameObject[] GoButtonLevelPool => goButtonLevelPool;
    //public GameObject[] PrefabTower => prefabTower;
    public TowerCard[] CardTower => cardTower;
    public int[] Pool => pool;

    private void Awake()
    {
        goCell = gameScene.GoCell;
        grid = new int[goCell.Length];
        goButtonLevelPool = gameScene.GoButtonLevelPool;
        prefabTower = gameScene.PrefabTower;
        cardTower = gameScene.CardTower;
        builtTower = new GameObject[grid.Length];   
    }

    private void Update()
    {
        if (isDragTower)
        {
            IsDragTower();
        }

        if (isDragElement)
        {
            IsDragElement();
        }

        if (Input.GetMouseButtonDown(1))
        {
            isDragTower = false;
            isDragElement = false;
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
                    SetNewTower(draggingTowerID, hit.collider.gameObject, id);
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
    private void IsDragElement()
    {
        Vector2 cursor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(cursor, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject.CompareTag("CellNone") && grid[TakegoCell(hit.collider.gameObject)] == neededElementTowerID)
        {
            dragging_go.transform.position = hit.collider.transform.position;
            if (Input.GetMouseButtonDown(0))
            {
                int id = TakegoCell(hit.collider.gameObject);

                if (grid[id] == neededElementTowerID)
                {
                    SetElementToTower(draggingTowerID, hit.collider.gameObject, id);
                    isDragElement = false;
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
        Debug.Log(buttonID);
        if (pool[buttonID] == 0)
            return;
        pressedButton = go;
        TowerType type = cardTower[pool[buttonID]].type;
        switch (type)
        {
            case TowerType.Element:
                StartElementDragging(buttonID);
                break;
            default:
                StartTowerDragging(buttonID);
                break;
        }
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

    public void StartElementDragging(int buttonID)
    {
        int elementID = pool[buttonID];
        draggingTowerID = buttonID;
        neededElementTowerID = 2;
        switch (elementID)
        {
            case 7:
                if (cardTower[pool[buttonID]].costFire <= bank.countFire)
                {
                    StartCoroutine(DragElement());
                }
                else
                {
                    BreakElementDrag();
                    Debug.Log("Fire element is not enough");
                }
                break;
            case 8:
                if (cardTower[pool[buttonID]].costWater <= bank.countWater)
                {
                    StartCoroutine(DragElement());
                }
                else
                {
                    BreakElementDrag();
                    Debug.Log("Water element is not enough");
                }
                break;
            case 9:
                if (cardTower[pool[buttonID]].costEarth <= bank.countEarth)
                {
                    Debug.Log("pool[buttonID]: " + pool[buttonID]);
                    StartCoroutine(DragElement());
                }
                else
                {
                    BreakElementDrag();
                    Debug.Log("Earth element is not enough");
                }
                break;
            case 10:
                if (cardTower[pool[buttonID]].costAir <= bank.countAir)
                {
                    Debug.Log("pool[buttonID]: " + pool[buttonID]);
                    StartCoroutine(DragElement());
                }
                else
                {
                    BreakElementDrag();
                    Debug.Log("Air element is not enough");
                }
                break;
        }
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

    private IEnumerator DragTower()
    {
        StartDrag();
        yield return new WaitUntil(() => isDragTower == false);
        BreakDrag();
    }

    private IEnumerator DragElement()
    {
        StartElementDrag();
        yield return new WaitUntil(() => isDragElement == false);
        BreakElementDrag();
    }
    private void StartElementDrag()
    {
        dragging_go.SetActive(true);
        isDragElement = true;
    }

    private void BreakElementDrag()
    {
        dragging_go.SetActive(false);
        neededElementTowerID = 0;
        pressedButton = null;
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

    private void SetNewTower(int tower_id, GameObject go, int cell_id)
    {
        grid[cell_id] = tower_id;
        GameObject new_tower = Instantiate(prefabTower[tower_id], go.transform.position, Quaternion.identity, transform);
        new_tower.GetComponent<TowerConfuguration>().SetNumber(cell_id);
        bank.DecreaseBank(this, prefabTower[tower_id].GetComponent<TowerConfuguration>().Card.cost);
        builtTower[cell_id] = new_tower;

        StartTowerCooldownEvent?.Invoke(pressedButton, prefabTower[tower_id].GetComponent<TowerConfuguration>().Card.build_cooldown);
        if (prefabTower[tower_id].GetComponent<TowerConfuguration>().Card.income > 0) bank.IncreaseIncome(this, 1);
    }

    private void SetElementToTower(int elementID, GameObject go, int cell_id)
    {
        int replacedCellID = grid[cell_id];
        RemoveTowerFromGame(cell_id, 0);
        int towerCard = cardTower[replacedCellID].upperTower[elementID - 7];

        SetNewTower(towerCard, go, cell_id);
        int eleCount = TakeElementCostCount(towerCard);
        bank.DecreaseElementCount(this, elementID - 6, eleCount);
    }

    private int TakeElementCostCount(int id)
    {
        TowerCard towerCard = cardTower[id];
        if (towerCard.costFire > 0)
        {
            Debug.Log(towerCard.costFire);
            return towerCard.costFire;
        }
        if (towerCard.costWater > 0)
        {
            Debug.Log(towerCard.costWater);
            return towerCard.costWater;
        }
        if (towerCard.costEarth > 0)
        {
            Debug.Log(towerCard.costEarth);
            return towerCard.costEarth;
        }
        if (towerCard.costAir > 0)
        {
            Debug.Log(towerCard.costAir);
            return towerCard.costAir;
        }
        return 0;
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
}
