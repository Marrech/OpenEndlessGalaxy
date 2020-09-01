using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidScript : MonoBehaviour
{
    [SerializeField] int pointsToAdd;
    float speed;
    float rotationValue;
    float size;
    [SerializeField]
    Sprite[] asteroidTypes;
    [SerializeField] GameObject dropObj;
    int asteroidType;
    float startScale;
    float quartOfScale;
    [SerializeField]
    GameObject emerald;
    public GameObject brownHitEffect;
    public GameObject grayHitEffect;
    GameObject particleEffect;

    void Start()
    {
        speed = 800f;
        speed = (speed + Random.Range(0, (speed / 100) * 50));
        asteroidType = Random.Range(0, asteroidTypes.Length);
        GetComponent<SpriteRenderer>().sprite = asteroidTypes[asteroidType];
        if(asteroidType == 4 || asteroidType == 5 || asteroidType == 6 || asteroidType == 7)
        {
            particleEffect = grayHitEffect;
        }
        else
        {
            particleEffect = brownHitEffect;
        }
        rotationValue = Random.Range(70f, 250f);
        size = Random.Range(2f, 2.3f);
        transform.localScale = transform.localScale * size;
        gameObject.AddComponent<PolygonCollider2D>();
        startScale = transform.localScale.x;
        quartOfScale = (startScale / 100) * 25;
        Destroy(gameObject, 6f);
        //iTween.RotateBy(gameObject, iTween.Hash("z", Random.Range(0.2f,1f), "easetype", "linear", "looptype", iTween.LoopType.none));
    }
    void Update()
    {
        transform.position -= (Vector3.right * speed * Time.deltaTime * (1 + (GameManager.Instance.difficultMolt * 2)));
        transform.Rotate(new Vector3(0,0,rotationValue) * Time.deltaTime);
    }

    IEnumerator HitEffect()
    {
        GameObject tmpHit = GameObject.Instantiate(particleEffect, transform.position, Quaternion.identity);
        tmpHit.GetComponent<ParticleSystem>().Play();
        iTween.MoveTo(tmpHit, iTween.Hash("position", new Vector3(tmpHit.transform.position.x - 1900, tmpHit.transform.position.y, tmpHit.transform.position.z), "easetype", iTween.EaseType.linear, "time", 1.5f));
        yield return new WaitForSeconds(1.5f);
        Destroy(tmpHit);
    }

    IEnumerator WaitEffectAndDestroy()
    {
        if(asteroidType != 8 && asteroidType != 9)
        {
            bool dropped = false;
            if (GameManager.Instance.GetResponseInPerc(35, 50) && !dropped)
            {
                PowerUp power = Instantiate(dropObj, gameObject.transform.position, Quaternion.identity).GetComponent<PowerUp>(); //Drop life
                power.PowerUP(0);
                dropped = true;
            }
            if (!dropped && GameManager.Instance.GetResponseInPerc(20,28))
            {
                PowerUp power = Instantiate(dropObj, gameObject.transform.position, Quaternion.identity).GetComponent<PowerUp>(); //Drop schield
                power.PowerUP(1);
                dropped = true;
            }
            if (!dropped && GameManager.Instance.GetResponseInPerc(64, 68))
            {
                PowerUp power = Instantiate(dropObj, gameObject.transform.position, Quaternion.identity).GetComponent<PowerUp>(); //Drop boost
                power.PowerUP(2);
                dropped = true;
            }
        }
        GetComponent<PolygonCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        GameObject tmpHit = GameObject.Instantiate(particleEffect, transform.position, Quaternion.identity);
        tmpHit.GetComponent<ParticleSystem>().Play();
        GameManager.Instance.PlayAsteroidExplosion();
        iTween.MoveTo(tmpHit, iTween.Hash("position", new Vector3(tmpHit.transform.position.x - 1900, tmpHit.transform.position.y, tmpHit.transform.position.z), "easetype", iTween.EaseType.linear, "time", 1.5f));
        yield return new WaitForSeconds(1.5f);
        Destroy(tmpHit);
        Destroy(gameObject);
    }

    void ScaleQuart(float molt)
    {
        StartCoroutine(HitEffect());
        transform.localScale -= new Vector3(quartOfScale, quartOfScale, 0) * molt;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.layer == 9)
        {
            switch (asteroidType)
            {
                case 0:
                    StartCoroutine(WaitEffectAndDestroy());
                    return;
                case 1:
                    ScaleQuart(0.5f);
                    break;
                case 2:
                    ScaleQuart(1f);
                    break;
                case 3:
                    ScaleQuart(0.5f);
                    break;
                case 4:
                    break;
                case 5:
                    return;
                case 6:
                    break;
                case 7:
                    break;
                case 8:
                    ScaleQuart(0.5f);
                    break;
                case 9:
                    ScaleQuart(1f);
                    break;
            }
        }
        if (transform.localScale.x <= (quartOfScale*2))
        {
            if(asteroidType == 8 || asteroidType == 9)
            {
                GameObject.Instantiate(emerald, gameObject.transform.position, Quaternion.identity);
            }
            GameManager.Instance.AddPoints(pointsToAdd);
            StartCoroutine(WaitEffectAndDestroy());
        }
    }

    public void DestroyMe() 
    {
        if (asteroidType == 8 || asteroidType == 9)
        {
            GameObject.Instantiate(emerald, gameObject.transform.position, Quaternion.identity);
        }
        GameManager.Instance.AddPoints(pointsToAdd);
        StartCoroutine(WaitEffectAndDestroy());
    }
}
