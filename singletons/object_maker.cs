using Godot;
using System;
using System.Collections.Generic;

public partial class object_maker : Node
{
	private static readonly IDictionary<string, PackedScene> bullets = new Dictionary<string, PackedScene>();
	private static readonly IDictionary<string, PackedScene> scenes = new Dictionary<string, PackedScene>();
	public object_maker() : base()
	{
		bullets[BulletKey.Player.ToString()] = GD.Load<PackedScene>("res://bullets/bullet_player/bullet_player.tscn");
		bullets[BulletKey.Enemy.ToString()] = GD.Load<PackedScene>("res://bullets/bullet_enemy/bullet_enemy.tscn");

		scenes[SceneKey.Explosion.ToString()] = GD.Load<PackedScene>("res://enemy_explosion/enemy_explosion.tscn");
		scenes[SceneKey.Pickup.ToString()] = GD.Load<PackedScene>("res://fruit_pickup/fruit_pickup.tscn");
	}

	public static void AddChildDeferred(Node childToAdd)
	{
		MainLoop mainLoop = Engine.GetMainLoop();
		SceneTree sceneTree = mainLoop as SceneTree;
		sceneTree.Root.AddChild(childToAdd);
	}

	public static void CallAddChild(Node childToAdd)
	{
		object_maker objectMaker = new();
		objectMaker.CallDeferred("AddChildDeferred", childToAdd);
	}

	public static void CreateBullet(
		float speed,
		Vector2 direction,
		Vector2 startPosition,
		float lifeSpan,
		BulletKey bulletKey
	)
	{
		bullet_base newBullet = bullets[bulletKey.ToString()].Instantiate<bullet_base>();
		newBullet.Setup(direction, lifeSpan, speed);
		newBullet.GlobalPosition = startPosition;
		CallAddChild(newBullet);
	}

	public static void CreateScene(Vector2 startPosition, SceneKey scene)
	{
		Node2D newScene = scenes[scene.ToString()].Instantiate<Node2D>();
		newScene.GlobalPosition = startPosition;
		CallAddChild(newScene);
	}
}
