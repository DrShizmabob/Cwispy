using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float decelerationForce;
    [SerializeField] private float maxShields;
    [SerializeField] private float maxHull;
    [SerializeField] protected GameObject shipExplosion;
    [SerializeField] protected GameObject shipSmoke;
    [SerializeField] protected bool isSmoking;
    //[SerializeField] private float shieldsPerSecond;
    //[SerializeField] private float shieldCooldown;

    private float shields;
    private float hull;
    private CircleCollider2D shieldCollider;
    private PolygonCollider2D hullCollider;

    public float getMaxHull()
    {
        return maxHull;
    }

    public float getHull()
    {
        return hull;
    }

    public float getMaxShield()
    {
        return maxShields;
    }

    public float getShield()
    {
        return shields;
    }

    public void regenShields(float amount)
    {
        shields += amount;
        if(shields > maxShields)
        {
            shields = maxShields;
        }
    }

    void Start()
    {
        //Debug.Log(gameObject + " " + gameObject.GetComponent<Ship>().maxShields + " " + gameObject.GetComponent<Ship>().maxHull);
        gameObject.GetComponent<Ship>().shields = gameObject.GetComponent<Ship>().maxShields;
        gameObject.GetComponent<Ship>().hull = gameObject.GetComponent<Ship>().maxHull;
        shieldCollider = gameObject.GetComponent<CircleCollider2D>();
        hullCollider = gameObject.GetComponent<PolygonCollider2D>();
        onStart();
        isSmoking = false;
    }

    protected void thrustForward(float magnitude)
    {
        Vector3 newVelocity = gameObject.GetComponent<Rigidbody2D>().velocity + (new Vector2(transform.up.x, transform.up.y) * acceleration * magnitude * Time.deltaTime);
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.ClampMagnitude(newVelocity, maxSpeed);
    }

    protected void inertialDampeners()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity /= (decelerationForce * Time.deltaTime) + 1;
    }

    protected void rotate(float angle)
    {
        transform.Rotate(new Vector3(0, 0, angle));
    }

    public void takeDamage(float damageAmount, Vector3 hitLocation) 
    {
        //Debug.Log(gameObject.GetComponent<Ship>().shields + " " + gameObject.GetComponent<Ship>().hull);
        Instantiate(shipSmoke, hitLocation, Quaternion.Euler(0, 0, Random.Range(-180, 180)));
        if(gameObject.GetComponent<Ship>().shields > 0)
        {
            if((gameObject.GetComponent<Ship>().shields -= damageAmount) <= 0)
            {                
                shieldCollider.enabled = false;
                shieldDestroyed();
            }
            else
            {
                shieldHit();
            }
        }
        else
        {            
            if((gameObject.GetComponent<Ship>().hull -= damageAmount) <= 0)
            {
                hullDestroyed();
            }
            if(gameObject.GetComponent<Ship>().hull < gameObject.GetComponent<Ship>().maxHull / 2)
            {
                isSmoking = true;
            }
            hullHit();
        }        
    }



    protected float getRotateSpeed()
    {
        return rotateSpeed;
    }

    virtual protected void hullHit() { }
    virtual protected void shieldHit() {}
    virtual protected void shieldDestroyed() {}
    virtual protected void hullDestroyed() {}
    virtual protected void onStart() {}
}
