using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeSettings : MonoBehaviour {

    public float lifetime;

    // Use this for initialization
    void Start () {
        Destroy(gameObject, Random.Range(0.5f, lifetime));
    }
	
	//// Update is called once per frame
	//void Update () {
		
	//}
}
