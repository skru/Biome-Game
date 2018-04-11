//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.AI;


//public class MainCameraController : MonoBehaviour {

//	public static float worldWidth {
//		get { return World.currentWorld.worldWidth; }
//	}
//	public static int width {
//		get { return World.currentWorld.chunkWidth; }
//	}
//	public static int height {
//		get { return World.currentWorld.chunkHeight; }
//	}
//	public static float brickHeight {
//		get { return World.currentWorld.brickHeight; }
//	}


//	private Vector3 point;
//	public float distance = 2.0f;
//	public float xSpeed = 20.0f;
//	public float ySpeed = 20.0f;
//	public float yMinLimit = -90f;
//	public float yMaxLimit = 90f;
//	public float distanceMin = 10f;
//	public float distanceMax = 10f;
//	public float smoothTime = 2f;
//	float rotationYAxis = 0.0f;
//	float rotationXAxis = 0.0f;
//	float velocityX = 0.0f;
//	float velocityY = 0.0f;
//	// Use this for initialization
//	void Start()
//	{
//		Vector3 angles = transform.eulerAngles;
//		rotationYAxis = angles.y;
//		rotationXAxis = angles.x;
//		// Make the rigid body not change rotation
//		if (GetComponent<Rigidbody>())
//		{
//			GetComponent<Rigidbody>().freezeRotation = true;
//		}
//	}
//	void LateUpdate()
//	{

//		if (Input.GetMouseButton(0))
//		{
//			velocityX += xSpeed * Input.GetAxis("Mouse X") * distance * 0.02f;
//			velocityY += ySpeed * Input.GetAxis("Mouse Y") * 0.02f;
//		}
//		if (Input.GetMouseButton (1)) {
//			distance = distance - Input.GetAxis ("Mouse Y") * 5;
//			//Debug.Log (distance);
//		}
//		rotationYAxis += velocityX;
//		rotationXAxis -= velocityY;
//		rotationXAxis = ClampAngle(rotationXAxis, yMinLimit, yMaxLimit);
//		//Quaternion fromRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
//		Quaternion toRotation = Quaternion.Euler(rotationXAxis, rotationYAxis, 0);
//		Quaternion rotation = toRotation;

////		distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);
////		RaycastHit hit;
////		if (Physics.Linecast(point, transform.position, out hit))
////		{
////		distance -= hit.distance;
////		}
//		Vector3 negDistance = new Vector3(0.0f, 0.0f, -(distance + worldWidth));
//		point = new Vector3 (width/2*worldWidth, height/4f, width/2*worldWidth);
//		//		transform.LookAt(point);
//		Vector3 position = rotation * negDistance + point;

//		transform.rotation = rotation;
//		transform.position = position;
//		velocityX = Mathf.Lerp(velocityX, 0, Time.deltaTime * smoothTime);
//		velocityY = Mathf.Lerp(velocityY, 0, Time.deltaTime * smoothTime);
//	}

//	public static float ClampAngle(float angle, float min, float max)
//	{
//		if (angle < -360F)
//			angle += 360F;
//		if (angle > 360F)
//			angle -= 360F;
//		return Mathf.Clamp(angle, min, max);
//	}

