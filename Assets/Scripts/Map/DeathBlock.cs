using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBlock : MonoBehaviour {

	[SerializeField]
	private int playerLayer;

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.layer == playerLayer){
			Transform otherTransform = other.GetComponent<Transform>();
			Vector3 characterPosition = GameManager.instance.map.GetRandomSpawnPosition();
			characterPosition.y += otherTransform.localScale.y;
			otherTransform.position = characterPosition;
		}
	}
}
