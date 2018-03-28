using UnityEngine;
using System.Collections;

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
    //public Chunk chunk;
    public PlayerIO player;
    bool state1 = false;
    bool state2 = false;
    bool state3 = false;
    int count = 0;
    private float timeSinceLastCalled;

    private float delay = 0.5f;
    //public static Camera current;

    // Use this for initialization
    void Awake()
    {
        currentWorld = this;
        if (seed == 0)
            seed = Random.Range(0, int.MaxValue);
        //current = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
       // Camera current;
        timeSinceLastCalled += Time.deltaTime;
        if (timeSinceLastCalled > delay)
        {


            Vector3 playerPos = player.transform.position;


            switch (count)
            {
                case 0:
                    for (float x = playerPos.x - viewRange; x < playerPos.x + viewRange; x += chunkWidth)
                    {

                        for (float z = playerPos.z - viewRange; z < playerPos.z + viewRange; z += chunkWidth)
                        {
                            Vector3 pos = new Vector3(x, playerPos.y, z);
                            Camera cam = Camera.current;
                            if (cam != null)
                            {
                                Vector3 viewPos = cam.WorldToViewportPoint(pos);
                                if (viewPos.x > -0.5F && viewPos.x <= 1.5F && viewPos.y > -0.5F && viewPos.y <= 1.5F && viewPos.z > 0.5F)
                                {
                                    //BuildWorldSection(playerPos, pos);
                                    pos.x = Mathf.Floor(pos.x / (float)chunkWidth) * chunkWidth;
                                    pos.y = Mathf.Floor(pos.y / (float)chunkHeight) * chunkHeight;
                                    pos.z = Mathf.Floor(pos.z / (float)chunkWidth) * chunkWidth;
                                    // Shave square.
                                    Vector3 delta = pos - playerPos;
                                    if (delta.magnitude > viewRange) continue;

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

                case 1:
                    for (float x = playerPos.x - viewRange; x < playerPos.x + viewRange; x += chunkWidth)
                    {

                        for (float z = playerPos.z - viewRange; z < playerPos.z + viewRange; z += chunkWidth)
                        {
                            Vector3 pos = new Vector3(x, playerPos.y + chunkHeight, z);
                            Camera cam = Camera.current;
                            if (cam != null)
                            {
                                Vector3 viewPos = cam.WorldToViewportPoint(pos);
                                if (viewPos.x > -0.5F && viewPos.x <= 1.5F && viewPos.y > -0.5F && viewPos.y <= 1.5F && viewPos.z > 0.5F)
                                {
                                    //BuildWorldSection(playerPos, pos);
                                    pos.x = Mathf.Floor(pos.x / (float)chunkWidth) * chunkWidth;
                                    pos.y = Mathf.Floor(pos.y / (float)chunkHeight) * chunkHeight;
                                    pos.z = Mathf.Floor(pos.z / (float)chunkWidth) * chunkWidth;
                                    // Shave square.
                                    Vector3 delta = pos - playerPos;
                                    if (delta.magnitude > viewRange) continue;

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
                    for (float x = playerPos.x - viewRange / 2; x < playerPos.x + viewRange / 4; x += chunkWidth)
                    {

                        for (float z = playerPos.z - viewRange / 2; z < playerPos.z + viewRange / 4; z += chunkWidth)
                        {
                            Vector3 pos = new Vector3(x, playerPos.y - chunkHeight, z);
                            Camera cam = Camera.current;
                            if (cam != null)
                            {
                                Vector3 viewPos = cam.WorldToViewportPoint(pos);
                                if (viewPos.x > -0.5F && viewPos.x <= 1.5F && viewPos.y > -0.5F && viewPos.y <= 1.5F && viewPos.z > 0.5F)
                                {
                                    //BuildWorldSection(playerPos, pos);
                                    pos.x = Mathf.Floor(pos.x / (float)chunkWidth) * chunkWidth;
                                    pos.y = Mathf.Floor(pos.y / (float)chunkHeight) * chunkHeight;
                                    pos.z = Mathf.Floor(pos.z / (float)chunkWidth) * chunkWidth;
                                    // Shave square.
                                    Vector3 delta = pos - playerPos;
                                    if (delta.magnitude > viewRange) continue;

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

                //case 3:
                //    for (float x = playerPos.x - viewRange; x < playerPos.x + viewRange; x += chunkWidth)
                //    {

                //        for (float z = playerPos.z - viewRange; z < playerPos.z + viewRange; z += chunkWidth)
                //        {
                //            Vector3 pos = new Vector3(x, playerPos.y + chunkHeight * 2, z);
                //            Camera cam = Camera.current;
                //            if (cam != null)
                //            {
                //                Vector3 viewPos = cam.WorldToViewportPoint(pos);
                //                if (viewPos.x > -0.5F && viewPos.x <= 1.5F && viewPos.y > -0.5F && viewPos.y <= 1.5F && viewPos.z > 0.5F)
                //                {
                //                    //BuildWorldSection(playerPos, pos);
                //                    pos.x = Mathf.Floor(pos.x / (float)chunkWidth) * chunkWidth;
                //                    pos.y = Mathf.Floor(pos.y / (float)chunkHeight) * chunkHeight;
                //                    pos.z = Mathf.Floor(pos.z / (float)chunkWidth) * chunkWidth;
                //                    // Shave square.
                //                    Vector3 delta = pos - playerPos;
                //                    if (delta.magnitude > viewRange) continue;

                //                    Chunk chunk = Chunk.FindChunk(pos);

                //                    if (chunk != null)
                //                    {
                //                        continue;
                //                    }
                //                    chunk = (Chunk)Instantiate(chunkFab, pos, Quaternion.identity);
                //                }
                //            }
                //        }

                //    }
                //    count += 1;
                //    break;

                //case 4:
                //    for (float x = playerPos.x - viewRange / 2; x < playerPos.x + viewRange / 4; x += chunkWidth)
                //    {

                //        for (float z = playerPos.z - viewRange / 2; z < playerPos.z + viewRange / 4; z += chunkWidth)
                //        {
                //            Vector3 pos = new Vector3(x, playerPos.y - chunkHeight * 2, z);
                //            Camera cam = Camera.current;
                //            if (cam != null)
                //            {
                //                Vector3 viewPos = cam.WorldToViewportPoint(pos);
                //                if (viewPos.x > -0.5F && viewPos.x <= 1.5F && viewPos.y > -0.5F && viewPos.y <= 1.5F && viewPos.z > 0.5F)
                //                {
                //                    //BuildWorldSection(playerPos, pos);
                //                    pos.x = Mathf.Floor(pos.x / (float)chunkWidth) * chunkWidth;
                //                    pos.y = Mathf.Floor(pos.y / (float)chunkHeight) * chunkHeight;
                //                    pos.z = Mathf.Floor(pos.z / (float)chunkWidth) * chunkWidth;
                //                    // Shave square.
                //                    Vector3 delta = pos - playerPos;
                //                    if (delta.magnitude > viewRange) continue;

                //                    Chunk chunk = Chunk.FindChunk(pos);

                //                    if (chunk != null)
                //                    {
                //                        continue;
                //                    }
                //                    chunk = (Chunk)Instantiate(chunkFab, pos, Quaternion.identity);
                //                }
                //            }
                //        }

                //    }
                //    count += 1;
                //    break;

                case 3:
                    for (int a = 0; a < Chunk.chunks.Count; a++)
                    {
                        Vector3 pos = Chunk.chunks[a].transform.position;
                        Vector3 delta = pos - playerPos;
                        if (delta.magnitude < viewRange + chunkWidth * 2) continue;
                        Destroy(Chunk.chunks[a].gameObject);

                    }
                    count = 0;
                    break;

                default:
                    //Debug.Log("DEF");
                    break;
            }

            timeSinceLastCalled = 0f;
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


}


