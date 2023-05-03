using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HadgehogRoadGameManager : MonoBehaviour
{
    [SerializeField] GameObject[] platforms;
    [SerializeField] GameObject[] platformsInGame = new GameObject[3];
    [SerializeField] GameObject car1, car2, car3, car4, car5;
    [SerializeField] float carSpeed;
    [SerializeField] Text scoreText;
    [SerializeField] Text coinsText;
    [SerializeField] GameObject gameOverMenu;

    private int score = 1;
    void Start()
    {
        platformsInGame[0] = Instantiate(platforms[3], new Vector3(-15, 0, 0), Quaternion.Euler(0, 0, -90));
        platformsInGame[1] = Instantiate(platforms[Random.Range(0, 2)], new Vector3(15, 0, 0), Quaternion.Euler(0, 0, -90));    
        platformsInGame[2] = Instantiate(platforms[Random.Range(0, 2)], new Vector3(45, 0, 0), Quaternion.Euler(0, 0, -90));
        StartCoroutine(CarSpawnTimer());
    }

    IEnumerator CarSpawnTimer()
    {
        SpawnCars();
        yield return new WaitForSeconds(5f);
        StartCoroutine(CarSpawnTimer());
        

    }
    void SpawnCars()
    {
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("Car Spawn Point");
        foreach(var spawnPoint in spawnPoints)
        {
            if(spawnPoint.transform.position.y<0)
            {
                Instantiate(car1, spawnPoint.transform.position + new Vector3(0, 0, -2), Quaternion.Euler(0, 0, 0));
            }
            else
            {
                Instantiate(car1, spawnPoint.transform.position + new Vector3(0, 0, -2), Quaternion.Euler(0, 0, 180));
            }
        }
    }

    public void AddRoad()
    {
        score++;
        UpdateScore();
        Destroy(platformsInGame[0]);
        platformsInGame[0] = platformsInGame[1];
        platformsInGame[1] = platformsInGame[2];
        platformsInGame[2] = Instantiate(platforms[Random.Range(0, 3)], new Vector3(15 + score * 30, 0 , 0), Quaternion.Euler(0, 0, -90));
        CarMovment.IncreaseSpeed(1);
    }

    public void StopGame()
    {
        coinsText.text = "Coins: " + (score * 2).ToString();
        FindObjectOfType<HadgehogControll>().GetComponent<HadgehogControll>().isAlive = false;
        gameOverMenu.SetActive(true);
        PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + (score - 1) * 2);
        PlayerPrefs.SetInt("Energy", PlayerPrefs.GetInt("Energy") - 5);
        if (PlayerPrefs.GetInt("Mood") + 10 <= 100)
            PlayerPrefs.SetInt("Mood", PlayerPrefs.GetInt("Mood") + 10);
        else
            PlayerPrefs.SetInt("Mood", 100);
    }
    private void UpdateScore()
    {
        scoreText.text = "Score: " + (score-1).ToString();
    }
}
