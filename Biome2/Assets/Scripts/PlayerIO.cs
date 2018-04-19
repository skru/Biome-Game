using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class PlayerIO : MonoBehaviour {

	public static int width {
		get { return World.currentWorld.chunkWidth; }
	}
	public static int height {
		get { return World.currentWorld.chunkHeight; }
	}

	public static PlayerIO currentPlayerIO;

    public float maxInteractionRange = 100;
    public float damageRadius = 1F;
    public bool createDebris = false;
    public float debrisLifetime;
    bool alterOrDestroy = false;
    bool deltaRadius = true;




    // Use this for initialization
    void Start () {
		currentPlayerIO = this;
        
    }
	
	// Update is called once per frame
	void Update () {
        


        if (Input.GetKey(KeyCode.Alpha1)) damageRadius = 0;       
        else if (Input.GetKey(KeyCode.Alpha2)) damageRadius = 1;
        else if (Input.GetKey(KeyCode.Alpha3)) damageRadius = 2;
        else if (Input.GetKey(KeyCode.Alpha4)) damageRadius = 3;      
        else if (Input.GetKey(KeyCode.Alpha5)) damageRadius = 4;
        else if (Input.GetKey(KeyCode.Alpha6)) damageRadius = 5;
        else if (Input.GetKey(KeyCode.Alpha7)) damageRadius = 6;
        else if (Input.GetKey(KeyCode.Alpha8)) damageRadius = 7;
        else if (Input.GetKey(KeyCode.Alpha9)) damageRadius = 8;
        else if (Input.GetKey(KeyCode.Alpha0)) damageRadius = 20;
        else if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if (createDebris) createDebris = false;
            else createDebris = true;
        }
        else if (Input.GetKey("escape")) Application.Quit();
        if (! Input.GetMouseButtonDown(0) ) return;

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
        Ray ray = GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.5f));
        //World.currentWorld.AlterWorld(ray, alterOrDestroy, damageRadius, createDebris, debrisLifetime, maxInteractionRange);
		
	}

    
}
