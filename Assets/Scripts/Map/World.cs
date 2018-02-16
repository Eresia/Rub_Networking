using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class World : MonoBehaviour {

	[System.Serializable]
	public struct WorldGeneration{
		public int mapWidth;
		public int mapLength;
		public float mapSeed;
		public float biomeSeed;

		public WorldGeneration(int mapWidth, int mapLength, float mapSeed, float biomeSeed){
			this.mapWidth = mapWidth;
			this.mapLength = mapLength;
			this.mapSeed = mapSeed;
			this.biomeSeed = biomeSeed;
		}
	}

	public MapBlock baseMapPrefab;
	public MapBlock[] blockPrefabs;

	[HideInInspector]
	public Transform selfTransform;

	[HideInInspector]
	public WorldGeneration worldGeneration;

	void Awake () {
		selfTransform = GetComponent<Transform>();
	}

	public void GenerateMap(int width, int length, float floatScale, float biomeSeed){
		GenerateMap(new WorldGeneration(width, length, floatScale, biomeSeed));
	}

	public void GenerateMap(WorldGeneration worldGeneration){
		this.worldGeneration = worldGeneration;
		float blockSize = baseMapPrefab.GetComponent<Transform>().localScale.x/* + 0.05f*/;
		int maxHeight = Mathf.RoundToInt(worldGeneration.mapSeed) + 1;
		MapBlock[,,] mapTable = new MapBlock[worldGeneration.mapWidth, worldGeneration.mapLength, maxHeight];
		// for(int i = 0; i < mapTable.Length; i++){
		// 	mapTable[i] = new MapBlock[length][];
		// 	for(int j = 0; j < mapTable[i].Length; j++){
		// 		mapTable[i][j] = new MapBlock[Mathf.RoundToInt(floatScale) + 1];
		// 	}
		// }

		for(int i = 0; i < worldGeneration.mapWidth; i++){
			for(int j = 0; j < worldGeneration.mapLength; j++){
				int iPos = i + (-worldGeneration.mapWidth / 2);
				int jPos = j + (-worldGeneration.mapLength / 2);

				int biomeNoise = (int) (MakePerlinNoise(i * worldGeneration.biomeSeed, j * worldGeneration.biomeSeed, blockPrefabs.Length + worldGeneration.biomeSeed));
				
				MapBlock prefab;
				if(((iPos != 0) || (jPos != 0))){
					prefab = blockPrefabs[biomeNoise];
				}
				else{
					prefab = baseMapPrefab;
				}

				int blockHeight;
				switch(biomeNoise){
					case 0:
						blockHeight = 1;
						break;

					// case 2:
					// 	blockHeight = 5;
					// 	break;

					default:
						int flatNoise =  Mathf.RoundToInt (MakePerlinNoise(i, j, worldGeneration.mapSeed));
						blockHeight = Mathf.Max(flatNoise, 2);
						break;
				}

				for(int k = blockHeight; k >= 1; k--){
					CreateBlock(mapTable, new Vector3(iPos, k, jPos), i, j, k, blockSize, selfTransform, prefab);
				}

				CreateBlock(mapTable, new Vector3(iPos, 0, jPos), i, j, 0, blockSize, selfTransform, blockPrefabs[2]);
			}
		}
	}

	private static float MakePerlinNoise(float i, float j, float flatscale){
		return Mathf.PerlinNoise (((float) i) / flatscale, ((float) j) / flatscale) * flatscale;
	}

	private static MapBlock CreateBlock(MapBlock[,,] mapTable, Vector3 worldPos, int i, int j, int k, float blockSize, Transform mapTransform, MapBlock mapBlockPrefab){
		MapBlock newBlock = Instantiate<MapBlock>(mapBlockPrefab);
		Transform mbTransform = newBlock.GetComponent<Transform>();
		mbTransform.SetParent(mapTransform);
		mbTransform.localRotation = Quaternion.identity;
		mbTransform.localPosition = new Vector3(worldPos.x * blockSize, worldPos.y, worldPos.z * blockSize);
		newBlock.selfRenderer = newBlock.GetComponent<Renderer>();

		mapTable[i, j, k] = newBlock;
		SetNeighbours(mapTable, newBlock, i, j, k);
		return newBlock;
		//mb.GetComponent<Renderer>().material.color = Random.ColorHSV(0, 1, 1, 1, 1, 1);
	}

	public static void SetNeighbours(MapBlock[,,] mapTable, MapBlock block, int i, int j, int k){
		mapTable[i,j,k] = block;
		MapBlock up = GetBlockAt(mapTable, i, j, k+1);
		MapBlock down = GetBlockAt(mapTable, i, j, k-1);
		MapBlock front = GetBlockAt(mapTable, i, j-1, k);
		MapBlock back = GetBlockAt(mapTable, i, j+1, k);
		MapBlock left = GetBlockAt(mapTable, i-1, j, k);
		MapBlock right = GetBlockAt(mapTable, i+1, j, k);
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
