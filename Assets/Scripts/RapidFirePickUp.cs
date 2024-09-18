using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro.EditorUtilities;
using UnityEngine;

public class RapidFirePickUp : MonoBehaviour
{
    [SerializeField] private float _rapidFireDuration = 5f;
    [SerializeField] private float _rotationSpeed = 50f;
    [SerializeField] private float _respawnTimer = 5f;
    private Vector3 _originalPosition;

    private void Start()
    {
        _originalPosition = transform.position;
    }


    private void Update()
    {
        transform.Rotate(_rotationSpeed * Time.deltaTime, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player")) 
        {
            Debug.Log("RapidFire pickup triggered");
            PlayerCharacter player = other.GetComponent<PlayerCharacter>();
            if (player != null)
            {
                player.GrantRapidFire(_rapidFireDuration);
            }

            StartCoroutine(HandlePickupRapidFire());
            
        }
    }
    
    private IEnumerator HandlePickupRapidFire()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - 100f, transform.position.z);
        
        yield return new WaitForSeconds(_respawnTimer);
        
        transform.position = _originalPosition;

    }
}
