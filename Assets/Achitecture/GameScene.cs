using System.Collections;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    [SerializeField] private GameObject[] goButtonLevelPool;
    [SerializeField] private GameObject[] prefabTower;
    [SerializeField] private GameObject[] prefabMob;
    [SerializeField] private ListTowerCard listTowerCard;
    [SerializeField] private MobCard[] cardMob;
    [SerializeField] private GameObject[] goCell;

    public GameObject[] GoButtonLevelPool => goButtonLevelPool;
    public GameObject[] PrefabTower => prefabTower;
    public GameObject[] PrefabMob => prefabMob;
    public ListTowerCard ListTowerCard => listTowerCard;
    public MobCard[] CardMob => cardMob;
    public GameObject[] GoCell => goCell;
}