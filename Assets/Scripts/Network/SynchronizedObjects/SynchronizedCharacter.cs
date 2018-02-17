using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class SynchronizedCharacter : SynchronizedElement {

    private Character character;

    protected override void Awake() {
        base.Awake();
        character = GetComponent<Character>();
    }

	public override ServerData SynchronizeFromServer(){
        return null;
    }

	public override ClientData SynchronizeFromClient(){
        return null;
    }
}
