using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMap : MonoBehaviour {

	public MapBlock blockPrefab;
	
	[HideInInspector]
	public Transform selfTransform;

	void Awake()
	{
		selfTransform = GetComponent<Transform>();
	}
	
	void Update () {
		
	}
}
