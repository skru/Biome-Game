using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class World : MonoBehaviour {
	
	public Biome[] biomes;
	
	public static World currentWorld;
	public int worldWidth = 2, chunkWidth = 20, chunkHeight = 20, seed = 0;
	public float viewRange = 30;
	public float brickHeight = 1;
	public Chunk chunkFab;
	public GameObject player;
	
	// Use this for initialization
	void Awake () {
		currentWorld = this;
		if (seed == 0)
			seed = Random.Range(0, int.MaxValue) * Random.Range(0, int.MaxValue);

		for (int x = 0; x < worldWidth; x++)
        {
            for (int y = -4; y < worldWidth; y++)
            {
                for (int z = 0; z < worldWidth; z++)
                {
                    Vector3 pos = new Vector3((x * chunkWidth), (y * chunkHeight), (z * chunkWidth));
                    pos.x = Mathf.Floor(pos.x / (float)chunkWidth) * chunkWidth;
                    pos.y = Mathf.Floor(pos.y / (float)chunkHeight) * chunkHeight * -1;
                    pos.z = Mathf.Floor(pos.z / (float)chunkWidth) * chunkWidth;
                    Chunk chunk = Chunk.FindChunk(pos);
                    if (chunk != null)
                    {
                        continue;
                    }
                    chunk = (Chunk)Instantiate(chunkFab, pos, Quaternion.identity);
                }
            }
		}

		Vector3 playerPos = new Vector3 ((chunkWidth * worldWidth / 2), chunkHeight+10, (chunkWidth * worldWidth / 2));
		player.transform.position = playerPos;

					
	}
	void Update () {
		if (Input.GetKeyDown(KeyCode.G))
		{
			if (seed != 1) {
				seed = Random.Range (0, int.MaxValue);
			}
			Vector3 playerPos = new Vector3 ((chunkWidth * worldWidth / 2), chunkHeight+10, (chunkWidth * worldWidth / 2));
			player.transform.position = playerPos;
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


