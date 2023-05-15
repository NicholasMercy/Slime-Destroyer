using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using TMPro;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject player;
    private Transform slimeObj;
    private Rigidbody rb;
    public float speed = 8f;
    private float halfSpeed = 4f;
    private Animator animator;
    public bool death;
    public ParticleSystem deathParticle;

    private HealthBar healthBar;

    public float hp;
    public float maxHealth;
    public float damage;
    public float meleeDamage;
    public int value;

    PlayerController playerController;
    UiManager uiManager;
    AudioManager audioManager; 
    
    public AudioSource audioSource;
    void Start()
    {
        //deathParticle.Stop();
        death = false;
        player = GameObject.Find("Player");       
        rb = GetComponent<Rigidbody>();
       // speed = Random.Range(3, speed);
        animator = gameObject.GetComponentInChildren<Animator>();
        slimeObj = gameObject.GetComponentInChildren<Transform>();

        healthBar = GetComponentInChildren<HealthBar>();
        healthBar.SetMaxHealth(maxHealth);
        hp = maxHealth;

        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        uiManager = GameObject.FindGameObjectWithTag("uiManager").GetComponent<UiManager>();
        uiManager.UpdateKillCounter(0);
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        audioManager.Play("SpawnSlimes");
       // audioSource.Play(); 

        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
       if(!death && !playerController.gameOver)
        {

            MoveToPlayer();
            DestroyEnemy();
        }
        

    }

    void MoveToPlayer()
    {
        if(player != null)
        {
            animator.Play("Taunt");
            
            Vector3 towardsPlayer = (player.transform.position - transform.position).normalized;
            //rb.AddForce(towardsPlayer*speed*Time.deltaTime,ForceMode.VelocityChange);
            transform.LookAt(player.transform.position);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            //transform.Translate(towardsPlayer * speed * Time.deltaTime);

            if (speed <= halfSpeed)
            {
                animator.speed = 1;

            }
            else if (speed > halfSpeed)
            {
                animator.speed = 2;

            }

        }
        
    }

    void DestroyEnemy()
    {
        if(hp <= 0)
        {
            StartCoroutine(OnDeath());
        }
    }
    public void TakeDamage(float dmg)
    {
        
        hp -= dmg;
        //Debug.Log(hp);
        healthBar.SetHealth(hp);
        deathParticle.Play();
    }
    public IEnumerator OnDeath()
    {
        death = true;       
        animator.Play("Die");
        uiManager.UpdateKillCounter(value);
        healthBar.SetHealth(hp = 0);
        deathParticle.Play();
        audioManager.Play("DieSlime");
        yield return new WaitForSeconds(1);
        if(gameObject != null)
        {
            Destroy(gameObject);
        }
       
    }
   
}
