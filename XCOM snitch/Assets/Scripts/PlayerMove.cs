using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : TacticsMove 
{
    // Use this for initialization
    //public GameObject roofTiles;
    public GameObject player1;
    public GameObject player2;
    public bool isUp = false;
    int currentPlayer1Action = 2;
    int currentPlayer2Action = 2;
	void Start () 
	{
        Init(); 
	}

    void Awake()
    {
        player1 = GameObject.FindGameObjectWithTag("Player");
        player2 = GameObject.FindGameObjectWithTag("Player 2");
        //roofTiles = GameObject.FindGameObjectWithTag("Roof");
        currentPlayer = player1;
        Tile.player = currentPlayer;
        animator = currentPlayer.GetComponent<Animator>();
        //roofTiles.SetActive(false);
    }

	// Update is called once per frame
	void Update () 
	{
        Debug.DrawRay(currentPlayer.transform.position, currentPlayer.transform.forward);
        Debug.DrawRay(currentPlayer.transform.position, currentPlayer.transform.up);

        Tile.player = currentPlayer;

        if (!turn)
        {
            return;
        }

        if (!moving)
        {
            animator.SetBool("Run", false);
            jumping = false;
            FindSelectableTiles();
            CheckMouse();

            if (currentPlayer == player1 && player1.GetComponent<LiveBar>().currentAction != 0)
            {
                currentPlayer1Action = player1.GetComponent<LiveBar>().currentAction;
            }
            else if (currentPlayer == player2 && player2.GetComponent<LiveBar>().currentAction != 0)
            {
                currentPlayer2Action = player2.GetComponent<LiveBar>().currentAction;
            }

            if (Input.GetKeyDown(KeyCode.Alpha1) && player1.GetComponent<LiveBar>().currentAction != 0)
            {
                currentPlayer = player1;
                player1.GetComponent<LiveBar>().currentAction = currentPlayer1Action;
                animator = player1.GetComponent<Animator>();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && player2.GetComponent<LiveBar>().currentAction != 0)
            {
                currentPlayer = player2;
                player2.GetComponent<LiveBar>().currentAction = currentPlayer2Action;
                animator = player2.GetComponent<Animator>();
            }
            
            if (stopMoving)
            {
                stopMoving = false;
                currentPlayer.GetComponent<LiveBar>().currentAction -= 1;
                if (currentPlayer.GetComponent<LiveBar>().currentAction == 0 && currentPlayer != player2) 
                {
                    currentPlayer = player2;
                    currentPlayer1Action = 2;
                    player2.GetComponent<LiveBar>().currentAction = currentPlayer2Action;
                    animator = player2.GetComponent<Animator>();
                }
                else if (currentPlayer.GetComponent<LiveBar>().currentAction == 0 && currentPlayer == player2)
                {
                    currentPlayer = player1;
                    currentPlayer2Action = 2;
                    player1.GetComponent<LiveBar>().currentAction = currentPlayer1Action;
                    animator = player1.GetComponent<Animator>();
                }
            }

            Ray ray = new Ray(currentPlayer.transform.position, currentPlayer.transform.up);

            RaycastHit hit;
            
            //if (Physics.Raycast(ray, out hit, 3, 1 << LayerMask.NameToLayer("LadderUp")) && !isUp)
            //{
            //    roofTiles.SetActive(true);
            //    currentPlayer.transform.position += new Vector3(1, 3, 0);
            //}

            //if (Physics.Raycast(ray, out hit, 3, 1 << LayerMask.NameToLayer("LadderDown")) && isUp)
            //{
            //    roofTiles.SetActive(false);
            //    currentPlayer.transform.position += new Vector3(-1, -3, 0);
            //}

        }
        else 
        {
            Move();

            if (currentPlayer.transform.position.y > 3)
            {
                isUp = true;
            }
            else
            {
                isUp = false;
            }

            stopMoving = true;

            Ray ray = new Ray(currentPlayer.transform.position, currentPlayer.transform.up);

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
