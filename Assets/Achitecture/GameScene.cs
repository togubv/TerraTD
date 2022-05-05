using System.Collections;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    protected GameObject[] goButtonDraft;
    protected GameObject[] goButtonDraftPool;
    [SerializeField] private GameObject[] goButtonLevelPool;
    [SerializeField] private GameObject[] prefabTower;
    [SerializeField] private GameObject[] prefabMob;
    [SerializeField] private TowerCard[] cardTower;
    [SerializeField] private MobCard[] cardMob;
    [SerializeField] private GameObject[] goCell;

    public GameObject[] GoButtonLevelPool => goButtonLevelPool;
    public GameObject[] PrefabTower => prefabTower;
    public GameObject[] PrefabMob => prefabMob;
    public TowerCard[] CardTower => cardTower;
    public MobCard[] CardMob => cardMob;
    public GameObject[] GoCell => goCell;
}