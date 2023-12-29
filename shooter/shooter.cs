using Godot;
using System;

public partial class shooter : Node2D
{
	private AudioStreamPlayer2D sound;
	private Timer shooterTimer;

	[Export]
	private double speed = 50.0;
	[Export]
	private double lifeSpan = 10.0;
	[Export]
	private BulletKey bulletKey;
	[Export]
	private double shootDelay = 0.7;

	private bool canShoot = true;

    public override void _Ready()
    {
        base._Ready();
		this.sound = GetNode<AudioStreamPlayer2D>("Sound");
		this.shooterTimer = GetNode<Timer>("ShooterTimer");
		this.shooterTimer.WaitTime = this.shootDelay;
    }

    public void Shoot(Vector2 direction)
	{
		if (!canShoot)
			return;

		canShoot = false;
		sound_manager.PlayClip(this.sound, sound_manager.SoundLaser);
		object_maker.CreateBullet(
			(float)this.speed,
			direction,
			GlobalPosition,
			(float)this.lifeSpan,
			this.bulletKey
		);
		this.shooterTimer.Start();
	}

	public void OnShooterTimerTimeout()
	{
		this.canShoot = true;
	}
}
