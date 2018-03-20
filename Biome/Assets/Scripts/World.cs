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

        for (int a = 0; a < Chunk.chunks.Count; a++)
        {
            Vector3 pos = Chunk.chunks[a].transform.position;
            Vector3 delta = pos - playerPos;
            if (delta.magnitude < viewRange + chunkWidth * 3) continue;
            Destroy(Chunk.chunks[a].gameObject);
            //Chunk.chunks[a].gameObject.SetActive(false);
        }
        
        //Debug.Log(playerPos.x);

        for (float x = playerPos.x - viewRange; x < playerPos.x + viewRange; x += chunkWidth)
        {
            for (float y = playerPos.y - viewRange; y < playerPos.y + viewRange; y += chunkHeight)
            {
                for (float z = playerPos.z - viewRange; z < playerPos.z + viewRange; z += chunkWidth)
                {
                    Vector3 pos = new Vector3(x, y, z);

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
                    //Debug.Log("CHUNKINGGS");
                    chunk = (Chunk)Instantiate(chunkFab, pos, Quaternion.identity);
                    //if (Chunk.chunks.Contains(chunk))
                    //{
                    //    Debug.Log("CONT CHUNK");
                    //    chunk.gameObject.SetActive(true);
                    //}
                    //else
                    //{
                    //    Debug.Log("NOT CONT CHUNK");
                    //    chunk = (Chunk)Instantiate(chunkFab, pos, Quaternion.identity);

                    //}
                }

            }
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


