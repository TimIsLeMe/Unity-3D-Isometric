using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 cameraOffset;
    private void FixedUpdate()
    {
        transform.position = player.transform.position;
    }
}
