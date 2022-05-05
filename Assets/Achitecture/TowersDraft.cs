using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TowersDraft : GameScene
{
    //[Header("[Need drag all children Grid_Draft objects]")]
    //[SerializeField] private GameObject[] button_draft;

    //[Header("[Need drag all children Grid_Pool objects]")]
    //[SerializeField] private GameObject[] button_pool;

    //[Header("[Need drag all towers prefabs]")]  
    //[SerializeField] private GameObject[] towers_list;

    protected Button[] buttonDraft, buttonPool;
    protected int draftSize, poolSize;
    protected int[] Pool => pool;
    protected int[] Draft => draft;

    private int[] pool;
    private int[] draft;

    //public GameObject[] Towers_list => towers_list;
    //public GameObject[] Button_draft => button_draft;
    //public GameObject[] Button_pool => button_pool;

    private void Awake()
    {
        //draftSize = goButtonDraft.Length;
        //poolSize = goButtonDraftPool.Length;
        //draft = new int[draftSize];
        //pool = new int[poolSize];
        //buttonDraft = new Button[draftSize];
        //buttonPool = new Button[poolSize];

        //for (int i = 0; i < draftSize; i++)
        //{
        //    buttonDraft[i] = goButtonDraft[i].GetComponent<Button>();
        //}

        //for (int i = 0; i < poolSize; i++)
        //{
        //    buttonPool[i] = goButtonDraftPool[i].GetComponent<Button>();
        //}

        //SetDraftList();
    }

    //private void SetDraftList()
    //{
    //    if (draftSize != prefabTower.Length)
    //    {
    //        Debug.Log("BUTTONDRAFT NOT EQUAL TO TOWERSLIST");
    //        return;
    //    }

    //    for (int i = 0; i < draftSize; i++)
    //    {
    //        draft[i] = towers_list[i].GetComponent<TowerConfuguration>().Card.id;
    //    }
    //}

    public void PickTower(GameObject go)
    {
        int id = GetButtonDraftID(go);
        for (int i = 0; i < poolSize; i++)
        {
            if (pool[i] == 0)
            {
                draft[id] = 0;
                pool[i] = id + 1;
                //buttonDraft[id].enabled = false;
                return;
            }
        }
        Debug.Log("FULL POOL");
    }

    public void DismissTower(GameObject go)
    {
        int id = GetButtonPoolID(go);
        if (pool[id] != 0)
        {
            draft[pool[id] - 1] = pool[id];
            pool[id] = 0;
            SortPoolButtons();
            return;
        }
    }

    private void SortPoolButtons()
    {
        int swapper;
        for (int i = 0; i < pool.Length - 1; i++)
        {
            if (pool[i] == 0 && pool[i + 1] != 0)
            {
                for (int j = i; j < pool.Length - 1; j++)
                {
                    swapper = pool[j];
                    pool[j] = pool[j + 1];
                    pool[j + 1] = swapper;
                }
            }
        }
        Debug.Log("SORTED");
    }

    private int GetButtonDraftID(GameObject go)
    {
        for (int i = 0; i < draftSize; i++)
        {
            if (go == goButtonDraft[i]) return i;
        }
        return -1;
    }

    private int GetButtonPoolID(GameObject go)
    {
        for (int i = 0; i < poolSize; i++)
        {
            if (go == goButtonDraftPool[i]) return i;
        }
        return -1;
    }
}