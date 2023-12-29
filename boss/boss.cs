using Godot;
using System;

public partial class boss : Node2D
{
	const String TriggerCondition = "parameters/conditions/onTrigger";
	const String HitCondition = "parameters/conditions/onHit";

	private AnimationTree animationTree;
	private Node2D visual;
	private Area2D hitBox;

	[Export]
	private int lives = 2;
	[Export]
	private int points = 5;

	private bool invincible = false;
	private bool hasTriggered = false;
	

	public override void _Ready()
	{
		this.animationTree = GetNode<AnimationTree>("AnimationTree");
		this.visual = GetNode<Node2D>("Visual");
		this.hitBox = GetNode<Area2D>("Visual/HitBox");
	}

	public override void _Process(double delta)
	{
	}

	private void TweenHit()
	{
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(this.visual, "position", Vector2.Zero, 1.0);
	}

	private void ReduceLives()
	{
		this.lives -= 1;
		if (this.lives <= 0)
		{
			GetNode<signal_manager>("/root/SignalManager").EmitOnBossKilledSignal(this.points);
			SetProcess(false);
			QueueFree();
		}
	}

	private void SetInvincible(bool invincible)
	{
		this.invincible = invincible;
		this.animationTree.Set(HitCondition, invincible);
	}

	private void TakeDamage()
	{
		if (!this.hasTriggered)
			return;

		if (this.invincible)
			return;

		this.SetInvincible(true);
		this.TweenHit();
		this.ReduceLives();
	}

	public void OnTriggerAreaEntered(Area2D area)
	{
		if (!(bool)this.animationTree.Get(TriggerCondition))
		{
			this.animationTree.Set(TriggerCondition, true);
			this.hasTriggered = true;
			this.hitBox.CollisionLayer = 4;
		}
	}

	public void OnHitBoxAreaEntered(Area2D area)
	{
		this.TakeDamage();
	}
}
