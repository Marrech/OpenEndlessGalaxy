using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Controls : MonoBehaviour
{
    public bool pullMovement;
    int inputofMove;

    [Header ("Movement")]
    public FollowWorld indicator;
    Vector3 pressedPoint;
    Vector3 moveToPoint;
    Vector3 finalPoint;
    public Canvas canvas;
    public float movementSpeed;

    [Header("Weapons")]
    public GameObject gunLeft;
    public GameObject gunRight;
    
    public GameObject[] weapons;

    public AudioSource[] laserSound;

    [Header("UIButtons")]
    public Button openFireBtn;
    public Button useShieldBtn;
    public Button useBoostBtn;
    public Image openFireCantUse;
    [SerializeField] TextMeshProUGUI txtBoostQuantity;
    [SerializeField] TextMeshProUGUI txtShieldQuantity;

    private void Awake()
    {
        openFireBtn.onClick.AddListener(delegate () { OnOpenFireButtonClick(); });
        useShieldBtn.onClick.AddListener(delegate () { UseShield(); });
        useBoostBtn.onClick.AddListener(delegate () { UseBoost(); });
    }


    private void Update()
    {
        if(Time.timeScale != 1)
        {
            openFireCantUse.gameObject.SetActive(true);
        }
        else
        {
            openFireCantUse.gameObject.SetActive(false);
        }
        Move();
    }

    void Move()
    {
        if (Input.touchCount > 0 && GameManager.Instance.inGame)
        {
            foreach (Touch touch in Input.touches)
            {
                //Touch touch = Input.GetTouch(0);
                if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    //inputofMove = touch.fingerId;
                    switch (touch.phase)
                    {
                        case TouchPhase.Began:
                            inputofMove = touch.fingerId;
                            Time.timeScale = (0.5f - ((0.025f) * GameManager.Instance.slowTimeLvl));
                            pressedPoint = touch.position;
                            if(pullMovement == true)
                            {
                                moveToPoint = touch.position;
                                finalPoint = new Vector3(moveToPoint.x, moveToPoint.y, 0);
                                finalPoint -= gameObject.transform.position;
                                indicator.SetOffset(finalPoint);
                            }
                            indicator.gameObject.SetActive(true);
                            break;

                        case TouchPhase.Moved:
                            if (touch.fingerId != inputofMove) { return; }
                            moveToPoint = touch.position;
                            if(pullMovement == false) 
                            {
                                finalPoint = new Vector3((pressedPoint.x - moveToPoint.x) * movementSpeed, (pressedPoint.y - moveToPoint.y) * movementSpeed, 0);
                            }
                            else
                            {
                                finalPoint = new Vector3(moveToPoint.x, moveToPoint.y, 0);
                                finalPoint -= gameObject.transform.position;
                            }
                            indicator.SetOffset(finalPoint);
                            break;

                        case TouchPhase.Ended:
                            if (touch.fingerId != inputofMove) { return; }
                            Vector3 proportion = Camera.main.ScreenToViewportPoint(indicator.gameObject.transform.position);
                            Vector3 goToPoint = new Vector3((1950) * proportion.x, (1080) * proportion.y, 0);
                            if (proportion.y >= 1f) { goToPoint = new Vector3(goToPoint.x, 1030f, goToPoint.z); };
                            if (proportion.y <= 0f) { goToPoint = new Vector3(goToPoint.x, 52f, goToPoint.z); };
                            if (proportion.x >= 1f) { goToPoint = new Vector3(1875f, goToPoint.y, goToPoint.z); };
                            if (proportion.x <= 0f) { goToPoint = new Vector3(40f, goToPoint.y, goToPoint.z); };
                            Time.timeScale = 1f;
                            iTween.MoveTo(gameObject, goToPoint, (0.6f - ((0.05f) * GameManager.Instance.movementLvl)));
                            indicator.SetOffset(Vector3.zero);
                            indicator.gameObject.SetActive(false);
                            inputofMove = 99;
                            break;
                    }
                }
            }
        }
        else if (inputofMove != 99)
        {
            inputofMove = 99;
        }
    }

    void OnOpenFireButtonClick()
    {
        if(Time.timeScale != 1f || !GameManager.Instance.inGame) { return; }
        Instantiate(weapons[0], new Vector3(gunLeft.transform.position.x + Random.Range(-70f, 70f), gunLeft.transform.position.y, gunLeft.transform.position.z), gunLeft.transform.rotation);
        Instantiate(weapons[0], new Vector3(gunRight.transform.position.x + Random.Range(-70f, 70f), gunRight.transform.position.y, gunRight.transform.position.z), gunRight.transform.rotation);
        laserSound[Random.Range(0, laserSound.Length - 1)].Play();
    }

    void UseShield()
    {
        if(GameManager.Instance.shieldsAmmount <= 0) { return; }
        GetComponent<Player>().GetShield();
        GameManager.Instance.shieldsAmmount--;
        RefrshUI();
    }

    void UseBoost()
    {
        if (GameManager.Instance.boostAmmount <= 0) { return; }
        GameManager.Instance.GetBoost(2);
        GameManager.Instance.boostAmmount--;
        RefrshUI();
    }

    public void RefrshUI()
    {
        txtBoostQuantity.text = GameManager.Instance.boostAmmount.ToString();
        txtShieldQuantity.text = GameManager.Instance.shieldsAmmount.ToString();
    }

    
}
