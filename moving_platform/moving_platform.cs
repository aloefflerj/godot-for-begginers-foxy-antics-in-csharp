using Godot;
using System;

public partial class moving_platform : AnimatableBody2D
{
	[Export]
	private Marker2D p1;
	[Export]
	private Marker2D p2;
	[Export]
	private double speed = 50.0;

	private double timeToMove = 0.0;
	private Tween tween;
	public override void _Ready()
	{
		this.SetTimeToMove();
		this.SetMoving();
	}

	public override void _ExitTree()
	{
		this.tween.Kill();
	}

	public void SetTimeToMove()
	{
		float delta = this.p1.GlobalPosition.DistanceTo(p2.GlobalPosition);
		this.timeToMove = delta/this.speed;
	}

	public void SetMoving()
	{
		this.tween = GetTree().CreateTween();
		this.tween.SetLoops(0);
		this.tween.TweenProperty(this, "global_position", p1.GlobalPosition, this.timeToMove);
		this.tween.TweenProperty(this, "global_position", p2.GlobalPosition, this.timeToMove);
	}
}
