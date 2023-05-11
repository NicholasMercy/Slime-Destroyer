using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunType
{
    ak47, ak48, ak49
}
public class Guns : MonoBehaviour
{
    public GunType type;
    public float speed;
    public float dmg;
    public float firerate;
    bool isShooting;
    public GameObject bullet;
    public Transform BulletSpawn;
    public ParticleSystem gunExplosion;
    BulletBehaviour bulletBehaviour;
    // Start is called before the first frame update
    void Start()
    {
        bulletBehaviour = bullet.GetComponent<BulletBehaviour>();       
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0) && !isShooting)
        {
            StartCoroutine(Shoot(firerate));                     
        }
    }
    IEnumerator Shoot(float firerate)
    {      
        isShooting = true;
        gunExplosion.Play();
        Instantiate(bullet, BulletSpawn.position, transform.rotation);
        bulletBehaviour.SetVariables(speed, dmg);
        yield return new WaitForSeconds(firerate);              
        isShooting=false;
    }
    
}
