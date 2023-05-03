using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    [SerializeField] Text appleCount1, appleCount2, bananaCount1, bananaCount2, mushroomCoun1, mushroomCoun2;
    [SerializeField] Text applePrice, bananaPrice, mushroomPrice;
    [SerializeField] GameObject canvasGame, canvasShower, shower;

    GameManager gameManager;
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }
    public void UpdateCount(Dictionary<int, int> foodCount)
    {
        appleCount1.text = appleCount2.text = foodCount[0].ToString();
        bananaCount1.text = bananaCount2.text = foodCount[1].ToString();
        mushroomCoun1.text = mushroomCoun2.text = foodCount[2].ToString();
    }
    public void UpdateCost(Dictionary<int, int> foodCost)
    {
        applePrice.text = foodCost[0].ToString();
        bananaPrice.text = foodCost[1].ToString();
        mushroomPrice.text = foodCost[2].ToString();
    }
    public void OpenGame()
    {
        canvasGame.SetActive(true);
        canvasShower.SetActive(false);
        shower.SetActive(false);
        if (gameManager.foodOnScreen != null)
            gameManager.foodOnScreen.SetActive(true);
    }
    public void OpenShower()
    {
        canvasGame.SetActive(false);
        canvasShower.SetActive(true);
        shower.SetActive(true);
        if (gameManager.foodOnScreen != null)
            gameManager.foodOnScreen.SetActive(false);
    }
}
