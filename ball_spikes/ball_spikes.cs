using Godot;
using System;

public partial class ball_spikes : PathFollow2D
{
	[Export]
	private double speed = 0.05;
	
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
		ProgressRatio += (float)delta * (float)this.speed;
	}
}
