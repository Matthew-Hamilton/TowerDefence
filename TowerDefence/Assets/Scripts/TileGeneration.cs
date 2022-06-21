using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class IListExtensions
{
    /// <summary>
    /// Shuffles the element order of the specified list.
    /// </summary>
    public static void Shuffle<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
}


public class TileGeneration : MonoBehaviour
{
    public static TileGeneration instance;
    public int size;
    public GameObject Tile;


    public string Seed;
    public bool UseRandomSeed = false;

    public List<List<Hexagon>> HexConstruction;
    public List<Node> MountainNodes;

    [SerializeField] int numSmoothOperations;
    [SerializeField] int landRatio = 50;

    public Material Mountain;
    public Material Ground;
    public Material Water;
    public Material Turret;

    void Start()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);

        Mountain = Resources.Load<Material>("Mountain");
        Ground = Resources.Load<Material>("Ground");
        Water = Resources.Load<Material>("Water");
        Turret = Resources.Load<Material>("Turret");

        MountainNodes = new List<Node>();
        HexConstruction = new List<List<Hexagon>>();
    }

    void ClearMap()
    {
        if (HexConstruction != null)
        {
            foreach (List<Hexagon> hexList in HexConstruction)
            {
                foreach(Hexagon hex in hexList)
                {
                    Destroy(hex.gameObject);
                }
            }
            HexConstruction.Clear();
        }
    }
    public void ConstructMap(int size)
    {
        Debug.Log("Called");
        ClearMap();
        int variableSize = size;
        int hexHeight = 2 * size-1;
        Vector2 startPos = new Vector2(-hexHeight*0.5f * 1.732051f + 0.75f + 0.1160256f, -size + 1);
        bool backtrack = false;
        for(int i=0; i< hexHeight; i++)
        {

            List<Hexagon> level = new List<Hexagon>();
            level.Add(Instantiate(Tile, startPos, Quaternion.identity).GetComponent<Hexagon>());

            for (int j = 0; j < variableSize; j++)
            {
                if (j < variableSize - 1)
                {
                    Hexagon tempHex = level[j].GenerateHex(0).GetComponent<Hexagon>();
                    level.Add(tempHex);
                }
                if (j != 0)
                    level[j].SetConnection(3, level[j - 1]);

            }
            if (variableSize < hexHeight && !backtrack)
            {
                variableSize++;
                startPos.y -= 1;
            }
            else
            {
                backtrack = true;
                variableSize--;
                startPos.y += 1;
            }
            startPos.x += 1.732051f;
            HexConstruction.Add(level);
            //Debug.Log(level.Count);
        }
        //Debug.Log("Help Me: " + HexConstruction[0].Count);
        ConnectMap(size, hexHeight);
    }

    void ConnectMap(int size, int hexHeight)
    {
        int variableSize = size;
        bool backtrack = false;

        //Loop through each collumn to connect tiles laterally
        for (int i = 0; i< hexHeight-1; i++)
        {

            //Loop through each tile in the collumn
            for(int j = 0; j< variableSize; j++)
            {
                //If not backtracking (eg. getting smaller) bottom right connection attached to the first(bottom) element of the next collumn and visa versa, while top right connection 
                if (!backtrack)
                {
                    HexConstruction[i][j].SetConnection(2, HexConstruction[i + 1][j]);
                    //if(j!= variableSize)
                        HexConstruction[i][j].SetConnection(1, HexConstruction[i + 1][j + 1]);

                }
                if(backtrack)
                {
                    
                    //Debug.Log("Backtracking");
                    if (j == 0)
                    {
                        HexConstruction[i][j].SetConnection(1, HexConstruction[i + 1][j]);
                        continue;
                    }
                    if (j == variableSize-1)
                    {
                        HexConstruction[i][j].SetConnection(2, HexConstruction[i + 1][j - 1]);
                        variableSize--;
                        //HexConstruction[i][j].gameObject.active = false;
                        continue;
                    }

                    HexConstruction[i][j].SetConnection(1, HexConstruction[i + 1][j]);
                    HexConstruction[i][j].SetConnection(2, HexConstruction[i + 1][j - 1]);
                }

            }
            if(backtrack)
            {
                variableSize--;
            }
            if (variableSize < hexHeight-1 && !backtrack)
            {
                variableSize++;
            }
            else
            {
                variableSize++;
                backtrack = true;
            }
        }
    }

    public void Randomise()
    {
        if(UseRandomSeed)
            Seed = Time.time.ToString();
        System.Random MyRandom = new System.Random(Seed.GetHashCode());
        foreach (List<Hexagon> hexList in HexConstruction)
        {
            foreach(Hexagon hex in hexList)
            {
                if (hex.tileType == Hexagon.TileType.Tower)
                    continue;
                int numConnected = 0;
                foreach(Node connectedHex in hex.node.connections)
                {
                    if (connectedHex != null)
                        numConnected++;
                }
                if(numConnected < 6)
                {
                    hex.tileType = Hexagon.TileType.Mountain;
                    Debug.Log("Position When Set Mountain: " + hex.transform.position.ToString());
                }
                else
                {
                    hex.tileType = (MyRandom.Next(0, 100) < landRatio) ? Hexagon.TileType.Water : Hexagon.TileType.Ground;
                }
            }
        }
    }

    public void Smooth()
    {
        for (int i = 0; i < numSmoothOperations; i++)
        {
            foreach (List<Hexagon> hexList in HexConstruction)
            {
                foreach (Hexagon hex in hexList)
                {
                    int numGround = 0;
                    int numWater = 0;
                    if (hex.tileType == Hexagon.TileType.Tower)
                        continue;
                    if (hex.tileType == Hexagon.TileType.Mountain)
                        continue;
                    foreach (Node connectedHex in hex.node.connections)
                    {
                        if (connectedHex == null)
                            continue;
                        if (connectedHex.attachedHex.tileType == Hexagon.TileType.Ground)
                            numGround++;
                        if (connectedHex.attachedHex.tileType == Hexagon.TileType.Water)
                            numWater++;
                    }
                    if (numWater > numGround + 1)
                    {
                        hex.tileType = Hexagon.TileType.Water;
                    }
                    if (numWater <= 2)
                    {
                        hex.tileType = Hexagon.TileType.Ground;
                    }
                }
            }
        }
    }

    public void Render()
    {
        MountainNodes.Clear();
        foreach (List<Hexagon> hexList in HexConstruction)
        {
            foreach (Hexagon hex in hexList)
            {
                hex.UpdateTile();
                if (hex.tileType == Hexagon.TileType.Mountain)
                {
                    MountainNodes.Add(hex.node);
                }
            }
        }
    }

    void SetMat(Hexagon hex, Material mat)
    {
        foreach (Transform t in hex.gameObject.transform)
        {
            if (t.tag == "TileInner")
                t.gameObject.GetComponentInChildren<SpriteRenderer>().material = mat;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Hexagon hex = Tile.GetComponent<Hexagon>();
       
    }

    bool ReRandomise()
    {
        bool pathFound = PathFinding.instance.FindPath();
        List<Node> tempMountainHolder = MountainNodes;
        tempMountainHolder.Shuffle();
        foreach (Node node in tempMountainHolder)
        {
            PathFinding.instance.startPoint = node;
            if (PathFinding.instance.FindPath())
            {
                pathFound = true;
                return pathFound;
            }
        }


        Randomise();
        Smooth();
        Render();
        PathFinding.instance.SetRandomStart();
        pathFound = PathFinding.instance.FindPath();
        return pathFound;
    }


    public static int GetNumHexes()
    {
        int numHexes = 1;

        for(int i = 0; i < instance.size; i++)
        {
            numHexes += i * 6;
        }
        Debug.Log(numHexes);
        return numHexes;
    }
}
