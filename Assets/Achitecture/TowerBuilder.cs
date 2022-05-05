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
    private TowerCard[] cardTower;
    [SerializeField] private int[] pool;
    private int[] grid;
    private GameObject[] goCell;

    private int draggingTowerID;
    private bool isDrag;
    private GameObject pressedButton;

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
    }

    private void OnEnable()
    {
        //pool = levelmanager.Pool;
        //prefabTower = levelmanager.Tower_prefab_list;
        //sprite_list = new Sprite[prefabTower.Length + 1];

        //for (int i = 1; i < sprite_list.Length; i++)
        //{
        //    sprite_list[i] = prefabTower[i - 1].GetComponent<TowerConfuguration>().Card.sprite;
        //}
    }

    private void Update()
    {
        if (isDrag)
        {
            Vector2 cursor = Camera.main.ScreenToWorldPoint(Input.mousePosition);          
            RaycastHit2D hit = Physics2D.Raycast(cursor, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject.CompareTag("CellNone"))
            {
                dragging_go.transform.position = hit.collider.transform.position;

                if (Input.GetMouseButtonDown(0))
                {
                    int id = TakegoCell(hit.collider.gameObject);
                    if (grid[id] == 0)
                    {
                        SetNewTower(draggingTowerID, hit.collider.gameObject, id);
                        isDrag = false;
                    }
                }
            }
            else
            {
                dragging_go.transform.position = cursor;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            isDrag = false;
        }
    }

    public void StartTowerDragging(GameObject go)
    {
        int i = TakeButtonNumber(go);
        if (pool[i] != 0 && cardTower[pool[i] - 1].cost <= bank.count)
        {
            pressedButton = go;
            draggingTowerID = pool[i];
            StartCoroutine(DragTower());
        }
        else
        {
            BreakDrag();
            Debug.Log("Gold is not enough");
        }
    }

        private int TakeButtonNumber(GameObject go)
    {
        for (int i = 0; i < goButtonLevelPool.Length; i++)
        {
            if (go == goButtonLevelPool[i]) return i;
        }
        Debug.Log("NOT FOUND BUTTON");
        return -1;
    }

    private IEnumerator DragTower()
    {
        StartDrag();
        yield return new WaitUntil(() => isDrag == false);
        BreakDrag();
    }

    private void StartDrag()
    {
        dragging_go.SetActive(true);
        isDrag = true;
        Debug.Log("DRAG");
    }

    private void BreakDrag()
    {
        dragging_go.SetActive(false);
        draggingTowerID = 0;
        pressedButton = null;
        Debug.Log("BREAK DRAG");
    }

    private void SetNewTower(int tower_id, GameObject go, int cell_id)
    {
        grid[cell_id] = tower_id;
        GameObject new_tower = Instantiate(prefabTower[tower_id - 1], go.transform.position, Quaternion.identity, transform);
        new_tower.GetComponent<TowerConfuguration>().SetNumber(cell_id);
        bank.DecreaseBank(this, prefabTower[tower_id - 1].GetComponent<TowerConfuguration>().Card.cost);

        StartTowerCooldownEvent?.Invoke(pressedButton, prefabTower[tower_id - 1].GetComponent<TowerConfuguration>().Card.build_cooldown);
        if (prefabTower[tower_id - 1].GetComponent<TowerConfuguration>().Card.income > 0) bank.IncreaseIncome(this, 1);
    }

    private int TakegoCell(GameObject go)
    {
        for (int i = 0; i < goCell.Length; i++)
        {
            if (go == goCell[i]) return i;
        }
        Debug.Log("NOT FOUND GO NUMBER IN ARRAY");
        isDrag = false;
        return -1;
    }

    public void RemoveTower(int number)
    {
        grid[number] = 0;
    }
}
