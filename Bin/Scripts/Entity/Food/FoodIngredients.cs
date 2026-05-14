using Godot;
using System;

// Author : Romain Chevalier

namespace Com.IsartDigital.OBG.Entity.Aliments
{
	public partial class FoodIngredients : Control
	{
		[Export] private Food[] listFoodItem;
		[Export] private Label foodCountLbl;
		private const string FOOD_COUNT = "/5";

		public override void _Ready()
		{
			base._Ready();
			ChangeElement(0);
		}
		public void ChangeElement(int pNewIndex)
		{
			for (int lIndex = 0; lIndex < listFoodItem.Length; lIndex++)
			{
				Food lCurrentFood = listFoodItem[lIndex];
				if (lIndex < pNewIndex) lCurrentFood.Collected();
				else if (lIndex == pNewIndex) lCurrentFood.Next();
				else lCurrentFood.Normal();
			}
			foodCountLbl.Text = pNewIndex + FOOD_COUNT;
		}
	}
}