using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : MonoBehaviour
{
    private Vector2 _direction;
    [SerializeField] private float speed = 1f;
    private CharacterController _controller;
    private Animator _animator;

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
        Vector3 direction = new Vector3(_direction.x, 5f, _direction.y);
        if(_direction.x != 0 || _direction.y != 0)
        {
            _controller.transform.rotation = Quaternion.LookRotation(direction);
        }
         
    }

    private void OnMove(InputValue inputValue)
    {
        _direction = inputValue.Get<Vector2>();
    }

    public float GetSpeed()
    {
        return _controller.velocity.magnitude;
    }
}
