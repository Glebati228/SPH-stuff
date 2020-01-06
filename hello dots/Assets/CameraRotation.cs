using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
#pragma warning disable 0649
    private float yaw = -90.0f;
    private float pitch = 0.0f;

    private Camera cam;
    [Range(0f, 10f), SerializeField] private float speed;
    [Range(0f, 10f), SerializeField] private float sensetivity;

    private Vector3 lastCursorPos;
#pragma warning restore 0649

    void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        lastCursorPos = Input.mousePosition;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.LookAt(); 
        MoveCam();
        if(lastCursorPos != Input.mousePosition)
        {
            RotateCam();
        }
    }

    void MoveCam()
    {
        if (Input.GetKey(KeyCode.W))
            transform.position += transform.forward * speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S))
            transform.position -= transform.forward * speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
            transform.position += Vector3.Cross(transform.forward, Vector3.up).normalized * speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.D))
            transform.position -= Vector3.Cross(transform.forward, Vector3.up).normalized * speed * Time.deltaTime;
    }

    void RotateCam()
    {
        float x = (lastCursorPos - Input.mousePosition).x;
        float y = (Input.mousePosition - lastCursorPos).y;

        lastCursorPos = Input.mousePosition;

        x *= sensetivity;
        y *= sensetivity;

        yaw += x;
        pitch += y;

        if (pitch > 89f)
            pitch = 89f;
        if (pitch < -89f)
            pitch = -89f;

        Vector3 camDirection;

        camDirection.x = Mathf.Cos(Mathf.Deg2Rad * yaw) * Mathf.Cos(Mathf.Deg2Rad * pitch);
        camDirection.y = Mathf.Sin(Mathf.Deg2Rad * pitch);
        camDirection.z = Mathf.Sin(Mathf.Deg2Rad * yaw) * Mathf.Cos(Mathf.Deg2Rad * pitch);

        camDirection = camDirection.normalized;

        transform.LookAt(camDirection + transform.position, Vector3.up);
    }
}
