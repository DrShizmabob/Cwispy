using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bolt : bolt
{
    override protected void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Ship>().takeDamage(damage, this.transform.position);
            Destroy(this.gameObject);
        }
    }
}
