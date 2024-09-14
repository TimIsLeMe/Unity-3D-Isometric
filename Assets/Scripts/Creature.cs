using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    [SerializeField] private float maxHitPoints = 100f;
    [SerializeField] private Entity entity;
    private float _hitPoints;
    // Start is called before the first frame update
    void Start()
    {
        _hitPoints = maxHitPoints;
    }

    private void FixedUpdate()
    {
        if (_hitPoints < 0)
        {
            entity.Die();
        }
    }

    public void ApplyDamage(float damage)
    {
        if(_hitPoints - damage > 0)
        {
            _hitPoints -= damage;
            entity.TakeDamage();
        } else
        {
            _hitPoints = 0;
        }
    }
    
}
