using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterClientData : SynchronizedElementClientData<SynchronizedCharacter> {

	private float horizontalAxis;

	private float verticalAxis;

	private bool jump;

	private SerializableTransform playerTransform;
	private SerializableVector3 cameraRotation;

	public CharacterClientData(int id, float horizontalAxis, float verticalAxis, bool jump, Transform playerTransform, Quaternion cameraRotation) : base(id){
		this.horizontalAxis = horizontalAxis;
		this.verticalAxis = verticalAxis;
		this.jump = jump;
		this.playerTransform = new SerializableTransform(playerTransform);
		this.cameraRotation = new SerializableVector3(cameraRotation);
	}

	protected override bool Validate(){
		return true;
	}

	protected override bool Execute(){
		return true;
	}

	public override void ExecuteOnMainThread(){
		SynchronizedCharacter element = GetSynchronizedElement();

		element.character.Move(horizontalAxis, verticalAxis);
		if(jump){
			element.character.Jump();
		}

		Vector3 newPosition = playerTransform.position.ToVector3();

		if(!element.synchronizedTransform.ExceedError(element.character.selfTranform.position, newPosition)){
			element.character.selfTranform.position = newPosition;
		}

		element.character.selfTranform.rotation = playerTransform.rotation.ToQuaternion();
		element.character.head.rotation = cameraRotation.ToQuaternion();
		
	}
}
