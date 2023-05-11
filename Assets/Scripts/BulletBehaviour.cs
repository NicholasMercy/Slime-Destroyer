using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    public float dmg;
    void Start()
    {
        StartCoroutine(DestroyAfterTime()); 
        Debug.Log(speed + " " + dmg);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward *speed* Time.deltaTime);  
    }


    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    public void SetVariables(float Speed, float Dmg)
    {
        Debug.Log("worrking");
        speed = Speed;
        dmg = Dmg;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy")) 
        {
            Enemy tempEnemy = other.GetComponent<Enemy>();
            tempEnemy.hp = tempEnemy.hp - dmg;
            Destroy(gameObject);    
        
        }
    }
}
