using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMapAction : NetworkAction {

	public World world;
	public World.WorldGeneration worldGeneration;
	public Character character;

	public CreateMapAction(World world, World.WorldGeneration worldGeneration, Character character){
		this.world = world;
		this.worldGeneration = worldGeneration;
		this.character = character;
	}

	public void Execute(){
		world.GenerateMap(worldGeneration);
		character.gameObject.SetActive(true);
	}
}
