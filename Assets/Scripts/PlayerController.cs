using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float intialSpeed;
    public float currentSpeed;
    public float doubleSpeed;
    private float zBound = 24f;
    private float xBound = 24f;
    private bool gotHit;   
    public float timeForPowerUp;

    Vector3 mousePos;
    public Transform player;
    Vector3 objectPos;
    float angle;
    private Animator animator;
    private Rigidbody playerRb;

    public ParticleSystem explosionChange;
    public ParticleSystem explosionSpeed;

    public PowerupType powerupType;

    public GameObject[] Guns;
    public Transform spawnPosGun;
    public bool hasGun;

    private HealthBar playerHealthBar;
    public float playerHp;
    public float playerMaxHp;

    public bool gameOver;
  
    UiManager uiManager;
    
    // Start is called before the first frame update
    void Start()
    {
        gameOver = false;
        ChangeGuns();
        currentSpeed = intialSpeed;
        gotHit = false;
        playerRb = GetComponent<Rigidbody>();
        player = GetComponent<Transform>();       
        animator = gameObject.GetComponentInChildren<Animator>();   
        playerHealthBar = GetComponentInChildren<HealthBar>();  
        playerHealthBar.SetMaxHealth(playerMaxHp);  
        uiManager = GameObject.FindGameObjectWithTag("uiManager").GetComponent<UiManager>();    
    }

    // Update is called once per frame
    void FixedUpdate()
    {           
        if(playerHp <= 0)
        {
            gameOver = true;
        }      
        if(!gameOver)
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
            playerHp -=other.GetComponent<Enemy>().meleeDamage;  
            playerHealthBar.SetHealth(playerHp);
            StartCoroutine(other.GetComponent<Enemy>().OnDeath());
            StartCoroutine(PlayGetHit());         
            //Debug.Log("-1 health");
        }
        else if (other.gameObject.CompareTag("Powerup"))
        {
            if(other.gameObject.GetComponent<PowerUp>().type == PowerupType.speedup)
            {
                StartCoroutine(SpeedUp());
                Destroy(other.gameObject);

            }
            else if(other.gameObject.GetComponent<PowerUp>().type == PowerupType.gunpick)
            {
                ChangeGuns();
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
        angle = Mathf.Atan2(mousePos.y, mousePos.x)*Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, -angle+90, 0));
    }
    IEnumerator PlayGetHit() 
    {
        gotHit = true;
        animator.Play("GetHit");
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
        
        for (int i = 0;i  < Guns.Length;i++)
        {
            Guns[i].gameObject.SetActive(false);
            Guns[i].gameObject.GetComponent<Guns>().isShooting = false;
        }
        int random = Random.Range(0, Guns.Length);
        explosionChange.Play();
        Guns[random].SetActive(true);

        
    }
}
