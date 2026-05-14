using Com.IsartDigital.OBG.Tools;
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
		private int previousIndex = 0;

		public override void _Ready()
		{
			base._Ready();
			foodCountLbl.PivotOffset = foodCountLbl.Size / 2f;
			ChangeElement(0);
		}
		public void ChangeElement(int pNewIndex)
		{
			PlayAnimation(pNewIndex);
			for (int lIndex = 0; lIndex < listFoodItem.Length; lIndex++)
			{
				Food lCurrentFood = listFoodItem[lIndex];
				if (lIndex < pNewIndex) lCurrentFood.Collected();
				else if (lIndex == pNewIndex) lCurrentFood.Next();
				else lCurrentFood.Normal();
			}
		}
		private void PlayAnimation(int pNewIndex)
		{
			// Main UI Pop effect
			Tween lTween = CreateTween();
			lTween.SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.Out);
			lTween.TweenProperty(this, Utils.TWEEN_SCALE, Vector2.One * 1.05f, 0.1f);
			lTween.Chain().TweenProperty(this, Utils.TWEEN_SCALE, Vector2.One, 0.15f);

			// choose text color
			Color lTargetColor = Colors.White;
			if (pNewIndex > previousIndex) lTargetColor = Colors.Green;
			else if (pNewIndex < previousIndex) lTargetColor = Colors.Red;
			//update label and index
			foodCountLbl.Text = pNewIndex + FOOD_COUNT;
			previousIndex = pNewIndex;
			// tween
			Tween lLabelTween = CreateTween();
			// Scale up and change color to the target color
			lLabelTween.SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.Out);
			lLabelTween.TweenProperty(foodCountLbl, Utils.TWEEN_SCALE, Vector2.One * 1.4f, 0.1f);
			lLabelTween.Parallel().TweenProperty(foodCountLbl, Utils.TWEEN_MODULATE, lTargetColor, 0.1f);
			// back to normal
			lLabelTween.Chain().TweenProperty(foodCountLbl, Utils.TWEEN_SCALE, Vector2.One, 0.2f);
			lLabelTween.Parallel().TweenProperty(foodCountLbl, Utils.TWEEN_MODULATE, Colors.White, 0.2f);

		}
	}
}