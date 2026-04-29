using Com.IsartDigital.OBG.Entity;
using Com.IsartDigital.OBG.Entity.Food;
using Com.IsartDigital.OBG.Tools;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

// Author : Romain Chevalier

namespace Com.IsartDigital.OBG
{
	public partial class ObjectSpawner : Sprite2D
	{
		[Export] private PackedScene[] objectToSpawn;
		[Export] private float[] percent;
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
				SpawnRandomObject();
				GD.Print("spawn object");
				timer = timeBetweenSpawn;
			}
		}
		private void SpawnFood(PackedScene pFoodscn)
		{
			Food lObject = pFoodscn.Instantiate() as Food;
			Main.GetInstance().gameContainer.CallDeferred(Node.MethodName.AddChild, lObject);
			//give random position
			float lPosX = Utils.rdG.RandfRange(startPos.X, endPos.X);
			float lPosY = Utils.rdG.RandfRange(startPos.Y, endPos.Y);
			lObject.GlobalPosition = new Vector2(lPosX, lPosY);
		}
		private void SpawnRandomObject()
		{
			// verification to see if list are the same size
			if (objectToSpawn == null ||
			percent == null ||
			objectToSpawn.Count() == 0 ||
			objectToSpawn.Count() != percent.Count())
			{
				GD.Print("something is null");
				return;
			}
			//set chance variables
			float lTotalChances = 100f;
			float lRandomValue = Utils.rdG.RandfRange(0f, lTotalChances);
			float lCumulativeChance = 0f;
			//loop for choosing an item
			for (int i = 0; i < objectToSpawn.Count(); i++)
			{
				lCumulativeChance += percent[i];
				if (lRandomValue <= lCumulativeChance)
				{
					//spawn the item and break loop
					SpawnFood(objectToSpawn[i]);
					break;
				}
			}
		}
	}
}
