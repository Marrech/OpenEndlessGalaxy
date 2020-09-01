using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int lifes;
    public bool gameOver;
    float gameTime;
    public int maxLifes;
    bool shield;
    public int shieldDurate;
    public int boostDurate;
    public GameObject shieldObj;
    PolygonCollider2D coll;
    public Controls controller;
    SpriteRenderer[] allSprites;
    IEnumerator shieldCoroutine;
    //IEnumerator shieldBarCoroutine;
    IEnumerator shieldCoroutineTmp;
    IEnumerator blockShieldforTime;
    IEnumerator blockBoostforTime;
    public Image shieldBar;
    public Image cantUseShield;
    public Image cantUseBoost;
    float tmpShieldDur;
    float effectiveShieldDurate;


    [Header("VFX")]
    public ParticleSystem hitEffect;
    public ParticleSystem explosion;

    [Header("SFX")]
    public AudioSource hitSound;
    public AudioSource explosionSound;

    void Awake()
    {
        shieldCoroutine = WaitAndLeaveShield();
        //shieldBarCoroutine = WaitBarShield();
        blockBoostforTime = BlockBoostBtn();
        blockShieldforTime = BlockShieldBtn();
        shieldCoroutineTmp = WaitAndLeaveShield(1);
        allSprites = GetComponentsInChildren<SpriteRenderer>();
        controller = gameObject.GetComponent<Controls>();
        coll = gameObject.GetComponent<PolygonCollider2D>();
        shield = false;
        gameOver = false;
        lifes = (3 + GameManager.Instance.startLife);
        //RefreshLifesUI();
        GameManager.Instance.RefreshLifesUI();
    }

    void Update()
    {
        if (gameOver)
        {
            transform.position -= (Vector3.right * 800 * Time.deltaTime);
        }
    }

    public void StartGame()
    {
        effectiveShieldDurate = shieldDurate + (2 * (GameManager.Instance.shieldLvl));
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == 11 || shield)
        {
            return;
        }
        lifes--;
        hitSound.Play();
        GameManager.Instance.RefreshLifesUI();
        if (lifes > 0)
        {
            HitEffect();
            return;
        }
        explosion.Play();
        GetComponent<PolygonCollider2D>().enabled = false;
        SetSprites(false);
        explosionSound.Play();
        gameOver = true;
        GameManager.Instance.GameOver();
        controller.enabled = false;
        Time.timeScale = 1f;
        StartCoroutine(WaitAndStopTime(1.5f));
    }

    IEnumerator WaitAndStopTime(float time)
    {
        yield return new WaitForSeconds(time);
        GameManager.Instance.gameOverView.SetActive(true);
        gameObject.SetActive(false);
        Time.timeScale = 0;
    }

    void HitEffect()
    {
        ParticleSystem tmpEffect = Instantiate(hitEffect, gameObject.transform.position, Quaternion.identity);
        tmpEffect.Play();
        HitInvulner();
        iTween.MoveTo(tmpEffect.gameObject, iTween.Hash("position", new Vector3(tmpEffect.transform.position.x - 1000, tmpEffect.transform.position.y, tmpEffect.transform.position.z), "easetype", iTween.EaseType.linear, "time", 1.5f));
        Destroy(tmpEffect, 1.5f);
    }

    public void AddLifes(int add)
    {
        lifes += add;
        GameManager.Instance.RefreshLifesUI();
    }

    public void GetShield()
    {
        shieldObj.SetActive(true);
        tmpShieldDur = effectiveShieldDurate;
        Debug.Log("Shield Time: " + tmpShieldDur);

        if (shieldObj.activeInHierarchy)
        {
            shieldObj.SetActive(false);
            shield = false;
            StopCoroutine(shieldCoroutine);
            //StopCoroutine(shieldBarCoroutine);
            StopCoroutine(shieldCoroutineTmp);
            StopCoroutine(blockShieldforTime);
            shieldBar.gameObject.transform.parent.gameObject.SetActive(false);
            tmpShieldDur = effectiveShieldDurate;
        }
        blockShieldforTime = BlockShieldBtn();
        //shieldBarCoroutine = WaitBarShield();
        shieldCoroutine = WaitAndLeaveShield();
        StartCoroutine(shieldCoroutine);
        //StartCoroutine(shieldBarCoroutine);
        StartCoroutine(blockShieldforTime);
    }

    //IEnumerator WaitBarShield()
    //{
    //    shieldBar.gameObject.transform.parent.gameObject.SetActive(true);
    //    float duration = GameManager.Instance.timeOfGame + effectiveShieldDurate;
    //    while(GameManager.Instance.timeOfGame < duration)
    //    {
    //        shieldBar.fillAmount = Mathf.InverseLerp(1, effectiveShieldDurate, (duration - GameManager.Instance.timeOfGame));
    //        yield return null;
    //    }
    //    shieldBar.gameObject.transform.parent.gameObject.SetActive(false);
    //    tmpShieldDur = effectiveShieldDurate;
    //    //yield return new WaitForSeconds(0.2f);
    //    //Debug.Log("Shield Time: " + tmpShieldDur);
    //    //if (time > 0f)
    //    //{
    //    //    time = (time - 0.2f);
    //    //    StopCoroutine(shieldBarCoroutine);
    //    //    shieldBarCoroutine
    //    //    StartCoroutine();
    //    //}
    //    //else
    //    //{
            
    //    //}
    //}


    IEnumerator WaitAndLeaveShield()
    {
        shieldBar.gameObject.transform.parent.gameObject.SetActive(true);
        float duration = GameManager.Instance.timeOfGame + effectiveShieldDurate;
        Debug.Log("Shield Time: " + effectiveShieldDurate);
        shield = true;
        shieldObj.SetActive(true);
        while (GameManager.Instance.timeOfGame < duration)
        {
            shieldBar.fillAmount = Mathf.InverseLerp(1, effectiveShieldDurate, (duration - GameManager.Instance.timeOfGame));
            yield return null;
        }
        shieldBar.gameObject.transform.parent.gameObject.SetActive(false);
        tmpShieldDur = effectiveShieldDurate;
        //float wait = effectiveShieldDurate;
        //yield return new WaitForSeconds(wait);
        shield = false;
        shieldObj.SetActive(false);
    }

    IEnumerator BlockShieldBtn()
    {
        cantUseShield.gameObject.SetActive(true);
        cantUseShield.gameObject.transform.parent.GetComponent<Button>().interactable = false;
        float duration = GameManager.Instance.timeOfGame + (effectiveShieldDurate*2);
        while (GameManager.Instance.timeOfGame < duration)
        {
            cantUseShield.fillAmount = Mathf.InverseLerp(1, (effectiveShieldDurate * 2), (duration - GameManager.Instance.timeOfGame));
            yield return null;
        }
        cantUseShield.gameObject.transform.parent.GetComponent<Button>().interactable = true;
        cantUseShield.gameObject.SetActive(false);
    }

    IEnumerator BlockBoostBtn()
    {
        cantUseBoost.gameObject.SetActive(true);
        cantUseBoost.gameObject.transform.parent.GetComponent<Button>().interactable = false;
        float duration = GameManager.Instance.timeOfGame + (boostDurate + 45);
        while (GameManager.Instance.timeOfGame < duration)
        {
            cantUseBoost.fillAmount = Mathf.InverseLerp(1, (boostDurate + 45), (duration - GameManager.Instance.timeOfGame));
            yield return null;
        }
        cantUseBoost.gameObject.transform.parent.GetComponent<Button>().interactable = true;
        cantUseBoost.gameObject.SetActive(false);
    }

    void GetShieldForTime(int time)
    {
        shield = true;
        shieldCoroutineTmp = WaitAndLeaveShield(time);
        shieldObj.SetActive(true);
        StartCoroutine(shieldCoroutineTmp);
    }

    IEnumerator WaitAndLeaveShield(int time)
    {
        yield return new WaitForSecondsRealtime(time);
        shield = false;
        shieldObj.SetActive(false);
    }

    public void GetBoost()
    {
        StopCoroutine(blockBoostforTime);
        coll.enabled = false;
        controller.enabled = false;
        Time.timeScale = 3f;
        blockBoostforTime = BlockBoostBtn();
        StartCoroutine(WaitAndStopBoost());
        StartCoroutine(blockBoostforTime);
    }

    IEnumerator WaitAndStopBoost()
    {
        GameObject.FindGameObjectWithTag("Generator").GetComponent<Generator>().StopGenerateForTime(boostDurate + (0.77f * (GameManager.Instance.boostLvl)));
        yield return new WaitForSecondsRealtime(boostDurate + (0.77f * (GameManager.Instance.boostLvl)));
        coll.enabled = true;
        controller.enabled = true;
        Time.timeScale = 1f;
        GameObject.FindGameObjectWithTag("Generator").GetComponent<Generator>().GenerateEnemy();
    }

    void HitInvulner()
    {
        StartCoroutine(FadeOut());
        GetShieldForTime(1);
    }

    IEnumerator FadeOut()
    {
        float time = 0.5f;
        Material mat = gameObject.GetComponent<SpriteRenderer>().material;
        while (mat.color.a > 0)
        {
            Color col = mat.color;
            col.a -= Time.deltaTime / time;
            mat.color = col;
            yield return null;
        }
        foreach (SpriteRenderer spr in allSprites)
        {
            spr.material = mat;
        }
        
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float time = 0.5f;
        Material mat = gameObject.GetComponent<SpriteRenderer>().material;
        while (mat.color.a < 1)
        {
            Color col = mat.color;
            col.a += Time.deltaTime / time;
            mat.color = col;
            yield return null;

        }
        foreach (SpriteRenderer spr in allSprites)
        {
            spr.material = mat;
        }
    }

    public void SetSprites(bool value)
    {
        foreach (SpriteRenderer spr in allSprites)
        {
            spr.enabled = value;
        }
    }
}
