using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emerald : MonoBehaviour
{
    [SerializeField] int value;
    [SerializeField] float speed;
    [SerializeField] float rotationValue;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= (Vector3.right * speed * Time.deltaTime);
        transform.Rotate(new Vector3(0, 0, rotationValue) * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if( col.gameObject.layer != 8) { return; }
        GameManager.Instance.AddEmerals(value);
        Destroy(gameObject);
    }
}
