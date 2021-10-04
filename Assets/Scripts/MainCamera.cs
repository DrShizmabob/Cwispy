using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField] private float cameraSpeed;
    [SerializeField] private GameObject player;

    private Vector3 targetPosition;
    private Vector3 prevTargetOffset;
    private float shakeStartTime;
    private float shakeDuration;
    private float shakeIntensity;

    // Start is called before the first frame update
    void Start()
    {
        prevTargetOffset = Vector3.zero;
        shakeDuration = 0;
        shakeIntensity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        targetPosition = player.transform.position + new Vector3(0, 0, -10);
        Vector3 targetOffset = Vector3.Lerp(player.transform.up * playerRb.velocity.magnitude * 0.25f, playerRb.velocity * 0.25f, 0.5f); ;
        //targetPosition += targetOffset / ((Vector3.Distance(prevTargetOffset, targetOffset)*10) + 1);
        //prevTargetOffset = targetOffset;
        float distance = Vector3.Distance(prevTargetOffset, targetOffset);
        prevTargetOffset = Vector3.Lerp(prevTargetOffset, targetOffset, 1 / (distance + 75));
        targetPosition += prevTargetOffset;
        if(Time.time - shakeStartTime < shakeDuration)
        {
            targetPosition += new Vector3(Random.Range(-1, 1) * shakeIntensity, Random.Range(-1, 1) * shakeIntensity, 0);
        }
        transform.position = targetPosition;                
    }

    public void CameraShake(float intensity, float duration)
    {        
        if(Time.time - shakeStartTime < shakeDuration)
        {
            if(intensity > shakeIntensity)
            {
                shakeIntensity = intensity;
            }
            if(shakeDuration - (Time.time - shakeStartTime) < duration)
            {
                shakeStartTime = Time.time;
                shakeDuration = duration;
            }
        }
        else
        {
            shakeStartTime = Time.time;
            shakeDuration = duration;
            shakeIntensity = intensity;
        }
    }
}
