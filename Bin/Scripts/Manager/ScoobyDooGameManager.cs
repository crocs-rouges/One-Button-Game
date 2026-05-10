using Com.IsartDigital.OBG.Entity.Aliments;
using Com.IsartDigital.OBG.Entity.Player;
using Godot;
using System.Collections.Generic;

// Author : Romain Chevalier

namespace Com.IsartDigital.OBG.Manager
{
	public partial class ScoobyDooGameManager : Node2D
	{
		[Export] public ScoobyPlayer player;
		[Export] public ObjectSpawner spawner;
		[Export] public FoodIngredients foodIngredients;

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
		public override void _Ready()
		{
			base._Ready();
			foodIngredients.ChangeElement(currentFoodIndex);
		}
		/// <returns>the next food in the list</returns>
		public FoodType GetNextFood()
		{
			FoodType lNextFood = foodOrder[currentFoodIndex];
			return lNextFood;
		}
		public void RemoveFood()
		{
			currentFoodIndex--;
			if (player.foods.Count == 0)
			{
				currentFoodIndex = 0;
				foodIngredients.ChangeElement(currentFoodIndex);
				return;
			}
			if (currentFoodIndex < 0) currentFoodIndex = foodOrder.Count - 1;
			foodIngredients.ChangeElement(currentFoodIndex);
		}
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
				foodIngredients.ChangeElement(currentFoodIndex);
				return true;
			}
			return false;
		}
	}
}
