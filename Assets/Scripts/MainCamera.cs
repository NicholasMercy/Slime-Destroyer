using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public GameObject player;
   
    UiManager uiManager;    
    // Start is called before the first frame update
    void Awake()
    {
        LeanTween.move(gameObject, new Vector3(0, 9.92f, 0), 1.5f).setEaseInCirc();
        //StartCoroutine(KillAnimation());
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
    }

    public IEnumerator KillAnimation()
    {
        LeanTween.rotate(gameObject, new Vector3(90.2f, 0.1f, 0.1f), 0.1f).setEaseInCirc();
        yield return new WaitForSeconds(0.1f);
        LeanTween.rotate(gameObject, new Vector3(90, 0f, 0), 0.1f).setEaseInCirc();
    }
    public IEnumerator WaveAnimation()
    {
        LeanTween.move(gameObject, new Vector3(0, 9.8f, 0), 0.1f).setEaseInCirc();
        yield return new WaitForSeconds(0.2f);
        LeanTween.move(gameObject, new Vector3(0, 9.91f, 0), 0.1f).setEaseInCirc();
    }
}
