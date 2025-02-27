﻿using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using SimplexNoise;

public enum BrickType {
	None,
	
	RoughStone,
	SmoothStone,
	DarkStone,
	PowderStone,
	Granite,
	Dirt, 
	Sand,
	GreenLattice,
	
	Taint,
	Dust,
	Rust,
	Ice,
	Snow,
	DirtyIce,
	Streaks,
	Lava
}

[RequireComponent (typeof(MeshRenderer))]
[RequireComponent (typeof(MeshCollider))]
[RequireComponent (typeof(MeshFilter))]
public class Chunk : MonoBehaviour {
	
	//public static List<Chunk> chunksWaiting = new List<Chunk>();
	public static List<Chunk> chunks = new List<Chunk>();

    //public List<Vector3> cubePositions = new List<Vector3>();
    public HashSet<Vector3> cubePositions = new HashSet<Vector3>();

    public static int worldWidth {
		get { return World.currentWorld.worldWidth; }
	}
	public static int width {
		get { return World.currentWorld.chunkWidth; }
	}
	public static int height {
		get { return World.currentWorld.chunkHeight; }
	}
	public static float brickHeight {
		get { return World.currentWorld.brickHeight; }
	}
	
	public byte[,,] map;
	public Mesh visualMesh;
	protected MeshRenderer meshRenderer;
	protected MeshCollider meshCollider;
	protected MeshFilter meshFilter;
	protected bool initialized = false;


    // Use this for initialization
    void Start () {
		
		chunks.Add(this);
		
		meshRenderer = GetComponent<MeshRenderer>();
		meshCollider = GetComponent<MeshCollider>();
		meshFilter = GetComponent<MeshFilter>();
		
//		chunksWaiting.Add(this);
//		
//		if (chunksWaiting[0] == this)
//		{
//			StartCoroutine(CalculateMapFromScratch());
//		}
		StartCoroutine(CalculateMapFromScratch());
		//StartCoroutine(CreateVisualMesh());
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.G)) {
			
			chunks.Add (this);
			meshRenderer = GetComponent<MeshRenderer> ();
			meshCollider = GetComponent<MeshCollider> ();
			meshFilter = GetComponent<MeshFilter> ();
			StartCoroutine (CalculateMapFromScratch ());
		}
	}
