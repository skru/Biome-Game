using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using UnityEngine.AI;
using UnityEditor;

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


    public int cubePoolSize;
    public GameObject cube;
    public Queue<GameObject> cubePool;

    // AlterWorld
    Chunk chunk;
    Chunk chunkNew;
    Dictionary<Chunk, List<Vector3>> chunkDict = new Dictionary<Chunk, List<Vector3>>();
    public int NPCPoolSize;
    public GameObject capsule;
    public Queue<GameObject> NPCPool;



    void Awake()
    {
        currentWorld = this;
        if (seed == 0)
            seed = Random.Range(0, int.MaxValue);

        NPCPool = new Queue<GameObject>();
        for (int i = 0; i < NPCPoolSize; i++)
        {
            GameObject obj = Instantiate(capsule);
            obj.SetActive(false);
            NPCPool.Enqueue(obj);
        }

        cubePool = new Queue<GameObject>();
        for (int i = 0; i < cubePoolSize; i++)
        {
            GameObject obj = Instantiate(cube);
            obj.SetActive(false);
            cubePool.Enqueue(obj);
        }

        for (float x = 0; x < worldWidth; x += chunkWidth)
        {

            for (float z = 0; z < worldWidth; z += chunkWidth)
            {
                Vector3 pos = new Vector3(x, 0, z);

                //BuildWorldSection(playerPos, pos);
                pos.x = Mathf.Floor(pos.x / (float)chunkWidth) * chunkWidth;
                //pos.y = Mathf.Floor(pos.y / (float)chunkHeight) * chunkHeight;
                pos.z = Mathf.Floor(pos.z / (float)chunkWidth) * chunkWidth;
                // Shave square.
                // Vector3 delta = pos - 0;
                // if (delta.magnitude > range * 2) continue;

                Chunk chunk = Chunk.FindChunk(pos);

                if (chunk != null)
                {
                    continue;
                }
                //Debug.Log("WW");
                chunk = (Chunk)Instantiate(chunkFab, pos, Quaternion.identity);
                CreateNPC(chunk);


            }

        }
    }

    void Start()
    {
        

        
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
                                    pos.x = Mathf.Round(pos.x / (float)chunkWidth) * chunkWidth;
                                    pos.y = Mathf.Round(pos.y / (float)chunkHeight) * chunkHeight;
                                    pos.z = Mathf.Round(pos.z / (float)chunkWidth) * chunkWidth;
                                    // Shave square.
                                    Vector3 delta = pos - playerPos;
                                    if (delta.magnitude > range) continue;

                                    Chunk chunk = Chunk.FindChunk(pos);

                                    if (chunk != null)
                                    {
                                        continue;
                                    }
                                    chunk = (Chunk)Instantiate(chunkFab, pos, Quaternion.identity);
                                    
                                    CreateNPC(chunk);

                                }
                            }

                        }

                    }
                    count += 1;
                    break;


                case 1:
                   // if (playerPos.y < 0) range -= viewRange / 2;
                    for (float x = playerPos.x - (range/4); x < playerPos.x + (range / 4); x += chunkWidth)
                    {

                        for (float z = playerPos.z - (range / 4); z < playerPos.z + (range / 4); z += chunkWidth)
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


                case 2:
                    for (int a = 0; a < Chunk.chunks.Count; a++)
                    {
                        Vector3 pos = Chunk.chunks[a].transform.position;
                        Vector3 delta = pos - playerPos;
                        //if (delta.magnitude < viewRange + chunkWidth * 3 && Chunk.chunks[a].enabled) continue;
                        //Destroy(Chunk.chunks[a].gameObject);
                    }
                    count = 0;
                    break;

                default:
                    break;
            }

            timeSinceLastCalled = 0f;
        } 
    }

    void CreateNPC(Chunk chunk)
    {
        if (Random.value > 0.5)
        {
            if (NPCPool.Count > 0)
            {
                GameObject clone;
                clone = NPCPool.Dequeue();
                clone.GetComponent<AgentController>().GetNPCPos(chunk.transform.position, chunkWidth, chunkHeight);
                clone.transform.rotation = Quaternion.identity;
                clone.SetActive(true);
            }

        }
    }
    public bool CheckLoadChunk(Vector3 pos, bool playerMoving, Camera cam)
    {
        Vector3 viewPos = cam.WorldToViewportPoint(pos);
        //if (playerMoving)
        //{
        //if (viewPos.x > -1.2F && viewPos.x <= 1.2F && viewPos.y > -1.2F && viewPos.y <= 1.2F && viewPos.z > 0)
        //{
        //    //Debug.Log("moving and in shot");
        //    return true;
        //}
        //else
        //{
        //    //Debug.Log("XXXXXXXXXt");
        //    return false;
        //}
        return true;
    }

    public void AlterWorld(Ray ray, bool alterOrDestroy, float damageRadius, bool createDebris, float debrisLifetime, float maxInteractionRange)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxInteractionRange))
        {
            chunkDict.Clear();
            Chunk chunk = hit.transform.GetComponent<Chunk>();
            //Debug.Log(chunk.transform.position.y);
            //Debug.Log(chunk.transform.position.x);
            //Debug.Log(chunk.transform.position.y);
            //Debug.Log(chunk.transform.position.z);
            if (chunk != null)
            {
                Vector3 p = hit.point;
                p.y /= World.currentWorld.brickHeight;
                if (alterOrDestroy)
                {
                    p += hit.normal / 4;
                }
                else
                {
                    p -= hit.normal / 4;
                }
               
                p.x = Mathf.Floor(p.x);
                p.y = Mathf.Floor(p.y);
                p.z = Mathf.Floor(p.z);
                //Debug.Log(hit.point);
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
                            float distance = Vector3.Distance(t, player.transform.position);
                            if (distance > 2F)
                            {
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
                                    chunkNew = Chunk.FindChunk(new Vector3(t.x, t.y, t.z));
                                    if (chunkNew != null)
                                    {
                                        if (!chunkDict.ContainsKey(chunkNew))
                                        {

                                            chunkDict.Add(chunkNew, new List<Vector3> { t });

                                        }
                                        else
                                        {
                                            chunkDict[chunkNew].AddRange(new List<Vector3> { t });
                                        }
                                    }
                                    else
                                    {
                                        Debug.Log("nochunk");
                                        Debug.Log(t.x);
                                        Debug.Log(t.y);
                                        Debug.Log(t.z);
                                        Vector3 newChunkPos = new Vector3(Mathf.Floor(t.x / (float)chunkWidth) * chunkWidth, Mathf.Floor(t.y / (float)chunkHeight) * chunkHeight, Mathf.Floor(t.z / (float)chunkWidth) * chunkWidth);
                             
                                        chunk = (Chunk)Instantiate(chunkFab, newChunkPos, Quaternion.identity);
                                        Debug.Log(chunk.transform.position.x);
                                        Debug.Log(chunk.transform.position.y);
                                        Debug.Log(chunk.transform.position.z);
                                        Selection.activeGameObject = chunk.gameObject;
                                        //GetTheoreticalByte(t);
                                    }
                                }
                            }
                            //}
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
                        if (alterOrDestroy)
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
                                    clone = cubePool.Dequeue();
                                    Rigidbody rclone = clone.GetComponent<Rigidbody>();
                                    rclone.transform.position = t;
                                    rclone.transform.rotation = Quaternion.identity;
                                    rclone.velocity = transform.TransformDirection(Vector3.forward * 20);
                                    Material m = rclone.GetComponent<Renderer>().material;
                                    m.SetTextureScale("_MainTex", new Vector2(0.125F, 0.125F));
                                    m.SetTextureOffset("_MainTex", new Vector2(d, offsetY));
                                    clone.SetActive(true);
                                    StartCoroutine(Waiter(clone, debrisLifetime));
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

    float NearestMultipleOf(float x, int multiple)
    {
        if (x >= 0)
        {
            return x - (x % multiple);
        }
        else
        {
            return x - (x % -multiple);
        }
        
        //float mod = x % multiple;
        //float midPoint = multiple / 2.0f;
        //if (mod > midPoint)
        //{
        //    return x + (multiple - mod);
        //}
        //else
        //{
        //    return x - mod;
        //}
    }

    IEnumerator Waiter(GameObject clone, float debrisLifetime)
    {
        float wait_time = Random.Range(0.5f, debrisLifetime);
        yield return new WaitForSeconds(wait_time);
        clone.SetActive(false);
        cubePool.Enqueue(clone);
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


}


