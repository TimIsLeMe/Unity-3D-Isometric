using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using TreeEditor;
using System.Threading.Tasks;

public class EnemyCharacter : MonoBehaviour, Entity
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private float damage = 20f;
    [SerializeField] public Material flashMaterial;
    [SerializeField] private int DamageTimeoutTime = 500; // ms
    [SerializeField] private AudioClip DeathSound;
    private Vector3 _direction;
    private bool _damageTimeout = false;
    private PlayerCharacter _player;
    private CharacterController _controller;
    private Animator _animator;
    private AudioSource _audioSource;
    private Material originalMaterial;
    private Renderer _renderer;
    private Vector3 _velocity; // current velocity including gravity
    private float _gravity = -15f;
    private float _groundCheckDistance = 0.6f;

    private void Start()
    {
        _player = FindObjectOfType<PlayerCharacter>();
        _controller = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Awake()
    {
        _renderer = GetComponentInChildren<Renderer>();
        originalMaterial = _renderer.material;
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource != null)
        {
            _audioSource.PlayOneShot(_audioSource.clip);
        }
    }
    private void FixedUpdate()
    {
        bool isGrounded = Physics.CheckSphere(transform.position + Vector3.down * _groundCheckDistance, _groundCheckDistance, LayerMask.GetMask("Ground"));

        if (isGrounded)
        {
            _velocity.y = 0;
        }
        else
        {
            _velocity.y += _gravity * Time.deltaTime;
        }
        _direction = (_player.GetLocation() - transform.position).normalized;
        _controller.Move(new Vector3(speed * _direction.x, _velocity.y, speed * _direction.z) * Time.deltaTime);
        if (_direction.magnitude > 0)
        {// only set rotation on direction change
            Vector3 direction3 = new Vector3(_direction.x, 0f, _direction.z);
            _controller.transform.rotation = Quaternion.LookRotation(direction3);
        }
        _animator.SetFloat("WalkSpeed", GetSpeed());
    }

    public float GetSpeed()
    {
        return _controller.velocity.magnitude;
    }

    public void Die()
    {
        if (_audioSource != null && DeathSound != null)
        {
            _audioSource.PlayOneShot(DeathSound);
            foreach (MeshRenderer mesh in GetComponentsInChildren<MeshRenderer>()) { mesh.enabled = false; }
            foreach (Collider col in GetComponents<Collider>()) { col.enabled = false; }
            this.enabled = false;
            Destroy(this.gameObject, 1f);
        } else
        {
            Destroy(this.gameObject);
        }

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
            Task.Delay(DamageTimeoutTime).ContinueWith((_) => _damageTimeout = false);
        }
    }
}
