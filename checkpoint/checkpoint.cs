using Godot;
using System;

public partial class checkpoint : Area2D
{
	private AnimationTree animationTree;
	const String TriggerCondition = "parameters/conditions/onTrigger";
	private signal_manager signalManager;
	private AudioStreamPlayer2D sound;
	
	public override void _Ready()
	{
		this.animationTree = GetNode<AnimationTree>("AnimationTree");
		this.signalManager = GetNode<signal_manager>("/root/SignalManager");
		this.sound = GetNode<AudioStreamPlayer2D>("Sound");
		
		this.signalManager.OnBossKilled += this.OnBossKilled;
	}

	public override void _Process(double delta)
	{
	}

	public void OnBossKilled(int _)
	{
		this.animationTree.Set(TriggerCondition, true);
		Monitoring = true;
		sound_manager.PlayClip(this.sound, sound_manager.SoundWin);
	}

	public void OnAreaEntered(Area2D area)
	{
		this.signalManager.EmitOnLevelCompleteSignal();
	}

	public override void _ExitTree()
	{
		this.signalManager.OnBossKilled -= this.OnBossKilled;
	}

}
