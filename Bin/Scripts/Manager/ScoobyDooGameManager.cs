using Com.IsartDigital.OBG;
using Com.IsartDigital.OBG.Entity.Aliments;
using Com.IsartDigital.OBG.Entity.Player;
using Godot;
using System;
using System.Collections.Generic;

// Author : Romain Chevalier

namespace Com.IsartDigital.OBG.Manager
{
	public partial class ScoobyDooGameManager : Node2D
	{
		[Export] public ScoobyPlayer player;
		[Export] public ObjectSpawner spawner;

		#region Food Variables
		public List<FoodType> foodOrder = new List<FoodType>
		{
			FoodType.Bread,
			FoodType.Cheese,
			FoodType.Ham,
			FoodType.Salad,
			FoodType.Bread
		};
		private int currentFoodIndex = 0;
		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <returns>the next food in the list</returns>
		public FoodType GetNextFood()
		{
			FoodType lNextFood = foodOrder[currentFoodIndex];
			return lNextFood;
		}

		// Call this when the player touches a food to check if it's the right one
		/// <summary>
		/// when player touches a food object it search wether or not the food is the right one, next in the list
		/// </summary>
		/// <param name="pCollectedFood"></param>
		/// <returns>a boolean on wether or not it's the right food next in the list</returns>
		public bool TryAddFood(FoodType pCollectedFood)
		{
			FoodType lRequiredFood = GetNextFood();
			if (pCollectedFood == lRequiredFood)
			{
				// if correct food move to new one
				currentFoodIndex++;
				if (currentFoodIndex >= foodOrder.Count)
					currentFoodIndex = 0;
				return true;
			}
			return false;
		}
	}
}
