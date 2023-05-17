using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Speed Variables; 
    public float intialSpeed;
    public float currentSpeed;
    public float doubleSpeed;

    //Range Varaibles;
    private float zBound = 24f;
    private float xBound = 24f;
    private bool gotHit;

    //Timer Variables;
    public float timeForPowerUp;

    //Mouse and Player
    Vector3 mousePos;
    public Transform player;
    Vector3 objectPos;
    float angle;

    //animator
    private Animator animator;

    //Particles
    public ParticleSystem explosionChange;
    public ParticleSystem explosionSpeed;
    public ParticleSystem bloodSplatter;
    public ParticleSystem explosionHealth;
    public ParticleSystem explosionDoubleDmg;

    //Types
    public PowerupType currentPowerupType;
    public GunType currentGunType;

    //Guns
    public GameObject[] Guns;
    public Transform spawnPosGun;
    public bool hasGun;

    //Health
    private HealthBar playerHealthBar;
    public float playerHp;
    public float playerMaxHp;

    //States
    public bool gameOver;
    int counter = 0;

    //UI
    UiManager uiManager;
    AudioManager audioManager;
    SpawnManager spawnManager;  

    // Start is called before the first frame update
    void Start()
    {
        BeginGame();
        
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerHp <= 0)
        {
            gameOver = true;
            counter++;
            if(counter ==1)
            {
                uiManager.UpdateGameOverScreen();   
            }
            else
            {
                counter = 3;
            }
            
        }
        if (!gameOver)
        {
            if (!gotHit)
            {
                animator.Play("WalkFWD");
            }
            MouseLook();
            ConstraintPlayerMove();
            MovePlayer();

        }

    }

    void MovePlayer()
    {
        
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        Vector3 moveDir = new Vector3(horizontalInput, 0, verticalInput).normalized;
        transform.position += moveDir * currentSpeed * Time.deltaTime;



    }
    void ConstraintPlayerMove()
    {
        //restrictions
        if (transform.position.z < -zBound)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -zBound);
        }
        if (transform.position.z > zBound)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zBound);
        }
        if (transform.position.x < -xBound)
        {
            transform.position = new Vector3(-xBound, transform.position.y, transform.position.z);
        }
        if (transform.position.x > xBound)
        {
            transform.position = new Vector3(xBound, transform.position.y, transform.position.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            playerHp -= other.GetComponent<Enemy>().meleeDamage;
            playerHealthBar.SetHealth(playerHp);
            StartCoroutine(other.GetComponent<Enemy>().OnDeath());
            StartCoroutine(PlayGetHit());
            //Debug.Log("-1 health");
        }
        else if (other.gameObject.CompareTag("Powerup"))
        {
            if (other.gameObject.GetComponent<PowerUp>().type == PowerupType.SPEEDUP)
            {
                currentPowerupType = PowerupType.SPEEDUP;
                uiManager.UpdatePowerUpName(currentPowerupType);
                audioManager.Play("Pickup");
                audioManager.Play("SpeedUp");
                StartCoroutine(SpeedUp());
                Destroy(other.gameObject);

            }
            else if (other.gameObject.GetComponent<PowerUp>().type == PowerupType.GUNPICKUP)
            {
                currentPowerupType = PowerupType.GUNPICKUP;
                uiManager.UpdatePowerUpName(currentPowerupType);
                audioManager.Play("Pickup");
                audioManager.Play("GunSwitch");
                ChangeGuns();
                Destroy(other.gameObject);
            }
            else if(other.gameObject.GetComponent<PowerUp>().type == PowerupType.HEALTH)
            {
                currentPowerupType = PowerupType.HEALTH;
                uiManager.UpdatePowerUpName(currentPowerupType);
                explosionHealth.Play();
                audioManager.Play("Pickup");
                audioManager.Play("Heal");
                if(playerHp < playerMaxHp)
                {
                    playerHp += 20;
                    if(playerHp > playerMaxHp)
                    {
                        playerHp = playerMaxHp; 
                    }
                    playerHealthBar.SetHealth(playerHp);
                }
                Destroy(other.gameObject);


            }
            else if(other.gameObject.GetComponent<PowerUp>().type == PowerupType.DOUBLEDAMAGE) 
            {
                currentPowerupType = PowerupType.DOUBLEDAMAGE;
                uiManager.UpdatePowerUpName(currentPowerupType);
                audioManager.Play("Pickup");
                audioManager.Play("DoubleDamage");
                explosionDoubleDmg.Play();  
                foreach (GameObject g in Guns)
                {
                   StartCoroutine(g.GetComponent<Guns>().DoubleDamage());   
                }
                Destroy(other.gameObject); 
            
            
            }
        }

    }

    void MouseLook()
    {
        mousePos = Input.mousePosition;
        mousePos.z = 5.23f;
        objectPos = Camera.main.WorldToScreenPoint(player.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;
        angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, -angle + 90, 0));
    }
    IEnumerator PlayGetHit()
    {
        gotHit = true;
        animator.Play("GetHit");
        audioManager.Play("GetHit");
        bloodSplatter.Play();
        yield return new WaitForSeconds(0.5f);
        gotHit = false;
    }
    IEnumerator SpeedUp()
    {
       
       
        currentSpeed = doubleSpeed;
        explosionSpeed.Play();
        yield return new WaitForSeconds(timeForPowerUp);
        currentSpeed = intialSpeed;
    }
    

    void ChangeGuns()
    {

        for (int i = 0; i < Guns.Length; i++)
        {
            Guns[i].gameObject.SetActive(false);
            Guns[i].gameObject.GetComponent<Guns>().isShooting = false;
        }
        int random = Random.Range(0, Guns.Length);
        explosionChange.Play();
        Guns[random].SetActive(true);
        currentGunType = Guns[random].GetComponent<Guns>().type;
        uiManager.UpdateGunName(currentGunType);


    }
    void BeginGame()
    {
        uiManager = GameObject.FindGameObjectWithTag("uiManager").GetComponent<UiManager>();
        gameOver = false;
        ChangeGuns();
        currentSpeed = intialSpeed;
        gotHit = false;
        player = GetComponent<Transform>();
        animator = gameObject.GetComponentInChildren<Animator>();
        playerHealthBar = GetComponentInChildren<HealthBar>();
        playerHealthBar.SetMaxHealth(playerMaxHp);
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();   
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        audioManager.Play("BackgroundSong");
    }

    public void SpeedPerWave(float speed)
    {
        if(currentSpeed <doubleSpeed - 3)
        {
           
            currentSpeed += speed;
            intialSpeed = currentSpeed;
        }
        
    }
}
