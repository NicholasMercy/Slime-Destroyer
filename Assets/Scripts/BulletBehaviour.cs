using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    public float dmg;
    public Vector3 scaleChange;
    
    void Start()
    {
        StartCoroutine(DestroyAfterTime()); 
        //Debug.Log(speed + " " + dmg);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward *speed* Time.deltaTime);  
        transform.localScale = scaleChange;
    }


    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    public void SetVariables(float Speed, float Dmg, Vector3 ScaleChange)
    {
        scaleChange = ScaleChange;
        speed = Speed;
        dmg = Dmg;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && other.gameObject.GetComponent<Enemy>().death == false) 
        {
            //Debug.Log("hit");
            Enemy tempEnemy = other.GetComponent<Enemy>();
            tempEnemy.TakeDamage(dmg);
            
            Destroy(gameObject);    
        
        }
    }
}
