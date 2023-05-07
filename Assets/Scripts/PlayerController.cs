using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float currentSpeed;
    private float zBound = 24f;
    private float xBound = 24f;

    private Rigidbody playerRb;
    // Start is called before the first frame update
    void Start()
    {

        playerRb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {

        ConstraintPlayerMove();
        MovePlayer();

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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("-1 health");
        }
    }

}
