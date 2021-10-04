using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic_Enemy : Enemy
{
    private float lastFireTime;
    private int clipCounter;
    [SerializeField] private float smokeChance;
    [SerializeField] private float reloadTime;
    [SerializeField] private int clipSize;
    [SerializeField] private GameObject enemyBolt;

    private Vector3 smokeOffset;

    private void Awake()
    {
        lastFireTime = Time.time;
        clipCounter = 0;
        smokeOffset = new Vector3(Random.Range(-10, 10), Random.Range(-1, 1), 0);  
    }

    private void Update()
    {
        calculateMovement();
        if (Time.time - lastFireTime > reloadTime + Random.value && GetComponent<Renderer>().isVisible)
        {
            //Debug.Log("Pew");
            Quaternion bulletRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + Random.Range(-2, 2));
            Instantiate(enemyBolt, transform.position + (transform.up * 10), bulletRotation);
            lastFireTime = Time.time;
            clipCounter++;
            if(clipCounter >= clipSize)
            {
                lastFireTime += reloadTime * 8;
                clipCounter = 0;
            }
        }
        if(isSmoking && Random.Range(1, this.getMaxHull()) > this.getHull() * smokeChance)
        { 
            GameObject newSmoke = Instantiate(shipSmoke, transform.position + smokeOffset, Quaternion.Euler(0, 0, Random.Range(-180, 180)));
            newSmoke.GetComponent<Rigidbody2D>().velocity = this.GetComponent<Rigidbody2D>().velocity + new Vector2(Random.Range(-10, 10), Random.Range(-10, 10));          
        }
    }

    private void calculateMovement()
    {
        float rotateSpeed = getRotateSpeed();        
        Vector3 playerVelocity = player.GetComponent<Player_Ship>().getPlayerVelocity();        
        Vector3 targetPosition = player.transform.position + (playerVelocity * Vector3.Distance(transform.position, player.transform.position) * 0.005f);    
        float angle = Vector3.SignedAngle(transform.up, transform.position - targetPosition, Vector3.forward);
        float rotateMultiplier = 1;        
        if (Vector3.Distance(transform.position, player.transform.position) > 200 && Mathf.Abs(angle) > 155)
        {
            rotateMultiplier = 0.33f;
            thrustForward(1);
        }        
        else 
        {
            inertialDampeners();
        }
        if(Mathf.Abs(angle - 180) < rotateSpeed * rotateMultiplier * Time.deltaTime)
        {
            rotate(angle - 180);
        }
        else
        {
            rotate(Mathf.Sign(angle) * -rotateSpeed * rotateMultiplier * Time.deltaTime);
        }        
        if(this.GetComponent<Rigidbody2D>().velocity.magnitude < 100.0f)
        {
            thrustForward(1);
        }
    }
}
