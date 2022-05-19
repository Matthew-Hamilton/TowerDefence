using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Camera cam;
    [SerializeField] int moveSpeed = 5;
    [SerializeField] float boundry = 20;
    Vector2 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        MoveCamera();
        ScrollCamera();
    }

    void ScrollCamera()
    {
        if (Input.mouseScrollDelta.y < 0 && Camera.main.orthographicSize < 300)
        {
            Camera.main.orthographicSize -= Input.mouseScrollDelta.y * 2;
        }
        if (Input.mouseScrollDelta.y > 0 && Camera.main.orthographicSize > 2)
        {
            Camera.main.orthographicSize -= Input.mouseScrollDelta.y * 2;
        }
    }

    void MoveCamera()
    {
        moveDirection.x = Input.GetAxis("Horizontal");
        moveDirection.y = Input.GetAxis("Vertical");
        cam.transform.position = new Vector3(Mathf.Clamp(cam.transform.position.x + moveDirection.x * moveSpeed * Time.deltaTime, -boundry, boundry),
        Mathf.Clamp(cam.transform.position.y + moveDirection.y * moveSpeed * Time.deltaTime, -boundry, boundry), 0);
    }
}
