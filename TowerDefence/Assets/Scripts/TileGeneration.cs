using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGeneration : MonoBehaviour
{
    public static TileGeneration instance;
    public int size;
    public GameObject Tile;


    public string Seed;
    public bool UseRandomSeed = false;

    public List<List<Hexagon>> HexConstruction;

    [SerializeField] int numSmoothOperations;
    [SerializeField] int landRatio = 50;

    Material Mountain;
    Material Ground;
    Material Water;

    void Start()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);

        Mountain = Resources.Load<Material>("Mountain");
        Ground = Resources.Load<Material>("Ground");
        Water = Resources.Load<Material>("Water");

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
    void ConstructMap(int size)
    {
        Debug.Log("Called");
        ClearMap();
        int variableSize = size;
        int hexHeight = 2 * size-1;
        Vector2 startPos = new Vector2(-hexHeight*0.5f * 1.732051f + 0.75f, -size*0.5f * 1.732051f + 0.66f);
        bool backtrack = false;
        for(int i=0; i< hexHeight; i++)
        {

            List<Hexagon> level = new List<Hexagon>();
            level.Add(Instantiate(Tile, startPos, Quaternion.identity).GetComponent<Hexagon>());

            for (int j = 0; j < variableSize; j++)
            {
                if(j < variableSize)
                    level.Add(level[j].GenerateHex(0).GetComponent<Hexagon>());
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
                int numConnected = 0;
                foreach(Node connectedHex in hex.node.connections)
                {
                    if (connectedHex != null)
                        numConnected++;
                }
                if(numConnected < 6)
                {
                    hex.tileType = "Mountain";
                }
                else
                {
                    hex.tileType = (MyRandom.Next(0, 100) < landRatio) ? "Water" : "Ground";
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
                    if (hex.tileType == "Mountain")
                        continue;
                    foreach (Node connectedHex in hex.node.connections)
                    {
                        if (connectedHex == null)
                            continue;
                        if (connectedHex.attachedHex.tileType == "Ground")
                            numGround++;
                        if (connectedHex.attachedHex.tileType == "Water")
                            numWater++;
                    }
                    if (numWater > numGround + 1)
                    {
                        hex.tileType = "Water";
                    }
                    if (numWater <= 2)
                    {
                        hex.tileType = "Ground";
                    }
                }
            }
        }
    }

    public void Render()
    {
        foreach (List<Hexagon> hexList in HexConstruction)
        {
            foreach (Hexagon hex in hexList)
            {
                switch(hex.tileType)
                {
                    case "Mountain":
                        SetMat(hex, Mountain);
                        break;
                    case "Ground":
                        SetMat(hex, Ground);
                        break;
                    case "Water":
                        SetMat(hex, Water);
                        break;
                    default:
                        break;

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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ConstructMap(size);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Randomise();
            Smooth();
            Render();
            GetNumHexes();
        }
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
