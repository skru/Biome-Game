using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMovementMouse : MonoBehaviour {

    public Transform target;
    public float speed = 1;
    public bool startMoving;
    Vector3 direction;
    public Vector3 clickPos;
    public static CameraMovementMouse currentCamera;

    public float maxInteractionRange = 100;
    public float damageRadius = 1F;
    public bool createDebris = false;
    public float debrisLifetime;
    bool alterOrDestroy = false;
    bool deltaRadius = true;

    // Use this for initialization
    void Start () {
        currentCamera = this;
	}
	
	// Update is called once per frame
	void Update () {
        if (startMoving)
        {
            target.position += direction;
        }
        if (Input.GetMouseButtonDown(1))
            
        {
           // Debug.Log("WW");
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000))
            {
                // agent.destination = hit.point;
                //Debug.Log(hit.point);
                clickPos = hit.point;
            }
        }


        else if (Input.GetMouseButtonDown(0))
        {
            if (Input.GetKey(KeyCode.Tab))
            {
                alterOrDestroy = true;
            }
            else
            {
                alterOrDestroy = false;
            }

            if (Input.GetKey(KeyCode.LeftControl))
            {
                deltaRadius = true;
            }
            else
            {
                deltaRadius = false;
            }
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            World.currentWorld.AlterWorld(ray, alterOrDestroy, damageRadius, createDebris, debrisLifetime, maxInteractionRange);
        }

        
        
        
    }
   
    public void StartMovingForward(bool opposite)
    {
        startMoving = true;
        
        direction = (!opposite) ? target.forward + target.right : -target.forward - target.right;
        direction *= speed;
    }

    public void StartMovingSides(bool opposite)
    {
        startMoving = true;

        direction = (!opposite) ? target.right - target.forward : -target.right + target.forward;
        direction *= speed;
    }

    public void StopMoving()
    {
        startMoving = false;
    }
}
