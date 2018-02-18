using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterServerData : SynchronizedElementServerData<SynchronizedCharacter> {

	private SerializableVector3 playerRotation;
	private SerializableVector3 cameraRotation;

	public CharacterServerData(int id, Quaternion playerRotation, Quaternion cameraRotation) : base(id){
		this.playerRotation = new SerializableVector3(playerRotation);
		this.cameraRotation = new SerializableVector3(cameraRotation);
	}

	protected override bool Validate(){
		return true;
	}

	protected override bool Execute(){
		return true;
	}

	public override void ExecuteOnMainThread(){
		SynchronizedObject obj = GetSynchronizedObject();
		SynchronizedCharacter element = GetSynchronizedElement();

		if(obj.owner != clientInformations.client.clientId){
			element.character.selfTranform.rotation = playerRotation.ToQuaternion();
			element.character.head.rotation = cameraRotation.ToQuaternion();
		}
	}
}
