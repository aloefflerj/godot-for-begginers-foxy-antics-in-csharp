using Godot;
using System;

public partial class main : CanvasLayer
{
	private game_manager gameManager;
	private score_manager scoreManager;
	private Label labelHighScore;
	
	public override void _Ready()
	{
		GetTree().Paused = false;
		
		this.gameManager = GetNode<game_manager>("/root/GameManager");
		this.scoreManager = GetNode<score_manager>("/root/ScoreManager");
		this.labelHighScore = GetNode<Label>("VBoxContainer/LabelHighScore");

		this.labelHighScore.Text = "HighScore: " + this.scoreManager.GetHighScore();
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("jump"))
			this.gameManager.LoadNextLevelScene();
	}
}
