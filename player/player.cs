using Godot;
using System;

public partial class player : CharacterBody2D
{
	private Sprite2D sprite2D;
	private AnimationPlayer animationPlayer;
	private AnimationPlayer animationPlayerInvincible;
	private Timer invincibleTimer;
	private Timer hurtTimer;
	private Label debugLevel;
	private AudioStreamPlayer2D soundPlayer;
	private Area2D hitBox;

	private signal_manager signalManager;
	private shooter shooter;

	private PlayerState state = PlayerState.Idle;
	private bool invincible = false;
	private int lives = 5;

	private const double Gravity = 1000.0;
	private const double FallenOffDistance = 100.0;
	private const double RunSpeed = 320.0;
	private const double MaxFall = 400.0;
	private const double JumpVelocity = -400.0;
	private readonly Vector2 HurtJumpVelocity = new(0, (float)-150.0);


	public override void _Ready()
	{
		this.sprite2D = GetNode<Sprite2D>("Sprite2D");

		this.animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		this.animationPlayerInvincible = GetNode<AnimationPlayer>("AnimationPlayerInvincible");

		this.invincibleTimer = GetNode<Timer>("InvincibleTimer");
		this.hurtTimer = GetNode<Timer>("HurtTimer");

		this.debugLevel = GetNode<Label>("DebugLevel");
		this.soundPlayer = GetNode<AudioStreamPlayer2D>("SoundPlayer");
		this.hitBox = GetNode<Area2D>("HitBox");

		this.signalManager = GetNode<signal_manager>("/root/SignalManager");
		this.shooter = GetNode<shooter>("Shooter");

		this.signalManager.EmitOnPlayerHitSignal(lives);
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		this.FallenOff();

		if (IsOnFloor() == false)
		{
			velocity.Y += (float)Gravity * (float)delta;
			Velocity = velocity;
		}

		this.GetInput();
		MoveAndSlide();
		this.CalculateStates();
		this.UpdateDebugLabel();

		if (Input.IsActionJustPressed("shoot"))
			this.Shoot();
	}

	public void UpdateDebugLabel()
	{
		this.debugLevel.Text = string.Format(
			"floor: {0} inv:{1}\n{2} lives: {3}\n{4},{5}",
			IsOnFloor(),
			this.invincible,
			Enum.GetName(typeof(PlayerState), this.state),
			this.lives,
			Velocity.X.ToString("0.00"),
			Velocity.Y.ToString("0.00")
		);
	}

	public void FallenOff()
	{
		if (GlobalPosition.Y < FallenOffDistance)
			return;

		this.lives = 1;
		this.ReduceLives();
	}

	public void Shoot()
	{
		if (this.sprite2D.FlipH)
			this.shooter.Shoot(Vector2.Left);
		else
			this.shooter.Shoot(Vector2.Right);
	}

	public void GetInput()
	{
		if (this.state == PlayerState.Hurt)
			return;

		Vector2 velocity = Velocity;

		velocity.X = 0;
		if (Input.IsActionPressed("left"))
		{
			velocity.X = -(float)RunSpeed;
			this.sprite2D.FlipH = true;
		}
		else if (Input.IsActionPressed("right"))
		{
			velocity.X = (float)RunSpeed;
			this.sprite2D.FlipH = false;
		}

		if (Input.IsActionPressed("jump") && IsOnFloor())
		{
			velocity.Y = (float)JumpVelocity;
			sound_manager.PlayClip(this.soundPlayer, sound_manager.SoundJump);
		}

		velocity.Y = Math.Clamp(velocity.Y, (float)JumpVelocity, (float)MaxFall);

		Velocity = velocity;
	}

	private void CalculateStates()
	{
		if (this.state == PlayerState.Hurt)
			return;

		if (IsOnFloor())
		{

			if (Velocity.X == 0)
			{
				this.SetState(PlayerState.Idle);
			}
			else
			{
				this.SetState(PlayerState.Run);
			}
		}
		else
		{
			if (Velocity.Y > 0)
			{
				this.SetState(PlayerState.Fall);
			}
			else
			{
				this.SetState(PlayerState.Jump);
			}
		}
	}

	private void SetState(PlayerState newState)
	{
		if (newState == this.state)
			return;

		if (this.state == PlayerState.Fall)
			if (newState == PlayerState.Idle || newState == PlayerState.Run)
				sound_manager.PlayClip(this.soundPlayer, sound_manager.SoundLand);


		this.state = newState;

		switch (newState)
		{
			case PlayerState.Idle:
				this.animationPlayer.Play("idle");
				break;
			case PlayerState.Run:
				this.animationPlayer.Play("run");
				break;
			case PlayerState.Jump:
				this.animationPlayer.Play("jump");
				break;
			case PlayerState.Fall:
				this.animationPlayer.Play("fall");
				break;
			case PlayerState.Hurt:
				this.ApplyHurtJump();
				break;
		}
	}

	public void ApplyHurtJump()
	{
		this.animationPlayer.Play("hurt");
		Velocity = HurtJumpVelocity;
		this.hurtTimer.Start();
	}

	public void GoInvincible()
	{
		this.invincible = true;
		this.animationPlayerInvincible.Play("invincible");
		this.invincibleTimer.Start();
	}

	public bool ReduceLives()
	{
		this.lives -= 1;
		this.signalManager.EmitOnPlayerHitSignal(lives);
		if (this.lives <= 0)
		{
			this.signalManager.EmitOnGameOverSignal();
			SetPhysicsProcess(false);
			return false;
		}

		return true;
	}

	public void ApplyHit()
	{
		if (this.invincible)
			return;

		if (!this.ReduceLives())
			return;

		this.GoInvincible();
		this.SetState(PlayerState.Hurt);
		sound_manager.PlayClip(this.soundPlayer, sound_manager.SoundDamage);
	}

	public void RetakeDamage()
	{
		foreach (Area2D area in this.hitBox.GetOverlappingAreas())
		{
			if (area.IsInGroup("Dangers"))
			{
				this.ApplyHit();
				break;
			}
		}
	}

	public void OnHitBoxAreaEntered(Area2D area)
	{
		this.ApplyHit();
	}

	public void OnInvincibleTimerTimeout()
	{
		this.invincible = false;
		this.animationPlayerInvincible.Stop();
		this.RetakeDamage();
	}

	public void OnHurtTimerTimeout()
	{
		this.SetState(PlayerState.Idle);
	}
}
