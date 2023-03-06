using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PathfinderNode : MonoBehaviour
{
    // These are for A* pathfinding 
    // G is the distance to get to this tile from the start node
    // H is the manhattan distance from this tile to the target node, ignoring obstacles
    // F is the total of G and H
    [SerializeField] protected int fCost, gCost, hCost;

    // Stores the location of the tile
    protected Vector2 location;

    // Tiles adjacent to this tile
    [SerializeField] protected List<Tile> adjacencies;
    
    public void Init(int x, int y)
    {
        location = new Vector2(x,y);
        fCost = gCost = hCost = -1;
    }

    public void SetCosts(int g, int h)
    {
        gCost = g;
        hCost = h;
        fCost = hCost+gCost;
    }
    public int GetG() {return gCost;}
    public int GetH() {return hCost;}
    public int GetF() {return fCost;}

    // Resets the F, G, and H values so that the pathfinding node can be reused
    public void Reset()
    {
        fCost = gCost = hCost = -1;
    }

    // The following functions have to do with adjacencies
    /*
    GenerateAdjacencies creates a list of adjacencies by using the "GetTile" method four times,
    once in each cardinal direction, and then it uses a while loop to delete all of the null values
    in the list of adjacencies, creating a list that only contains adjacent tiles
    */
    public void GenerateAdjacencies(int x, int y)
    {
        adjacencies = new List<Tile>();

        adjacencies.Add(GridManager.Instance.GetTile(new Vector2(x + 1,y)));
        adjacencies.Add(GridManager.Instance.GetTile(new Vector2(x,y + 1)));
        adjacencies.Add(GridManager.Instance.GetTile(new Vector2(x - 1,y)));
        adjacencies.Add(GridManager.Instance.GetTile(new Vector2(x,y - 1)));
        while(adjacencies.Contains(null))
        {
            adjacencies.Remove(null);
        }
    }
    public List<Tile> GetAdjacencies()
    {
        return new List<Tile>(adjacencies);
    }

    /*
    GetEnumerator returns an enumerator of the list of tile adjacencies, allowing tiles to be used
    in foreach loops without grabbing the list within the tile directly
    */
    public List<Tile>.Enumerator GetEnumerator()
    {
        return adjacencies.GetEnumerator();
    }

    // Self explanatory
    public Vector2 GetLocation()
    {
        return location;
    }
}
