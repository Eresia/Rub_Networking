﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ServerInformations{
	public Server server;
	public World world;
}

public struct ClientInformations{
	public Client client;
	public int clientId;
	public World world;
}