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
    public float scale;
    Vector3 scaleChanger;
    public bool isShooting;
    public GameObject bullet;
    public Transform BulletSpawn;
    public ParticleSystem gunExplosion;  
    BulletBehaviour bulletBehaviour;
    PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        scaleChanger = new Vector3 (scale, scale, scale);
        //transform.Rotate(0, 30f, 0);        
        bulletBehaviour = bullet.GetComponent<BulletBehaviour>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0) && !isShooting && !player.gameOver)
        {
            StartCoroutine(Shoot(firerate));                     
        }
    }
    IEnumerator Shoot(float firerate)
    {      
        isShooting = true;
        gunExplosion.Play();
        Instantiate(bullet, BulletSpawn.position, transform.rotation);
        bulletBehaviour.SetVariables(speed, dmg, scaleChanger);
        yield return new WaitForSeconds(firerate);              
        isShooting=false;
    }
    
}
