using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    [SerializeField] private float maxHitPoints = 100f;
    private Entity _entity;
    private float _hitPoints;
    // Start is called before the first frame update
    void Start()
    {
        _hitPoints = maxHitPoints;
        _entity = GetComponent<Entity>(); // enforces the need of Entity at same level as Creature
    }

    private void FixedUpdate()
    {
        if (_hitPoints <= 0)
        {
            _entity.Die();
        }
    }

    public void ApplyDamage(float damage)
    {
        if(_hitPoints - damage > 0)
        {
            _hitPoints -= damage;
        } else
        {
            _hitPoints = 0;
        }
        if(_entity != null) _entity.TakeDamage();
    }
    
}
