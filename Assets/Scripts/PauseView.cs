using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseView : MonoBehaviour
{
    public Button resumeBtn;
    public Button backToMainMenuBtn;
    public Toggle immortalTestTog;
    public Slider movementSlider;
    public Button changeMusic;
    public Button changeSound;
    public Toggle pullToggle;


    private void Awake()
    {
        GameManager.Instance.SetPlayerSpeed(movementSlider.value);
        resumeBtn.onClick.AddListener(delegate () { OnResumeButtonClick(); });
        backToMainMenuBtn.onClick.AddListener(delegate () 
        {
            GameManager.Instance.GameOver();
            GameManager.Instance.BackToMainMenu();
        });
        immortalTestTog.onValueChanged.AddListener(delegate { ImmortalValueChanged(immortalTestTog.isOn); });
        movementSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        changeMusic.onClick.AddListener(delegate () { GameManager.Instance.ChangeMusicState(); });
        changeSound.onClick.AddListener(delegate () { GameManager.Instance.ChangeSoundState(); });
        pullToggle.onValueChanged.AddListener(delegate { GameManager.Instance.ChangeMovementModePause(); });
    }

    void OnResumeButtonClick()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
        GameObject.Find("GameManager").GetComponent<GameManager>().inPause = false;
    }

    void ImmortalValueChanged(bool isOn)
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PolygonCollider2D>().enabled = !isOn;
    }

    public void ValueChangeCheck()
    {
        GameManager.Instance.SetPlayerSpeed(movementSlider.value);
    }
}
