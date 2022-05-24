using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Camera cam;
    [SerializeField] int moveSpeed = 5;
    [SerializeField] public float boundry;
    Vector2 moveDirection;

    public bool selectedTile = false;
    public Hexagon.TileType tileType;

    public static PlayerController instance;

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
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
        moveDirection.x = Input.GetAxisRaw("Horizontal");
        moveDirection.y = Input.GetAxisRaw("Vertical");
        cam.transform.position = new Vector3(Mathf.Clamp(cam.transform.position.x + moveDirection.x * moveSpeed * Time.deltaTime, -boundry, boundry),
        Mathf.Clamp(cam.transform.position.y + moveDirection.y * moveSpeed * Time.deltaTime, -boundry, boundry), 0);
    }

    public void PlaceATile(int tile)
    {
        StartCoroutine(PlaceTile((Hexagon.TileType)tile));
    }

    IEnumerator PlaceTile(Hexagon.TileType _tileType)
    {
        tileType = _tileType;
        selectedTile = true;
        if (Input.GetMouseButtonDown(1))
        {
            selectedTile = false;
            yield return null;
        }

        yield return new WaitForSeconds(5);
        selectedTile = false;
    }
}
