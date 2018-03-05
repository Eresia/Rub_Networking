using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynchronizedTransform : SynchronizedElement {

    public bool synchronizeRotation;

    public bool simulateOnClient;

    public float error;

    public Transform selfTransform {get ; private set;}

    protected override void Awake() {
        base.Awake();
        selfTransform = GetComponent<Transform>();
    }

	public override ServerData SynchronizeFromServer(){
        return new TransformData(synchronizedObject.id, selfTransform);
    }

	public override ServerData SynchronizeFromServerToOwner(){
		return null;
	}

	public override ClientData SynchronizeFromClient(){
        return null;
    }

    public bool ExceedError(Vector3 pos1, Vector3 pos2){
        Vector3 posError = pos1 - pos2;
        return (posError.sqrMagnitude > error);
    }
}
