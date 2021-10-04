using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MindgroundElement : MonoBehaviour
{
    [SerializeField] private float distance;
    [SerializeField] private GameObject player;
    private Camera mainCamera;
    private Vector3 initialOffset;

    void Start()
    {
        mainCamera = Camera.main;
        initialOffset = transform.position;
    }

    void Update()
    {
        
    }

    private void LateUpdate()
    {
        Vector3 cameraPos = mainCamera.transform.position;
        //Vector3 playerPos = player.transform.position;
        transform.position = cameraPos + new Vector3(-cameraPos.x / distance, -cameraPos.y / distance, 10) + initialOffset;
    }
}
