using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shieldHit : MonoBehaviour
{
    public void animationEnd()
    {
        Destroy(this.gameObject);
    }
}
