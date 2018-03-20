using UnityEngine;
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
	public float maxInteractionRange = 8;
	public int damageRadius = 1;
    public bool createDebris = true;
    public float debrisLifetime;
    public int poolSize;
    public GameObject cube;
    public Queue<GameObject> objectPool;
    System.Random rand = new System.Random();
    public float viewRange = 30;
    public Chunk chunkFab;

    // Use this for initialization
    void Start () {
		currentPlayerIO = this;
        objectPool = new Queue<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            //GameObject obj = Instantiate(cube);
            //obj.SetActive(false);
            //objectPool.Enqueue(obj);
        }
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
        else if (Input.GetKey(KeyCode.LeftAlt))
        {
            if (createDebris) createDebris = false;
            else createDebris = true;
        }
        else if (Input.GetKey("escape")) Application.Quit();
        if (! Input.GetMouseButtonDown(0) ) return;
        
		Ray ray = GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.5f));
		RaycastHit hit;	
		if (Physics.Raycast(ray, out hit, maxInteractionRange))
		{
			Chunk chunkw = hit.transform.GetComponent<Chunk>();
			if (chunkw == null)
			{
				return;
			}
            Dictionary<Chunk, List<Vector3>> chunkDict = new Dictionary<Chunk, List<Vector3>>();
            Vector3 p = hit.point;

			p.y /= World.currentWorld.brickHeight;
			p -= hit.normal / 4;
            for (float x = p.x - damageRadius; x < p.x + damageRadius+1; x++) {
				for (float y = p.y - damageRadius; y < p.y + damageRadius+1; y++) {
					for (float z = p.z - damageRadius; z < p.z + damageRadius+1; z++) {
                        Chunk chunk = hit.transform.GetComponent<Chunk>();
						Vector3 t = p;
						t.x = Mathf.Round (x);
						t.y = Mathf.Round (y);
						t.z = Mathf.Round (z);                       
                        float distance = Vector3.Distance (t, p);
                        if (distance <= damageRadius) {
						    if (x >= chunk.transform.position.x && x <= (chunk.transform.position.x + width) && z >= chunk.transform.position.z && z <= (chunk.transform.position.z + width) && y >= chunk.transform.position.y && y <= (chunk.transform.position.y + height)) {
                                if (!chunkDict.ContainsKey(chunk))
                                {                                   
                                    chunkDict.Add(chunk, new List<Vector3> { t });                                
                                } else
                                {
                                    chunkDict[chunk].AddRange(new List<Vector3> { t });
                                }
						    } else {
                                if (true)
                                {
                                    chunk = Chunk.FindChunk(new Vector3(t.x, t.y, t.z));
                                    if (chunk != null)
                                    {
                                        if (!chunkDict.ContainsKey(chunk))
                                        {

                                            chunkDict.Add(chunk, new List<Vector3> { t });

                                        }
                                        else
                                        {
                                            chunkDict[chunk].AddRange(new List<Vector3> { t });
                                        }
                                    }
                                }									
						    }
						}
					}
				}
			}

            foreach (KeyValuePair<Chunk, List<Vector3>> c in chunkDict)
            {
                for (int xe = 0; xe < c.Value.Count; xe++)
                {
                    Vector3 t = p;
                    t.x = Mathf.FloorToInt(c.Value[xe].x);
                    t.y = Mathf.FloorToInt(c.Value[xe].y);
                    t.z = Mathf.FloorToInt(c.Value[xe].z);
                    byte cubeColor = c.Key.GetByte(t);
                    if (Input.GetKey(KeyCode.Tab))
                    {
                        c.Key.SetBrick(1, t);
                    }
                    else
                    {
                        if (c.Key.cubePositions.Contains(t))
                        {
                            c.Key.SetBrick(0, t);

                            //if (createDebris)
                            //{
                            //    float offsetY;
                            //    float d = (cubeColor % 8 - 1) / 8f;
                            //    if (cubeColor < 8)
                            //    {
                            //        offsetY = 0.875F;
                            //    }
                            //    else
                            //    {
                            //        offsetY = 0.750F;
                            //    }
                            //    GameObject clone;
                            //    clone = objectPool.Dequeue();
                            //    Rigidbody rclone = clone.GetComponent<Rigidbody>();
                            //    rclone.transform.position = t;
                            //    rclone.transform.rotation = Quaternion.identity;
                            //    rclone.velocity = transform.TransformDirection(Vector3.forward * 10);
                            //    Material m = rclone.GetComponent<Renderer>().material;
                            //    m.SetTextureScale("_MainTex", new Vector2(0.125F, 0.125F));
                            //    m.SetTextureOffset("_MainTex", new Vector2(d, offsetY));
                            //    clone.SetActive(true);
                            //    StartCoroutine(waiter(clone));
                            //}
                        }
                    }
                }
                StartCoroutine(c.Key.CreateVisualMesh());
            }
        } 
	}
    IEnumerator waiter(GameObject clone)
    {
        float wait_time = Random.Range(0.5f, debrisLifetime);
        yield return new WaitForSeconds(wait_time);
        clone.SetActive(false);
        objectPool.Enqueue(clone);

    }
}
