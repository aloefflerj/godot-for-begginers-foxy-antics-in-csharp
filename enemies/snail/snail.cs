using Godot;
using System;

public partial class snail : enemy_base
{

	private RayCast2D floorDetection;
	private AnimatedSprite2D animatedSprite2D;

	public override void _Ready()
	{
		this.floorDetection = GetNode<RayCast2D>("FloorDetection");
		this.animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		Vector2 velocity = Velocity;

		if (!IsOnFloor())
		{
			velocity.Y += (float)this.gravity * (float)delta;
		}
		else
		{
			velocity.X = (float)this.speed * (float)this.facing;
		}

		Velocity = velocity;
		MoveAndSlide();

		if (IsOnFloor())
			if (IsOnWall() || !this.floorDetection.IsColliding())
				FlipMe();
	}

	private void FlipMe()
	{
		this.animatedSprite2D.FlipH = !this.animatedSprite2D.FlipH;

		this.floorDetection.Position = new(
			this.floorDetection.Position.X * -1,
			this.floorDetection.Position.Y
		);

		if (this.facing == Facing.Left)
			this.facing = Facing.Right;
		else
			this.facing = Facing.Left;
	}
}
