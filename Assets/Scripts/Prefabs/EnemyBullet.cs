using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    GameObject hitEffect;
    void Start()
    {
        //Physics.IgnoreLayerCollision(7, 8);
        Destroy(gameObject, 3f);
    }

    void Update()
    {
        transform.position -= (Vector3.right * speed * Time.deltaTime * (1 + (GameManager.Instance.difficultMolt * 2)));
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        GameObject.Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
