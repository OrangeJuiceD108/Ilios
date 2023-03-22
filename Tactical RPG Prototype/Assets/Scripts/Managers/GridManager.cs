using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    // This hunk of code is what makes this a singleton
    public static GridManager Instance;
    void Awake()
    {
        Instance = this;
    }

    // This stores the camera so that it can be moved when generating the grid
    public GameObject camera;
    


    /* This holds the tile prefab that is generated with the grid, might need a list of 
    these at some point for holding various tile types (passable, impassable, 
    difficult terrain, etc.) */
    public GameObject tilePrefab;



    // This is the field for the dimensions of the grid
    [Header("Grid Dimensions")]
    
    [SerializeField] private int width, height;



    private Tile goalTile;
    


    // Field for the input map
    [SerializeField] private TextAsset mapPlan;
    // Integer representation of the map, using a three digit integer
    // Integer one represents walkable or otherwise
    // Integer two represents if there is a unit on the tile, and what faction that unit is
    // Integer three represents the index of the unit prefab in the unit array for the given faction
    /*  Key is as follows
        Digit 1                         Digit 2                     Digit 3
        0 => Walkable Tile              0 => Empty Tile             0 => Unit in the 0 position (Aeneas if Player)
        1 => Impassable Tile            1 => Player Unit            1 => Unit in the 1 position
        2 => Goal Tile                  2 => Enemy Unit             etc.
                                        3 => NPC Unit
    */
    private int[,] intMap;
    
    /* This is a Dictionary that contains all of the tiles on the map, using a Vector2 as
    the key (NOTE: The Vector2 used as a key does not corresponed to the actual location of 
    the tile, as all of the tiles are shifted up and right by a half unit) */
    private Dictionary<Vector2, Tile> tiles;

    /* 
    - GenerateGrid does so using nested for loops, generating tiles in columns, starting 
    from the bottom left 
    - - (i.e. it generates the far right column starting with the bottom-most 
    tile and going up, and then it moves left for each successive column). When it generates
    a tile, it changes the name of that tile to "Tile" and its (x,y) position and then it stores
    the tile in the tiles Dictionary. 
    - After all the tiles have been generated, the function
    uses another set of nested loops to generate the adjacencies for each individual, since
    it can't be done on tile creation because all tiles need to be present for adjacencies 
    to generate properly. 
    - After generating the grid and the adjacencies of each tile, the camera is moved to the
    center of the grid.
    */
    public void GenerateGrid()
    {
        tiles = new Dictionary<Vector2, Tile>();
        convertToArray();

        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                Tile newTile = Instantiate(tilePrefab, new Vector3(x + 0.5f, y + 0.5f, 0), Quaternion.identity).GetComponent<Tile>();
                newTile.gameObject.name = "Tile " + x + " " + y;
                newTile.Init(x,y);
                
                if(intMap[x,y] / 100 == 1)
                {
                    newTile.SetWalkable(false);
                }
                else
                {
                    newTile.SetWalkable(true);
                    if(intMap[x,y] / 100 == 2)
                    {
                        goalTile = newTile;
                        newTile.goalHighlight.enabled = true;
                    }
                }

                if((intMap[x,y] / 10) % 10 > 0)
                {
                    UnitManager.Instance.AddUnit(intMap[x,y] % 100, x, y);
                }
                tiles.Add(new Vector2(x, y), newTile);
            }
        }
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                GetTile(new Vector2(x, y)).GenerateAdjacencies(x,y);
            }
        }
        camera.transform.position = new Vector3(width/2, height/2, -10);
    }



    private void convertToArray()
    {
        intMap = new int[width, height];
        string map = mapPlan.ToString();
        string[] firstPass = map.Split("\n", StringSplitOptions.None);
        Array.Reverse(firstPass);
        for(int i = 0; i < height; i++)
        {
            string[] row = firstPass[i].Split(" ", StringSplitOptions.None);
            for(int j = 0; j < width; j++)
            {
                intMap[j,i] = Int32.Parse(row[j]);
            }
        }
    }


    // Returns the tile associated with the input vector if it exists, otherwise returns null.
    public Tile GetTile(Vector2 vec)
    {
        Tile retTile;
        if(tiles.TryGetValue(vec, out retTile))
        {
            return retTile;
        }
        return null;
    }

    public Tile GetGoalTile()
    {
        return goalTile;
    }

    // Might need a set tile function at some point for modifying terrain (abilities that create walls and such). Could also do this on the tile itself with variables for maneuverability on the tiles and methods to change those variables.
}


