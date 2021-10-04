using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float thrustRotateMultiplier;
    [SerializeField] private float acceleration;
    [SerializeField] private float maxSpeed;
    [SerializeField] private GameObject star;
    [SerializeField] private Sprite playerShipTop;
    [SerializeField] private Sprite playerShip45;
    [SerializeField] private Sprite playerShipSide;
    [SerializeField] private GameObject engineAnim;
    [SerializeField] private GameObject playerBolt;
    [SerializeField] private float reloadTime;
    [SerializeField] private float decelerationForce;

    private MainCamera mainCamera;
    private bool isThrust;
    private bool hasInitialThrustBurst;
    private float engineStartupTime;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private GameObject engineAnimHolder;
    private float lastFireTime;    

    void Start()
    {
        mainCamera = Camera.main.GetComponent<MainCamera>();
        isThrust = false;
        rb = this.GetComponent<Rigidbody2D>();
        sr = this.GetComponent<SpriteRenderer>();
        hasInitialThrustBurst = false;
    }

    void Update()
    {
        doMovement();
        starShadow();
        rotation();
    }

    public void takeDamage(float damage)
    {

    }

    private void rotation()
    {
        float currentRotation = transform.rotation.eulerAngles.z;
        if(currentRotation < 22.5f)
        {
            sr.sprite = playerShipTop;
        }
        else if(currentRotation < 67.5f)
        {
            sr.sprite = playerShip45;
            sr.flipX = false;
        }
        else if(currentRotation < 112.5f)
        {
            sr.sprite = playerShipSide;
            sr.flipX = false;
        }
        else if(currentRotation < 157.5)
        {
            sr.sprite = playerShip45;
            sr.flipX = false;
        }
        else if(currentRotation < 202.5)
        {
            sr.sprite = playerShipTop;
            sr.flipX = true;
        }
        else if(currentRotation < 247.5)
        {
            sr.sprite = playerShip45;
            sr.flipX = true;
        }
        else if(currentRotation < 292.5)
        {
            sr.sprite = playerShipSide;
            sr.flipX = true;
        }
        else if(currentRotation < 337.5)
        {
            sr.sprite = playerShip45;
            sr.flipX = true;
        }
        else
        {
            sr.sprite = playerShipTop;
        }
    }

    private void starShadow()
    {
        SpriteRenderer sr = this.GetComponent<SpriteRenderer>();
        if(rb.IsTouching(star.GetComponent<CircleCollider2D>()))
        {            
            float distanceToStar = Vector3.Distance(star.transform.position, transform.position);
            if (distanceToStar > 45)
            {
                sr.color = new Color(distanceToStar/90, distanceToStar/90, distanceToStar/90);
            }
            else
            {
                sr.color = Color.black;
            }            
        }
        else
        {
            sr.color = Color.white;
        }
    }

    private void doMovement()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            isThrust = true;
            engineStartupTime = Time.time;            
            hasInitialThrustBurst = false;
            if(engineAnimHolder != null) { Destroy(engineAnimHolder); }            
            engineAnimHolder = Instantiate(engineAnim, this.transform);
            engineAnimHolder.transform.localPosition = new Vector3(0, -5.75f, 0);
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            isThrust = false;
            Destroy(engineAnimHolder);
        }
        if (isThrust)
        {
            if (Time.time - engineStartupTime > 0.25)
            {
                Vector3 newVelocity;
                if (!hasInitialThrustBurst)
                {
                    newVelocity = rb.velocity + (new Vector2(transform.up.x, transform.up.y) * acceleration * 10 * Time.deltaTime);
                    hasInitialThrustBurst = true;
                    mainCamera.CameraShake(2, 0.25f);
                }
                else
                {
                    newVelocity = rb.velocity + (new Vector2(transform.up.x, transform.up.y) * acceleration * Time.deltaTime);
                }
                rb.velocity = Vector3.ClampMagnitude(newVelocity, maxSpeed);
            }
            else
            {
                rb.velocity /= (decelerationForce * Time.deltaTime) + 1;
            }
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (isThrust)
            {
                transform.Rotate(new Vector3(0, 0, -rotateSpeed * thrustRotateMultiplier * Time.deltaTime));
            }
            else
            {
                transform.Rotate(new Vector3(0, 0, -rotateSpeed * Time.deltaTime));
            }
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (isThrust)
            {
                transform.Rotate(new Vector3(0, 0, rotateSpeed * thrustRotateMultiplier * Time.deltaTime));
            }
            else
            {
                transform.Rotate(new Vector3(0, 0, rotateSpeed * Time.deltaTime));
            }
        }
        if(Input.GetKey(KeyCode.DownArrow))
        {
            rb.velocity /= (decelerationForce * Time.deltaTime) + 1;
        }
        if(Input.GetKey(KeyCode.Space))
        {
            if(Time.time - lastFireTime > reloadTime)
            {
                lastFireTime = Time.time;
                Quaternion bulletRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + Random.Range(-2, 2));
                Instantiate(playerBolt, transform.position + (transform.up * 10), bulletRotation);
                mainCamera.CameraShake(1, 0.05f);
            }            
        }
    }
}
