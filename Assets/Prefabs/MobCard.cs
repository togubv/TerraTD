using UnityEngine;

[CreateAssetMenu(fileName = "Mob_Card", menuName = "New Mob Card")]
public class MobCard : ScriptableObject
{
    [SerializeField] private int _id;
    [SerializeField] private string _mob_name;
    [SerializeField] private Sprite _sprite;
    [Header("Stats")]
    [SerializeField] private float _max_hp;
    [SerializeField] private float _speed;
    [SerializeField] private float _damage;
    [SerializeField] private float _cooldown;
    [SerializeField] private int _rewardElementID;
    [SerializeField] private int _rewardCount;

    public int id => _id;
    public string mob_nameame => _mob_name;
    public Sprite sprite => _sprite;
    public float max_hp => _max_hp;
    public float speed => _speed;
    public float damage => _damage;
    public float cooldown => _cooldown;
    public int rewardElementID => _rewardElementID;
    public int rewardCount => _rewardCount;
}
