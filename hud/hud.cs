using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class hud : Control
{
	private ColorRect colorRect;
	private VBoxContainer levelCompleteVerticalBox;
	private VBoxContainer gameOverVerticalBox;
	private HBoxContainer heartsHorizontalBox;
	private Label scoreLabel;
	private signal_manager signalManager;
	private game_manager gameManager;
	private score_manager scoreManager;

	private Godot.Collections.Array<Node> hearts;

	public override void _Ready()
	{
		this.colorRect = GetNode<ColorRect>("ColorRect");
		this.levelCompleteVerticalBox = GetNode<VBoxContainer>("ColorRect/BoxLevelComplete");
		this.gameOverVerticalBox = GetNode<VBoxContainer>("ColorRect/BoxGameOver");
		this.heartsHorizontalBox = GetNode<HBoxContainer>("MC/HB/HBHearts");
		this.scoreLabel = GetNode<Label>("MC/HB/ScoreLabel");

		this.signalManager = GetNode<signal_manager>("/root/SignalManager");
		this.gameManager = GetNode<game_manager>("/root/GameManager");
		this.scoreManager = GetNode<score_manager>("/root/ScoreManager");

		this.signalManager.OnLevelComplete += this.OnLevelComplete;
		this.signalManager.OnGameOver += this.OnGameOver;
		this.signalManager.OnPlayerHit += this.OnPlayerHit;
		this.signalManager.OnScoreUpdated += this.OnScoreUpdated;

		this.hearts = this.heartsHorizontalBox.GetChildren();
	}

	public override void _Process(double delta)
	{
		if (this.levelCompleteVerticalBox.Visible)
			if (Input.IsActionJustPressed("jump"))
				this.gameManager.LoadNextLevelScene();

		if (this.gameOverVerticalBox.Visible)
			if (Input.IsActionJustPressed("jump"))
				this.gameManager.LoadMainScene();
	}

	private void ShowHud()
	{
		// Engine.TimeScale = 0;
		GetTree().Paused = true;
		this.colorRect.Visible = true;
	}

	public void OnLevelComplete()
	{
		this.levelCompleteVerticalBox.Visible = true;
		this.ShowHud();
	}

	public void OnGameOver()
	{
		this.gameOverVerticalBox.Visible = true;
		this.ShowHud();
	}

	public void OnPlayerHit(int lives)
	{
		foreach (int life in GD.Range(this.hearts.Count))
		{
			(this.hearts[life] as TextureRect).Visible = lives > life;
		}
	}

	public void OnScoreUpdated()
	{
		this.scoreLabel.Text = this.scoreManager.GetScore().ToString().PadLeft(5, '0');
	}

	public override void _ExitTree()
	{
		this.signalManager.OnLevelComplete -= this.OnLevelComplete;
		this.signalManager.OnGameOver -= this.OnGameOver;
		this.signalManager.OnPlayerHit -= this.OnPlayerHit;
		this.signalManager.OnScoreUpdated -= this.OnScoreUpdated;
	}
}
