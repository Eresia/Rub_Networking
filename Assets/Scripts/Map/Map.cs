using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

	public MapBlock baseMapPrefab;
	public MapBlock blockPrefab;
	public Material[] possibleMaterials;

	[HideInInspector]
	public Transform selfTransform;

	void Awake () {
		selfTransform = GetComponent<Transform>();
	}
}
