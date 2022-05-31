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
    [SerializeField] public Node node;
    public GameObject tileInner;
    public bool active = false;
    public GameObject Tile;
    public int moveDifficulty = 1;
    public TileType tileType;

    public bool isTower =false;

    void Awake()
    {
        node = new Node(true, transform.position + Vector3.up, this);
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
                return Instantiate(Tile, transform.position + new Vector3(0, 2, 0), Quaternion.identity);
            case 1:
                otherScript.node.connections = new Node[6];
                return Instantiate(Tile, transform.position + new Vector3(1.732051f, 1, 0), Quaternion.identity);
            case 2:
                otherScript.node.connections = new Node[6];
                return Instantiate(Tile, transform.position + new Vector3(1.732051f, -1, 0), Quaternion.identity);
            case 3:
                otherScript.node.connections = new Node[6];
                return Instantiate(Tile, transform.position + new Vector3(0, -2, 0), Quaternion.identity);
            case 4:
                otherScript.node.connections = new Node[6];
                return Instantiate(Tile, transform.position + new Vector3(-1.732051f, -1, 0), Quaternion.identity);
            case 5:
                otherScript.node.connections = new Node[6];
                return Instantiate(Tile, transform.position + new Vector3(-1.732051f, 1, 0), Quaternion.identity);
            default:
                return null;
        }
    }


    //Converts tile to new tile type then checks that all enemies can still path to the goal, otherwise changes back to the original tile
    public void myMouseDown()
    {
        if (PlayerController.instance.selectedTile)
        {
            TileType tempTileType = tileType;
            tileType = PlayerController.instance.tileType;
            UpdateTile();
            if(!EnemyController.instance.CheckAllCanPath())
            {
                tileType = tempTileType;
                UpdateTile();
            }
            else
            {
                EnemyController.instance.UpdatePaths();
            }

        }
    }

    public void UpdateTile()
    {
        if ((transform.position - Vector3.zero).magnitude < 0.01)
        {
            isTower = true;
            gameObject.name = "Tower";
            tileType = TileType.Tower;
            PathFinding.instance.endPoint = node; 
        }
        SpriteRenderer spriteRenderer = tileInner.GetComponentInChildren<SpriteRenderer>();

        switch(tileType)
        {
            case TileType.Ground:
                spriteRenderer.material = TileGeneration.instance.Ground;
                node.walkable = true;
                break;
            case TileType.Mountain:
                spriteRenderer.material = TileGeneration.instance.Mountain;
                node.walkable = false;
                break;
            case TileType.Water:
                spriteRenderer.material = TileGeneration.instance.Water;
                node.walkable = false;
                break;
            case TileType.Tower:
                spriteRenderer.sprite = Resources.Load<Sprite>("TowerSprite");
                node.walkable = true;
                break ;
            case TileType.Turret:
                spriteRenderer.sprite = Resources.Load<Sprite>("TurretSprite");
                AddTowerStript();
                node.walkable = false;
                break;
            default:
                tileType = TileType.Ground;
                spriteRenderer.material = TileGeneration.instance.Ground;
                node.walkable = true;
                break;

        }
    }

    void AddTowerStript()
    {
        if(!gameObject.TryGetComponent<TurretBase>(out _))
        {
            gameObject.AddComponent<TurretBase>();
            return;
        }
    }

    public enum TileType
    {
        Ground =0,
        Mountain = 1,
        Water = 2,
        Tower = 3,
        Turret = 4
    }

}
