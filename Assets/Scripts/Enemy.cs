using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Ship 
{
    [SerializeField] private GameObject chevron;
    [SerializeField] private GameObject shieldHitPrefab;

    protected GameObject player;
    protected Camera mainCamera;    

    // Start is called before the first frame update
    override protected void onStart()
    {
        mainCamera = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void LateUpdate()
    {
        if (chevron.activeSelf)
        {
            radarSignature();
        }
    }    

    private void radarSignature()
    {
        Vector3 vectorBetween = new Vector3(this.gameObject.transform.position.x - player.transform.position.x, this.transform.position.y - player.transform.position.y);
        Vector3 thisScreenPos = mainCamera.WorldToScreenPoint(this.transform.position);
        Vector3 playerScreenPos = mainCamera.WorldToScreenPoint(player.transform.position);

        float distanceX;
        float distanceY;
        float angleX;
        float angleY;

        if (thisScreenPos.x < playerScreenPos.x)
        {
            angleX = Vector3.Angle(Vector3.left, vectorBetween);
            distanceX = playerScreenPos.x / Mathf.Cos(Mathf.Deg2Rad * angleX);
        }
        else
        {
            angleX = Vector3.Angle(Vector3.right, vectorBetween);
            distanceX = (mainCamera.pixelWidth - playerScreenPos.x) / Mathf.Cos(Mathf.Deg2Rad * angleX);
        }

        if (thisScreenPos.y < playerScreenPos.y)
        {
            angleY = Vector3.Angle(Vector3.down, vectorBetween);
            distanceY = playerScreenPos.y / Mathf.Cos(Mathf.Deg2Rad * angleY);
        }
        else
        {
            angleY = Vector3.Angle(Vector3.up, vectorBetween);
            distanceY = (mainCamera.pixelHeight - playerScreenPos.y) / Mathf.Cos(Mathf.Deg2Rad * angleY);
        }

        float distance;
        if (distanceX < distanceY)
        {
            distance = distanceX;
        }
        else
        {
            distance = distanceY;
        }

        //Debug.Log(distance);

        chevron.transform.position = mainCamera.ScreenToWorldPoint(Vector3.MoveTowards(playerScreenPos, thisScreenPos, distance - 50));
        float rotateAngle = Vector3.SignedAngle(Vector3.up, vectorBetween, Vector3.forward);
        chevron.transform.rotation = Quaternion.Euler(0, 0, rotateAngle);
        float scale = 1 / (((Vector3.Distance(player.transform.position, this.transform.position) - distance + 1) * 0.001f) + 1);
        chevron.transform.localScale = new Vector3(scale, scale, 1);
        chevron.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, scale); ;
    }



    private void OnBecameInvisible()
    {
        chevron.SetActive(true);
    }

    private void OnBecameVisible()
    {
        chevron.SetActive(false);
    }

    protected override void shieldHit()
    {
        Instantiate(shieldHitPrefab, transform.position, transform.rotation, transform);
    }

    protected override void shieldDestroyed()
    {
        //throw new System.NotImplementedException();
    }

    protected override void hullDestroyed()
    {
        Instantiate(shipExplosion,transform.position,Quaternion.identity);
        for(int i = 0; i < Random.Range(3,10); i++)
        {
            GameObject newSmoke = Instantiate(shipSmoke, transform.position, Quaternion.Euler(0, 0, Random.Range(-180, 180)));
            newSmoke.GetComponent<Rigidbody2D>().velocity = this.GetComponent<Rigidbody2D>().velocity + new Vector2(Random.Range(-100, 100), Random.Range(-100, 100));
        }
        Destroy(this.gameObject);
    }
}
