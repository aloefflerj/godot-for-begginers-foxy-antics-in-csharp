using Godot;
using System;

public partial class level_base : Node2D
{

	private Camera2D playerCam = null;
	private CharacterBody2D player = null;
	private game_manager gameManager;


	public override void _Ready()
	{
		// Engine.TimeScale = 1;
		GetTree().Paused = false;
		this.playerCam = GetNode<Camera2D>("PlayerCam");
		this.player = GetNode<CharacterBody2D>("Player");
		this.gameManager = GetNode<game_manager>("/root/GameManager");
	}

	public override void _Process(double delta)
	{
		playerCam.Position = player.Position;
	}
}
