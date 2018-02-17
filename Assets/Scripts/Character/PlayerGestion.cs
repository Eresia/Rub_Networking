using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGestion : MonoBehaviour {

	public Character characterPrefab;

	[HideInInspector]
	public Dictionary<int, Character> characters;

	private void Awake() {
		characters = new Dictionary<int, Character>();
	}

	public Character CreateCharacter(int id, World world){
		Character newCharacter = null;
		if(!characters.ContainsKey(id)){
			newCharacter = Instantiate<Character>(characterPrefab);
			Vector3 characterPosition =  world.GetRandomSpawnPosition();
			characterPosition.y += newCharacter.selfTranform.localScale.y;
			newCharacter.selfTranform.position = characterPosition;
			characters.Add(id, newCharacter);
		}
		return newCharacter;
	}

	public void RemoveCharacter(int id){
		if(characters.ContainsKey(id)){
			Character character = characters[id];
			characters.Remove(id);
			Destroy(character.gameObject);
		}
	}
}
