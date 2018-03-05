using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBlock : MonoBehaviour {

	[SerializeField]
	private int playerLayer;

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.layer == playerLayer){
			Character character = other.GetComponent<Character>();
			if(character != null){
				character.Kill();
			}
		}
	}
}
