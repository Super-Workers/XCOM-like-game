using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour 
{
    public bool walkable = true;
    public bool current = false;
    public bool target = false;
    public bool underPlayers = false;
    public bool selectable = false;
    public bool selectableTwo = false;
    public bool notToMove = false;
    public static GameObject player;
    public List<Tile> adjacencyList = new List<Tile>();

    //Needed BFS (breadth first search)
    public bool visited = false;
    public Tile parent = null;
    public int distance = 0;

    //For A*
    public float f = 0;
    public float g = 0;
    public float h = 0;


	// Use this for initialization
	void Start () 
	{
        GetComponent<Renderer>().material.color = Color.clear;
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (current)
        {
            GetComponent<Renderer>().material.color = Color.blue;
        }
        else if (target)
        {
            GetComponent<Renderer>().material.color = Color.blue;
        }
        else if (selectable && !notToMove)
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
        else if (selectable && notToMove)
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
        else if (visited)
        {
            GetComponent<Renderer>().material.color = Color.clear;
        }
        else if (selectableTwo)
        {
            GetComponent<Renderer>().material.color = Color.yellow;
        }
        else if (underPlayers)
        {
            GetComponent<Renderer>().material.color = Color.gray;
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.clear;
        }
	}

    public void Reset()
    {
        adjacencyList.Clear();

        current = false;
        target = false;
        underPlayers = false;
        selectable = false;
        selectableTwo = false;

        visited = false;
        parent = null;
        distance = 0;

        f = g = h = 0;
    }

    public void FindNeighbors(float jumpHeight, Tile target)
    {
        Reset();

        CheckTile(Vector3.forward, jumpHeight, target);
        CheckTile(-Vector3.forward, jumpHeight, target);
        CheckTile(Vector3.right, jumpHeight, target);
        CheckTile(-Vector3.right, jumpHeight, target);
    }

    public void CheckTile(Vector3 direction, float jumpHeight, Tile target)
    {
        Vector3 halfExtents = new Vector3(0.25f, (1 + jumpHeight) / 2.0f, 0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents);

        foreach (Collider item in colliders)
        {
            Tile tile = item.GetComponent<Tile>();
            if (tile != null && tile.walkable)
            {
                RaycastHit hit;
                Debug.DrawRay(player.transform.position, player.transform.up, Color.yellow);

                if (!Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1))
                {
                    if (!Physics.Raycast(tile.transform.position + new Vector3(0.6f, 0, 0), Vector3.up, out hit, 1, 1 << LayerMask.NameToLayer("Fence")) 
                    && !Physics.Raycast(tile.transform.position + new Vector3(-0.6f, 0, 0), Vector3.up, out hit, 1, 1 << LayerMask.NameToLayer("Fence")) 
                    && !Physics.Raycast(tile.transform.position + new Vector3(0, 0, 0.6f), Vector3.up, out hit, 1, 1 << LayerMask.NameToLayer("Fence"))
                    && !Physics.Raycast(tile.transform.position + new Vector3(0, 0, -0.6f), Vector3.up, out hit, 1, 1 << LayerMask.NameToLayer("Fence")) 
                    && Physics.Raycast(player.transform.position, player.transform.up, out hit, 3, 1 << LayerMask.NameToLayer("Inside")))
                    {
                        adjacencyList.Add(tile);
                    }
                    else if(!Physics.Raycast(tile.transform.position + new Vector3(0.4f, 0, 0), Vector3.up, out hit, 1, 1 << LayerMask.NameToLayer("Fence")) 
                    && !Physics.Raycast(tile.transform.position + new Vector3(-0.4f, 0, 0), Vector3.up, out hit, 1, 1 << LayerMask.NameToLayer("Fence")) 
                    && !Physics.Raycast(tile.transform.position + new Vector3(0, 0, 0.4f), Vector3.up, out hit, 1, 1 << LayerMask.NameToLayer("Fence"))
                    && !Physics.Raycast(tile.transform.position + new Vector3(0, 0, -0.4f), Vector3.up, out hit, 1, 1 << LayerMask.NameToLayer("Fence")) 
                    && !Physics.Raycast(player.transform.position, player.transform.up, out hit, 3, 1 << LayerMask.NameToLayer("Inside")))
                    {
                        adjacencyList.Add(tile);
                    }
                }

                if (Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1))
                {
                    if (hit.collider.tag == "Player" || hit.collider.tag == "Player 2")
                    {
                        tile.underPlayers = true;
                    }
                }
            }
        }
    }
}
