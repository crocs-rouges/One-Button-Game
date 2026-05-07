using Godot;

// Author : Romain Chevalier

namespace Com.IsartDigital.OBG.Entity.Aliments
{
	public enum FoodType
	{
		Bread, Ham, Salad, Cheese,
	}
	public partial class Food : Node2D
	{
		[Export] public FoodType type;
		[Export] public bool canFall = true;
		[Export] public float fallSpeed = 700;

		[Export] private const float FOOD_DIVIDE_SIZE = 1.2f;

		public override void _Ready()
		{
			base._Ready();
		}
		public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;
			base._Process(pDelta);
			if (canFall)
			{
				GlobalPosition += Vector2.Down * fallSpeed * lDelta;
				if (GlobalPosition.Y > 2160 || GlobalPosition.Y < -2160)
					QueueFree();
			}
		}
		public void Capture()
		{
			canFall = false;
			Scale /= FOOD_DIVIDE_SIZE;
			Area2D lArea = GetChild<Area2D>(0);
			lArea.SetDeferred(Area2D.PropertyName.Monitorable, false);
			lArea.SetDeferred(Area2D.PropertyName.Monitoring, false);
		}
		public void Explode()
		{
			QueueFree();
		}
	}
}
