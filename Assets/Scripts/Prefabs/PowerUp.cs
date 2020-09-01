using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public int powerType;
    public float speed;
    [SerializeField] Sprite[] image;
    

    public void PowerUP(int type)
    {
        powerType = type;
        gameObject.GetComponent<SpriteRenderer>().sprite = image[powerType];
        Destroy(gameObject, 6f);
    }

    void Update()
    {
        transform.position -= (Vector3.right * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer != 8) { return; }
        GameManager.Instance.GetBoost(powerType);
        Destroy(gameObject);
    }
}
