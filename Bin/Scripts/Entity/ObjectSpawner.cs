using Com.IsartDigital.OBG.Tools;
using Godot;
using System;

// Author : Romain Chevalier

namespace Com.IsartDigital.OBG
{
	public partial class ObjectSpawner : Sprite2D
	{
		[Export] private PackedScene objectToSpawn;
		[Export] private float timeBetweenSpawn = 1f;
		private float timer;
		private Vector2 startPos;
		private Vector2 endPos;

		public override void _Ready()
		{
			base._Ready();
			timer = timeBetweenSpawn;
			Vector2 lTextSize = Texture.GetSize();
			startPos = GlobalPosition - (lTextSize * Scale / 2);
			endPos = GlobalPosition + (lTextSize * Scale / 2);
		}
		public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;
			base._Process(pDelta);
			timer -= lDelta;
			if (timer <= 0)
			{
				SpawnObject();
				timer = timeBetweenSpawn;
			}
		}
		private void SpawnObject()
		{
			Node2D lObject = objectToSpawn.Instantiate() as Node2D;
			Main.GetInstance().gameContainer.AddChild(lObject);

			float lPosX = Utils.rdG.RandfRange(startPos.X, endPos.X);
			float lPosY = Utils.rdG.RandfRange(startPos.Y, endPos.Y);
			lObject.GlobalPosition = new Vector2(lPosX, lPosY);


			if (lObject is Rock lRock)
			{
				lRock.canFall = true;
			}
		}
	}
}
