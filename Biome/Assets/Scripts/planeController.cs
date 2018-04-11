using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planeController : MonoBehaviour {
    public static int viewRange
    {
        get { return World.currentWorld.viewRange; }
    }

    public static planeController plane;
    // Use this for initialization
    void Start () {
        plane = this;
        plane.transform.position = new Vector3((viewRange * 2) / 2, 12, (viewRange * 2) / 2);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
