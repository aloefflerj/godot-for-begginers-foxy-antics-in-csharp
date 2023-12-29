using Godot;
using System;

public partial class bullet_base : Area2D
{
	private Vector2 direction = Vector2.Right;
	private double lifeSpan = 20.0;
	private double lifeTime = 0.0;
	
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
		CheckExpired(delta);
		Position += this.direction * (float)delta;
	}

	public void Setup(Vector2 direction, double lifeSpan, double speed)
	{
		this.direction = direction.Normalized() * (float)speed;
		this.lifeSpan = lifeSpan;
	}

	public void CheckExpired(double delta)
	{
		this.lifeTime += delta;
		if (this.lifeTime > this.lifeSpan)
			QueueFree();
	}

	public void OnAreaEntered(Area2D area)
	{
		QueueFree();
	}
}
