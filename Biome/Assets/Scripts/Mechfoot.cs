using UnityEngine;
using System.Collections;

public class Mechfoot : MonoBehaviour {
	
	public MechController mech;
	public float footSpeed = 0;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter(Collider collider) {
	}
	void OnTriggerStay(Collider collider) {
		
		Vector3 motion = mech.GetComponent<Rigidbody>().velocity;
		motion.y = 0;
		motion *= 1 - Time.deltaTime / 2;
		motion.y = mech.GetComponent<Rigidbody>().velocity.y;
		mech.GetComponent<Rigidbody>().velocity = motion;
		
		mech.GetComponent<Rigidbody>().AddForce((transform.forward * 1.5f + transform.right) * footSpeed * -100000 * Time.deltaTime);
	}
}
