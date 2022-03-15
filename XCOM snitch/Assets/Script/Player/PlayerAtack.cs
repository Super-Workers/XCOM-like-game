using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAtack : MonoBehaviour
{
    public GameObject Enemy;
    public GameObject MainUI;
    public bool isShooting = false;
    [SerializeField] protected LineOfSight lineOfSight;
    
    void Awake()
    {
        Enemy = GameObject.FindGameObjectWithTag("Enemy");
        MainUI = GameObject.FindGameObjectWithTag("MainUI");
    }

    public void OnSee()
    {
        if (lineOfSight.visibleTargets.Count > 0 && MainUI.GetComponent<PlayerMove>().currentPlayer.GetComponent<LiveBar>().currentAction != 0)
        {
            if (Input.GetMouseButtonUp(1))
            {
                Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Enemy")))
                {
                    Enemy.GetComponent<LiveBar>().currentLive -= 25;
                    
                    MainUI.GetComponent<PlayerMove>().currentPlayer.GetComponent<LiveBar>().currentAction -= 1;

                    isShooting = true;
                }
            }
        }
        else
        {
            isShooting = false;
        }

    }
    void Update()
    {
        OnSee();

        Debug.DrawRay(transform.position, transform.forward);
        Debug.DrawRay(transform.position, transform.up);
    }

    
}

