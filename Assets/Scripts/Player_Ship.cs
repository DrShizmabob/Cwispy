using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player_Ship : Ship
{
    [SerializeField] GameObject shieldHitPrefab;
    [SerializeField] GameObject engineAnimationPrefab;
    [SerializeField] GameObject playerBoltPrefab;
    [SerializeField] private Sprite playerShipTop;
    [SerializeField] private Sprite playerShip45;
    [SerializeField] private Sprite playerShipSide;
    [SerializeField] float reloadTime;
    [SerializeField] float shieldsPerSecond;
    [SerializeField] float shieldsBufferTime;
    [SerializeField] float shieldDownTime;
    [SerializeField] Text shieldsOfflineText;
    [SerializeField] Image shieldHitImage;
    [SerializeField] Image shieldCrackImage;
    [SerializeField] Image shieldHeatSliderFill;
    [SerializeField] Slider shieldHeatSlider;
    [SerializeField] Text damage1;
    [SerializeField] Text damage2;
    [SerializeField] Text damage3;
    [SerializeField] Text damage4;
    [SerializeField] Text damage5;

    private GameObject engineAnimationHolder;
    private SpriteRenderer sr;
    private MainCamera mainCamera;
    private float engineStartupTime;
    private float lastFireTime;
    private bool isThrust;
    private bool hasInitialThrustBurst;
    private float timeUntilShieldWillStartRegen;
    private Vector3 targetPosition;
    private Vector3 velocity;

    override protected void onStart()
    {
        mainCamera = Camera.main.GetComponent<MainCamera>();
        sr = this.GetComponent<SpriteRenderer>();        
        hasInitialThrustBurst = false;
        shieldsOfflineText.gameObject.SetActive(false);
        shieldHitImage.gameObject.SetActive(false);
        shieldCrackImage.gameObject.SetActive(false);
        damage1.gameObject.SetActive(false);
        damage2.gameObject.SetActive(false);
        damage3.gameObject.SetActive(false);
        damage4.gameObject.SetActive(false);
        damage5.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        doInput();
        rotation();
        shieldBar();
    }

    public Vector3 getPlayerVelocity()
    {
        return new Vector3(GetComponent<Rigidbody2D>().velocity.x, GetComponent<Rigidbody2D>().velocity.y, 0);
    }

    private void shieldBar()
    {
        shieldHeatSlider.value = (this.getMaxShield() - this.getShield()) / this.getMaxShield();
        shieldHeatSliderFill.color = Color.Lerp(Color.red, Color.blue, this.getShield() / this.getMaxShield());
        if(timeUntilShieldWillStartRegen <= 0)
        {
            shieldsOfflineText.gameObject.SetActive(false);
            this.regenShields(shieldsPerSecond * Time.deltaTime);
        }
        else
        {
            timeUntilShieldWillStartRegen -= Time.deltaTime;
        }
        
    }

    private void doInput()
    {
        float thrustRotateMultiplier = 1;
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            isThrust = true;
            engineStartupTime = Time.time;
            hasInitialThrustBurst = false;
            if (engineAnimationHolder != null) { Destroy(engineAnimationHolder); }
            engineAnimationHolder = Instantiate(engineAnimationPrefab, this.transform);
            engineAnimationHolder.transform.localPosition = new Vector3(0, -5.75f, 0);
        }
        if(Input.GetKeyUp(KeyCode.UpArrow))
        {
            isThrust = false;
            Destroy(engineAnimationHolder);
        }
        if(Input.GetKey(KeyCode.UpArrow))
        {            
            thrustRotateMultiplier = 0.33f;
        }
        if(Input.GetKey(KeyCode.DownArrow))
        {
            inertialDampeners();
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rotate(this.getRotateSpeed() * thrustRotateMultiplier * Time.deltaTime);
        }
        if(Input.GetKey(KeyCode.RightArrow))
        {
            rotate(-this.getRotateSpeed() * thrustRotateMultiplier * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            if (Time.time - lastFireTime > reloadTime)
            {
                lastFireTime = Time.time;
                Quaternion bulletRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + Random.Range(-2, 2));
                Instantiate(playerBoltPrefab, transform.position + (transform.up * 10), bulletRotation);
                mainCamera.CameraShake(1, 0.05f);
            }
        }
        if (isThrust)
        {
            if (Time.time - engineStartupTime > 0.25)
            {                
                if (!hasInitialThrustBurst)
                {
                    thrustForward(10);
                    hasInitialThrustBurst = true;
                    mainCamera.CameraShake(2, 0.25f);
                }
                else
                {
                    thrustForward(1);
                }               
            }
            else
            {
                inertialDampeners();
            }
        }
    }

    private void rotation()
    {
        float currentRotation = transform.rotation.eulerAngles.z;
        if (currentRotation < 22.5f)
        {
            sr.sprite = playerShipTop;
        }
        else if (currentRotation < 67.5f)
        {
            sr.sprite = playerShip45;
            sr.flipX = false;
        }
        else if (currentRotation < 112.5f)
        {
            sr.sprite = playerShipSide;
            sr.flipX = false;
        }
        else if (currentRotation < 157.5)
        {
            sr.sprite = playerShip45;
            sr.flipX = false;
        }
        else if (currentRotation < 202.5)
        {
            sr.sprite = playerShipTop;
            sr.flipX = true;
        }
        else if (currentRotation < 247.5)
        {
            sr.sprite = playerShip45;
            sr.flipX = true;
        }
        else if (currentRotation < 292.5)
        {
            sr.sprite = playerShipSide;
            sr.flipX = true;
        }
        else if (currentRotation < 337.5)
        {
            sr.sprite = playerShip45;
            sr.flipX = true;
        }
        else
        {
            sr.sprite = playerShipTop;
        }
    }

    protected override void hullHit()
    {
        mainCamera.CameraShake(10, 0.5f);
        if(getHull()/getMaxHull() < 1.0f)
        {
            damage1.gameObject.SetActive(true);
        }
        if (getHull() / getMaxHull() < 4.0f / 5.0f)
        {
            damage2.gameObject.SetActive(true);
        }
        if (getHull() / getMaxHull() < 3.0f / 5.0f)
        {
            damage3.gameObject.SetActive(true);
        }
        if (getHull() / getMaxHull() < 2.0f / 5.0f)
        {
            damage4.gameObject.SetActive(true);
        }
        if (getHull() / getMaxHull() < 1.0f / 5.0f)
        {
            damage5.gameObject.SetActive(true);
        }
    }

    protected override void shieldHit()
    {
        Instantiate(shieldHitPrefab, this.transform.position, this.transform.rotation, this.transform);
        shieldHitImage.gameObject.SetActive(false);
        shieldHitImage.gameObject.SetActive(true);
        timeUntilShieldWillStartRegen = shieldsBufferTime;
    }

    protected override void shieldDestroyed()
    {
        shieldsOfflineText.gameObject.SetActive(true);
        shieldCrackImage.gameObject.SetActive(true);
        timeUntilShieldWillStartRegen = shieldDownTime;
    }

    protected override void hullDestroyed()
    {
        SceneManager.LoadScene("GameOver");
    }
}
