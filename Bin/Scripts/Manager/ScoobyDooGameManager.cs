using Com.IsartDigital.OBG;
using Com.IsartDigital.OBG.Entity.Player;
using Godot;
using System;

// Author : Romain Chevalier

namespace Com.IsartDigital.OBG.Manager
{
	public partial class ScoobyDooGameManager : Node2D
	{
		[Export] public ScoobyPlayer player;
		[Export] public ObjectSpawner spawner;
	}
}
