using Godot;
using System;

public partial class frog : enemy_base
{
	private AnimatedSprite2D animatedSprite2D;
	private Timer jumpTimer;
	private readonly Vector2 jumpVelocity = new(100, -150);
	private readonly Vector2 jumpWaitRange = new((float)2.5, (float)5.0);
	private bool jump = false;
	private bool seenPlayer = false;

	public override void _Ready()
	{
		base._Ready();
		this.animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		this.jumpTimer = GetNode<Timer>("JumpTimer");

		this.StartTimer();
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		Vector2 velocity = Velocity;

		if (!IsOnFloor())
		{
			velocity.Y += (float)this.gravity * (float)delta;
			Velocity = velocity;
		}
		else
		{
			velocity.X = 0;
			Velocity = velocity;

			this.animatedSprite2D.Play("idle");
		}

		this.ApplyJump();
		MoveAndSlide();
		this.FlipMe();
	}

	public void ApplyJump()
	{
		if (!IsOnFloor())
			return;

		if (!this.seenPlayer || !this.jump)
			return;

		Velocity = this.jumpVelocity;

		if (!this.animatedSprite2D.FlipH)
			Velocity = new(Velocity.X * -1, Velocity.Y);

		this.jump = false;
		this.animatedSprite2D.Play("jump");
		this.StartTimer();
	}

	public void FlipMe()
	{
		if (
			this.playerRef.GlobalPosition.X > GlobalPosition.X &&
			!this.animatedSprite2D.FlipH
		)
		{
			this.animatedSprite2D.FlipH = true;
		}
		else if (
			this.playerRef.GlobalPosition.X < GlobalPosition.X &&
			this.animatedSprite2D.FlipH
		)
		{
			this.animatedSprite2D.FlipH = false;
		}
	}

	public void StartTimer()
	{
		this.jumpTimer.WaitTime = GD.RandRange(this.jumpWaitRange.X, this.jumpWaitRange.Y);
		this.jumpTimer.Start();
	}

	public void OnJumpTimerTimeout()
	{
		this.jump = true;
	}

	public override void OnVisibleOnScreenNotifier2dScreenEntered()
	{
		this.seenPlayer = true;
	}
}
