using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    int clickForCloseGame;

    public static GameManager Instance { get; set; }
    [SerializeField] Player player;
    [SerializeField] Controls playerControls;
    [SerializeField] Generator generator;
    [SerializeField] Transform defaultPlayerPosition;

    [Header("Moltiplicators")]
    public int experienceLvl1;
    public int pointsAtSec;
    public double experienceMolti;
    public float difficultMolt;

    #region PlayerData
    [Header("PlayerData")]
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
    public float movementSpeed;
    public int experienceForLevelUp;
    public bool musicOn;
    public bool soundOn;
    public bool pullMovement;
    #endregion

    [Header("GameInfo")]
    public bool inGame;
    public bool inPause;
    int gamePoints;
    public int enemyIngame;
    public bool enemySpawned;

    #region GameUI
    [Header("InGameView")]
    public float timeOfGame;
    public GameObject lifesUIObj;
    public Sprite emptyLife;
    public Sprite fullLife;
    [SerializeField] TextMeshProUGUI minsLbl;
    [SerializeField] TextMeshProUGUI secLbl;
    public TextMeshProUGUI pointsIndicator;
    public Image[] lifesUI;
    public TextMeshProUGUI emeraldsInGameTxt;
    public Button pauseBtn;
    #endregion

    [Header("GameOver")]
    public GameObject gameOverView;
    public Button restartGameBtn;

    #region MainMenuUI
    [Header("MainMenu")]
    public Button startGame;
    //public Button rewardAdButton;
    public Button upgradesButton;
    public Button helpButton;
    public Button closeHelpButton;
    public Button changeMusic;
    public Image mutedMusicImg;
    public Image mutedMusicImgPause;
    public Button changeSound;
    public Image mutedSoundImg;
    public Image mutedSoundImgPause;
    public Image expBar;
    public TextMeshProUGUI explbl;
    public TextMeshProUGUI levelLbl;
    public Toggle pullToggle;
    public TextMeshProUGUI pointsRecordTxt;
    public TextMeshProUGUI minRecordTxt;
    public TextMeshProUGUI secRecordTxt;
    public TextMeshProUGUI emeraldsOwnedTxt;
    [SerializeField] GameObject helpViewObj;
    #endregion

    [Header("Sounds")]
    [SerializeField] AudioSource dropSound;
    [SerializeField] AudioSource laserSound;
    bool soundLaserOn;
    public AudioSource[] asteroidExpSound;
    [SerializeField] AudioSource musicBG;
    [SerializeField] AudioSource[] allSFX;

    [Header("Views")]
    [SerializeField] PauseView pauseView;
    [SerializeField] GameObject mainView;
    [SerializeField] UpgradesView upgradesView;

    void Awake()
    {
        soundLaserOn = false;
        enemySpawned = false;
        inGame = false;
        inPause = false;
        timeOfGame = 0;
        Instance = this;
        restartGameBtn.onClick.AddListener(delegate () { BackToMainMenu(); });
        pauseBtn.onClick.AddListener(delegate () { OnPauseButtonClick(); });
        startGame.onClick.AddListener(delegate () { StartGame(); });
        upgradesButton.onClick.AddListener(delegate () { OpenUpgradesView(); });
        helpButton.onClick.AddListener(delegate () { OpenHelpView(); });
        closeHelpButton.onClick.AddListener(delegate () { CloseHelpView(); });
        changeMusic.onClick.AddListener(delegate () { ChangeMusicState(); });
        changeSound.onClick.AddListener(delegate () { ChangeSoundState(); });
        pullToggle.onValueChanged.AddListener(delegate { ChangeMovementMode(); });
    }

    private void Start()
    {
        SetDefaultPlayerData();
        if (SaveSystem.SaveExist())
        {
            LoadPlayer();
        }
        else
        {
            SavePlayer();
        }
        InitUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            clickForCloseGame++;
            if (inGame && !inPause)
            {
                OnPauseButtonClick();
            }

            if (clickForCloseGame > 1 && !inGame)
            {
                Application.Quit();
            }

            StartCoroutine(ResetCloseGameClick());
        }

        if (!inGame) { return; };

        minsLbl.text = Mathf.Floor(timeOfGame / 60).ToString("00");
        secLbl.text = (timeOfGame % 60).ToString("00");
        pointsIndicator.text = gamePoints.ToString();
        enemyIngame = GameObject.FindGameObjectsWithTag("Enemy").Length;
        emeraldsInGameTxt.text = emeralds.ToString();

        if (enemyIngame == 0 && generator.GetGenera() && enemySpawned == false)
        {
            enemySpawned = true;
            generator.GenerateEnemy();
        }

        if (timeOfGame <= 600)
        {
            difficultMolt = Mathf.InverseLerp(0, 600, timeOfGame);
        }
    }

    IEnumerator ResetCloseGameClick()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        clickForCloseGame = 0;
    }

    IEnumerator GetSecNotScaled()
    {
        yield return new WaitForSecondsRealtime(1f);
        if (inGame && !inPause)
        {
            timeOfGame++;
        }
        StartCoroutine(GetSecNotScaled());
    }

    public void RefreshMainView()
    {
        CheckForLevelUP();
        if (level < 100)
        {
            expBar.fillAmount = Mathf.InverseLerp(0, experienceForLevelUp, experience);
        }
        else
        {
            expBar.transform.parent.gameObject.SetActive(false);
            explbl.text = "Exp: MAX";
        }
        pointsRecordTxt.text = pointRecord.ToString();
        minRecordTxt.text = Mathf.Floor(timeRecord / 60).ToString("00");
        secRecordTxt.text = (timeRecord % 60).ToString("00");
        emeraldsOwnedTxt.text = emeralds.ToString();
    }

    void CheckForLevelUP()
    {
        experienceForLevelUp = 1024 * level * level;
        if (experience >= experienceForLevelUp && level < 100)
        {
            level++;
            experience = experience - experienceForLevelUp; ;
            experienceForLevelUp = experienceLvl1 * level * level;
            levelLbl.text = level.ToString();
            CheckForLevelUP();
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    void OpenUpgradesView()
    {
        upgradesView.gameObject.SetActive(true);
    }

    void StartGame()
    {
        player.lifes = (3 + GameManager.Instance.startLife);
        player.GetComponent<Controls>().RefrshUI();
        player.StartGame();
        RefreshLifesUI();
        mainView.SetActive(false);
        Time.timeScale = 1;
        inGame = true;
        StartCoroutine(AddPointsEverySec());
        StartCoroutine(GetSecNotScaled());
        SavePlayer();
    }

    void RestartGame()
    {
        StartCoroutine(AddPointsEverySec());
        player.GetComponent<Controls>().RefrshUI();
        RefreshLifesUI();
    }

    IEnumerator AddPointsEverySec()
    {
        yield return new WaitForSeconds(0.1f);
        if (inGame)
        {
            gamePoints += (int)((pointsAtSec / 10) * (1 + (level / 100)) * (1 + difficultMolt));
            StartCoroutine(AddPointsEverySec());
        }
    }

    public void RefreshLifesUI()
    {
        if (player.lifes > player.maxLifes)
        {
            player.lifes = player.maxLifes;
        }
        for (int i = 0; i < lifesUI.Length; i++)
        {
            if (i < player.lifes)
            {
                lifesUI[i].sprite = fullLife;
            }
            else
            {
                lifesUI[i].sprite = emptyLife;
            }
        }
    }

    void OnPauseButtonClick()
    {
        Time.timeScale = 0f;
        pauseView.gameObject.SetActive(true);
        inPause = true;
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
        RefreshMainView();
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        level = data.level;
        experience = data.experience;
        emeralds = data.emeralds;
        timeRecord = data.timeRecord;
        pointRecord = data.pointRecord;
        boostAmmount = data.boostAmmount;
        shieldsAmmount = data.shieldsAmmount;
        startLife = data.startLife;
        slowTimeLvl = data.slowTimeLvl;
        shieldLvl = data.shieldLvl;
        boostLvl = data.boostLvl;
        movementLvl = data.movementLvl;
        laser = data.laser;
        missili = data.missili;
        wtf = data.wtf;
        soundOn = data.soundOn;
        musicOn = data.musicOn;
        SetPlayerSpeed(data.speedMovement);
        pauseView.movementSlider.value = data.speedMovement;
        if (slowTimeLvl == 0) { slowTimeLvl = 1; };
        if (shieldLvl == 0) { shieldLvl = 1; };
        if (boostLvl == 0) { boostLvl = 1; };
        if (movementLvl == 0) { movementLvl = 1; };
        pullToggle.isOn = data.pullMovement;
        ChangeMovementMode();
        ChangeMovementModePause();
    }

    public void AddExperience()
    {
        experience += (int)(gamePoints * (level * experienceMolti));
    }

    public void AddExperience(int exp)
    {
        experience += exp;
    }

    public void AddPoints(int points)
    {
        gamePoints += (int)(points * (1 + (level / 100)));
    }

    public void AddEmerals(int quantity)
    {
        emeralds += (int)(((float)quantity * (1f + (level / 15f))) / 2);
    }

    public void UseEmerals(int quantity)
    {
        emeralds -= quantity;
    }

    public void GameOver()
    {
        inGame = false;
        if (gamePoints > pointRecord)
        {
            pointRecord = gamePoints;
        }
        if (timeOfGame > timeRecord)
        {
            timeRecord = timeOfGame;
        }
        AddExperience();
        SavePlayer();
    }

    public bool GetResponseInPerc(int min, int max)
    {
        int num = Random.Range(0, 100);
        if (num >= min && num <= max)
        {
            return true;
        }
        return false;
    }

    public void GetBoost(int boostCode)
    {
        dropSound.Play();
        switch (boostCode)
        {
            case 0:
                player.AddLifes(1);
                break;
            case 1:
                player.GetShield();
                break;
            case 2:
                player.GetBoost();
                DestroyAllEnemys();
                break;
        }
    }

    public void DestroyAllEnemys()
    {
        GameObject[] enemysObj = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemysObj)
        {
            enemy.GetComponent<Enemy>().InstaDestroy();
        }
    }

    public void SetPlayerSpeed(float speed)
    {
        playerControls.movementSpeed = speed;
        pauseView.movementSlider.value = speed;
    }

    public float GetPlayerSpeed()
    {
        return playerControls.movementSpeed;
    }

    public void PlayAsteroidExplosion()
    {
        asteroidExpSound[Random.Range(0, asteroidExpSound.Length - 1)].Play();
    }

    public void PlayLaserSound()
    {
        if (!soundLaserOn)
        {
            laserSound.Play();
            soundLaserOn = true;
        }
    }
    public void StopLaserSound()
    {
        if (soundLaserOn)
        {
            laserSound.Stop();
            soundLaserOn = false;
        }
    }

    public void CheckForMusicOn()
    {
        if (musicOn)
        {
            musicBG.gameObject.SetActive(true);
            mutedMusicImg.gameObject.SetActive(false);
            mutedMusicImgPause.gameObject.SetActive(false);
        }
        else
        {
            musicBG.gameObject.SetActive(false);
            mutedMusicImg.gameObject.SetActive(true);
            mutedMusicImgPause.gameObject.SetActive(true);
        }
    }

    public void CheckForSoundOn()
    {
        if (soundOn)
        {
            foreach (AudioSource sfx in allSFX)
            {
                sfx.gameObject.SetActive(true);
            }
            mutedSoundImg.gameObject.SetActive(false);
            mutedSoundImgPause.gameObject.SetActive(false);
        }
        else
        {
            foreach (AudioSource sfx in allSFX)
            {
                sfx.gameObject.SetActive(false);
            }
            mutedSoundImg.gameObject.SetActive(true);
            mutedSoundImgPause.gameObject.SetActive(true);
        }
    }

    public void ChangeMusicState()
    {
        if (musicOn)
        {
            musicOn = false;
        }
        else
        {
            musicOn = true;
        }
        CheckForMusicOn();
        SavePlayer();
    }

    public void ChangeSoundState()
    {
        if (soundOn)
        {
            soundOn = false;
        }
        else
        {
            soundOn = true;
        }
        CheckForSoundOn();
        SavePlayer();
    }

    public void ChangeMovementMode()
    {
        playerControls.pullMovement = pullToggle.isOn;
        pullMovement = pullToggle.isOn;
        pauseView.pullToggle.isOn = pullToggle.isOn;
        SavePlayer();
    }

    public void ChangeMovementModePause()
    {
        playerControls.pullMovement = pauseView.pullToggle.isOn;
        pullMovement = pauseView.pullToggle.isOn;
        pullToggle.isOn = pauseView.pullToggle.isOn;
        SavePlayer();
    }

    void OpenHelpView()
    {
        helpViewObj.SetActive(true);
    }

    void CloseHelpView()
    {
        helpViewObj.SetActive(false);
    }

    void SetDefaultPlayerData()
    {
        clickForCloseGame = 0;
        Time.timeScale = 0f;
        level = 1;
        experience = 1;
        emeralds = 0;
        timeRecord = 0;
        pointRecord = 0;
        boostAmmount = 0;
        shieldsAmmount = 0;
        startLife = 0;
        slowTimeLvl = 1;
        shieldLvl = 1;
        boostLvl = 1;
        movementLvl = 1;
        laser = 0;
        missili = 0;
        wtf = 0;
        soundOn = true;
        musicOn = true;
        pullMovement = true;
        SetPlayerSpeed(2.5f);
    }

    void InitUI()
    {
        CheckForLevelUP();
        CheckForMusicOn();
        CheckForSoundOn();
        levelLbl.text = level.ToString();
        if (level < 100)
        {
            expBar.fillAmount = Mathf.InverseLerp(0, experienceForLevelUp, experience);
        }
        else
        {
            expBar.transform.parent.gameObject.SetActive(false);
            explbl.text = "Exp: MAX";
        }
        pointsRecordTxt.text = pointRecord.ToString();
        minRecordTxt.text = Mathf.Floor(timeRecord / 60).ToString("00");
        secRecordTxt.text = (timeRecord % 60).ToString("00");
        emeraldsOwnedTxt.text = emeralds.ToString();
        upgradesView.RefreshUI();
    }
}
