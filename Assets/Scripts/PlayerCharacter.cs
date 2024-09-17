using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerCharacter : MonoBehaviour, Entity
{
    private Vector2 _direction;
    [SerializeField] private float speed = 5f;
    private CharacterController _controller;
    private Animator _animator;
    [SerializeField] private Bullet _bullet;
    [SerializeField] private Vector3 _bulletOffset = Vector3.zero;
    private bool _lockedRotation = false;
    [SerializeField] public Material flashMaterial;
    private Material originalMaterial;
    private Renderer _renderer;
    private BulletEffect _currentBulletEffect;
    private int _xpLevel = 1;
    public int ExpirienceLevel { get { return _xpLevel; } set { _xpLevel = value; } } // for UI
    [SerializeField] private float expirienceNeeded = 100;
    public float ExpirienceNeeded { get { return expirienceNeeded; } set { expirienceNeeded = value; } } // for UI
    private float _experience = 0;
    public float Experience { get { return _experience; } set { _experience = value; } } // for UI
    
    
    private Vector3 _velocity; // Store the current velocity including gravity
    private float _gravity = -15f; // Gravity value, adjust as needed
    private float _groundCheckDistance = 0.6f;
    
    
    private void Start()
    {
        _currentBulletEffect = new BulletEffect();
        _controller = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
        if (_controller == null) throw new MissingComponentException("Missing main component in PlayerCharacter!");
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

    private void CheckExpirience()
    {
        if(_experience >= expirienceNeeded)
        {
            _experience -= expirienceNeeded;
            _xpLevel++;
            GetBoon();
        }
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
                Debug.Log("Grounded");
            }
            else
            {
                _velocity.y += _gravity * Time.deltaTime;
                Debug.Log("notGrounded");
            }
            
            _controller.Move(new Vector3(speed * _direction.x, _velocity.y, speed * _direction.y) * Time.deltaTime);
            _animator.SetFloat("WalkSpeed", GetSpeed());

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
        Physics.Raycast(castPoint, out hit);
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
        _currentBulletEffect.AdditionalBulletCount = 1;
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
        // TODO: game over
        Debug.Log("Player died!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void TakeDamage()
    {
        TriggerFlash(0.3f);
    }
}
