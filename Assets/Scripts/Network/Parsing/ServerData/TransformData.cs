using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TransformData : SynchronizedElementServerData<SynchronizedTransform> {

	private SerializableTransform transform;

	public TransformData(int id, Transform transform) : base(id){
		this.transform = new SerializableTransform(transform);
	}

	protected override bool Validate(){
		return true;
	}

	protected override bool Execute(){
		return true;
	}

	public override void ExecuteOnMainThread(){
		SynchronizedTransform element = GetSynchronizedElement();

		Vector3 newPosition = transform.position.ToVector3();

		bool isOwner = (clientInformations.client.clientId == element.owner);

		bool needToSynch = element.NeedResynch(isOwner, newPosition);

		if(needToSynch){
			element.selfTransform.position = newPosition;
			if(element.synchronizeRotation){
				element.selfTransform.rotation = transform.rotation.ToQuaternion();
			}
		}
		
	}
}
