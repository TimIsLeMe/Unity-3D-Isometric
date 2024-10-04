using UnityEngine;

public class Creature : MonoBehaviour
{
    [SerializeField] private float maxHitPoints = 100f;
    [SerializeField] private float expirienceDrop = 5f;
    public float ExpirienceDrop { get { return expirienceDrop; } }
    private Entity _entity;
    private float _hitPoints;
    private bool _invulnerable = false;
    private bool _dead = false;
    // Start is called before the first frame update
    void Start()
    {
        _hitPoints = maxHitPoints;
        _entity = GetComponent<Entity>(); // enforces the need of Entity at same level as Creature
        if (_entity == null) throw new MissingComponentException("Creature must have an Entity Component on the same level!");
    }

    private void FixedUpdate()
    {
        if(_dead) return;
        if (_hitPoints <= 0)
        {
            _entity.Die();
            _dead = true;
        }
    }

    public void ApplyDamage(float damage)
    {
        if (_invulnerable) return;
        if(_hitPoints - damage > 0)
        {
            _hitPoints -= damage;
        } else
        {
            _hitPoints = 0;
        }
        if(_entity != null) _entity.TakeDamage();
    }
    
    public bool WillDie(float damage)
    {
        return _hitPoints - damage <= 0 && !_invulnerable;
    }

    public void SetInvulnerable(bool inv)
    {
        _invulnerable = inv;
    }

}
