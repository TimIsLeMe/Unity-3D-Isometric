using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImmunityPickup : MonoBehaviour
{
    [SerializeField] private float _invulnerabilityDuration = 5f; // Duration of invulnerability
    [SerializeField] private float _rotationSpeed = 50f;


    private void Update()
    {
        transform.Rotate(0, _rotationSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Immunity pickup triggered");

        if (other.CompareTag("Player")) // Ensure your player has the "Player" tag
        {
            // Grant invulnerability to the player
            PlayerCharacter player = other.GetComponent<PlayerCharacter>();
            if (player != null)
            {
                player.GrantInvulnerability(_invulnerabilityDuration);
            }
            
            Destroy(gameObject);
        }
    }
}
