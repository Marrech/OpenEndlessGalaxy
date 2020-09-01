using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    GameObject hitEffect;
    public float damage;
    void Start()
    {
        Physics.IgnoreLayerCollision(7, 8);
        Destroy(gameObject, 1.55f);
    }

    void Update()
    {
        transform.position -= (Vector3.left * speed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.gameObject.layer == 12)
        {
            col.gameObject.GetComponentInChildren<Enemy>().ApplyDamage(damage);
        }
        GameObject.Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
