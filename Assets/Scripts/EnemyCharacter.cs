using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using TreeEditor;
using System.Threading.Tasks;

public class EnemyCharacter : MonoBehaviour, Entity
{
    private Vector3 _direction;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float damage = 20f;
    private int _damageTimeoutTime = 500;
    private bool _damageTimeout = false;
    private PlayerCharacter _player;
    private CharacterController _controller;
    private Animator _animator;
    [SerializeField] public Material flashMaterial; 
    private Material originalMaterial;
    private Renderer _renderer;

    private void Start()
    {
        _player = FindObjectOfType<PlayerCharacter>();
        _controller = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
        if (_controller == null) throw new MissingComponentException("Missing main component in EnemyCharacter!");
    }

    private void Awake()
    {
        _renderer = GetComponentInChildren<Renderer>();
        originalMaterial = _renderer.material;
    }
    private void FixedUpdate()
    {
        _direction = (_player.GetLocation() - transform.position).normalized;
        _controller.Move(new Vector3(speed * _direction.x, 0f, speed * _direction.z));
        _animator.SetFloat("WalkSpeed", GetSpeed());
        if(_direction.magnitude > 0)
        {// only set rotation on direction change
            Vector3 direction3 = new Vector3(_direction.x, 0f, _direction.z); 
            _controller.transform.rotation = Quaternion.LookRotation(direction3);
        }
    }

    public float GetSpeed()
    {
        return _controller.velocity.magnitude;
    }

    public void Die()
    {
        Destroy(this.gameObject, 0f);
    }

    public void TriggerFlash(float seconds)
    {
        StartCoroutine(Flash(seconds));
    }

    IEnumerator Flash(float seconds)
    {
        _renderer.material = flashMaterial;
        yield return new WaitForSeconds(seconds);
        _renderer.material = originalMaterial;
    }
    public void TakeDamage()
    {
        TriggerFlash(0.3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        OnTrigger(other);
    }

    private void OnTriggerStay(Collider other)
    {
        OnTrigger(other);
    }

    public void OnTrigger(Collider other)
    {
        PlayerCharacter player = other.gameObject.GetComponent<PlayerCharacter>();
        if (player != null && !_damageTimeout)
        {
            player.GetComponent<Creature>().ApplyDamage(damage);
            _damageTimeout = true;
            Task.Delay(_damageTimeoutTime).ContinueWith((_) => _damageTimeout = false);
        }
    }
}
