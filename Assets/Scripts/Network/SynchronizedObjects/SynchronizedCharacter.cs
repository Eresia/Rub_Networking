using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character), typeof(SynchronizedTransform))]
public class SynchronizedCharacter : SynchronizedElement {

    public LayerMask ownPlayerLayer;

    [HideInInspector]
    public Character character;

    [HideInInspector]
    public SynchronizedTransform synchronizedTransform;

    protected override void Awake() {
        base.Awake();
        character = GetComponent<Character>();
        synchronizedTransform = GetComponent<SynchronizedTransform>();
    }

    public override void Init(int owner, bool isServer){
        base.Init(owner, isServer);
        if(!isServer && synchronizedObject.IsOwner()){
            if(synchronizedObject.IsOwner()){
                GameManager.instance.serverCamera.SetActive(false);
                character.selfCamera.gameObject.SetActive(true);
                character.gameObject.layer = ownPlayerLayer;
                character.head.gameObject.layer = ownPlayerLayer;
            }
            else{
                character.selfRigidbody.isKinematic = true;
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

    public static void CreateCharacter(int id, Character characterPrefab, World world){
		Character newCharacter = Instantiate<Character>(characterPrefab);
        Vector3 characterPosition = world.GetRandomSpawnPosition();
        characterPosition.y += newCharacter.selfTranform.localScale.y;
        newCharacter.selfTranform.position = characterPosition;
	}
}
