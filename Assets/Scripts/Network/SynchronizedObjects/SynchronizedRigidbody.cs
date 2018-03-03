using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(SynchronizedTransform))]
public class SynchronizedRigidbody : SynchronizedElement {

    public Rigidbody selfRigidbody {get ; private set;}

    public SynchronizedTransform synchronizedTransform {get ; private set;}

    protected override void Awake() {
        base.Awake();
        selfRigidbody = GetComponent<Rigidbody>();
        synchronizedTransform = GetComponent<SynchronizedTransform>();
    }

	public override ServerData SynchronizeFromServer(){
        return new RigidbodyData(synchronizedObject.id, selfRigidbody);
    }

	public override ClientData SynchronizeFromClient(){
        return null;
    }
}
