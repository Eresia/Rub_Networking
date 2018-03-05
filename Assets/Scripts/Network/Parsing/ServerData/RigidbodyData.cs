using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RigidbodyData : SynchronizedElementServerData<SynchronizedRigidbody> {

	private SerializableVector3 velocity;

	public RigidbodyData(int id, Rigidbody rigidbody) : base(id){
		this.velocity = new SerializableVector3(rigidbody.velocity);
	}

	protected override bool Validate(){
		return true;
	}

	protected override bool Execute(){
		return true;
	}

	public override void ExecuteOnMainThread(){
		SynchronizedRigidbody element = GetSynchronizedElement();

		if(element.synchronizedTransform.needResync){
			element.selfRigidbody.velocity = velocity.ToVector3();
		}
	}
}
