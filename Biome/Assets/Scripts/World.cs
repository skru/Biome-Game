using UnityEngine;
using System.Collections;

public class World : MonoBehaviour
{

    public Texture2D defaultBrickTexture;

    public Biome[] biomes;

    public static World currentWorld;
    public int chunkWidth = 20, chunkHeight = 20, seed = 0;
    public int worldWidth = 20;
    public float viewRange = 30;

    public float brickHeight = 1;

    public Chunk chunkFab;
    public PlayerIO player;
    bool state1 = true;
    bool state2 = true;


    // Use this for initialization
    void Awake()
    {
        currentWorld = this;
        if (seed == 0)
            seed = Random.Range(0, int.MaxValue);  
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = player.transform.position;

        if (state1 && state2)
        {
            for (int a = 0; a < Chunk.chunks.Count; a++)
            {
                Vector3 pos = Chunk.chunks[a].transform.position;
                Vector3 delta = pos - playerPos;
                if (delta.magnitude < viewRange + chunkWidth * 2) continue;
                Destroy(Chunk.chunks[a].gameObject);

            }
            state1 = false;
            state2 = true;

        }
        else if (!state1 && state2)
        {
            for (float x = playerPos.x - viewRange; x < playerPos.x + viewRange; x += chunkWidth)
            {

                for (float z = playerPos.z - viewRange; z < playerPos.z + viewRange; z += chunkWidth)
                {
                    Vector3 pos = new Vector3(x, playerPos.y, z);

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
            state1 = false;
            state2 = false;
        }
        else if (!state1 && !state2)
        {
            for (float x = playerPos.x - viewRange / 2; x < playerPos.x + viewRange / 2; x += chunkWidth)
            {

                for (float z = playerPos.z - viewRange / 2; z < playerPos.z + viewRange / 2; z += chunkWidth)
                {
                    Vector3 pos = new Vector3(x, playerPos.y + chunkHeight, z);

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
            state1 = true;
            state2 = false;
        }
        else
        {
            for (float x = playerPos.x - viewRange / 2; x < playerPos.x + viewRange / 2; x += chunkWidth)
            {
              
                    for (float z = playerPos.z - viewRange / 2; z < playerPos.z + viewRange / 2; z += chunkWidth)
                    {
                        Vector3 pos = new Vector3(x, playerPos.y- chunkHeight, z);

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
            state1 = true;
            state2 = true;
        }
    }

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


