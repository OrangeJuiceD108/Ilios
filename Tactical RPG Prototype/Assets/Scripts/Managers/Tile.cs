using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer highlight;

    void OnMouseEnter()
    {
        highlight.enabled = true;
    }

    void OnMouseExit()
    {
        highlight.enabled = false;
    }
}
