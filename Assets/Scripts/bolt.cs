using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bolt : MonoBehaviour
{
    [SerializeField] float lifetime;
    [SerializeField] float speed;
    [SerializeField] protected float damage;
    float startTime;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        this.gameObject.GetComponent<Rigidbody2D>().velocity = transform.up * speed; 
    }

    // Update is called once per frame
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    

    virtual protected void OnTriggerEnter2D(Collider2D collision)
    {        
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().takeDamage(damage, this.transform.position);
            Destroy(this.gameObject);       
        }
    }
}
