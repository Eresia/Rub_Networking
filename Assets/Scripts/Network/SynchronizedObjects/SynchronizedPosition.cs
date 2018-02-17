using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynchronizedPosition : SynchronizedElement {

    private Transform selfTransform;

    protected override void Awake() {
        base.Awake();
        selfTransform = GetComponent<Transform>();
    }

	public override ServerData SynchronizeFromServer(){
        // return new PositionData(synchronizedObject.id);
    }

	public override ClientData SynchronizeFromClient(){
        return null;
    }
}
