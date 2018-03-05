using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterOwnerServerData : SynchronizedElementServerData<SynchronizedCharacter> {

	private float pushProgression;

	public CharacterOwnerServerData(int id, float pushProgression) : base(id){
		this.pushProgression = pushProgression;
	}

	protected override bool Validate(){
		if((pushProgression < 0) || (pushProgression > 1)){
			return false;
		}

		if(clientInformations.client.clientId != GetSynchronizedElement().owner){
			return false;
		}

		return true;
	}

	protected override bool Execute(){
		return true;
	}

	public override void ExecuteOnMainThread(){
		GameManager.instance.pushSlider.value = pushProgression;
	}
}
