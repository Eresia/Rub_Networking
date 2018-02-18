using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class SynchronizedCharacter : SynchronizedElement {

    [HideInInspector]
    public Character character;

    protected override void Awake() {
        base.Awake();
        character = GetComponent<Character>();
    }

    public override void Init(int owner, bool isServer){
        base.Init(owner, isServer);
        if(!isServer && synchronizedObject.IsOwner()){
            GameManager.instance.serverCamera.SetActive(false);
            character.selfCamera.gameObject.SetActive(true);
        }
    }

	public override ServerData SynchronizeFromServer(){
        return null;
    }

	public override ClientData SynchronizeFromClient(){
        return null;
    }

    public static void CreateCharacter(int id, Character characterPrefab, World world){
		Character newCharacter = Instantiate<Character>(characterPrefab);
        Vector3 characterPosition = world.GetRandomSpawnPosition();
        characterPosition.y += newCharacter.selfTranform.localScale.y;
        newCharacter.selfTranform.position = characterPosition;
	}
}
