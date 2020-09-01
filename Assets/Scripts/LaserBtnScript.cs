using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LaserBtnScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public LayerMask layer_masks;
    public GameObject topLaserObj;
    public LineRenderer topLaser;
    bool topLaserCoroutineActive = false;
    public Transform topLaserHit;
    RaycastHit2D hitTop;
    public GameObject downLaserObj;
    public LineRenderer downLaser;
    bool downLaserCoroutineActive = false;
    public Transform downLaserHit;
    RaycastHit2D hitDown;
    bool pressed = false;
    [SerializeField] float timeForDestroyLaser;
    [SerializeField] float maxTimeLaser;
    float timeRecharge;
    public Image chargeIndicator;
    public Image chargeIndicatorTop;
    bool canUse;

    public void OnPointerDown(PointerEventData eventData)
    {
        topLaser.gameObject.SetActive(true);
        downLaser.gameObject.SetActive(true);
        pressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        topLaser.gameObject.SetActive(false);
        downLaser.gameObject.SetActive(false);
        pressed = false;
    }

    private void Start()
    {
        canUse = true;
        float tmpTopTime = Random.Range(0.3f, 0.7f);
        float tmpDownTime = Random.Range(0.3f, 0.7f);
        iTween.ScaleTo(topLaser.gameObject, iTween.Hash("x", 0.125f, "looptype", iTween.LoopType.pingPong, "time", tmpTopTime));
        iTween.ScaleTo(downLaser.gameObject, iTween.Hash("x", 0.125f, "looptype", iTween.LoopType.pingPong, "time", tmpDownTime));
    }

    void Update()
    {
        chargeIndicator.fillAmount = Mathf.InverseLerp(0, maxTimeLaser, timeRecharge);
        chargeIndicatorTop.fillAmount = Mathf.InverseLerp(0, maxTimeLaser, timeRecharge);
        if (pressed && canUse && timeRecharge < maxTimeLaser && GameManager.Instance.inGame)
        {
            Laser();
            chargeIndicatorTop.color = Color.green;
            GameManager.Instance.PlayLaserSound();
        }
        else
        {
            GameManager.Instance.StopLaserSound();
            chargeIndicatorTop.color = Color.red;
            topLaser.SetPosition(0, topLaser.transform.position);
            downLaser.SetPosition(0, downLaser.transform.position);
            topLaser.SetPosition(1, topLaser.transform.position);
            downLaser.SetPosition(1, downLaser.transform.position);
            topLaserHit.position = new Vector3(100000f,100000f,0f);
            downLaserHit.position = new Vector3(100000f, 100000f, 0f);
            if (timeRecharge < 0)
            {
                timeRecharge = 0;
            }
            if(timeRecharge > 0)
            {
                timeRecharge -= Time.deltaTime * 2;
            }
            if (timeRecharge == 0)
            {
                canUse = true;
            }
            else
            {
                canUse = false;
            }
        }
    }

    void Laser()
    {
        if(timeRecharge == 0)
        {
            timeRecharge = 1f;
        }
        timeRecharge += Time.deltaTime;
        //Debug.DrawRay(rays[3].transform.position, right, Color.green, 1f);
        topLaser.SetPosition(0, topLaserObj.transform.position);
        downLaser.SetPosition(0, downLaserObj.transform.position);
        Vector3 right = transform.TransformDirection(Vector3.right) * 400;
        hitTop = Physics2D.Raycast(topLaserObj.transform.position + new Vector3(100, 0, 0), right, 1800f, layer_masks);
        Debug.DrawRay(downLaserObj.transform.position + new Vector3(100, 0, 0), right, Color.green, 0.05f);

        Debug.DrawRay(downLaserObj.transform.position + new Vector3(100, 0, 0), right, Color.green, 0.05f);
        hitDown = Physics2D.Raycast(downLaserObj.transform.position + new Vector3(100, 0, 0), right, 1800f, layer_masks);
        if (hitTop.transform != null)
        {
            topLaser.SetPosition(1, hitTop.point);
        }
        else
        {
            topLaser.SetPosition(1, topLaserObj.transform.position + (Vector3.right * 3000));
        }
        if (hitDown.transform != null)
        {
            downLaser.SetPosition(1, hitDown.point);
        }
        else
        {
            downLaser.SetPosition(1, downLaserObj.transform.position + (Vector3.right * 3000));
        }
        if (!topLaserCoroutineActive && hitTop.transform != null)
        {
            StartCoroutine(TopLaserHit());
        }
        if (!downLaserCoroutineActive && hitDown.transform != null)
        {
            StartCoroutine(DownLaserHit());
        }
        topLaserHit.position = topLaser.GetPosition(1) - (Vector3.right * 10);

        downLaserHit.position = downLaser.GetPosition(1) - (Vector3.right * 10);
    }

    IEnumerator TopLaserHit()
    {
        topLaserCoroutineActive = true;
        GameObject firstHit = hitTop.transform.gameObject;
        yield return new WaitForSecondsRealtime(timeForDestroyLaser);
        if (hitTop.transform != null && pressed)
        {
            if (GameObject.ReferenceEquals(hitTop.transform.gameObject, firstHit) /*|| GameObject.ReferenceEquals(hitDown.transform.gameObject, firstHit)*/)
            {
                if (string.Equals(hitTop.transform.tag, "Asteroid"))
                {
                    hitTop.transform.gameObject.GetComponent<AsteroidScript>().DestroyMe();
                }
                else if (string.Equals(hitTop.transform.tag, "Enemy"))
                {
                    hitTop.transform.gameObject.GetComponent<Enemy>().InstaDestroy();
                }
                else if (string.Equals(hitTop.transform.tag, "EnemyBullet"))
                {
                    Debug.Log("EnemyBullet");
                    Destroy(hitTop.transform.gameObject);
                }
            }
        }
        topLaserCoroutineActive = false;

    }
    IEnumerator DownLaserHit()
    {
        downLaserCoroutineActive = true;
        GameObject firstHit = hitDown.transform.gameObject;
        yield return new WaitForSecondsRealtime(timeForDestroyLaser);
        if (hitDown.transform != null && pressed)
        {
            if (GameObject.ReferenceEquals(hitDown.transform.gameObject, firstHit) /*|| GameObject.ReferenceEquals(hitTop.transform.gameObject, firstHit)*/)
            {
                if(string.Equals(hitDown.transform.tag, "Asteroid"))
                {
                    hitDown.transform.gameObject.GetComponent<AsteroidScript>().DestroyMe();
                }
                else if (string.Equals(hitDown.transform.tag, "Enemy"))
                {
                    hitDown.transform.gameObject.GetComponent<Enemy>().InstaDestroy();
                }
                else if (string.Equals(hitDown.transform.tag, "EnemyBullet"))
                {
                    Debug.Log("EnemyBullet");
                    Destroy(hitDown.transform.gameObject);
                }
                //else
                //{
                //    Destroy(hitDown.transform.gameObject);
                //}
            }
        }
        downLaserCoroutineActive = false;
    }
}
