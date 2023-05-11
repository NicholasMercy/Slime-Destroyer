using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject player;
    private Transform slimeObj;
    private Rigidbody rb;
    private float speed = 8f;
    private float halfSpeed = 4f;
    private Animator animator;
    public ParticleSystem death;

    public float hp;
    void Start()
    {

        player = GameObject.Find("Player");       
        rb = GetComponent<Rigidbody>();
        speed = Random.Range(3, speed);
        animator = gameObject.GetComponentInChildren<Animator>();
        slimeObj = gameObject.GetComponentInChildren<Transform>();

        

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
       
        MoveToPlayer();
        DestroyEnemy();

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
            death.Play();
            Destroy(gameObject);
        }
    }
   
}
