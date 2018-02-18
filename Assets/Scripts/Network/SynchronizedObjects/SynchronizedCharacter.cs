using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character), typeof(SynchronizedTransform))]
public class SynchronizedCharacter : SynchronizedElement {

    public int ownPlayerLayer;

    public Character character {get ; private set;}

    public SynchronizedTransform synchronizedTransform {get ; private set;}

    protected override void Awake() {
        base.Awake();
        character = GetComponent<Character>();
        synchronizedTransform = GetComponent<SynchronizedTransform>();
    }

    public override void Init(int owner, bool isServer){
        base.Init(owner, isServer);
        if(!isServer){
            if(synchronizedObject.IsOwner()){
                GameManager.instance.serverCamera.SetActive(false);
                character.selfCamera.gameObject.SetActive(true);
                character.gameObject.layer = ownPlayerLayer;
                character.head.gameObject.layer = ownPlayerLayer;
                for(int i = 0; i < character.head.childCount; i++){
                    character.head.GetChild(i).gameObject.layer = ownPlayerLayer;
                }
            }
            else{
                character.selfRigidbody.useGravity = false;
            }
        }
    }

	public override ServerData SynchronizeFromServer(){
        return new CharacterServerData(synchronizedObject.id, character.selfTranform.rotation, character.head.rotation);
    }

	public override ClientData SynchronizeFromClient(){
        if(synchronizedObject.IsOwner()){
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            bool jump = Input.GetButton ("Jump");

            character.Move(horizontal, vertical);
            if(jump){
                character.Jump();
            }

            character.Rotate(Input.GetAxis ("Mouse X"), Input.GetAxis ("Mouse Y"));
            return new CharacterClientData(synchronizedObject.id, horizontal, vertical, jump, character.selfTranform, character.head.rotation);
        }
        else{
            return null;
        }
    }

    public static void CreateCharacter(int owner, SynchronizedCharacter characterPrefab, World world){
		SynchronizedCharacter newCharacter = Instantiate<SynchronizedCharacter>(characterPrefab);
        newCharacter.synchronizedObject.SetOwner(owner);
        Vector3 characterPosition = world.GetRandomSpawnPosition();
        characterPosition.y += newCharacter.character.selfTranform.localScale.y;
        newCharacter.character.selfTranform.position = characterPosition;
	}
}
