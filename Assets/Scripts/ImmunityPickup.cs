using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImmunityPickup : MonoBehaviour
{
    [SerializeField] private float _invulnerabilityDuration = 5f; 
    [SerializeField] private float _rotationSpeed = 50f;
    [SerializeField] private float _respawnTimer = 5f;
    private Vector3 _originalPosition;


    private void Start()
    {
        _originalPosition = transform.position;
    }

    private void Update()
    {
        transform.Rotate(0, _rotationSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            Debug.Log("Immunity pickup triggered");
            PlayerCharacter player = other.GetComponent<PlayerCharacter>();
            if (player != null)
            {
                player.GrantInvulnerability(_invulnerabilityDuration);
            }
            
            StartCoroutine(HandlePickupImmunity());
            
        }
    }
    
    private IEnumerator HandlePickupImmunity()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - 100f, transform.position.z);
        
        yield return new WaitForSeconds(_respawnTimer);
        
        transform.position = _originalPosition;

    }
}
