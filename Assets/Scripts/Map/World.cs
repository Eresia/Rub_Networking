using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class World : MonoBehaviour {

	[System.Serializable]
	public struct WorldGeneration{
		public int mapWidth;
		public int mapLength;
		public int minHeight;
		public float mapSeed;
		public float biomeSeed;
		

		public WorldGeneration(int mapWidth, int mapLength, int minHeight, float mapSeed, float biomeSeed){
			this.mapWidth = mapWidth;
			this.mapLength = mapLength;
			this.minHeight = minHeight;
			this.mapSeed = mapSeed;
			this.biomeSeed = biomeSeed;
		}
	}

	public MapBlock[] blockPrefabs;

	public int nbSpawnBlock;

	public bool createInside;

	public float deathZone;

	public Transform selfTransform {get ; private set;}

	public WorldGeneration worldGeneration {get ; private set;}

	private MapBlock[,,] mapTable;

	private Vector3Int[] spawnBlocks;

	private float blockSize;

	void Awake () {
		selfTransform = GetComponent<Transform>();
	}

	public void GenerateMap(int width, int length, int minHeight, float floatScale, float biomeSeed){
		GenerateMap(new WorldGeneration(width, length, minHeight, floatScale, biomeSeed));
	}

	public void GenerateMap(WorldGeneration worldGeneration){
		this.worldGeneration = worldGeneration;
		blockSize = blockPrefabs[0].GetComponent<Transform>().localScale.x/* + 0.05f*/;
		int maxHeight = Mathf.RoundToInt(worldGeneration.mapSeed) +worldGeneration.minHeight + 1;
		mapTable = new MapBlock[worldGeneration.mapWidth, worldGeneration.mapLength, maxHeight];
		// for(int i = 0; i < mapTable.Length; i++){
		// 	mapTable[i] = new MapBlock[length][];
		// 	for(int j = 0; j < mapTable[i].Length; j++){
		// 		mapTable[i][j] = new MapBlock[Mathf.RoundToInt(floatScale) + 1];
		// 	}
		// }

		for(int i = 0; i < worldGeneration.mapWidth; i++){
			for(int j = 0; j < worldGeneration.mapLength; j++){
				Vector2 realPos = GetRealVector2BlockPosition(i, j);

				int biomeNoise = (int) (MakePerlinNoise(i * worldGeneration.biomeSeed, j * worldGeneration.biomeSeed, blockPrefabs.Length + worldGeneration.biomeSeed));
				
				MapBlock prefab = blockPrefabs[biomeNoise];

				int blockHeight = GetBlockHeight(worldGeneration.mapSeed, biomeNoise, i, j, worldGeneration.minHeight, blockPrefabs);

				if(createInside){
					for(int k = blockHeight; k >= 1; k--){
						CreateBlock(new Vector3(realPos.x, k * blockSize, realPos.y), new Vector3Int(i, j, k), selfTransform, prefab);
					}

					CreateBlock(new Vector3(realPos.x, 0, realPos.y), new Vector3Int(i, j, 0), selfTransform, blockPrefabs[2]);
				}
				else{
					CreateBlock(new Vector3(realPos.x, blockHeight * blockSize, realPos.y), new Vector3Int(i, j, blockHeight), selfTransform, prefab);
				}

				
			}
		}

		spawnBlocks = new Vector3Int[nbSpawnBlock];
		for(int i = 0; i < nbSpawnBlock; i++){
			spawnBlocks[i] = GenerateSpawnBlock(worldGeneration, blockPrefabs);
		}
	}

	public Vector3 GetRandomSpawnPosition(){
		Vector3Int pos = spawnBlocks[UnityEngine.Random.Range(0, spawnBlocks.Length)];
		return GetRealVector3BlockPosition(pos);
	}

	public Vector2 GetRealVector2BlockPosition(int x, int z){
		return new Vector2((x + (-worldGeneration.mapWidth / 2)) * blockSize, (z + (-worldGeneration.mapLength / 2)) * blockSize);
	}

	public Vector3 GetRealVector3BlockPosition(Vector3Int pos){
		return GetRealVector3BlockPosition(pos.x, pos.y, pos.z);
	}

	public Vector3 GetRealVector3BlockPosition(int x, int y, int z){
		return new Vector3((x + (-worldGeneration.mapWidth / 2)) * blockSize, y * blockSize, (z + (-worldGeneration.mapLength / 2)) * blockSize);
	}

	private static Vector3Int GenerateSpawnBlock(WorldGeneration worldGeneration, MapBlock[] possibleBlocks){
		int posX;
		int posZ;
		int biomeNoise;

		do{
			posX = UnityEngine.Random.Range(0, worldGeneration.mapWidth);
			posZ = UnityEngine.Random.Range(0, worldGeneration.mapLength);
			biomeNoise = (int) (MakePerlinNoise(posX * worldGeneration.biomeSeed, posZ * worldGeneration.biomeSeed, possibleBlocks.Length + worldGeneration.biomeSeed));
		} while(!possibleBlocks[biomeNoise].canSpawnOn);

		int blockHeight = GetBlockHeight(worldGeneration.mapSeed, biomeNoise, posX, posZ, worldGeneration.minHeight, possibleBlocks);

		return new Vector3Int(posX, blockHeight, posZ);
	}

	private static float MakePerlinNoise(float i, float j, float flatscale){
		return Mathf.PerlinNoise (((float) i) / flatscale, ((float) j) / flatscale) * flatscale;
	}

	private static int GetBlockHeight(float mapSeed, int biomeNoise, int i, int j, int minHeight, MapBlock[] possibleBlocks){
		if(possibleBlocks[biomeNoise].fixedHeight != -1){
			return possibleBlocks[biomeNoise].fixedHeight;
		}
		else{
			return (Mathf.RoundToInt(MakePerlinNoise(i, j, mapSeed)) + minHeight);
		}
	}

	private MapBlock CreateBlock(Vector3 worldPos, Vector3Int tablePos, Transform mapTransform, MapBlock mapBlockPrefab){
		MapBlock newBlock = Instantiate<MapBlock>(mapBlockPrefab);
		Transform mbTransform = newBlock.GetComponent<Transform>();
		mbTransform.SetParent(mapTransform);
		mbTransform.localRotation = Quaternion.identity;
		mbTransform.localPosition = worldPos;

		mapTable[tablePos.x, tablePos.y, tablePos.z] = newBlock;
		SetNeighbours(newBlock, tablePos);
		return newBlock;
		//mb.GetComponent<Renderer>().material.color = Random.ColorHSV(0, 1, 1, 1, 1, 1);
	}

	public void SetNeighbours(MapBlock block, Vector3Int tablePos){
		mapTable[tablePos.x, tablePos.y, tablePos.z] = block;
		MapBlock up = GetBlockAt(mapTable, tablePos.x, tablePos.y, tablePos.z+1);
		MapBlock down = GetBlockAt(mapTable, tablePos.x, tablePos.y, tablePos.z-1);
		MapBlock front = GetBlockAt(mapTable, tablePos.x, tablePos.y-1, tablePos.z);
		MapBlock back = GetBlockAt(mapTable, tablePos.x, tablePos.y+1, tablePos.z);
		MapBlock left = GetBlockAt(mapTable, tablePos.x-1, tablePos.y, tablePos.z);
		MapBlock right = GetBlockAt(mapTable, tablePos.x+1, tablePos.y, tablePos.z);
		block.neighbours = new MapBlock.Neighbours(block, up, down, front, back, left, right);
		block.CalculateVisibility();
	}

	private static MapBlock GetBlockAt(MapBlock[,,] mapTable, int i, int j, int k){
		if((i < 0) || (mapTable.GetLength(0) <= i)){
			return null;
		}

		if((j < 0) || (mapTable.GetLength(1) <= j)){
			return null;
		}

		if((k < 0) || (mapTable.GetLength(2) <= k)){
			return null;
		}

		return mapTable[i,j,k];
	}
}
