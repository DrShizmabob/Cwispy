using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundStars : MonoBehaviour
{
    [SerializeField] private GameObject starField;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject star;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        for(int h = -3; h <= 3; h++)
        {
            for(int v = -2; v <= 2; v++)
            {
                float random = Random.value;
                if(random < 0.25f)
                {
                    Instantiate(starField, new Vector3(h * 100, v * 100, 0), Quaternion.Euler(0, 0, 0), this.transform);
                }
                else if(random >= 0.25f && random < 0.5f)
                {
                    Instantiate(starField, new Vector3(h * 100, v * 100, 0), Quaternion.Euler(0, 0, 90), this.transform);
                }
                else if(random >= 0.5f && random < 0.75f)
                {
                    Instantiate(starField, new Vector3(h * 100, v * 100, 0), Quaternion.Euler(0, 0, 180), this.transform);
                }
                else
                {
                    Instantiate(starField, new Vector3(h * 100, v * 100, 0), Quaternion.Euler(0, 0, 270), this.transform);
                }                
            }            
        }
    }
    
    void Update()
    {        
        float distanceToStar = Vector3.Distance(player.transform.position, star.transform.position);
        if (distanceToStar < 1000)
        {
            foreach (SpriteRenderer s in GetComponentsInChildren<SpriteRenderer>())
            {
                Color newColor = s.color;
                newColor.a = (distanceToStar + 300f)/1300f;
                s.color = newColor;
            }
        }
    }

    private void LateUpdate()
    {
        Vector3 cameraPos = mainCamera.transform.position;
        transform.position = cameraPos + new Vector3(-cameraPos.x / 50, -cameraPos.y / 50, 10);
    }
}
