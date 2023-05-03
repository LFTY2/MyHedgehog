using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodDropGameManager : MonoBehaviour
{
    [SerializeField] float throwFrequency;
    [SerializeField] GameObject[] foods;
    [SerializeField] private int lives = 3; // remove SF
    public int score;

    [SerializeField] Text scoreText, livesText, coins;
    [SerializeField] GameObject gameOverMenu;
    
    void Start()
    {
        StartCoroutine(ThrowObject());
    }

    IEnumerator ThrowObject()
    {
        Throw();
        yield return new WaitForSeconds(throwFrequency);
        StartCoroutine(ThrowObject());
    }

    void Throw()
    { 
        Instantiate(foods[Random.Range(0, foods.Length)], new Vector2(Random.Range(-2.5f, 2.5f), -6), Quaternion.identity);
    }

    public void ReduceLives()
    {
        lives--;
        Updatelives();
        if (lives == 0)
        {
            StopGame();
        }
    }

    void StopGame()
    {
        int coinsCount = ((int)(score / 40) + 2);
        coins.text = "Coins " + coinsCount.ToString();
        StopAllCoroutines();
        gameOverMenu.SetActive(true);
        PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + coinsCount + 2);
        PlayerPrefs.SetInt("Energy", PlayerPrefs.GetInt("Energy") - 5);
        if(PlayerPrefs.GetInt("Mood") + 10 <= 100)
            PlayerPrefs.SetInt("Mood", PlayerPrefs.GetInt("Mood") + 10);
        else
            PlayerPrefs.SetInt("Mood", 100);

    }

    public void UpdateScore(int add)
    {
        score += add;
        scoreText.text = "Score: " + score.ToString();
    }

    void Updatelives()
    {
        livesText.text = "Lives: " + lives.ToString();
    }
}
