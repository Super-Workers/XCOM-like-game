using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MenuScript 
{
    [MenuItem("Tools/Assign Tile Material")]
    public static void AssignTileMaterial()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        Material material = Resources.Load<Material>("Tile");

        foreach (GameObject t in tiles)
        {
            t.GetComponent<Renderer>().material = material;
        }
    }

    [MenuItem("Tools/Assign Tile Script")]
    public static void AssignTileScript()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");

        foreach (GameObject t in tiles)
        {
            t.AddComponent<Tile>();
        }
    }

    [MenuItem("Tools/Assign Tile Tag")]
    public static void AssignTileTag()
    {
        GameObject floor = GameObject.FindGameObjectWithTag("Floor");
        Transform[] transform = floor.GetComponentsInChildren<Transform>();
        int x = -1;

        foreach (Transform child in transform)
        {
            child.tag = "Tile";
            child.name = "Tile" + " (" + x.ToString() + ")";
            x++;
        }
    }

    [MenuItem("Tools/Assign Tile colliders")]
    public static void AssignTileColliders()
    {
        GameObject floor = GameObject.FindGameObjectWithTag("Floor");
        Transform[] transform = floor.GetComponentsInChildren<Transform>();

        foreach (Transform child in transform)
        {
            child.gameObject.AddComponent<BoxCollider>();
        }
    }

}
