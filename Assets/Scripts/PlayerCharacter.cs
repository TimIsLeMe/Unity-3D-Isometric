using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerCharacter : MonoBehaviour, Entity
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private Bullet _bullet;
    [SerializeField] private Vector3 _bulletOffset = Vector3.zero;
    [SerializeField] public Material flashMaterial;
    [SerializeField] private float expirienceNeeded = 100;
    [SerializeField] private float _gravity = -15f;
    private Vector3 _velocity; // velocity including gravity
    private Vector2 _direction;
    private CharacterController _controller;
    private Animator _animator;
    private Camera _camera;
    private Vector3 _movement;
    private bool _lockedRotation = false;
    private Creature _creature;
    private Material originalMaterial;
    private Renderer _renderer;
    private BulletEffect _currentBulletEffect;
    private int _xpLevel = 1;
    public int ExpirienceLevel { get { return _xpLevel; } set { _xpLevel = value; } } // for UI
    public float ExpirienceNeeded { get { return expirienceNeeded; } set { expirienceNeeded = value; } } // for UI
    private float _experience = 0;
    public float Experience { get { return _experience; } set { _experience = value; } } // for UI
    private float _groundCheckDistance = 0.6f;
    private bool _isInvulnerable;
    private float _invulnerabilityTimer;
    public Material invulnerabilityMaterial;
    private bool _hasRapidFire;
    private const float RapidFireVlaue = 0.5f;
    private float _rapidFireTimer;
    public Material rapidFireMaterial;
    private float _baseFirerate = 1f;
    private float _firerateModifier = 1f;

    private void Start()
    {
        _currentBulletEffect = new BulletEffect();
        _controller = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
        _animator.SetFloat("Firerate", _baseFirerate * _firerateModifier);
        _camera = Camera.main;
        _creature = GetComponentInChildren<Creature>();
    }

    private void Awake()
    {
        _renderer = GetComponentInChildren<Renderer>();
        originalMaterial = _renderer.material;
    }
    private void FixedUpdate()
    {
        HandleMovement();
        CheckExpirience();
    }
    
    private void Update()
    {
        HandleInvulnerability();
        HandleRapidfire();
    }

    private void HandleInvulnerability()
    {
        if (_isInvulnerable)
        {
            _renderer.material = invulnerabilityMaterial;
            _invulnerabilityTimer -= Time.deltaTime;

            if (_invulnerabilityTimer <= 0f)
            {
                _isInvulnerable = false;
                if (_creature) _creature.SetInvulnerable(_isInvulnerable);
                _renderer.material = originalMaterial;
            }
        }
    }

    private void HandleRapidfire()
    {
        if (_hasRapidFire)
        {
            _renderer.material = rapidFireMaterial;
            _rapidFireTimer -= Time.deltaTime;
            if (_rapidFireTimer <= 0f)
            {
                _hasRapidFire = false;
                _animator.SetFloat("Firerate", _baseFirerate * _firerateModifier);
                _renderer.material = originalMaterial;
            }
        }
    }

    private void CheckExpirience()
    {
        if(_experience >= expirienceNeeded)
        {
            _experience -= expirienceNeeded;
            _xpLevel++;
            GetBoon();
        }
    }

    public void GrantInvulnerability(float duration)
    {
        _isInvulnerable = true;
        _invulnerabilityTimer = duration;
        if (_creature != null) _creature.SetInvulnerable(_isInvulnerable);
        Debug.Log("invulnerabilty granted for " + duration +" seconds");
    }
    
    
    
    public void GrantRapidFire(float duration)
    {
        _hasRapidFire = true;
        _rapidFireTimer = duration;
        _animator.SetFloat("Firerate", (_baseFirerate + RapidFireVlaue) * _firerateModifier);
        Debug.Log("RapidFire granted for " + duration +" seconds");
    }

    public void GetBoon()
    {
        _currentBulletEffect = BulletEffect.MergeEffects(_currentBulletEffect, Boon.GetRandomBoon().BulletEffect);
        Debug.Log("new effect" + _currentBulletEffect.ToString());
    }

    public void HandleMovement()
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
        
        if (_lockedRotation && _animator.GetBool("DoneShooting"))
        {
            _lockedRotation = false;
            _animator.SetBool("DoneShooting", false);
        }
        if (!_lockedRotation)
        {
            Vector3 mousPos = GetRelativeMousePosition();
            Vector3 direction3D = (mousPos - transform.position).normalized;
            Vector3 lookDirection = Vector3.Scale(direction3D, new Vector3(1, 0, 1)); // Use X and Z for rotation
            _controller.transform.rotation = Quaternion.LookRotation(lookDirection);
        }
        Quaternion camRotation = _camera.transform.rotation.normalized;
        _movement = camRotation * new Vector3(speed * _direction.x, _velocity.y, speed * _direction.y) * Time.deltaTime;
        _movement.y = _velocity.y;
        _controller.Move(_movement);
        _animator.SetFloat("WalkSpeed", GetSpeed());
    }
    
    private void OnDrawGizmos()
    {
        // Set the color for the Gizmos
        Gizmos.color = Color.red;

        // Draw the ground check sphere
        Gizmos.DrawSphere(transform.position + Vector3.down * _groundCheckDistance, _groundCheckDistance);
    }

    public Vector3 GetRelativeMousePosition()
    {
        Vector3 mousePos = Input.mousePosition;
        RaycastHit hit;
        Ray castPoint = Camera.main.ScreenPointToRay(mousePos);
        Physics.Raycast(castPoint, out hit, 10000, LayerMask.GetMask("Ground"));
        return hit.point;
    }

    private void OnMove(InputValue inputValue)
    {
        _direction = inputValue.Get<Vector2>();
    }

    private void OnFire()
    {
        bool onCooldown = _animator.GetCurrentAnimatorStateInfo(0).IsName("Walking.Cooldown");
        bool stillShooting = _animator.GetCurrentAnimatorStateInfo(0).IsName("Walking.Shoot");
        if (onCooldown || stillShooting) return;
        _animator.SetBool("Attacking", true);
        _lockedRotation = true;
        SpawnBullet();
    }

    private void SpawnBullet()
    {
        Quaternion direction = Quaternion.LookRotation(transform.forward);
        Bullet bullet = Instantiate(_bullet, transform.position + direction * _bulletOffset, direction);
        bullet.SetBulletEffect(_currentBulletEffect);
        bullet.InitEffects(gameObject);
        bullet.initialPlayerVelocity = _controller.velocity;
    }

    public float GetSpeed()
    {
        return _controller.velocity.magnitude;
    }
    public Vector3 GetLocation()
    {
        return transform.position;
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
    public void Die()
    {
        GameOver();
    }
    public void GameOver()
    {
        Debug.Log("Player died!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void TakeDamage()
    {
        TriggerFlash(0.3f);
    }
}
