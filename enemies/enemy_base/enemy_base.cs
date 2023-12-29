using Godot;
using System;

public partial class enemy_base : CharacterBody2D
{
	const double OffScreenKillMe = 1000.0;

	[Export]
	protected static Facing defaultFacing = Facing.Left;
	[Export]
	protected int points = 1;
	[Export]
	protected double speed = 30.0;
	protected double gravity = 800.0;
	protected Facing facing = defaultFacing;
	protected player playerRef;
	protected bool dying = false;

	public override void _Ready()
	{
		this.playerRef = (player)GetTree().GetNodesInGroup(game_manager.GroupPlayer)[0];
		MainLoop mainLoop = Engine.GetMainLoop();
		SceneTree sceneTree = mainLoop as SceneTree;
	}

	public override void _PhysicsProcess(double delta)
	{
		FallenOff();
	}

	public void FallenOff()
	{
		if (GlobalPosition.Y > OffScreenKillMe)
			QueueFree();
	}

	public void Die()
	{
		if (this.dying)
			return;

		this.dying = true;
		GetNode<signal_manager>("/root/SignalManager").EmitEnemyHitSignal(this.points, GlobalPosition);
		object_maker.CreateScene(GlobalPosition, SceneKey.Explosion);
		object_maker.CreateScene(GlobalPosition, SceneKey.Pickup);
		SetPhysicsProcess(false);
		Hide();
		QueueFree();
	}

	public virtual void OnVisibleOnScreenNotifier2dScreenEntered()
	{
	}

	public virtual void OnVisibleOnScreenNotifier2dScreenExited()
	{
	}

	public virtual void OnHitBoxAreaEntered(Area2D area)
	{
		GD.Print("Enemy hit:", area);
		Die();
	}

	private void KillEnemy(int points, Vector2 enemyPosition)
	{
		GD.Print("Enemy killed. Points: ", points);
	}
}
