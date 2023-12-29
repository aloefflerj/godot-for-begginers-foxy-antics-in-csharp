using Godot;
using System;

public partial class signal_manager : Node
{
	[Signal]
	public delegate void OnEnemyHitEventHandler(int points, Vector2 enemyPosition);
	[Signal]
	public delegate void OnPickupHitEventHandler(int points);
	[Signal]
	public delegate void OnBossKilledEventHandler(int points);
	[Signal]
	public delegate void OnPlayerHitEventHandler(int lives);
	[Signal]
	public delegate void OnLevelCompleteEventHandler();
	[Signal]
	public delegate void OnGameOverEventHandler();
	[Signal]
	public delegate void OnScoreUpdatedEventHandler();

	public override void _Ready()
	{
	}

	public void EmitEnemyHitSignal(int points, Vector2 enemyPosition)
	{
		EmitSignal(SignalName.OnEnemyHit, points, enemyPosition);
	}

	public void EmitOnPickupHitSignal(int points)
	{
		EmitSignal(SignalName.OnPickupHit, points);
	}

	public void EmitOnBossKilledSignal(int points)
	{
		EmitSignal(SignalName.OnBossKilled, points);
	}

	public void EmitOnPlayerHitSignal(int lives)
	{
		EmitSignal(SignalName.OnPlayerHit, lives);
	}

	public void EmitOnLevelCompleteSignal()
	{
		EmitSignal(SignalName.OnLevelComplete);
	}

	public void EmitOnGameOverSignal()
	{
		EmitSignal(SignalName.OnGameOver);
	}

	public void EmitOnScoreUpdated()
	{
		EmitSignal(SignalName.OnScoreUpdated);
	}
}
