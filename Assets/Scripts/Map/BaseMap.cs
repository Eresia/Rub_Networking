using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMap : MonoBehaviour {
	
	[HideInInspector]
	public Transform selfTransform;

	void Awake()
	{
		selfTransform = GetComponent<Transform>();
	}
	
	void Update () {
		
	}
}
