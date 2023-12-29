using Godot;
using System;
using System.Collections.Generic;
using System.Text.Json;

public partial class score_manager : Node
{
	const String HighScoreFile = "user://SCORES.dat";
	const String HighScoreKey = "highscore";
	private int score = 0;
	private int highScore = 0;

	private signal_manager signalManager;
	
	public override void _Ready()
	{
		this.LoadHighScore();
		this.signalManager = GetNode<signal_manager>("/root/SignalManager");

		this.signalManager.OnBossKilled += this.OnBossKilled;
		this.signalManager.OnPickupHit += this.OnPickupHit;
		this.signalManager.OnEnemyHit += this.OnEnemyHit;

		this.signalManager.OnLevelComplete += this.OnLevelComplete;
		this.signalManager.OnGameOver += this.OnGameOver;
	}

	public override void _ExitTree()
    {
		this.signalManager.OnBossKilled += this.OnBossKilled;
		this.signalManager.OnPickupHit += this.OnPickupHit;
		this.signalManager.OnEnemyHit += this.OnEnemyHit;
		
		this.signalManager.OnLevelComplete += this.OnLevelComplete;
		this.signalManager.OnGameOver += this.OnGameOver;
    }

	public void UpdateScore(int points)
	{
		this.score += points;
		if (this.highScore < this.score)
			this.highScore = this.score;

		GD.Print("Update score: ", this.score);
		this.signalManager.EmitOnScoreUpdated();
	}

	public int GetScore()
	{
		return this.score;
	}

	public int GetHighScore()
	{
		return this.highScore;
	}

	public void ResetScore()
	{
		this.score = 0;
	}

	public void SaveHighscore()
	{
		FileAccess file = FileAccess.Open(HighScoreFile, FileAccess.ModeFlags.Write);

		var stdObject = new {
			highscore = this.highScore
		};

		file.StoreString(JsonSerializer.Serialize(stdObject));
		file.Close();
	}

	private void LoadHighScore()
	{
		if (!FileAccess.FileExists(HighScoreFile))
		{
			return;
		}

		FileAccess file = FileAccess.Open(HighScoreFile, FileAccess.ModeFlags.Read);
		String textData = file.GetAsText();

		if (String.IsNullOrWhiteSpace(textData))
		{
			file.Close();
			return;
		}
		
		Dictionary<string, int> data = JsonSerializer.Deserialize<Dictionary<string, int>>(file.GetAsText());

		GD.Print("data: ", data);

		if (data.ContainsKey(HighScoreKey))
		{
			this.highScore = data[HighScoreKey];
			GD.Print("loaded: ", this.highScore);
		}

		file.Close();
	}

	public void OnBossKilled(int points)
	{
		this.UpdateScore(points);
	}
	
	public void OnPickupHit(int points)
	{
		this.UpdateScore(points);
	}
	
	public void OnEnemyHit(int points, Vector2 _)
	{
		this.UpdateScore(points);
	}

	public void OnLevelComplete()
	{
		GD.Print("On Level Complete");
		this.SaveHighscore();
	}
	
	public void OnGameOver()
	{
		this.SaveHighscore();
	}
}
