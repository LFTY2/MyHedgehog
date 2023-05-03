using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{ 
    [SerializeField] private GameObject mainGameScreen;
    [SerializeField] private GameObject shopScreen;
    [SerializeField] private GameObject gameScreen;
    [SerializeField] private GameObject deathScreen;
    

    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject storedFoodPanel;

    [SerializeField] private GameObject feedWarning;
    [SerializeField] private GameObject Hedgehog;

    [SerializeField] private Text moneyText;
    [SerializeField] private Text moneyTextInShop;

    

    [SerializeField] private GameObject energyWarning;

    private DateTime lastUpdateTime;

    [SerializeField] private float updateInterval;


    Dictionary<int, int> foodCounts = new();
    private readonly Dictionary<int, int> foodPrice = new();

    [SerializeField] GameObject[] foods;
    [SerializeField] GameObject[] dirt;

    public GameObject foodOnScreen;

    [SerializeField] private UIManager uIManager;

    private Vector3 foodStartPos = new(0, -3, 1);

    public Slider hungerSlider;
    public Slider energySlider;
    public Slider moodSlider;
    [SerializeField] private int hungerValue; //remove SF
    [SerializeField] private int hiddenHunger;
    [SerializeField] private int energyValue; //remove SF
    [SerializeField] private int moodValue; //remove SF
    [SerializeField] private int money; //remove SF
    [SerializeField] public int dirtCount;
    [SerializeField] private int dirtToSpawn;
    bool isDead;

    private void Start()
    {
        FillFoodDictionary();
        RestoreData();
        UpdateMoney();
        
        FillFoodPriceDictionary();
        UpdateSlider();
        SpawnDirt();
        uIManager.UpdateCount(foodCounts);
        uIManager.UpdateCost(foodPrice);
    }
    private void Update()
    {
        UpdateData();
    }
    public bool Eat(int saturationValue, int foodID)
    {
        if (hungerValue == 100)
            return false;
        if(hungerValue + saturationValue > 100)
        {
            hungerValue = 100;

        }
        else
        {
            hungerValue += saturationValue;
            
        }
        hiddenHunger = hungerValue;
        foodCounts[foodID]--;
        SaveData();
        UpdateSlider();
        if (foodCounts[foodID] == 0)
        {
            foodOnScreen = null;
            return true;
        }       
        else
            return false;
    }
    void UpdateSlider()
    {
        energySlider.value = energyValue;
        hungerSlider.value = hungerValue;
        moodSlider.value = moodValue;
    }

    void FillFoodDictionary()
    {
        foodCounts.Add(0, 0); //Apple
        foodCounts.Add(1, 0); //Banana
        foodCounts.Add(2, 0); //Mushroom
    }
    void FillFoodPriceDictionary()
    {
        foodPrice.Add(0, 5); //Apple
        foodPrice.Add(1, 7); //Banana
        foodPrice.Add(2, 10); //Mushroom
    }
    private void UpdateFood(int foodId)
    {
        if(foodOnScreen != null)
        {
            Destroy(foodOnScreen);
        }
        foodOnScreen = Instantiate(foods[foodId], foodStartPos, Quaternion.identity);
    }

    public void BuyFood(int foodId)
    {
        int price = foodPrice[foodId];
        if (money >= price)
        {
            foodCounts[foodId]++;
            money -= price;
            UpdateMoney();
            uIManager.UpdateCount(foodCounts);
            SaveData();
        }
        
    }
    public void SelectFood(int foodId) //
    {
        if (foodCounts[foodId] > 0)
        {
            ShopClose();
            UpdateFood(foodId);
        }
    }
    void UpdateMoney()
    {
        moneyText.text = money.ToString();
        moneyTextInShop.text = money.ToString();
    }
    
    void SaveData()
    {
        PlayerPrefs.SetInt("Money", money);
        PlayerPrefs.SetInt("Hunger", hungerValue);
        PlayerPrefs.SetInt("HiddenHunger", hungerValue);
        PlayerPrefs.SetInt("Energy", energyValue);
        PlayerPrefs.SetInt("Mood", moodValue);
        PlayerPrefs.SetInt("Dirt", dirtToSpawn + dirtCount);
        PlayerPrefs.SetInt("AppleCount", foodCounts[0]);
        PlayerPrefs.SetInt("BananaCount", foodCounts[1]);
        PlayerPrefs.SetInt("MushroomCount", foodCounts[2]);
        PlayerPrefs.SetString("lastUpdateTime", lastUpdateTime.ToString());
        PlayerPrefs.Save();
    }
    void RestoreData()
    {
        string timeString = PlayerPrefs.GetString("lastUpdateTime", DateTime.Now.ToString());
        lastUpdateTime = DateTime.Parse(timeString);
        money = PlayerPrefs.GetInt("Money", 100);
        hungerValue = PlayerPrefs.GetInt("Hunger", 20);
        hiddenHunger = PlayerPrefs.GetInt("HiddenHunger", 20);
        energyValue = PlayerPrefs.GetInt("Energy", 50);
        moodValue = PlayerPrefs.GetInt("Mood", 50);
        dirtToSpawn = PlayerPrefs.GetInt("Dirt", 0);
        foodCounts[0] = PlayerPrefs.GetInt("AppleCount", 2);
        foodCounts[1] = PlayerPrefs.GetInt("BananaCount", 2);
        foodCounts[2] = PlayerPrefs.GetInt("MushroomCount", 2);
    }

    void UpdateData()
    {
        TimeSpan timeSinceLastUpdate = DateTime.Now - lastUpdateTime;

        if (timeSinceLastUpdate.TotalSeconds >= updateInterval)
        {
            // Update the value
            float value = (float)timeSinceLastUpdate.TotalSeconds / updateInterval;
            energyValue += (int)value;
            hungerValue -= (int)value;
            hiddenHunger -= (int)value;
            moodValue -= (int)value;
            dirtToSpawn += (int)value;
            if (hiddenHunger < -500)
            {
                Death();
            }
            if(hungerValue<0)
            {
                hungerValue = 0;
            }
            if(moodValue < 0)
            {
                moodValue = 0;
            }
            if(energyValue>100)
            {
                energyValue = 100;
            }
            if(dirtCount + dirtToSpawn > 6)
            {
                dirtToSpawn = 6 - dirtCount;
            }

            // Save the new value and update time to PlayerPrefs
           
            lastUpdateTime = DateTime.Now;
            SaveData();
            UpdateSlider();
            SpawnDirt();
        }
    }

    void SpawnDirt()
    {
        if (dirtCount > 6) return;
        int availableDirt = 0;
        foreach (GameObject d in dirt)
        {
            if (d.GetComponent<SpriteRenderer>().color.a <= 0)
            {
                availableDirt++;
            }
        }
        if (availableDirt == 0) return;
        while (dirtToSpawn > 0)
        {
            int randNum = UnityEngine.Random.Range(0, 6);
            SpriteRenderer dirtSR = dirt[randNum].GetComponent<SpriteRenderer>();
            if (dirtSR.color.a <= 0)
            {
                dirtToSpawn--;
                dirtCount++;
                dirt[randNum].SetActive(true);
                dirtSR.color = new Color(0, 0, 0, 0.5f);
                
            }
        } 
    }

    public void RemovingDirt()
    {
        money++;
        dirtCount--;
        SaveData();
    }
    void Death()
    {
        OpenDeathScreen();
        hiddenHunger = hungerValue;
    }
    public void StartGameHadgehogOnTheRoad()
    {
        if(energyValue>=5)
            SceneManager.LoadScene("Hadgehog on the Road");
        else
        {
            EnergyWarning();
        }
    }
    public void StartGameFoodDrop()
    {
        if (energyValue >= 5)
            SceneManager.LoadScene("Food Drop");
        else
        {
            EnergyWarning();
        }
    }
    public void StartGameHedgehogJump()
    {
        if (energyValue >= 5)
            SceneManager.LoadScene("Hadgehog Jump");
        else
        {
            EnergyWarning();
        }
    }

    void EnergyWarning()
    {
        energyWarning.SetActive(true);
    }
    public void CloseEnergyWarning()
    {
        energyWarning.SetActive(false);
    }

    public void ShopOpen()
    {
        uIManager.UpdateCount(foodCounts);
        shopScreen.SetActive(true);
        mainGameScreen.SetActive(false);
        Hedgehog.SetActive(false);
        if (foodOnScreen != null)
            foodOnScreen.SetActive(false);

    }
    public void ShopClose()
    {
        shopScreen.SetActive(false);
        Hedgehog.SetActive(true);
        if (foodOnScreen != null)
            foodOnScreen.SetActive(true);
        mainGameScreen.SetActive(true);
    }

    public void ShopButtonsOpen()
    {
        shopPanel.SetActive(true);
        storedFoodPanel.SetActive(false);
    }
    public void StoredFoodsOpen()
    {
        shopPanel.SetActive(false);
        storedFoodPanel.SetActive(true);
    }

    public void GameMenuOpen()
    {
        mainGameScreen.SetActive(false);
        Hedgehog.SetActive(false);
        gameScreen.SetActive(true);
        if (foodOnScreen != null)
            foodOnScreen.SetActive(false);
    }
    public void GameMenuClose()
    {
        mainGameScreen.SetActive(true);
        Hedgehog.SetActive(true);
        gameScreen.SetActive(false);
        if (foodOnScreen != null)
            foodOnScreen.SetActive(true);
    }

    public void OpenDeathScreen()
    {
        mainGameScreen.SetActive(false);
        Hedgehog.SetActive(false);
        deathScreen.SetActive(true);
        if (foodOnScreen != null)
            foodOnScreen.SetActive(false);
    }

    public void CloseDeathScreen()
    {
        mainGameScreen.SetActive(true);
        Hedgehog.SetActive(true);
        gameScreen.SetActive(false);
        foodOnScreen = null;
    }

    public void OpenFeedWarning()
    {
        feedWarning.SetActive(true);
    }

}


