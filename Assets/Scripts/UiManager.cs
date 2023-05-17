using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public TextMeshProUGUI killCounter;
    public TextMeshProUGUI enemiesCounter;
    public TextMeshProUGUI waveCounter;
    public TextMeshProUGUI gunName;
    public TextMeshProUGUI powerUpActivation;
    public TextMeshProUGUI yourScoreTxt;
    public TextMeshProUGUI debugTxt;

    public GameObject GameOverScreen;
    public GameObject mainCamera;
    AudioManager audioManager;

    public int kills;
    public int enemies;
    public int waves;

    PlayFabManager playFabManager;  

    private void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        playFabManager = FindAnyObjectByType<PlayFabManager>();
       
    }
    public void UpdateKillCounter(int nokills)
    {
        kills += nokills;
        killCounter.text = "SCORE: " + kills;
        StartCoroutine(OriginalSize(killCounter.gameObject));
        StartCoroutine(mainCamera.GetComponent<MainCamera>().KillAnimation());
        
    }
    public void UpdateEnemiesCounter(int noEnemies) 
    { 
        enemies = noEnemies;
        enemiesCounter.text = "SLIMES LEFT: " + enemies;
        //StartCoroutine(OriginalSize(enemiesCounter.gameObject));
    }
    public void UpdateWaveCounter(int noWave)
    {
        waves = noWave;
        waveCounter.text = "WAVE: " + waves;
        StartCoroutine(OriginalSize(waveCounter.gameObject));
        StartCoroutine(mainCamera.GetComponent<MainCamera>().WaveAnimation());
    }

    public void UpdateGunName(GunType GunName)
    {
        //Debug.Log(gunName.text);    
        gunName.text =  "GUN: " + GunName.ToString();
        StartCoroutine(OriginalSize(gunName.gameObject));
    }

    public void UpdatePowerUpName(PowerupType PowerUp)
    {
        powerUpActivation.text = PowerUp.ToString();
        StartCoroutine(OriginalSize(powerUpActivation.gameObject));
    }

    public void UpdateGameOverScreen()
    {
        GameOverScreen.SetActive(true);
        audioManager.Play("Swish");
        yourScoreTxt.text = " Score: " + kills;
        LeanTween.scale(GameOverScreen, new Vector3(1.1f, 1.1f, 1.1f), 0.2f);
        //playFabManager.SendLeaderBoard(kills);
        //playFabManager.GetLeaderBoard();
        
        
    }

    public void UpdateDebug(string  debug)
    {
        debugTxt.text = debug;
    }

    IEnumerator OriginalSize(GameObject GGameObject)
    {
        LeanTween.scale(GGameObject, new Vector3(2, 2, 2), 0.1f).setEaseInCirc();
        yield return new WaitForSeconds(0.2f);
        LeanTween.scale(GGameObject, new Vector3(1, 1, 1), 0.1f).setEaseInCirc();   

    }

    
    public void RestartGameMethod()
    {
        StartCoroutine(RestartGame());
    }
    public void ExitSceneMethod()
    {
        StartCoroutine(ExitScene());    
    }
    //button functions 
     IEnumerator RestartGame()
    {
        audioManager.Play("Click");
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
     IEnumerator ExitScene()
    {
        audioManager.Play("Click");
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(0);
        
    }
}
