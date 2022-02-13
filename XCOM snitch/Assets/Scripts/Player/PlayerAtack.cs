using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAtack : MonoBehaviour
{
    public GameObject Enemy;
    [SerializeField] protected LineOfSight lineOfSight;
    
    public void OnSee()
    {
        if (lineOfSight.visibleTargets.Count > 0)
        {
            if (Input.GetMouseButtonUp(1))
            {
                Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Enemy")))
                {

                    Enemy.GetComponent<LiveBar>().currentLive -= 25;
                   

                }
            }
        }

    }
        void Start()
        {

        }

        void Update()
        {
            OnSee();

            Debug.DrawRay(transform.position, transform.forward);
            Debug.DrawRay(transform.position, transform.up);
        }

    
}

