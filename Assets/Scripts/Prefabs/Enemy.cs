using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int pointsToAdd;
    [SerializeField] float impactDmg;
    [SerializeField] float HP;
    [SerializeField] PolygonCollider2D thisColl;
    [SerializeField] ParticleSystem hitEffect;
    [SerializeField] ParticleSystem explosion;
    [SerializeField] EnemyAI AI;
    [SerializeField] AudioSource hitSound;
    [SerializeField] AudioSource explosionSound;
    public bool destroyed;

    //[SerializeField] GameObject enemy;

    private void Start()
    {
        destroyed = false;
        thisColl.enabled = false;
        AI.enabled = false;
        StartCoroutine(Init());
    }

    void Update()
    {
        if (destroyed)
        {
            transform.position -= (Vector3.right * 800 * Time.deltaTime);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (GameManager.Instance.soundOn)
        {
            hitSound.Play();
        }
        if (col.transform.gameObject.layer == 10)
        {
            HP = HP - impactDmg;
        }
        if(HP > 0)
        {
            HitEffect();
            return;
        }
        if (col.transform.gameObject.layer == 9)
        {
            GameManager.Instance.AddPoints(pointsToAdd);
        }
        SpriteRenderer[] allSprites = GetComponentsInChildren<SpriteRenderer>();
        explosion.Play();
        thisColl.enabled = false;
        AI.enabled = false;
        foreach (SpriteRenderer spr in allSprites)
        {
            spr.enabled = false;
        }
        destroyed = true;
        GameObject.Find("GameManager").GetComponent<GameManager>().enemySpawned = false;
        if (GameManager.Instance.soundOn)
        {
            explosionSound.Play();
        }
        Destroy(gameObject, 3f);
    }

    //public void DestroyMe()
    //{
    //    GameManager.Instance.AddPoints(pointsToAdd);
    //    SpriteRenderer[] allSprites = GetComponentsInChildren<SpriteRenderer>();
    //    explosion.Play();
    //    thisColl.enabled = false;
    //    AI.enabled = false;
    //    foreach (SpriteRenderer spr in allSprites)
    //    {
    //        spr.enabled = false;
    //    }
    //    explosionSound.Play();
    //    destroyed = true;
    //    GameObject.Find("GameManager").GetComponent<GameManager>().enemySpawned = false;
    //    Destroy(gameObject, 1.5f);
    //}

    void HitEffect()
    {
        ParticleSystem tmpEffect = Instantiate(hitEffect, gameObject.transform.position, Quaternion.Euler(0,0,180));
        tmpEffect.Play();
        iTween.MoveTo(tmpEffect.gameObject, iTween.Hash("position", new Vector3(tmpEffect.transform.position.x - 1000, tmpEffect.transform.position.y, tmpEffect.transform.position.z), "easetype", iTween.EaseType.linear, "time", 1.5f));
        Destroy(tmpEffect, 1.5f);
    }

    public void ApplyDamage(float damage)
    {
        HP -= damage;
    }

    IEnumerator Init()
    {
        iTween.MoveTo(gameObject, iTween.Hash("position", new Vector3(1500, transform.position.y, 0), "easetype", iTween.EaseType.easeInOutQuart, "time", 1f, "space", Space.World));
        yield return new WaitForSeconds(1f);
        thisColl.enabled = true;
        AI.enabled = true;
    }

    public void InstaDestroy()
    {
        GameManager.Instance.AddPoints(pointsToAdd);
        SpriteRenderer[] allSprites = GetComponentsInChildren<SpriteRenderer>();
        explosion.Play();
        thisColl.enabled = false;
        AI.enabled = false;
        foreach (SpriteRenderer spr in allSprites)
        {
            spr.enabled = false;
        }
        destroyed = true;
        GameObject.Find("GameManager").GetComponent<GameManager>().enemySpawned = false;
        if (GameManager.Instance.soundOn)
        {
            explosionSound.Play();
        }
        Destroy(gameObject, 3f);
    }
}
