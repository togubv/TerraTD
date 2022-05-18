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
    [Tooltip("Fire, Water, Earth, Air")]
    [SerializeField] private RequiredResources _requiredResources;
    [SerializeField] private int[] _costElement;
    [SerializeField] private int _cost;
    [SerializeField] private float _build_cooldown;
    [Header("Up level")]
    [SerializeField] private int[] _upperTower = new int[4];
    [Header("Stats")]
    [SerializeField] private int _income;
    [SerializeField] private float _max_hp;
    [SerializeField] private DamageType _damage_type;
    [SerializeField] private float _damage;
    [SerializeField] private float _cooldown;
    [SerializeField] private float _shell_speed;
    [SerializeField] private float _shell_radius;

    public int id => _id;
    public string tower_name => _tower_name;
    public Sprite sprite => _sprite;
    public GameObject weapon_type => _weapon_type;
    public GameObject shell => _shell;
    public RequiredResources requiredResources => _requiredResources;
    public int[] costElement => _costElement;
    public int cost => _cost;
    public float build_cooldown => _build_cooldown;
    public int[] upperTower => _upperTower;
    public int income => _income;
    public float max_hp => _max_hp;
    public DamageType damage_type => _damage_type;
    public float damage => _damage;
    public float cooldown => _cooldown;
    public float shell_speed => _shell_speed;
    public float shell_radius => _shell_radius;
}

public enum RequiredResources
{
    None,
    Gold,
    Element,
    All
}

public enum DamageType
{
    None,
    Fire,
    Water,
    Earth,
    Air,
    Mixed
}