//	
//	void OnDestroy()
//	{
//		if (chunksWaiting.Contains(this)) chunksWaiting.Remove(this);
//		chunks.Remove(this);
//	}
	
	public static byte GetTheoreticalByte(Vector3 pos) {
		Random.seed = World.currentWorld.seed;
		
		Vector3 grain0Offset = new Vector3(Random.value * 10000, Random.value * 10000, Random.value * 10000);
		Vector3 grain1Offset = new Vector3(Random.value * 10000, Random.value * 10000, Random.value * 10000);
		Vector3 grain2Offset = new Vector3(Random.value * 10000, Random.value * 10000, Random.value * 10000);
		
		return GetTheoreticalByte(pos, grain0Offset, grain1Offset, grain2Offset);
		
	}
	
	public static byte GetTheoreticalByte(Vector3 pos, Vector3 offset0, Vector3 offset1, Vector3 offset2)
	{
		float moisture = CalculateNoiseValue(pos, offset2,  0.001f);		
		float rockiness = CalculateNoiseValue(pos, offset2,  0.003f);		
		
		Biome biome = World.GetIdealBiome(moisture, rockiness);
		
			
		//float heightBase = biome.minHeight;
		//float maxHeight = biome.maxHeight;
		//float heightSwing = maxHeight - heightBase;
		
		
		float blobValue = CalculateNoiseValue(pos, offset1,  0.05f);
		float mountainValue = CalculateNoiseValue(pos, offset0,  0.009f);
		
		//mountainValue += biome.mountainPowerBonus;
		//if (mountainValue < 0) mountainValue = 0;
		
		//if (biome.mountainPower != 1)
			//mountainValue = Mathf.Pow(mountainValue, biome.mountainPower);  //Mathf.Sqrt(mountainValue);
		
		byte brick = biome.GetBrick(Mathf.FloorToInt(pos.y), mountainValue, blobValue, moisture, rockiness);
		
		
		//mountainValue *= heightSwing;
		//mountainValue += heightBase;
		
		//mountainValue += (blobValue * 10) - 5f;
					
					
		//if (mountainValue >= pos.y)
		return brick;
		//return 0;
	}
	
	public virtual IEnumerator CalculateMapFromScratch() {
		
		map = new byte[width, height, width];
        
		Random.seed = World.currentWorld.seed;
		Vector3 grain0Offset = new Vector3(Random.value * 10000, Random.value * 10000, Random.value * 10000);
		Vector3 grain1Offset = new Vector3(Random.value * 10000, Random.value * 10000, Random.value * 10000);
		Vector3 grain2Offset = new Vector3(Random.value * 10000, Random.value * 10000, Random.value * 10000);
		
		
		
		for (int x = 0; x < World.currentWorld.chunkWidth; x++)
		{
			for (int y = 0; y < height; y++)
			{
				for (int z = 0; z < width; z++)
				{
                    Vector3 pos = new Vector3(x, y, z);
                    pos = pos + transform.position;
                    map[x, y, z] = GetTheoreticalByte(pos, grain0Offset, grain1Offset, grain2Offset);
                    

				
				}
			}
		}
        
		yield return 0;
		StartCoroutine(CreateVisualMesh());
		
		initialized = true;
		

//		chunksWaiting.Remove(this);
//		
//		while ( (chunksWaiting.Count > 0) && (chunksWaiting[0] == null) )
//			chunksWaiting.RemoveAt(0);
//			
//		if (chunksWaiting.Count > 0)
//		{
//			StartCoroutine(chunksWaiting[0].CalculateMapFromScratch());
//		}
		
	}
	
	public static float CalculateNoiseValue(Vector3 pos, Vector3 offset, float scale)
	{
		
		float noiseX = Mathf.Abs((pos.x + offset.x) * scale);
		float noiseY = Mathf.Abs((pos.y + offset.y) * scale);
		float noiseZ = Mathf.Abs((pos.z + offset.z) * scale);
		
		return Mathf.Max(0, Noise.Generate(noiseX, noiseY, noiseZ));
		
	}

    public virtual IEnumerator CreateVisualMesh() {
		visualMesh = new Mesh();
		
		List<Vector3> verts = new List<Vector3>();
		List<Vector2> uvs = new List<Vector2>();
		List<int> tris = new List<int>();

        cubePositions.Clear();
        for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				for (int z = 0; z < width; z++)
				{
					if (map[x,y,z] == 0) continue;
					
					byte brick = map[x,y,z];
					// Left wall
					if (IsTransparent(x - 1, y, z))
						StartCoroutine(BuildFace (brick, new Vector3(x, y, z), Vector3.up, Vector3.forward, false, verts, uvs, tris));
					// Right wall
					if (IsTransparent(x + 1, y , z))
                        StartCoroutine(BuildFace (brick, new Vector3(x + 1, y, z), Vector3.up, Vector3.forward, true, verts, uvs, tris));
					
					// Bottom wall
					if (IsTransparent(x, y - 1 , z))
                        StartCoroutine(BuildFace (brick, new Vector3(x, y, z), Vector3.forward, Vector3.right, false, verts, uvs, tris));
					// Top wall
					if (IsTransparent(x, y + 1, z))
                        StartCoroutine(BuildFace (brick, new Vector3(x, y + 1, z), Vector3.forward, Vector3.right, true, verts, uvs, tris));
					
					// Back
					if (IsTransparent(x, y, z - 1))
                        StartCoroutine(BuildFace (brick, new Vector3(x, y, z), Vector3.up, Vector3.right, true, verts, uvs, tris));
					// Front
					if (IsTransparent(x, y, z + 1))
                        StartCoroutine(BuildFace (brick, new Vector3(x, y, z + 1), Vector3.up, Vector3.right, false, verts, uvs, tris));

                    Vector3 t = new Vector3(x, y, z) + transform.position;
                    cubePositions.Add(t);


                }
			}
		}
					
		visualMesh.vertices = verts.ToArray();
		visualMesh.uv = uvs.ToArray();
		visualMesh.triangles = tris.ToArray();
		visualMesh.RecalculateBounds();
		visualMesh.RecalculateNormals();
		meshFilter.mesh = visualMesh;
		
		
		meshCollider.sharedMesh = null;
		meshCollider.sharedMesh = visualMesh;
		yield return 1;
		
	}
	public virtual IEnumerator BuildFace(byte brick, Vector3 corner, Vector3 up, Vector3 right, bool reversed, List<Vector3> verts, List<Vector2> uvs, List<int> tris)
	{
		int index = verts.Count;
		
		
		float uvRow = ((corner.y + up.y) % 7);
		if (uvRow >= 4) uvRow = 7 - uvRow;
		uvRow /= 4f;
		Vector2 uvCorner = new Vector2(0.00f, uvRow);
		
		if (brick < 8)
			uvCorner.y += 0.125f;
		
		
		
		corner.y *= brickHeight;
		up.y *= brickHeight;
		right.y *= brickHeight;
		
		
		verts.Add (corner);
		verts.Add (corner + up);
		verts.Add (corner + up + right);
		verts.Add (corner + right);
		
		Vector2 uvWidth = new Vector2(0.125f, 0.125f);
		
		uvCorner.x += (float)((brick) % 8 - 1) / 8f;
		
		uvs.Add(uvCorner);
		uvs.Add(new Vector2(uvCorner.x, uvCorner.y + uvWidth.y));
		uvs.Add(new Vector2(uvCorner.x + uvWidth.x, uvCorner.y + uvWidth.y));
		uvs.Add(new Vector2(uvCorner.x + uvWidth.x, uvCorner.y));
		
		if (reversed)
		{
			tris.Add(index + 0);
			tris.Add(index + 1);
			tris.Add(index + 2);
			tris.Add(index + 2);
			tris.Add(index + 3);
			tris.Add(index + 0);
		}
		else
		{
			tris.Add(index + 1);
			tris.Add(index + 0);
			tris.Add(index + 2);
			tris.Add(index + 3);
			tris.Add(index + 2);
			tris.Add(index + 0);
		}
        yield return 0;
		
	}
	public virtual bool IsTransparent (int x, int y, int z)
	{
		//if ( y < 0) return false;
		byte brick = GetByte(x,y,z);
		switch (brick)
		{
		case 0: 
			return true;
		default:
			return false;
		}
	}
	public virtual byte GetByte (int x, int y , int z)
	{
		
		
//		if ((y < 0) || (y >= height))
//			return 0;
//		
		Vector3 worldPos = new Vector3(x, y, z) + transform.position;
//		if (! initialized)
//			return GetTheoreticalByte(worldPos);
		
		
		if ( (x < 0) || (z < 0)  || (x >= width) || (z >= width) || (y < 0) || (y >= height))
		{
			return 0;
			
//			Chunk chunk = Chunk.FindChunk(worldPos);
//			if (chunk == this) return 0;
//			if (chunk == null) 
//			{
//				return GetTheoreticalByte(worldPos);
//			}
//			return chunk.GetByte (worldPos);
		}
		return map[x,y,z];
	}
	public virtual byte GetByte(Vector3 worldPos) {
		worldPos -= transform.position;
		int x = Mathf.FloorToInt(worldPos.x);
		int y = Mathf.FloorToInt(worldPos.y);
		int z = Mathf.FloorToInt(worldPos.z);
		return GetByte (x, y, z);
	}
	
	public static Chunk FindChunk(Vector3 pos) {
		
		for (int a = 0; a < chunks.Count; a++)
		{
			Vector3 cpos = chunks[a].transform.position;
			if ( ( pos.x < cpos.x) || (pos.y < cpos.y)||(pos.z < cpos.z) || (pos.x > cpos.x + width) || (pos.y > cpos.y + height)||(pos.z > cpos.z + width) ) continue;
			return chunks[a];
			
		}
		return null;
		
	}
	
	public bool SetBrick (byte brick, Vector3 worldPos, Chunk chunk)
	{
		worldPos -= transform.position;
		return SetBrick(brick, Mathf.FloorToInt(worldPos.x),Mathf.FloorToInt(worldPos.y),Mathf.FloorToInt(worldPos.z), chunk);
	}
	public bool SetBrick (byte brick, int x, int y, int z, Chunk chunk)
	{
		if ( ( x < 0) || (y < 0) || (z < 0) || (x > (width * worldWidth)) || (y > height) || (z > (width * worldWidth)) )
		{
			return false;
		}
		if (brick == 0) {
			map [x, y, z] = brick;
		} else {
			map [x, y+1, z] = brick;
		}
		return true;
	}

    public virtual byte IsBrick(byte brick, Vector3 worldPos, Chunk chunk)
    {
        worldPos -= transform.position;
        return IsBrick(brick, Mathf.FloorToInt(worldPos.x), Mathf.FloorToInt(worldPos.y), Mathf.FloorToInt(worldPos.z), chunk);
    }
    public virtual byte IsBrick(byte brick, int x, int y, int z, Chunk chunk)
    {
        Vector3 worldPos = new Vector3(x, y, z) + transform.position;
        return GetTheoreticalByte(worldPos);
    }

    public virtual byte ColorBrick(byte brick, Vector3 worldPos, Chunk chunk)
    {
        worldPos -= transform.position;
        return ColorBrick(brick, Mathf.FloorToInt(worldPos.x), Mathf.FloorToInt(worldPos.y), Mathf.FloorToInt(worldPos.z), chunk);
    }
    public virtual byte ColorBrick(byte brick, int x, int y, int z, Chunk chunk)
    {
        Vector3 worldPos = new Vector3(x, y, z) + transform.position;
        return GetTheoreticalByte(worldPos);
    }
}




