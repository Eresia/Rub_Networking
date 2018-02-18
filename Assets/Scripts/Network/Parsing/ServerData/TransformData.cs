using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TransformData : ServerData {

	private int id;

	private SerializableTransform transform;

	public TransformData(int id, SerializableTransform transform){
		this.id = id;
		this.transform = transform;
	}

	protected override bool Validate(){
		SynchronizedObject obj = clientInformations.client.network.GetSynchronizedObject(id);
		if(obj == null){
			return false;
		}

		return obj.synchronizedElements.ContainsKey(typeof(SynchronizedTransform));
	}

	protected override bool Execute(){
		return true;
	}

	public override void ExecuteOnMainThread(){
		SynchronizedObject obj = clientInformations.client.network.GetSynchronizedObject(id);
		SynchronizedTransform element = (SynchronizedTransform) obj.synchronizedElements[typeof(SynchronizedTransform)];
		element.selfTransform.position = transform.position.ToVector3();
		element.selfTransform.rotation = transform.rotation.ToQuaternion();
	}
}
