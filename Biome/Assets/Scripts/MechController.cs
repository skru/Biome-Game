using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MechController : MonoBehaviour {
	
	public Animator animator;
	public List<Mechfoot> feet;
	
	// Use this for initialization
	void Start () {
		Camera.main.transform.parent = null;
	}
	
	// Update is called once per frame
	void Update () {
		float horz = Input.GetAxis("Horizontal");
		float vert = Input.GetAxis("Vertical");
		if (feet != null)
		{
			for (int a = 0; a < feet.Count; a++)
			{
				feet[a].footSpeed = vert;
			}
		}
		animator.SetFloat("Forward", vert);
		
		
		//rigidbody.velocity += (vert * transform.forward + horz * transform.right ) * Time.deltaTime * 10;
		
		
		
		Vector3 idealPos = transform.position + Vector3.up * 6 - Camera.main.transform.forward * 10;
		Camera.main.transform.position = idealPos; //Vector3.Lerp(Camera.main.transform.position, idealPos, Time.deltaTime * 3);
		
		Vector3 idealForward = Camera.main.transform.forward;
		idealForward.y = 0;
		idealForward.Normalize();
		transform.rotation = Quaternion.LookRotation(idealForward); //Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(idealForward), Time.deltaTime * 3);
		
	}
}
