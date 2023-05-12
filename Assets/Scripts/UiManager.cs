using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public TextMeshProUGUI killCounter;
    public TextMeshProUGUI enemiesCounter;
    public TextMeshProUGUI waveCounter;

    public int kills;
    public int enemies;
    public int waves;
    
    public void UpdateKillCounter(int nokills)
    {
        kills += nokills;
        killCounter.text = "Slimes Killed: " + kills;   
    }
    public void UpdateEnemiesCounter(int noEnemies) 
    { 
        enemies = noEnemies;
        enemiesCounter.text = "Enemies Left: " + enemies;
    }
    public void UpdateWaveCounter(int noWave)
    {
        waves = noWave;
        waveCounter.text = "Wave: " + waves;
    }
}
