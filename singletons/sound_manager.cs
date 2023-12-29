using Godot;
using System;
using System.Collections.Generic;

public partial class sound_manager : Node
{
	public const String SoundCheckpoint = "checkpoint";
	public const String SoundDamage = "damage";
	public const String SoundKill = "kill";
	public const String SoundGameover = "gameover1";
	public const String SoundImpact = "impact";
	public const String SoundJump = "jump";
	public const String SoundLand = "land";
	public const String SoundLaser = "laser";
	public const String SoundMusic1 = "music1";
	public const String SoundMusic2 = "music2";
	public const String SoundPickup = "pickup";
	public const String SoundBossArrive = "boss_arrive";
	public const String SoundWin = "win";

	public static IDictionary<string, AudioStream> sounds = new Dictionary<string, AudioStream>();

	public sound_manager(): base()
	{
		sounds[SoundCheckpoint] = GD.Load<AudioStream>("res://assets/sound/checkpoint.wav");
		sounds[SoundDamage] = GD.Load<AudioStream>("res://assets/sound/damage.wav");
		sounds[SoundKill] = GD.Load<AudioStream>("res://assets/sound/pickup5.ogg");
		sounds[SoundGameover] = GD.Load<AudioStream>("res://assets/sound/game_over.ogg");
		sounds[SoundImpact] = GD.Load<AudioStream>("res://assets/sound/impact.wav");
		sounds[SoundJump] = GD.Load<AudioStream>("res://assets/sound/jump.wav");
		sounds[SoundLand] = GD.Load<AudioStream>("res://assets/sound/land.wav");
		sounds[SoundLaser] = GD.Load<AudioStream>("res://assets/sound/laser.wav");
		sounds[SoundMusic1] = GD.Load<AudioStream>("res://assets/sound/Farm Frolics.ogg");
		sounds[SoundMusic2] = GD.Load<AudioStream>("res://assets/sound/Flowing Rocks.ogg");
		sounds[SoundPickup] = GD.Load<AudioStream>("res://assets/sound/pickup5.ogg");
		sounds[SoundBossArrive] = GD.Load<AudioStream>("res://assets/sound/boss_arrive.wav");
		sounds[SoundWin] = GD.Load<AudioStream>("res://assets/sound/you_win.ogg");
	}

	public static void PlayClip(AudioStreamPlayer2D player, String clipKey)
	{
		if (!sounds.ContainsKey(clipKey))
			return;

		player.Stream = sounds[clipKey];
		player.Play();
	}
}
