using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : TacticsMove 
{

    // Use this for initialization
    public GameObject roofTiles;
    public GameObject player1;
    public GameObject player2;
    public bool isUp = false;
	void Start () 
	{
        Init();
        roofTiles.SetActive(false);
	}

    void Stop()
    {
        RemoveSelectableTiles();
        player2.GetComponent<Player2Move>().enabled = true;
        player1.GetComponent<PlayerMove>().enabled = false;
        currentAction = 2;
    }
	
	// Update is called once per frame
	void Update () 
	{
        Debug.DrawRay(transform.position, transform.forward);
        Debug.DrawRay(transform.position, transform.up);

        if (!turn)
        {
            return;
        }

        if (currentAction == 2)
        {
            Start();
        }

        if (currentAction == 0 || Input.GetKeyDown(KeyCode.Alpha2))
        {
            Stop();
        }

        if (!moving)
        {
            animator.SetBool("Run", false);
            jumping = false;
            
            if (currentAction > 0)
            {
                FindSelectableTiles();
                CheckMouse();
            }
            
            if (stopMoving)
            {
                stopMoving = false;
                currentAction -= 1;
            }

            Ray ray = new Ray(transform.position, transform.up);

            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, 3, 1 << LayerMask.NameToLayer("LadderUp")) && !isUp)
            {
                roofTiles.SetActive(true);
                transform.position += new Vector3(1, 3, 0);
            }

            if (Physics.Raycast(ray, out hit, 3, 1 << LayerMask.NameToLayer("LadderDown")) && isUp)
            {
                roofTiles.SetActive(false);
                transform.position += new Vector3(-1, -3, 0);
            }

        }
        else if (currentAction != 0)
        {
            Move();

            if (transform.position.y > 3)
            {
                isUp = true;
            }
            else
            {
                isUp = false;
            }

            stopMoving = true;

            Ray ray = new Ray(transform.position, transform.up);

            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, 1, 1 << LayerMask.NameToLayer("forJump")))
            {
                if (!jumping)
                {
                    animator.SetTrigger("Jump");
                    jumping = true;
                    moveSpeed = 1;
                }
            }
            else
            {
                moveSpeed = 3;
            }
                       
            animator.SetBool("Run", true);
        }
        
	}

    public void ChangeMoveSpeed()
    {
        moveSpeed = 3;
    }

    void CheckMouse()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Ground")))
            {
                if (hit.collider.tag == "Tile")
                {
                    Tile t = hit.collider.GetComponent<Tile>();

                    if (t.selectable)
                    {
                        MoveToTile(t);
                    }

                    if (t.selectableTwo)
                    {
                        MoveToTile(t);
                    }
                }
            }
        }
    }
}
