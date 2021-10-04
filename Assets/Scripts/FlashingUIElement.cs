using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashingUIElement : MonoBehaviour
{
    [SerializeField] private float flashTime;

    private float timeSinceLastFlash;
    private Text r;

    void Awake()
    {
        r = gameObject.GetComponent<Text>();
    }

    void OnEnable()
    {
        timeSinceLastFlash = flashTime;
        r.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastFlash -= Time.deltaTime;
        if(timeSinceLastFlash <= 0)
        {
            timeSinceLastFlash = flashTime;
            r.enabled = !r.enabled;
        }
    }
}
