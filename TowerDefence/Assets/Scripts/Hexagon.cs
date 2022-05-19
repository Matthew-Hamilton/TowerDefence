using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class vector3Helper
{
    public static Vector2 xy(this Vector3 myVector3)
    {
        return new Vector2(myVector3.x, myVector3.y);
    }
}
public class Hexagon : MonoBehaviour
{
    public Node node;
    public GameObject tileInner;
    public bool active = false;
    public GameObject Tile;
    public int moveDifficulty = 1;
    public string tileType = "";

    void Awake()
    {
        node = new Node(true, transform.position, this);
        moveDifficulty = 1;
    }

    public Hexagon(int _connection, Node connectedHexagon, Vector2 _position, int _moveDifficulty)
    {
        node.connections = new Node[6];
        node.connections[_connection] = connectedHexagon;
        node.worldPos = _position;
        moveDifficulty = _moveDifficulty;
    }
    public Hexagon(int _connection, Node connectedHexagon, Vector2 _position)
    {
        node.connections = new Node[6];
        node.connections[_connection] = connectedHexagon;
        node.worldPos = _position;
        moveDifficulty = 1;
    }
    public Hexagon(Vector2 _position, int _moveDifficulty)
    {
        node.connections = new Node[6];
        node.worldPos = _position;
        moveDifficulty = _moveDifficulty;
        moveDifficulty = 1;
    }
    public Hexagon(Vector2 _position)
    {
        node.connections = new Node[6];
        node.worldPos = _position;
        moveDifficulty = 1;
}

    public void SetConnection(int Direction, Hexagon hex)
    {
        node.connections[Direction] = hex.node;
        switch(Direction)
        {
            case 0:
                hex.node.connections[3] = this.node;
                break;
            case 1:
                hex.node.connections[4] = this.node;
                break;
            case 2:
                hex.node.connections[5] = this.node;
                break;
            case 3:
                hex.node.connections[0] = this.node;
                break;
            case 4:
                hex.node.connections[1] = this.node;
                break;
            case 5:
                hex.node.connections[2] = this.node;
                break;
        }
    }

    public GameObject GenerateHex(int connectionDirection)
    {
        Hexagon otherScript = Tile.GetComponent<Hexagon>();
        switch(connectionDirection)
        {
            case 0:
                otherScript.node.connections = new Node[6];
                otherScript.node.worldPos = transform.position.xy() + new Vector2(0, 2);
                return Instantiate(Tile, transform.position + new Vector3(0, 2, 0), Quaternion.identity);
            case 1:
                otherScript.node.connections = new Node[6];
                otherScript.node.worldPos = transform.position.xy() + new Vector2(1.732051f, 1);
                return Instantiate(Tile, transform.position + new Vector3(1.732051f, 1, 0), Quaternion.identity);
            case 2:
                otherScript.node.connections = new Node[6];
                otherScript.node.worldPos = transform.position.xy() + new Vector2(1.732051f, -1);
                return Instantiate(Tile, transform.position + new Vector3(1.732051f, -1, 0), Quaternion.identity);
            case 3:
                otherScript.node.connections = new Node[6];
                otherScript.node.worldPos = transform.position.xy() + new Vector2(0, -2);
                return Instantiate(Tile, transform.position + new Vector3(0, -2, 0), Quaternion.identity);
            case 4:
                otherScript.node.connections = new Node[6];
                otherScript.node.worldPos = transform.position.xy() + new Vector2(-1.732051f, -1);
                return Instantiate(Tile, transform.position + new Vector3(-1.732051f, -1, 0), Quaternion.identity);
            case 5:
                otherScript.node.connections = new Node[6];
                otherScript.node.worldPos = transform.position.xy() + new Vector2(-1.732051f, 1);
                return Instantiate(Tile, transform.position + new Vector3(-1.732051f, 1, 0), Quaternion.identity);
            default:
                return null;
        }
    }

    public void myMouseDown()
    {
        if(PathFinding.GetStartNode() == null)
        {
            Debug.Log("Start Node Set to:" + node );
            PathFinding.SetStart(node);
        }
        else
        {
            Debug.Log("End Node Set to " + node);
            PathFinding.SetEnd(node);
            tileInner.GetComponentInChildren<SpriteRenderer>().color = Color.black;
        }
    }

}
