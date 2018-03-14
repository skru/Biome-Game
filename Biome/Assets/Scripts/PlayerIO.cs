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
	
	public byte selectedInventory = 0;

    public Rigidbody cube;

    System.Random rand = new System.Random();


    // Use this for initialization
    void Start () {
		currentPlayerIO = this;
	}
	
	// Update is called once per frame
	void Update () {
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
						t.x = Mathf.Abs (x);
						t.y = Mathf.Abs (y);
						t.z = Mathf.Abs (z);                       
                        float distance = Vector3.Distance (t, p);
						if (distance <= damageRadius) {
						    if (x >= chunk.transform.position.x && x <= (chunk.transform.position.x + width) && z >= chunk.transform.position.z && z <= (chunk.transform.position.z + width)) {                         
                                if (!chunkDict.ContainsKey(chunk))
                                {                                   
                                    chunkDict.Add(chunk, new List<Vector3> { t });                                
                                } else
                                {
                                    chunkDict[chunk].AddRange(new List<Vector3> { t });
                                }
						    } else {
								chunk = Chunk.FindChunk (new Vector3(Mathf.Abs(t.x), 1, Mathf.Abs(t.z)));
                                if (chunk != null) {
									if (!chunkDict.ContainsKey (chunk)) {

										chunkDict.Add (chunk, new List<Vector3> { t });

									} else {
										chunkDict [chunk].AddRange (new List<Vector3> { t });
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
                    if (c.Key != null)
                    {
                        
                        byte cubeColor = c.Key.GetByte(c.Value[xe]);
                        if (Input.GetKey(KeyCode.Tab))
                        {
                            c.Key.SetBrick(1, c.Value[xe], c.Key);
                        }
                        else
                        {
                            //Debug.Log(c.Key.IsBrick(0, c.Value[xe], c.Key));
                            if (c.Key.IsBrick(0, c.Value[xe], c.Key) != 0)
                            {
                                c.Key.SetBrick(0, c.Value[xe], c.Key);
                                // create debris
                                Rigidbody clone;
                                float offsetY;
                                float d = (cubeColor % 8 - 1) / 8f;
                                if (cubeColor <= 7)
                                {
                                    offsetY = 0.875F;
                                }
                                else
                                {
                                    offsetY = 0.750F;
                                }
                                clone = Instantiate(cube, c.Value[xe], transform.rotation) as Rigidbody;
                                clone.velocity = transform.TransformDirection(Vector3.forward * 10);
                                Material m = clone.GetComponent<Renderer>().material;

                                m.SetTextureScale("_MainTex", new Vector2(0.125F, 0.125F));
                                m.SetTextureOffset("_MainTex", new Vector2(d, offsetY));



                            }
                        }

                            
                        
                    } 
                }
                StartCoroutine(c.Key.CreateVisualMesh());
            }
        } 
		
	}


}
