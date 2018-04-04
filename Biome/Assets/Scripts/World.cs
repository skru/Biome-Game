using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class World : MonoBehaviour
{

    //public Texture2D defaultBrickTexture;

    public Biome[] biomes;
    
    public static World currentWorld;
    public int chunkWidth = 20, chunkHeight = 20, seed = 0;
    public int worldWidth = 20;
    public float viewRange = 30;

    public float brickHeight = 1;

    public Chunk chunkFab;
    public PlayerIO player;
    bool playerMoving = false;
    int count = 0;

    private float timeSinceLastCalled;
    private float delay = 0.1f; // delay update function


    public int poolSize;
    public GameObject capsule;
    public Queue<GameObject> objectPool;
    public Queue<GameObject> NPCPool;
    public GameObject cube;
    public float debrisLifetime;

    public float maxInteractionRange = 8;
    public float damageRadius = 1F;
    Dictionary<Chunk, List<Vector3>> chunkDict = new Dictionary<Chunk, List<Vector3>>();
    public bool createDebris = false;

    void Awake()
    {
        currentWorld = this;
        if (seed == 0)
            seed = Random.Range(0, int.MaxValue);

        //surface.BuildNavMesh();
       
    }

    void Start()
    {
        objectPool = new Queue<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(capsule);
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }

        NPCPool = new Queue<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(cube);
            obj.SetActive(false);
            NPCPool.Enqueue(obj);
        }
    }

    // Update is called once per frame
    void Update()
    {
       // Camera current;
        timeSinceLastCalled += Time.deltaTime;
        if (timeSinceLastCalled > delay)
        {


            Vector3 playerPos = player.transform.position;

            float range = viewRange;

            //if (player.GetComponentInParent<Rigidbody>().velocity.magnitude > 0)
            //{
            //    playerMoving = true;
            //}
            //else
            //{
            //    playerMoving = false;
            //}


            switch (count)
            {
                case 0:
                    if (playerPos.y < 0) range = viewRange / 2;
                    for (float x = playerPos.x - range; x < playerPos.x + range; x += chunkWidth)
                    {
                        for (float z = playerPos.z - range; z < playerPos.z + range; z += chunkWidth)
                        {
                            Vector3 pos = new Vector3(x, playerPos.y, z);
                            Camera cam = Camera.current;
                            if (cam != null)
                            {
                                if (CheckLoadChunk(pos, playerMoving, cam))
                                {
                                    //BuildWorldSection(playerPos, pos);
                                    pos.x = Mathf.Floor(pos.x / (float)chunkWidth) * chunkWidth;
                                    pos.y = Mathf.Floor(pos.y / (float)chunkHeight) * chunkHeight;
                                    pos.z = Mathf.Floor(pos.z / (float)chunkWidth) * chunkWidth;
                                    // Shave square.
                                    Vector3 delta = pos - playerPos;
                                    if (delta.magnitude > range) continue;

                                    Chunk chunk = Chunk.FindChunk(pos);

                                    if (chunk != null)
                                    {
                                        continue;
                                    }
                                    chunk = (Chunk)Instantiate(chunkFab, pos, Quaternion.identity);
                                    if (Random.value > 0.5)
                                    {
                                        if (objectPool.Count > 0)
                                        {
                                            GameObject clone;
                                            clone = objectPool.Dequeue();
                                            clone.GetComponent<AgentController>().GetNPCPos(chunk.transform.position, chunkWidth, chunkHeight);
                                            clone.transform.rotation = Quaternion.identity;
                                            clone.SetActive(true);
                                        }
                                        
                                    }

                                }
                            }

                        }

                    }
                    count += 1;
                    break;


                case 1:
                   // if (playerPos.y < 0) range -= viewRange / 2;
                    for (float x = playerPos.x - (range/2); x < playerPos.x + (range / 2); x += chunkWidth)
                    {

                        for (float z = playerPos.z - (range / 2); z < playerPos.z + (range / 2); z += chunkWidth)
                        {
                            Vector3 pos = new Vector3(x, playerPos.y - chunkHeight, z);
                            Camera cam = Camera.current;
                            if (cam != null)
                            {
                                if (true)
                                {
                                    //BuildWorldSection(playerPos, pos);
                                    pos.x = Mathf.Floor(pos.x / (float)chunkWidth) * chunkWidth;
                                    pos.y = Mathf.Floor(pos.y / (float)chunkHeight) * chunkHeight;
                                    pos.z = Mathf.Floor(pos.z / (float)chunkWidth) * chunkWidth;
                                    // Shave square.
                                    Vector3 delta = pos - playerPos;
                                    if (delta.magnitude > range*2) continue;

                                    Chunk chunk = Chunk.FindChunk(pos);
                                
                                        if (chunk != null)
                                        {
                                            continue;
                                        }
                                        chunk = (Chunk)Instantiate(chunkFab, pos, Quaternion.identity);

                                    
                                }
                            }
                        }

                    }
                    count += 1;
                    break;


                //case 2:
                //    if (playerPos.y < 0) range -= viewRange / 4;
                //    for (float x = playerPos.x - range / 2; x < playerPos.x + range / 2; x += chunkWidth)
                //    {

                //        for (float z = playerPos.z - range / 2; z < playerPos.z + range / 2; z += chunkWidth)
                //        {
                //            Vector3 pos = new Vector3(x, playerPos.y - chunkHeight * 2, z);
                //            Camera cam = Camera.current;
                //            if (cam != null)
                //            {
                //                if (CheckLoadChunk(pos, playerMoving, cam))
                //                {
                //                    //BuildWorldSection(playerPos, pos);
                //                    pos.x = Mathf.Floor(pos.x / (float)chunkWidth) * chunkWidth;
                //                    pos.y = Mathf.Floor(pos.y / (float)chunkHeight) * chunkHeight;
                //                    pos.z = Mathf.Floor(pos.z / (float)chunkWidth) * chunkWidth;
                //                    // Shave square.
                //                    Vector3 delta = pos - playerPos;
                //                    if (delta.magnitude > range) continue;

                //                    Chunk chunk = Chunk.FindChunk(pos);
                //                    if (chunk != null && chunk.GetComponent<MeshRenderer>().isVisible)
                //                    {
                //                        continue;
                //                    }
                //                    chunk = (Chunk)Instantiate(chunkFab, pos, Quaternion.identity);

                //                }
                //            }
                //        }

                //    }
                //    StartCoroutine(BuildNavmesh(surface));
                //    count += 1;
                //    break;

                //case 3:
                //    StartCoroutine(BuildNavmesh(surface));
                //    count += 1;
                //    break;

                case 2:
                    for (int a = 0; a < Chunk.chunks.Count; a++)
                    {
                        Vector3 pos = Chunk.chunks[a].transform.position;
                        Vector3 delta = pos - playerPos;
                        if (delta.magnitude < viewRange + chunkWidth * 3 && Chunk.chunks[a].enabled) continue;
                        Destroy(Chunk.chunks[a].gameObject);
                    }
                    count = 0;
                    break;

                default:
                    break;
            }

            timeSinceLastCalled = 0f;
        } 
    }
    public bool CheckLoadChunk(Vector3 pos, bool playerMoving, Camera cam)
    {
        Vector3 viewPos = cam.WorldToViewportPoint(pos);
        //if (playerMoving)
        //{
            if (viewPos.x > -1.2F && viewPos.x <= 1.2F && viewPos.y > -1.2F && viewPos.y <= 1.2F && viewPos.z > 0)
            {
                //Debug.Log("moving and in shot");
                return true;
            }
            else
            {
                //Debug.Log("XXXXXXXXXt");
                return false;
            }
    }

    // void BuildWorldSection (Vector3 playerPos, Vector3 pos)
    //{

    //    pos.x = Mathf.Floor(pos.x / (float)chunkWidth) * chunkWidth;
    //    pos.y = Mathf.Floor(pos.y / (float)chunkHeight) * chunkHeight;
    //    pos.z = Mathf.Floor(pos.z / (float)chunkWidth) * chunkWidth;
    //    // Shave square.
    //    Vector3 delta = pos - playerPos;
    //    if (delta.magnitude > viewRange)
    //    {
    //        //Debug.Log("CHUNKYYY  "+ playerpos.x);
    //        Chunk chunk = Chunk.FindChunk(pos);

    //        if (chunk != null)
    //        {
    //           // Debug.Log("CHUNK2");
    //            chunk = (Chunk)Instantiate(chunkFab, pos, Quaternion.identity);

    //        }
    //        else
    //        {
    //            Debug.Log("CUNT");
    //        }
    //    }



    //}

 

    public static Biome GetIdealBiome(float moisture, float rockiness)
    {
        float bestBid = 0;
        Biome biome = currentWorld.biomes[0];
        for (int a = 0; a < currentWorld.biomes.Length; a++)
        {
            float bid = currentWorld.biomes[a].Bid(moisture, rockiness);
            if (bid > bestBid)
            {
                bestBid = bid;
                biome = currentWorld.biomes[a];
            }
        }
        return biome;
    }

    public void DestroyWorld(Ray ray, bool cord)
    {
        
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxInteractionRange))
        {
            chunkDict.Clear();
            Chunk chunk = hit.transform.GetComponent<Chunk>();
            //Debug.Log(chunk.transform.position.y);
            if (chunk != null)
            {
                Vector3 p = hit.point;
                //p.y /= World.currentWorld.brickHeight;
                p -= hit.normal / 4;
                p.x = Mathf.Floor(p.x);
                p.y = Mathf.Floor(p.y);
                p.z = Mathf.Floor(p.z);
                for (float x = p.x - damageRadius; x < p.x + damageRadius + 1; x++)
                {
                    for (float y = p.y - damageRadius; y < p.y + damageRadius + 1; y++)
                    {
                        for (float z = p.z - damageRadius; z < p.z + damageRadius + 1; z++)
                        {
                            Vector3 t = new Vector3(x, y, z);
                            //float distance = Vector3.Distance(t, p);
                            //if (distance <= damageRadius)
                            //{
                            if (x >= chunk.transform.position.x && x < (chunk.transform.position.x + chunkWidth) && z >= chunk.transform.position.z && z < (chunk.transform.position.z + chunkWidth) && y >= chunk.transform.position.y && y < (chunk.transform.position.y + chunkHeight))
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
                            else
                            {
                                //Debug.Log("FIND CHUNK");
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
                                else
                                {
                                    //Debug.Log("nochunk");
                                }
                            }
                            // }
                        }
                    }
                }

                foreach (KeyValuePair<Chunk, List<Vector3>> c in chunkDict)
                {
                    for (int xe = 0; xe < c.Value.Count; xe++)
                    {
                        Vector3 t = p;
                        t.x = Mathf.RoundToInt(c.Value[xe].x);
                        t.y = Mathf.RoundToInt(c.Value[xe].y);
                        t.z = Mathf.RoundToInt(c.Value[xe].z);
                        byte cubeColor = c.Key.GetByte(t);
                        if (cord)
                            //if (Input.GetKey(KeyCode.Tab))
                            {
                            c.Key.SetBrick(12, t);
                        }
                        else
                        {
                            if (c.Key.cubePositions.Contains(t))
                            {
                                c.Key.SetBrick(0, t);
                                if (createDebris)
                                {
                                    float offsetY;
                                    float d = (cubeColor % 8 - 1) / 8f;
                                    if (cubeColor < 8)
                                    {
                                        offsetY = 0.875F;
                                    }
                                    else
                                    {
                                        offsetY = 0.750F;
                                    }
                                    GameObject clone;
                                    clone = NPCPool.Dequeue();
                                    Rigidbody rclone = clone.GetComponent<Rigidbody>();
                                    rclone.transform.position = t;
                                    rclone.transform.rotation = Quaternion.identity;
                                    rclone.velocity = transform.TransformDirection(Vector3.forward * 20);
                                    Material m = rclone.GetComponent<Renderer>().material;
                                    m.SetTextureScale("_MainTex", new Vector2(0.125F, 0.125F));
                                    m.SetTextureOffset("_MainTex", new Vector2(d, offsetY));
                                    clone.SetActive(true);
                                    StartCoroutine(Waiter(clone));
                                }
                            }
                        }
                    }
                    StartCoroutine(c.Key.CreateVisualMesh());

                    //surface.BuildNavMesh();
                }
            }

        }
    }
    IEnumerator Waiter(GameObject clone)
    {
        float wait_time = Random.Range(0.5f, debrisLifetime);
        yield return new WaitForSeconds(wait_time);
        clone.SetActive(false);
        NPCPool.Enqueue(clone);
    }


}


