using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject camera;
    [SerializeField] private float moveSpeed;

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.W))
        {
            Vector3 pos = camera.transform.position;
            pos.y += moveSpeed;
            camera.transform.position = pos;
        }
        if(Input.GetKey(KeyCode.S))
        {
            Vector3 pos = camera.transform.position;
            pos.y -= moveSpeed;
            camera.transform.position = pos;
        }
        if(Input.GetKey(KeyCode.A))
        {
            Vector3 pos = camera.transform.position;
            pos.x -= moveSpeed;
            camera.transform.position = pos;
        }
        if(Input.GetKey(KeyCode.D))
        {
            Vector3 pos = camera.transform.position;
            pos.x += moveSpeed;
            camera.transform.position = pos;
        }
    }
}
