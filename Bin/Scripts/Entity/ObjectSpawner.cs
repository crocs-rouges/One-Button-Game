using Com.IsartDigital.OBG.Entity;
using Com.IsartDigital.OBG.Entity.Aliments;
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
		public bool isDown;
		[Export] private PackedScene[] objectToSpawn;
		[Export] private float[] percent;
		[Export] private float timeBetweenSpawn = 0.6f;
		private float timer;
		private Vector2 startPos;
		private Vector2 endPos;

		public override void _Ready()
		{
			base._Ready();
			// if (GlobalPosition.Y > 1080) fallDown = true;
			// else fallDown = false;
			timer = timeBetweenSpawn;
			Vector2 lTextSize = Texture.GetSize();
			Vector2 lScale = lTextSize * Scale / 2;
			startPos = -lScale;
			endPos = lScale;
		}
		public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;
			base._Process(pDelta);
			timer -= lDelta;
			if (timer <= 0)
			{
				SpawnRandomObject();
				timer = timeBetweenSpawn;
			}
		}
		private void SpawnFood(PackedScene pFoodscn)
		{
			Food lFood = pFoodscn.Instantiate() as Food;
			GetParent().CallDeferred(Node.MethodName.AddChild, lFood);
			//give random position
			float lPosX = Utils.rdG.RandfRange(startPos.X, endPos.X);
			lFood.GlobalPosition = new Vector2(lPosX, 0);
			if (isDown) GD.Print(lFood.GlobalPosition + "" + lFood.Scale);
			if (!isDown) lFood.fallSpeed *= -1f;
		}
		private void SpawnRandomObject()
		{
			// verification to see if list are the same size
			if (objectToSpawn == null ||
			percent == null ||
			objectToSpawn.Count() == 0 ||
			objectToSpawn.Count() != percent.Count()) return;
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
