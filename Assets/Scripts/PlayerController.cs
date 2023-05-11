using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float currentSpeed;
    private float zBound = 24f;
    private float xBound = 24f;
    private bool gotHit;
    private bool isMoving;

    Vector3 mousePos;
    public Transform player;
    Vector3 objectPos;
    float angle;
    private Animator animator;
    private Rigidbody playerRb;

    // Start is called before the first frame update
    void Start()
    {
        isMoving = false;   
        gotHit = false;
        playerRb = GetComponent<Rigidbody>();
        player = GetComponent<Transform>();       
        animator = gameObject.GetComponentInChildren<Animator>();   
    }

    // Update is called once per frame
    void FixedUpdate()
    {           
        if (!gotHit)
        {
            animator.Play("WalkFWD");
        }
        MouseLook();
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


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
           // Destroy(other.gameObject);
            StartCoroutine(PlayGetHit());         
            //Debug.Log("-1 health");
        }
        else if (other.gameObject.CompareTag("Powerup"))
        {
            Destroy(other.gameObject);
            //Debug.Log("Powerup");
        }
        else if (other.gameObject.CompareTag("Gunpickup"))
        {
            Destroy(other.gameObject);
            //Debug.Log("Gunswitch");
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
 

}
