using Godot;
using System;

// Author : Romain Chevalier

namespace Com.IsartDigital.OBG.Entity.Aliments
{
	public partial class FoodIngredients : Node2D
	{
		[Export] private Sprite2D[] listFoodItem;


		public override void _Ready()
		{
			base._Ready();
			HideElements();
		}
		public void ChangeElement(int pNewIndex)
		{
			HideElements();
			listFoodItem[pNewIndex].Visible = true;
		}
		private void HideElements()
		{
			foreach (Sprite2D lFoodText in listFoodItem)
				lFoodText.Visible = false;
		}
	}
}
