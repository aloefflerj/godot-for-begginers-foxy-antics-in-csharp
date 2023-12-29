using Godot;
using System;

public partial class eagle : enemy_base
{
	private AnimatedSprite2D animatedSprite2D;
	private RayCast2D playerDetector;
	private Timer directionTimer;
	private readonly Vector2 flySpeed = new(35, 15);
	private Vector2 flyDirection = Vector2.Zero;
	private shooter shooter;

	public override void _Ready()
	{
		base._Ready();
		this.animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		this.playerDetector = GetNode<RayCast2D>("PlayerDetector");
		this.directionTimer = GetNode<Timer>("DirectionTimer");
		this.shooter = GetNode<shooter>("Shooter");
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		Velocity = this.flyDirection;
		MoveAndSlide();
		this.Shoot();
	}

	public void Shoot()
	{
		if (this.playerDetector.IsColliding())
			this.shooter.Shoot(
				GlobalPosition.DirectionTo(this.playerRef.GlobalPosition)
			);
	}

	public void SetAndFlip()
	{
		int xDirection = Mathf.Sign(this.playerRef.GlobalPosition.X - GlobalPosition.X);
		if (xDirection > 0)
		{
			this.animatedSprite2D.FlipH = true;
		}
		else
		{
			this.animatedSprite2D.FlipH = false;
		}

		this.flyDirection = new Vector2(xDirection, 1) * this.flySpeed;
	}

	public void FlyToPlayer()
	{
		SetAndFlip();
		this.directionTimer.Start();
	}

	public override void OnVisibleOnScreenNotifier2dScreenEntered()
	{
		this.animatedSprite2D.Play("fly");
		FlyToPlayer();
	}

	public void OnDirectionTimerTimeout()
	{
		FlyToPlayer();
	}
}
