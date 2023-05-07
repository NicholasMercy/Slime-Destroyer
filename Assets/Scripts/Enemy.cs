using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject player;
    public float speed = 10f;
    
    void Start()
    {
        player = GameObject.Find("Player");

    }

    // Update is called once per frame
    void Update()
    {
       MoveToPlayer();
    }

    void MoveToPlayer()
    {
        if(player != null)
        {
            Vector3 towardsPlayer = player.transform.position - transform.position;
            transform.Translate(towardsPlayer * speed * Time.deltaTime);
        }
        
    }
}
