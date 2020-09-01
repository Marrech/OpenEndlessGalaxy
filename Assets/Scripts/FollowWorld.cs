using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWorld : MonoBehaviour
{
    public Transform lookAt;
    public Vector3 offset;

    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        Vector3 pos = cam.WorldToScreenPoint(lookAt.position + offset);
        //Vector3 pos = cam.ScreenToViewportPoint(lookAt.position + offset);
        if (transform.position != pos)
        {
            transform.position = pos;
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
    }

    public void SetOffset(Vector3 vec)
    {
        offset = vec;
    }

    public Vector3 GetOffset()
    {
        return offset;
    }
}
