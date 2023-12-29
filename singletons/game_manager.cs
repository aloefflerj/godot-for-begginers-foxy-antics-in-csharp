using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class game_manager : Node
{
	public const String GroupPlayer = "player";
	private const int TotalLevels = 3;
	private PackedScene mainScene;

	private int currentLevel = 0;
	private IDictionary<int, PackedScene> levelScenes = new Dictionary<int, PackedScene>();
	private score_manager scoreManager;


	public override void _Ready()
	{
		this.mainScene = GD.Load<PackedScene>("res://main/main.tscn");
		this.scoreManager = GetNode<score_manager>("/root/ScoreManager");
		this.InitLevelScenes();
	}

	public void InitLevelScenes()
	{
		foreach(int levelNumber in GD.Range(1, TotalLevels + 1))
		{
			this.levelScenes[levelNumber] = GD.Load<PackedScene>(
				String.Format("res://level_base/levels/level_{0}.tscn", levelNumber)
			);
		}
	}

	public void LoadMainScene()
	{
		this.currentLevel = 0;
		this.scoreManager.ResetScore();
		GetTree().ChangeSceneToPacked(this.mainScene);
	}

	public void LoadNextLevelScene()
	{
		this.SetNextLevel();
		this.GetTree().ChangeSceneToPacked(this.levelScenes[this.currentLevel]);
	}

	public void SetNextLevel()
	{
		this.currentLevel += 1;
		if (this.currentLevel > TotalLevels)
			this.currentLevel = 1;
	}
}
