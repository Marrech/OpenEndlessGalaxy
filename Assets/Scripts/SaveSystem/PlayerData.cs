using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int level;
    public int experience;
    public int emeralds;
    public float timeRecord;
    public int pointRecord;
    public int boostAmmount;
    public int shieldsAmmount;
    public int startLife;
    public int slowTimeLvl;
    public int shieldLvl;
    public int boostLvl;
    public int movementLvl;
    public int laser;
    public int missili;
    public int wtf;
    public bool musicOn;
    public bool soundOn;
    public bool pullMovement;

    //Options
    public float speedMovement;


    public PlayerData(GameManager gameManager)
    {
        level = gameManager.level;
        experience = gameManager.experience;
        emeralds = gameManager.emeralds;
        timeRecord = gameManager.timeRecord;
        pointRecord = gameManager.pointRecord;
        boostAmmount = gameManager.boostAmmount;
        shieldsAmmount = gameManager.shieldsAmmount;
        startLife = gameManager.startLife;
        slowTimeLvl = gameManager.slowTimeLvl;
        shieldLvl = gameManager.shieldLvl;
        boostLvl = gameManager.boostLvl;
        movementLvl = gameManager.movementLvl;
        laser = gameManager.laser;
        missili = gameManager.missili;
        wtf = gameManager.wtf;
        musicOn = gameManager.musicOn;
        soundOn = gameManager.soundOn;
        speedMovement = gameManager.GetPlayerSpeed();
        pullMovement = gameManager.pullMovement;
    }
}
