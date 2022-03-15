using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : TacticsMove 
{
    // Use this for initialization
    //public GameObject roofTiles;
    public GameObject player1;
    public GameObject player2;
    public GameObject Enemy1;
    public GameObject Enemy2;
    public bool isUp = false;
    public bool isShooting = false;
    int currentPlayer1Action = 2;
    int currentPlayer2Action = 2;
    [SerializeField] protected LineOfSight lineOfSight1;
    [SerializeField] protected LineOfSight lineOfSight2;
    [SerializeField] protected LineOfSight lineOfSightEnemy1;
    [SerializeField] protected LineOfSight lineOfSightEnemy2;
    [SerializeField] protected LineOfSight currentLineOfSight;
	void Start () 
	{
        Init(); 
	}

    void Awake()
    {
        player1 = GameObject.FindGameObjectWithTag("Player");
        player2 = GameObject.FindGameObjectWithTag("Player 2");
        Enemy1 = GameObject.FindGameObjectWithTag("Enemy1");
        Enemy2 = GameObject.FindGameObjectWithTag("Enemy2");
        //roofTiles = GameObject.FindGameObjectWithTag("Roof");
        currentPlayer = player1;
        currentLineOfSight = lineOfSight1;
        Tile.player = currentPlayer;
        animator = currentPlayer.GetComponent<Animator>();
        //roofTiles.SetActive(false);
    }

    public void OnSee()
    {
        if (currentLineOfSight.visibleTargets.Count > 0 && currentPlayer.GetComponent<LiveBar>().currentAction != 0)
        {
            if (Input.GetMouseButtonUp(1))
            {
                Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Enemy")))
                {
                    hit.transform.GetComponent<LiveBar>().currentLive -= 25;
                    
                    currentPlayer.GetComponent<LiveBar>().currentAction -= 1;

                    isShooting = true;
                }
            }
        }
        else
        {
            isShooting = false;
        }
    }

    public void OnSeeEnemy()
    {
        if (lineOfSightEnemy1.visibleTargets.Count > 0)
        {
            Enemy1.SetActive(true);
        }
        else
        {
            Enemy1.SetActive(false);
        }

        if (lineOfSightEnemy2.visibleTargets.Count > 0)
        {
            Enemy2.SetActive(true);
        }
        else
        {
            Enemy2.SetActive(false);
        }
    }

	// Update is called once per frame
	void Update () 
	{
        Debug.DrawRay(currentPlayer.transform.position, currentPlayer.transform.forward);
        Debug.DrawRay(currentPlayer.transform.position, currentPlayer.transform.up);

        OnSee();
        OnSeeEnemy();

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
                currentLineOfSight = lineOfSight1;
                player1.GetComponent<LiveBar>().currentAction = currentPlayer1Action;
                animator = player1.GetComponent<Animator>();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && player2.GetComponent<LiveBar>().currentAction != 0)
            {
                currentPlayer = player2;
                currentLineOfSight = lineOfSight2;
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
                    currentLineOfSight = lineOfSight2;
                    currentPlayer1Action = 2;
                    player2.GetComponent<LiveBar>().currentAction = currentPlayer2Action;
                    animator = player2.GetComponent<Animator>();
                }
                else if (currentPlayer.GetComponent<LiveBar>().currentAction == 0 && currentPlayer == player2)
                {
                    currentPlayer = player1;
                    currentLineOfSight = lineOfSight1;
                    currentPlayer2Action = 2;
                    player1.GetComponent<LiveBar>().currentAction = currentPlayer1Action;
                    animator = player1.GetComponent<Animator>();
                }
            }

            //
            if (isShooting)
            {
                if (currentPlayer.GetComponent<LiveBar>().currentAction == 0 && currentPlayer != player2) 
                {
                    currentPlayer = player2;
                    currentLineOfSight = lineOfSight2;
                    currentPlayer1Action = 2;
                    player2.GetComponent<LiveBar>().currentAction = currentPlayer2Action;
                    animator = player2.GetComponent<Animator>();
                }
                else if (currentPlayer.GetComponent<LiveBar>().currentAction == 0 && currentPlayer == player2)
                {
                    currentPlayer = player1;
                    currentLineOfSight = lineOfSight1;
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
