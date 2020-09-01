using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradesView : MonoBehaviour
{
    public Button backBtn;
    public Button buyBtn;
    [SerializeField] Button[] upgradesButtons;
    public Image selectionBg;
    int selectedUpgrade;


    int boostCost;
    int shieldCost;

    int lifeCost;
    [SerializeField] Image[] lvlImgsLifes;
    [SerializeField] TextMeshProUGUI txtCostLifes;
    int slowMotionCost;
    [SerializeField] Image[] lvlImgsSlowMotion;
    [SerializeField] TextMeshProUGUI txtCostSlowMotion;
    int shieldDurationCost;
    [SerializeField] Image[] lvlImgShieldDuration;
    [SerializeField] TextMeshProUGUI txtCostShieldDuration;
    int boostDurationCost;
    [SerializeField] Image[] lvlImgBoostDuration;
    [SerializeField] TextMeshProUGUI txtCostBoostDuration;
    int movementCost;
    [SerializeField] Image[] lvlImgMovement;
    [SerializeField] TextMeshProUGUI txtCostMovement;
    [SerializeField] TextMeshProUGUI txtBoostQuantity;
    [SerializeField] TextMeshProUGUI txtShieldQuantity;
    [SerializeField] TextMeshProUGUI txtLaserCost;

    [SerializeField] Sprite emptyLvl;
    [SerializeField] Sprite fullLvl;

    //laser
    public GameObject[] laserObjs;


    void Awake()
    {
        boostCost = 75000;
        shieldCost = 12000;
        selectedUpgrade = 99;
        buyBtn.onClick.AddListener(delegate () { BuyUpgrade(); });
        buyBtn.interactable = false;
        backBtn.onClick.AddListener(delegate () { CloseView(); });
        upgradesButtons[0].onClick.AddListener(delegate () { SelectUpgrade(0); });
        upgradesButtons[1].onClick.AddListener(delegate () { SelectUpgrade(1); });
        upgradesButtons[2].onClick.AddListener(delegate () { SelectUpgrade(2); });
        upgradesButtons[3].onClick.AddListener(delegate () { SelectUpgrade(3); });
        upgradesButtons[4].onClick.AddListener(delegate () { SelectUpgrade(4); });
        upgradesButtons[5].onClick.AddListener(delegate () { SelectUpgrade(5); });
        upgradesButtons[6].onClick.AddListener(delegate () { SelectUpgrade(6); });
        upgradesButtons[7].onClick.AddListener(delegate () { SelectUpgrade(7); });
        //upgradesButtons[8].onClick.AddListener(delegate () { SelectUpgrade(8); });
        //upgradesButtons[9].onClick.AddListener(delegate () { SelectUpgrade(9); });
    }

    void Update()
    {
        if(selectedUpgrade == 99) { return; }
        selectionBg.transform.position = upgradesButtons[selectedUpgrade].gameObject.transform.position;
    }

    void SelectUpgrade(int index)
    {
        Debug.Log(index);
        buyBtn.interactable = true;
        selectedUpgrade = index;
    }

    void BuyUpgrade()
    {
        if(selectedUpgrade == 99) { return; }
        RefreshUI();
        switch (selectedUpgrade)
        {
            case 0:
                if(GameManager.Instance.emeralds >= boostCost)
                {
                    GameManager.Instance.boostAmmount++;
                    GameManager.Instance.UseEmerals(boostCost);
                }
                break;
            case 1:
                if (GameManager.Instance.emeralds >= shieldCost)
                {
                    GameManager.Instance.shieldsAmmount++;
                    GameManager.Instance.UseEmerals(shieldCost);
                }
                break;
            case 2:
                if (GameManager.Instance.emeralds >= lifeCost && GameManager.Instance.startLife < 5)
                {
                    GameManager.Instance.startLife++;
                    GameManager.Instance.UseEmerals(lifeCost);
                }
                break;
            case 3:
                if (GameManager.Instance.emeralds >= slowMotionCost && GameManager.Instance.slowTimeLvl < 9)
                {
                    GameManager.Instance.slowTimeLvl++;
                    GameManager.Instance.UseEmerals(slowMotionCost);
                }
                break;
            case 4:
                if (GameManager.Instance.emeralds >= shieldDurationCost && GameManager.Instance.shieldLvl < 9)
                {
                    GameManager.Instance.shieldLvl++;
                    GameManager.Instance.UseEmerals(shieldDurationCost);
                }
                break;
            case 5:
                if (GameManager.Instance.emeralds >= boostDurationCost && GameManager.Instance.boostLvl < 9)
                {
                    GameManager.Instance.boostLvl++;
                    GameManager.Instance.UseEmerals(boostDurationCost);
                }
                break;
            case 6:
                if (GameManager.Instance.emeralds >= movementCost && GameManager.Instance.movementLvl < 9)
                {
                    GameManager.Instance.movementLvl++;
                    GameManager.Instance.UseEmerals(movementCost);
                }
                break;
            case 7:
                if (GameManager.Instance.emeralds >= 10000000 && GameManager.Instance.laser == 0)
                {
                    GameManager.Instance.laser = 1;
                    GameManager.Instance.UseEmerals(10000000);
                }
                break;
            case 8:
                break;
            case 9:
                break;
        }
        GameManager.Instance.SavePlayer();
        GameManager.Instance.RefreshMainView();
        RefreshUI();
    }

    void CloseView()
    {
        gameObject.SetActive(false);
    }

    public void RefreshUI()
    {
        for(int i = 0; i < GameManager.Instance.startLife; i++)
        {
            lvlImgsLifes[i].sprite = fullLvl;
        }
        for (int i = 0; i < (GameManager.Instance.slowTimeLvl - 1); i++)
        {
            lvlImgsSlowMotion[i].sprite = fullLvl;
        }
        for (int i = 0; i < (GameManager.Instance.shieldLvl - 1); i++)
        {
            lvlImgShieldDuration[i].sprite = fullLvl;
        }
        for (int i = 0; i < (GameManager.Instance.boostLvl - 1); i++)
        {
            lvlImgBoostDuration[i].sprite = fullLvl;
        }
        for (int i = 0; i < (GameManager.Instance.movementLvl - 1); i++)
        {
            lvlImgMovement[i].sprite = fullLvl;
        }
        if(GameManager.Instance.startLife == 0)
        {
            lifeCost = 25000;
        }
        else if(GameManager.Instance.startLife == 1)
        {
            lifeCost = 100000;
        }
        else
        {
            lifeCost = (int)(25000 * Mathf.Pow(GameManager.Instance.startLife + 1 /** 2f*/, 2f));
        }
        if(GameManager.Instance.slowTimeLvl == 1)
        {
            slowMotionCost = 20000;
        }
        else
        {
            slowMotionCost = (int)(20000 * Mathf.Pow(GameManager.Instance.slowTimeLvl /** 2f*/, 2f));
        }
        if(GameManager.Instance.shieldLvl == 1)
        {
            shieldDurationCost = 14750;
        }
        else
        {
            shieldDurationCost = (int)(14750 * Mathf.Pow(GameManager.Instance.shieldLvl /** 2f*/, 2f));
        }
        if(GameManager.Instance.boostLvl == 1)
        {
            boostDurationCost = 17500;
        }
        else
        {
            boostDurationCost = (int)(17500 * Mathf.Pow(GameManager.Instance.boostLvl /** 2f*/, 2f));
        }
        if (GameManager.Instance.movementLvl == 1)
        {
            movementCost = 12500;
        }
        else
        {
            movementCost = (int)(12500 * Mathf.Pow(GameManager.Instance.movementLvl /** 2f*/, 2f));
        }
        if(GameManager.Instance.startLife < 5)
        {
            txtCostLifes.text = lifeCost.ToString();
        }
        else
        {
            txtCostLifes.text = "Max level";
        }
        if (GameManager.Instance.slowTimeLvl < 9)
        {
            txtCostSlowMotion.text = slowMotionCost.ToString();
        }
        else
        {
            txtCostSlowMotion.text = "Max level";
        }
        if (GameManager.Instance.shieldLvl < 9)
        {
            txtCostShieldDuration.text = shieldDurationCost.ToString();
        }
        else
        {
            txtCostShieldDuration.text = "Max level";
        }
        if (GameManager.Instance.boostLvl < 9)
        {
            txtCostBoostDuration.text = boostDurationCost.ToString();
        }
        else
        {
            txtCostBoostDuration.text = "Max level";
        }
        if (GameManager.Instance.movementLvl < 9)
        {
            txtCostMovement.text = movementCost.ToString();
        }
        else
        {
            txtCostMovement.text = "Max level";
        }
        txtBoostQuantity.text = GameManager.Instance.boostAmmount.ToString();
        txtShieldQuantity.text = GameManager.Instance.shieldsAmmount.ToString();

        foreach (GameObject obj in laserObjs)
        {
            if (GameManager.Instance.laser > 0)
            {
                obj.SetActive(true);
                txtLaserCost.text = "Max level";
            }
            else
            {
                obj.SetActive(false);
            }
        }
    }
}
