using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class EnemyCharacter : MonoBehaviour, Entity
{
    private Vector3 _direction;
    [SerializeField] private float speed = 1f;
    private PlayerCharacter _player;
    private CharacterController _controller;
    private Animator _animator;
    

    private void Start()
    {
        _player = FindObjectOfType<PlayerCharacter>();
        _controller = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
        if (_controller == null) throw new MissingComponentException("Missing main component in EnemyCharacter!");
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
        throw new NotImplementedException();
    }

    public void TakeDamage()
    {
        throw new NotImplementedException();
    }
}
