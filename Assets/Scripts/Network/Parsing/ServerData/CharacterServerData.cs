using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterServerData : ServerData {

	protected override bool Validate(){
		return false;
	}

	protected override bool Execute(){
		return false;
	}

	public override void ExecuteOnMainThread(){
		
	}
}
