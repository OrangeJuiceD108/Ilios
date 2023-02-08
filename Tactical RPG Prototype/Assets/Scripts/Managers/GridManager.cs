using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject tilePrefab;
    [SerializeField] private int width, height;
    public new GameObject camera;

    public static GridManager Instance {get; private set;}

    void Awake()
    {
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                GameObject newTile = Instantiate(tilePrefab, new Vector3(x + 0.5f, y + 0.5f, 0), Quaternion.identity);
                newTile.gameObject.name = "Tile " + x + " " + y;
            }
        }
        camera.transform.position = new Vector3(width/2, height/2, -10);
    }
}


