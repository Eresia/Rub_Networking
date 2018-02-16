using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class World : MonoBehaviour {

	public MapBlock baseMapPrefab;
	public MapBlock[] blockPrefabs;

	[HideInInspector]
	public Transform selfTransform;

	void Awake () {
		selfTransform = GetComponent<Transform>();
	}

	public void GenerateMap(int width, int length, float floatScale, float biomeSeed){
		float blockSize = baseMapPrefab.GetComponent<Transform>().localScale.x/* + 0.05f*/;
		int maxHeight = Mathf.RoundToInt(floatScale) + 1;
		MapBlock[,,] mapTable = new MapBlock[width,length,maxHeight];
		// for(int i = 0; i < mapTable.Length; i++){
		// 	mapTable[i] = new MapBlock[length][];
		// 	for(int j = 0; j < mapTable[i].Length; j++){
		// 		mapTable[i][j] = new MapBlock[Mathf.RoundToInt(floatScale) + 1];
		// 	}
		// }

		for(int i = 0; i < width; i++){
			for(int j = 0; j < length; j++){
				int iPos = i + (-width / 2);
				int jPos = j + (-length / 2);

				int biomeNoise = (int) (MakePerlinNoise(i*biomeSeed, j*biomeSeed, blockPrefabs.Length + biomeSeed));
				
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
						int flatNoise =  Mathf.RoundToInt (MakePerlinNoise(i, j, floatScale));
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
