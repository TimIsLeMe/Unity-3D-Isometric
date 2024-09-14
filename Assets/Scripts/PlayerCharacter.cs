using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : MonoBehaviour, Entity
{
    private Vector2 _direction;
    [SerializeField] private float speed = 1f;
    private CharacterController _controller;
    private Animator _animator;
    [SerializeField] private Bullet _bullet;
    [SerializeField] private Vector3 _bulletOffset = Vector3.zero;
    private bool _lockedRotation = false;
    LayerMask _playerLayer;
    

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
        if (_controller == null) throw new MissingComponentException("Missing main component in PlayerCharacter!");
    }
    private void FixedUpdate()
    {
        _controller.Move(new Vector3(speed * _direction.x, 0f, speed * _direction.y));
        _animator.SetFloat("WalkSpeed", GetSpeed());
        if (_lockedRotation && _animator.GetBool("DoneShooting"))
        {
            _lockedRotation = false;
            _animator.SetBool("DoneShooting", false);
        }
        if(!_lockedRotation && _direction.magnitude > 0)
        {// only set rotation on direction change
            Vector3 mousPos = GetRelativeMousePosition();
            Vector3 direction3D = (mousPos - transform.position).normalized;
            Vector3 lookDireciton = Vector3.Scale(direction3D, new Vector3(1, 0, 1));// new Vector3(_direction.x, 0f, _direction.y); 
            _controller.transform.rotation = Quaternion.LookRotation(lookDireciton);
        }
         
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
        Bullet bullet = Instantiate(_bullet, transform.position + direction * _bulletOffset, direction);
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

    public void Die()
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage()
    {
        throw new System.NotImplementedException();
    }
}
