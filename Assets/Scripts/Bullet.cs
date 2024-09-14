using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody _rigidbody;
    [SerializeField] private float speed = 10f;
    public Vector3 initialPlayerVelocity { get; set; }
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = transform.forward * speed + initialPlayerVelocity;
        if(initialPlayerVelocity.magnitude > 0) initialPlayerVelocity -= initialPlayerVelocity / 10; // makes sure bullet travels away from player fast enough
    }
    
}
