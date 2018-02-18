using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynchronizedTransform : SynchronizedElement {

    [HideInInspector]
    public Transform selfTransform;

    protected override void Awake() {
        base.Awake();
        selfTransform = GetComponent<Transform>();
    }

	public override ServerData SynchronizeFromServer(){
        return new TransformData(synchronizedObject.id, new SerializableTransform(selfTransform));
    }

	public override ClientData SynchronizeFromClient(){
        return null;
    }
}
