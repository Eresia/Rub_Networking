using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SynchronizedObject))]
public abstract class SynchronizedElement : MonoBehaviour{

    protected int owner;

	protected SynchronizedObject synchronizedObject;

	protected virtual void Awake() {
		synchronizedObject = GetComponent<SynchronizedObject>();
	}

	public virtual void Init(int owner, bool isServer){
		this.owner = owner;
	}

	public abstract ServerData SynchronizeFromServer();

	public abstract ClientData SynchronizeFromClient();
}
