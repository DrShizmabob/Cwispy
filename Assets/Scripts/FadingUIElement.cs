using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadingUIElement : MonoBehaviour
{
    [SerializeField] private float fadeTime;

    private float awakeTime;

    // Start is called before the first frame update
    void OnEnable()
    {
        awakeTime = fadeTime;
    }

    // Update is called once per frame
    void Update()
    {
        awakeTime -= Time.deltaTime;
        this.GetComponent<Image>().color = new Color(1, 1, 1, awakeTime/fadeTime);
        if (awakeTime <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }
}

