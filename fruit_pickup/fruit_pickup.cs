using Godot;
using System;
using System.Collections.Generic;

public partial class fruit_pickup : Area2D
{
	private AnimatedSprite2D animatedSprite2D;

	private readonly String[] Fruits = { "melon", "kiwi", "banana", "cherry" };
	private const double GravityValue = 160.0;
	private const double Jump = -80.0;
	private const int Points = 2;
	private double startY;
	private double speedY = Jump;
	private bool stopped;


	public override void _Ready()
	{
		this.startY = GlobalPosition.Y;
		this.animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		Random random = new();
		int randomIndex = random.Next(0, this.Fruits.Length);
		this.animatedSprite2D.Play(this.Fruits[randomIndex]);
	}

	public override void _Process(double delta)
	{
		if (stopped)
			return;

		Vector2 position = Position;
		position.Y += (float)this.speedY * (float)delta;
		Position = position;

		this.speedY += GravityValue * delta;

		if (GlobalPosition.Y > this.startY)
			this.stopped = true;
	}

	private void KillMe()
	{
		SetProcess(false);
		Hide();
		QueueFree();
	}

	public void OnLifeTimerTimeout()
	{
		this.KillMe();
	}

	public void OnAreaEntered(Area2D area)
	{
		GetNode<signal_manager>("/root/SignalManager").EmitOnPickupHitSignal(Points);
		KillMe();
	}
}
