using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//public class DraftUI : TowersDraft
//{
//    //[SerializeField] private TowersDraft draft;

//    //private GameObject[] goButtonDraft;
//    //private GameObject[] goButtonDraftPool;

//    private Image[] imageButtonDraft;
//    private Image[] imageButtonDraftPool;
//    //private Button[] buttonDraft;
//    //private Button[] button_pool_button;
//    //private GameObject[] towers_list;
//    private Sprite[] towerSprite;

//    void Start()
//    {
//        //goButtonDraft = draft.Button_draft;
//        //goButtonDraftPool = draft.Button_pool;
//        imageButtonDraft = new Image[draftSize];
//        imageButtonDraftPool = new Image[poolSize];
//        //buttonDraft = new Button[draftSize.Length];
//        //button_pool_button = new Button[poolSize.Length];
//        //towers_list = draft.Towers_list;
//        towerSprite = new Sprite[prefabTower.Length + 1];

//        for (int i = 1; i < towerSprite.Length; i++)
//        {
//            towerSprite[i] = prefabTower[i - 1].GetComponent<TowerConfuguration>().Card.sprite;
//        }

//        for (int i = 0; i < goButtonDraft.Length; i++)
//        {
//            imageButtonDraft[i] = goButtonDraft[i].GetComponent<Image>();
//            buttonDraft[i] = goButtonDraft[i].GetComponent<Button>();
//        }

//        for (int i = 0; i < goButtonDraftPool.Length; i++)
//        {
//            imageButtonDraftPool[i] = goButtonDraftPool[i].GetComponent<Image>();
//        }

//        for (int i = 0; i < goButtonDraft.Length; i++)
//        {
//            imageButtonDraft[i].sprite = towerSprite[Draft[i]];
//        }

//        for (int i = 0; i < goButtonDraftPool.Length; i++)
//        {
//            imageButtonDraftPool[i].sprite = towerSprite[Pool[i]];
//        }
//    }

//    public void UpdateAllButton()
//    {
//        for (int i = 0; i < goButtonDraft.Length; i++)
//        {
//            imageButtonDraft[i].sprite = towerSprite[Draft[i]];
//            if (Draft[i] == 0)
//            {
//                buttonDraft[i].enabled = false;
//            }
//            else
//            {
//                buttonDraft[i].enabled = true;
//            }
//        }

//        for (int i = 0; i < goButtonDraftPool.Length; i++)
//        {
//            imageButtonDraftPool[i].sprite = towerSprite[Pool[i]];
//            if (Pool[i] == 0)
//            {
//                buttonPool[i].enabled = false;
//            }
//            else
//            {
//                buttonPool[i].enabled = true;
//            }
//        }
//    }
//}