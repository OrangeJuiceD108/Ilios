using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    
    public new GameObject camera;
    
    public GameObject tilePrefab;

    [SerializeField] private int width, height;
    
    private Dictionary<Vector2, Tile> tiles;

    void Awake()
    {
        Instance = this;
    }

    public void GenerateGrid()
    {
        tiles = new Dictionary<Vector2, Tile>();

        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                Tile newTile = Instantiate(tilePrefab, new Vector3(x + 0.5f, y + 0.5f, 0), Quaternion.identity).GetComponent<Tile>();
                newTile.gameObject.name = "Tile " + x + " " + y;

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

    public Tile GetTile(Vector2 vec)
    {
        Tile retTile;
        if(tiles.TryGetValue(vec, out retTile))
        {
            return retTile;
        }
        return null;
    }

    // Might need a set tile function at some point for modifying terrain (abilities that create walls and such). Could also do this on the tile itself with variables for maneuverability on the tiles and methods to change those variables.
}


