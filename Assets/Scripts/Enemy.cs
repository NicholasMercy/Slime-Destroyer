using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject player;
    private Rigidbody rb;
    public float speed = 20f;
    
    void Start()
    {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody>();
        speed = Random.Range(10, speed);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveToPlayer();
      
    }

    void MoveToPlayer()
    {
        if(player != null)
        {
           
            Vector3 towardsPlayer = (player.transform.position - transform.position).normalized;
            //rb.AddForce(towardsPlayer*speed*Time.deltaTime,ForceMode.VelocityChange);      
            transform.Translate(towardsPlayer * speed * Time.deltaTime);
        }
        
    }
}
