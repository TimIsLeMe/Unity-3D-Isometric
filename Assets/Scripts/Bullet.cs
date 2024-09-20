using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody _rigidbody;
    [SerializeField] private float timeToLive = 5f;
    [SerializeField] private float baseSpeed = 10f;
    [SerializeField] private float baseDamage = 50f;
    [SerializeField] private float offset = 3f;
    private float _speed;
    private float _damage;
    private Vector3[] offsetDict = new Vector3[]
    {
        new Vector3(0.75f, 0, 0),
        new Vector3(-0.75f, 0, 0),
        new Vector3(1.25f, 0.25f, 0),
        new Vector3(-1.25f, 0.25f, 0),
        new Vector3(0.75f, 0.5f, 0),
        new Vector3(-0.75f, 0.5f, 0),
        new Vector3(-1.25f, 0.25f, 0),
        new Vector3(1.25f, 0.25f, 0)
    };
    private int _collisionMaxCount = 1;
    private bool _isChild = false;
    private BulletEffect _effect;
    private GameObject _parent;
    private PlayerCharacter _playerParent;
    public Vector3 initialPlayerVelocity { get; set; }
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Awake()
    {
        Destroy(gameObject, timeToLive);
    }

    public void InitEffects(GameObject parent)
    {
        _parent = parent;
        _playerParent = _parent.GetComponent<PlayerCharacter>();
        _speed = baseSpeed;
        _damage = baseDamage;
        if (_effect != null)
        {
            transform.localScale = _effect.Scale;
            _speed = baseSpeed * _effect.SpeeModifier;
            _damage = baseDamage * _effect.DamageModifier;
            _collisionMaxCount += _effect.AdditionalCollisionMaxCount;
            if (!_isChild)
            {
                for (int i = 0; i < _effect.AdditionalBulletCount; i++)
                {
                    SpawnChildBullet(i);
                }
            }
        }
    }

    public void SpawnChildBullet(int index)
    {
        Quaternion direction = Quaternion.LookRotation(transform.forward);
        Vector3 bulletOffset = offsetDict[index % offsetDict.Length] * offset;
        GameObject bulletObj = Instantiate(gameObject, transform.position + direction * bulletOffset, direction);
        Bullet newBullet = bulletObj.GetComponent<Bullet>();
        newBullet.SetIsChild(true);
        newBullet.SetBulletEffect(_effect);
        newBullet.InitEffects(gameObject);
    }

    public void SetIsChild(bool b)
    {
        _isChild = b;
    }

    public void SetBulletEffect(BulletEffect effect)
    {
        _effect = effect;
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = transform.forward * _speed; // + initialPlayerVelocity; // player velocity is (gradually less) applied for 10 ticks
        // if (initialPlayerVelocity.magnitude > 0.1f) initialPlayerVelocity -= initialPlayerVelocity / 10;
    }

    private void OnTriggerEnter(Collider other)
    {
        Creature creature = other.gameObject.GetComponent<Creature>();
        if (creature != null && creature.GetComponent<PlayerCharacter>() == null)
        {
            if (_playerParent != null && creature.WillDie(_damage)) _playerParent.Experience += creature.ExpirienceDrop; // xp gain
            creature.ApplyDamage(_damage);
            if (--_collisionMaxCount <= 0)
            {
                Destroy(gameObject);
            }
        } else if (other.gameObject.GetComponent<Creature>() != null)
        {

        } else
        {
            Destroy(gameObject);
        }
    }
}
