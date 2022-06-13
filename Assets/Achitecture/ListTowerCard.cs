using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "ListTowerCardSO", menuName = "Create List Tower Card SO")]
public class ListTowerCard : ScriptableObject
{   
    [SerializeField] private TowerCard[] towerCard;

    public TowerCard[] TowerCard => towerCard;
}