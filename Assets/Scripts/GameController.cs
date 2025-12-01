using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private Airplane player;
    private WaveSpawner waveSpawner;
    private int waveNum;
    [SerializeField] private int waveValueInitial;
    [SerializeField] private int waveValueIncrementer;
    [SerializeField] private int maxWaveValue;

    private GameObject option1;
    private GameObject option2;
    private GameObject option3;
    

    [SerializeField] private int RoundLength;
    private int roundCounter;
    [SerializeField] private int upgradeHealthChange;
    [SerializeField] private float upgradeMoveSpeedChange;
    [SerializeField] private float upgradeBulletSpeedChange;
    [SerializeField] private float upgradeRangeChange;
    [SerializeField] private float upgradeFireRateChange;
    [SerializeField] private int upgradeDamageChange;

    public bool shopIsOn;

    [SerializeField] private Upgrade[] upgrades;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Airplane>();
        waveSpawner = GameObject.FindGameObjectWithTag("WaveSpawner").GetComponent<WaveSpawner>();

        option1 = transform.GetChild(0).GetChild(0).gameObject;
        option2 = transform.GetChild(0).GetChild(1).gameObject;
        option3 = transform.GetChild(0).GetChild(2).gameObject;

        waveNum++;
        waveSpawner.waveValue = waveNum * waveValueIncrementer + waveValueInitial;
        waveSpawner.StartWave();
        

        roundCounter = RoundLength;     
        roundCounter--;
    }

    void Update()
    {
        if (waveSpawner.waveFinished == true && roundCounter > 0 && shopIsOn == false)
        {  
            waveNum++; 
            waveSpawner.waveValue = waveNum * waveValueIncrementer;

            waveSpawner.StartWave();

            roundCounter--;
        }
        else if (waveSpawner.waveFinished == true && roundCounter <= 0 && shopIsOn == false)
        {
            shopIsOn = true;
            ShowUpgrades();
        }

        if (player.gameOver == true)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void ShowUpgrades()
    {
        List<int> upgradeNums = new List<int>()
        {
            1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12
        };

        
        int num1 = upgradeNums[Random.Range(0, upgradeNums.Count - 1)];
        option1.GetComponent<UpgradeOption>().upgradeNum = upgrades[num1].upgradeNumber;
        option1.GetComponent<SpriteRenderer>().sprite = upgrades[num1].sprite;
        upgradeNums.Remove(num1);

        int num2 = upgradeNums[Random.Range(0, upgradeNums.Count - 1)];
        option2.GetComponent<UpgradeOption>().upgradeNum = upgrades[num2].upgradeNumber;
        option2.GetComponent<SpriteRenderer>().sprite = upgrades[num2].sprite;
        upgradeNums.Remove(num2);

        int num3 = upgradeNums[Random.Range(0, upgradeNums.Count - 1)];
        option3.GetComponent<UpgradeOption>().upgradeNum = upgrades[num3].upgradeNumber;
        option3.GetComponent<SpriteRenderer>().sprite = upgrades[num3].sprite;
        upgradeNums.Remove(num3);

        option1.SetActive(true);
        option2.SetActive(true);
        option3.SetActive(true);

        option1.GetComponent<UpgradeOption>().DisplayOption();
        option2.GetComponent<UpgradeOption>().DisplayOption();
        option3.GetComponent<UpgradeOption>().DisplayOption();
    }


    public void ActivateUpgrade(int upgradeNum, int optionNum)
    {
        if (upgradeNum == 1)
        {
            //Armor Piercing
            player.bulletDamage += upgradeDamageChange;
            player.bulletLifespan -= upgradeRangeChange;
        }
        else if (upgradeNum == 2)
        {
            //Bulky
            player.playerHP += upgradeHealthChange;
            player.bulletSpeed -= upgradeBulletSpeedChange;
        }  
        else if (upgradeNum == 3)
        {
            //Bullet Hell
            player.shootCooldownTime -= upgradeFireRateChange;
            player.bulletSpeed -= upgradeBulletSpeedChange;
        }  
        else if (upgradeNum == 4)
        {
            //Close Quarters
            player.moveSpeed += upgradeMoveSpeedChange;
            player.bulletLifespan -= upgradeRangeChange;
        }  
        else if (upgradeNum == 5)
        {
            //Fast And Furious
            player.moveSpeed += upgradeMoveSpeedChange;
            player.playerHP -= upgradeHealthChange;
        }  
        else if (upgradeNum == 6)
        {
            //glass cannon
            player.bulletDamage += upgradeDamageChange;
            player.playerHP -= upgradeHealthChange;
        }  
        else if (upgradeNum == 7)
        {
            //Lasers
            player.bulletSpeed += upgradeBulletSpeedChange;
            player.bulletDamage -= upgradeDamageChange;
        }
        else if (upgradeNum == 8)
        {
            //LightWeight        
            player.shootCooldownTime -= upgradeFireRateChange;
            player.moveSpeed -= upgradeMoveSpeedChange;
        }  
        else if (upgradeNum == 9)
        {
            //Minigun
            player.shootCooldownTime -= upgradeFireRateChange;
            player.bulletLifespan -= upgradeRangeChange;
        }  
        else if (upgradeNum == 10)
        {
            //Slow And Steady
            player.bulletDamage += upgradeDamageChange;
            player.moveSpeed -= upgradeMoveSpeedChange;
        }  
        else if (upgradeNum == 11)
        {
            //Sniper
            player.bulletDamage += upgradeDamageChange;
            player.shootCooldownTime += upgradeFireRateChange;
        }  
        else if (upgradeNum == 12)
        {
            //tank
            player.playerHP += upgradeHealthChange;
            player.moveSpeed -= upgradeMoveSpeedChange;
        }  

        if (player.moveSpeed < 1)
        {
            player.moveSpeed = 1;
        }
        if (player.bulletSpeed < 3)
        {
            player.bulletSpeed = 3;
        }
        if (player.bulletDamage < 1)
        {
            player.bulletDamage = 1;
        }
        if (player.bulletLifespan < 0.2f)
        {
            player.bulletLifespan = 0.2f;
        }
        if (player.shootCooldownTime < 0.1f)
        {
            player.shootCooldownTime = 0.1f;
        }
        if (player.playerHP < 1)
        {
            player.playerHP = 1;
        }



        if (optionNum == 1)
        {   
            option1.GetComponent<UpgradeOption>().PickShop();
            option2.GetComponent<UpgradeOption>().HideShop();
            option3.GetComponent<UpgradeOption>().HideShop();
        }
        else if (optionNum == 2)
        {
            option1.GetComponent<UpgradeOption>().HideShop();
            option2.GetComponent<UpgradeOption>().PickShop();
            option3.GetComponent<UpgradeOption>().HideShop();
        }
        else if (optionNum == 3)
        {
            option1.GetComponent<UpgradeOption>().HideShop();
            option2.GetComponent<UpgradeOption>().HideShop();
            option3.GetComponent<UpgradeOption>().PickShop();
        }
    }

    public void ShopHidden()
    {
        if (shopIsOn == true)
        {
            roundCounter = RoundLength; 
            shopIsOn = false;
        }
    }
}

[System.Serializable]
public class Upgrade
{
    public Sprite sprite;
    public int upgradeNumber;
}
