using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] GameObject[] rays;
    [SerializeField] RaycastHit2D[] hits;
    [SerializeField] GameObject[] guns;
    [SerializeField] GameObject[] weapons; 
    [SerializeField] float fireRate;
    [SerializeField] AudioSource fireSound;

    GameObject player;
    Enemy thisEnemy;

    bool inMovement;
    int layer_mask;

    void Awake()
    {
        layer_mask = LayerMask.GetMask("Asteroid");
        inMovement = false;
        hits = new RaycastHit2D[rays.Length];
        player = GameObject.FindGameObjectWithTag("Player");
        thisEnemy = gameObject.GetComponent<Enemy>();
    }

    void Start()
    {
        StartCoroutine(OpenFire());
    }

    void Update()
    {
        CheckAsteroid();
    }

    void CheckAsteroid()
    {
        if (inMovement == true) { return; }
        int hitted = 15; 
        Vector3 right = transform.TransformDirection(Vector3.up) * 400;
        //Debug.DrawRay(rays[0].transform.position, right, Color.green, 1f);
        //Debug.DrawRay(rays[1].transform.position, right, Color.green, 1f);
        //Debug.DrawRay(rays[2].transform.position, right, Color.green, 1f);
        //Debug.DrawRay(rays[3].transform.position, right, Color.green, 1f);
        hits[0] = Physics2D.Raycast(rays[0].transform.position + new Vector3(100,0,0), right, 400f, layer_mask);
        hits[1] = Physics2D.Raycast(rays[1].transform.position + new Vector3(100,0,0), right, 400f, layer_mask);
        hits[2] = Physics2D.Raycast(rays[2].transform.position + new Vector3(100,0,0), right, 400f, layer_mask);
        hits[3] = Physics2D.Raycast(rays[3].transform.position + new Vector3(100,0,0), right, 400f, layer_mask);
        if (hits[0].transform != null || hits[1].transform != null || hits[2].transform != null || hits[3].transform != null)
        {
            hitted = 0;
        }
        else
        {
            Move(2);
            return;
        }
        if (hits[0].transform != null)
        {
            if (hits[0].transform.gameObject.layer == 10) { hitted = hitted - 4; }
        }
        if (hits[1].transform != null)
        {
            if (hits[1].transform.gameObject.layer == 10) { hitted = hitted - 7; }
        }
        if (hits[2].transform != null)
        {
            if (hits[2].transform.gameObject.layer == 10) { hitted = hitted +9; }
        }
        if (hits[3].transform != null)
        {
            if (hits[3].transform.gameObject.layer == 10) { hitted = hitted +5; }
        }
        if (hits[0].transform != null && hits[1].transform != null && hits[2].transform != null && hits[3].transform != null)
        {
            hitted = 11;
        }
        Move(hitted);
    }

    void Move(int moveMolt)
    {
        if (moveMolt == 15) { inMovement = false; return; }
        if(player.transform.position.y > transform.position.y)
        {
            moveMolt = Mathf.Abs(moveMolt);
        } 
        else if(player.transform.position.y < transform.position.y)
        {
            moveMolt = Mathf.Abs(moveMolt) * (-1);
        }
        Vector3 goToPoint = new Vector3(transform.position.x, transform.position.y + (30 * moveMolt),0f);
        if (goToPoint.y >= 1030f) { goToPoint = new Vector3(goToPoint.x, 1029f, goToPoint.z); };
        if (goToPoint.y <= 50f) { goToPoint = new Vector3(goToPoint.x, 51f, goToPoint.z); };
        StartCoroutine(MoveAndReset(goToPoint));
    }

    IEnumerator MoveAndReset(Vector3 goToPoint)
    {
        inMovement = true;
        iTween.MoveTo(gameObject, iTween.Hash("position", goToPoint, "easetype", iTween.EaseType.linear, "time", 0.2f, "space", Space.World));
        yield return new WaitForSeconds(0.2f);
        inMovement = false;
    }

    IEnumerator OpenFire()
    {
        if (!thisEnemy.destroyed)
        {
            if (GameManager.Instance.soundOn)
            {
                fireSound.Play();
            }
            Instantiate(weapons[0], new Vector3(guns[0].transform.position.x + Random.Range(-100f, -150f), guns[0].transform.position.y, guns[0].transform.position.z), guns[0].transform.rotation);
            Instantiate(weapons[0], new Vector3(guns[1].transform.position.x + Random.Range(-100f, -150f), guns[1].transform.position.y, guns[1].transform.position.z), guns[1].transform.rotation);
        }
        yield return new WaitForSeconds(fireRate + Random.Range(0.5f, 1f));
        StartCoroutine(OpenFire());
    }
}
