using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : TacticsMove 
{

    // Use this for initialization
	void Start () 
	{
        Init();
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

        if (!moving)
        {
            FindSelectableTiles();
            CheckMouse();
            animator.SetBool("Run", false);
            jumping = false;
        }
        else
        {
            Move();

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