//}
////
////	//This code should attached to an empty object
////	//I would name it "Camera Controller"
////	public Transform target;
////	public Transform camera; //In case you have more than one camera
////	public float distance = 10.0F;
////	public int cameraSpeed = 5;
////
////	float xSpeed = 175.0F;
////	float ySpeed = 75.0F;
////
////	int yMinLimit = 20; //Lowest vertical angle in respect with the target.
////	int yMaxLimit = 80;
////
////	int minDistance = 5; //Min distance of the camera from the target
////	int maxDistance = 20;
////
////	private float x = 0.0F;
////	private float y = 0.0F;
////
////
////
////	void Start () {
////		var angles = transform.eulerAngles;
////		x = angles.y;
////		y = angles.x;
////
////		// Make the rigid body not change rotation
////		if (GetComponent<Rigidbody>())
////			GetComponent<Rigidbody>().freezeRotation = true;
////	}
////
////	void Update () {
////		//if (target  camera) {
////
////			//Zooming with mouse
////			distance += Input.GetAxis("Mouse ScrollWheel")*distance;
////			distance = Mathf.Clamp(distance, minDistance, maxDistance);
////
////			//Detect mouse drag;
////			if(Input.GetMouseButton(0))   {
////
////				x += Input.GetAxis("Mouse X") * xSpeed * 0.02F;
////				y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02F;      
////			}
////			y = ClampAngle(y, yMinLimit, yMaxLimit);
////
////			Quaternion rotation = Quaternion.Euler(y, x, 0);
////			Transform position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;
////
////			transform.position = new Vector3.Lerp (transform.position, position, cameraSpeed*Time.deltaTime);
////			transform.rotation = rotation;      
////		//}
////	}
////
////	static float ClampAngle (float angle, float min, float max) {
////		if (angle < -360)
////			angle += 360;
////		if (angle > 360)
////			angle -= 360;
////
////		return Mathf.Clamp (angle, min, max);
////	}
////}
//	//public GameObject target;//the target object
//	//private float speedMod = 0.5f;//a speed modifier
////	private Vector3 point;//the coord to the point where the camera looks at
////
////	void Start () {
////		//Set up things on the start method
////		//point = target.transform.position;//get target's coords
////		point = new Vector3 (width/2*worldWidth, 0, width/2*worldWidth);
////		transform.LookAt(point);
////	}
////	
////	// Update is called once per frame
////	public float dragSpeed = 2;
////	private Vector3 dragOrigin;
////	void Update () {
////		point = new Vector3 (width/2*worldWidth, 0, width/2*worldWidth);
////		transform.LookAt(point);
////		Swipe ();
////	
//////
//////		Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
//////		Vector3 move = new Vector3(pos.x * dragSpeed, 0, pos.y * dragSpeed);
//////
//////		transform.Translate(move, Space.World);  
////
////
////		
////	}
////
////	Vector2 firstPressPos;
////	Vector2 secondPressPos;
////	Vector2 currentSwipe;
////
////	public void Swipe()
////	{
////		point = new Vector3 (width/2*worldWidth, 0, width/2*worldWidth);
////		if(Input.GetMouseButtonDown(0))
////		{
////			//save began touch 2d point
////			firstPressPos = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
////		}
////		if(Input.GetMouseButtonUp(0))
////		{
////			//save ended touch 2d point
////			secondPressPos = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
////
////			//create vector from the two points
////			currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
////
////			//normalize the 2d vector
////			currentSwipe.Normalize();
////
////			//swipe upwards
////			if(currentSwipe.y > 0 && currentSwipe.x > -0.5f  && currentSwipe.x < 0.5f)
////			{
////				Debug.Log("up swipe");
////				//Vector3 move = new Vector3(pos.x * dragSpeed, 0, pos.y * dragSpeed);
////
////				//transform.Translate(move, Space.World);  
////				transform.RotateAround (point, new Vector3(0.0f,(20),0.0f), 100);
////			}
////			//swipe down
////			if(currentSwipe.y < 0  && currentSwipe.x > -0.5f  && currentSwipe.x < 0.5f)
////			{
////				Debug.Log("down swipe");
////			}
////			//swipe left
////			if(currentSwipe.x < 0  && currentSwipe.y > -0.5f  && currentSwipe.y < 0.5f)
////			{
////				Debug.Log("left swipe");
////			}
////			//swipe right
////			if(currentSwipe.x > 0  && currentSwipe.y > -0.5f  && currentSwipe.y < 0.5f)
////			{
////				Debug.Log("right swipe");
////			}
////		}
////
////		if(Input.touches.Length > 0)
////		{
////			Touch t = Input.GetTouch(0);
////			if(t.phase == TouchPhase.Began)
////			{
////				//save began touch 2d point
////				firstPressPos = new Vector2(t.position.x,t.position.y);
////			}
////			if(t.phase == TouchPhase.Ended)
////			{ 
////				//save ended touch 2d point
////				secondPressPos = new Vector2(t.position.x,t.position.y);
////
////				//create vector from the two points
////				currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
////
////				//normalize the 2d vector
////				currentSwipe.Normalize();
////
////				//swipe upwards
////				if(currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
////				{
////					Debug.Log("up swipe TOUCH");
////				}
////				//swipe down
////				if(currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
////				{
////					Debug.Log("down swipe TOUCH");
////				}
////				//swipe left
////				if(currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
////				{
////					Debug.Log("left swipe TOUCH");
////				}
////				//swipe right
////				if(currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
////				{
////					Debug.Log("right swipe TOUCH");
////				}
////			}
////		}
////	}
////}
////
////
////
////
////
////
//////void Update () {//makes the camera rotate around "point" coords, rotating around its Y axis, 20 degrees per second times the speed modifier
//////	transform.RotateAround (point, new Vector3(0.0f,-0.5f,0.0f), 10 * Time.deltaTime * speedMod);
//////}
