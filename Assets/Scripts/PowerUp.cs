using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerupType
{
    gunpick, speedup
}
public class PowerUp : MonoBehaviour
{
    public PowerupType type;    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyAfterTime());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 50 * Time.deltaTime, 50 * Time.deltaTime);
    }
    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }
}
