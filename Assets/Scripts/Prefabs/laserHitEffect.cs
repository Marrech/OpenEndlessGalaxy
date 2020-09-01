using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserHitEffect : MonoBehaviour
{
    public float duration;
    //public float speed;
    void Start()
    {
        GetComponent<ParticleSystem>().Play();
        //iTween.MoveTo(gameObject, iTween.Hash("position", new Vector3(transform.position.x - speed, transform.position.y, transform.position.z), "easetype", iTween.EaseType.linear, "time", 0.5f));
        Destroy(gameObject, duration);
    }
}
