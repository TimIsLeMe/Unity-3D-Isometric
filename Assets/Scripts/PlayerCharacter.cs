using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : MonoBehaviour
{
    private Vector2 _direction;
    private Vector3 _direction3;
    [SerializeField] private float speed = 1f;
    private CharacterController _controller;
    private Animator _animator;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private Vector3 _bulletOffset = Vector3.zero;
    

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        if (_controller == null) throw new MissingComponentException("Missing main component in PlayerCharacter!");
    }
    private void FixedUpdate()
    {
        _controller.Move(new Vector3(speed * _direction.x, 0, speed * _direction.y));
        _animator.SetFloat("WalkSpeed", GetSpeed());
        if(_direction.x != 0 || _direction.y != 0)
        {
            _direction3 = new Vector3(_direction.x, 9f, _direction.y);
            _controller.transform.rotation = Quaternion.LookRotation(_direction3);
        }
         
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
        SpawnBullet();
    }


    private void SpawnBullet()
    {
        Quaternion direction = Quaternion.LookRotation(new Vector3(_direction.x, 0f, _direction.y));
        Instantiate(_bullet, transform.position + direction * _bulletOffset, direction);
    }

    public float GetSpeed()
    {
        return _controller.velocity.magnitude;
    }
}
