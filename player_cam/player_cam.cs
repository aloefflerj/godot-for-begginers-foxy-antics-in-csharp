using Godot;
using System;

public partial class player_cam : Camera2D
{
	private signal_manager signalManager;
	private Timer shakeTimer;
	private double shakeAmount = 3.0;

	public override void _Ready()
	{
		SetProcess(false);
		this.shakeTimer = GetNode<Timer>("ShakeTimer");
		this.signalManager = GetNode<signal_manager>("/root/SignalManager");
		this.signalManager.OnPlayerHit += this.OnPlayerHit;
		this.signalManager.OnGameOver += this.OnGameOver;
	}

	public override void _Process(double delta)
	{
		Offset = this.GetRandomOffset();
	}

	private Vector2 GetRandomOffset()
	{
		return new Vector2(
			(float)GD.RandRange(-this.shakeAmount, this.shakeAmount),
			(float)GD.RandRange(-this.shakeAmount, this.shakeAmount)
		);
	}

	private void Shake()
	{
		SetProcess(true);
		this.shakeTimer.Start();
	}

	public void OnPlayerHit(int lives)
	{
		this.Shake();
	}

	public void ResetCamera()
	{
		SetProcess(false);
		Offset = Vector2.Zero;
	}

	public void OnGameOver()
	{
		this.ResetCamera();
	}

	private void OnShakeTimerTimeout()
	{
		this.ResetCamera();
	}

	public override void _ExitTree()
	{
		this.signalManager.OnPlayerHit -= this.OnPlayerHit;
		this.signalManager.OnGameOver -= this.OnGameOver;
	}
}
