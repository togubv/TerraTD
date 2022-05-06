using UnityEngine;

[CreateAssetMenu(fileName = "Tower_Card", menuName = "New Tower Card")]
public class TowerCard : ScriptableObject
{
    [SerializeField] private int _id;
    [SerializeField] private string _tower_name;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private GameObject _weapon_type;
    [SerializeField] private GameObject _shell;
    [Header("Build")]
    [SerializeField] private TowerType _type;
    [SerializeField] private int _elementCost;
    [SerializeField] private int _cost;
    [SerializeField] private float _build_cooldown;
    [Header("Up level")]
    [SerializeField] private int[] _upperTower = new int[4];
    [Header("Stats")]
    [SerializeField] private int _income;
    [SerializeField] private float _max_hp;
    [SerializeField] private float _damage;
    [SerializeField] private float _cooldown;
    [SerializeField] private float _shell_speed;

    public int id => _id;
    public string tower_name => _tower_name;
    public Sprite sprite => _sprite;
    public GameObject weapon_type => _weapon_type;
    public GameObject shell => _shell;
    public TowerType type => _type;
    public int elementCost => _elementCost;
    public int cost => _cost;
    public float build_cooldown => _build_cooldown;
    public int[] upperTower => _upperTower;
    public int income => _income;
    public float max_hp => _max_hp;
    public float damage => _damage;
    public float cooldown => _cooldown;
    public float shell_speed => _shell_speed;
}

public enum TowerType
{
    Base,
    Miner,
    Element
}

