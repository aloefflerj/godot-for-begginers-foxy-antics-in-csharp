using Godot;
using System;

public partial class enemy_explosion : AnimatedSprite2D
{
	public void OnAnimationFinished()
	{
		QueueFree();
	}
}
