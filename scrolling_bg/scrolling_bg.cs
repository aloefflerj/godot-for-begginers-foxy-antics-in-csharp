using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class scrolling_bg : ParallaxBackground
{

	[Export(PropertyHint.Range, "1,4,")]
	private int levelNumber;
	[Export]
	private double mirrorX = 1440;
	[Export]
	private Vector2 spriteOffset = new(0, -540);
	private Vector2 spriteScale = new((float)0.75, (float)0.75);
	private readonly List<List<CompressedTexture2D>> bgFiles = new(4);
	public override void _Ready()
	{
		List<CompressedTexture2D> l1 = new(7)
		{
			GD.Load<CompressedTexture2D>("res://assets/backgrounds/game_background_1/layers/sky.png"),
			GD.Load<CompressedTexture2D>("res://assets/backgrounds/game_background_1/layers/clouds_1.png"),
			GD.Load<CompressedTexture2D>("res://assets/backgrounds/game_background_1/layers/clouds_2.png"),
			GD.Load<CompressedTexture2D>("res://assets/backgrounds/game_background_1/layers/clouds_3.png"),
			GD.Load<CompressedTexture2D>("res://assets/backgrounds/game_background_1/layers/clouds_4.png"),
			GD.Load<CompressedTexture2D>("res://assets/backgrounds/game_background_1/layers/rocks_1.png"),
			GD.Load<CompressedTexture2D>("res://assets/backgrounds/game_background_1/layers/rocks_2.png")
		};

		List<CompressedTexture2D> l2 = new(9)
		{
			GD.Load<CompressedTexture2D>("res://assets/backgrounds/game_background_2/layers/sky.png"),
			GD.Load<CompressedTexture2D>("res://assets/backgrounds/game_background_2/layers/birds.png"),
			GD.Load<CompressedTexture2D>("res://assets/backgrounds/game_background_2/layers/clouds_1.png"),
			GD.Load<CompressedTexture2D>("res://assets/backgrounds/game_background_2/layers/clouds_2.png"),
			GD.Load<CompressedTexture2D>("res://assets/backgrounds/game_background_2/layers/clouds_3.png"),
			GD.Load<CompressedTexture2D>("res://assets/backgrounds/game_background_2/layers/pines.png"),
			GD.Load<CompressedTexture2D>("res://assets/backgrounds/game_background_2/layers/rocks_1.png"),
			GD.Load<CompressedTexture2D>("res://assets/backgrounds/game_background_2/layers/rocks_2.png"),
			GD.Load<CompressedTexture2D>("res://assets/backgrounds/game_background_2/layers/rocks_3.png")
		};

		List<CompressedTexture2D> l3 = new(8)
		{
			GD.Load<CompressedTexture2D>("res://assets/backgrounds/game_background_3/layers/sky.png"),
			GD.Load<CompressedTexture2D>("res://assets/backgrounds/game_background_3/layers/clouds_1.png"),
			GD.Load<CompressedTexture2D>("res://assets/backgrounds/game_background_3/layers/clouds_2.png"),
			GD.Load<CompressedTexture2D>("res://assets/backgrounds/game_background_3/layers/ground_1.png"),
			GD.Load<CompressedTexture2D>("res://assets/backgrounds/game_background_3/layers/ground_2.png"),
			GD.Load<CompressedTexture2D>("res://assets/backgrounds/game_background_3/layers/ground_3.png"),
			GD.Load<CompressedTexture2D>("res://assets/backgrounds/game_background_3/layers/plant.png"),
			GD.Load<CompressedTexture2D>("res://assets/backgrounds/game_background_3/layers/rocks.png")
		};

		List<CompressedTexture2D> l4 = new(5)
		{
			GD.Load<CompressedTexture2D>("res://assets/backgrounds/game_background_4/layers/sky.png"),
			GD.Load<CompressedTexture2D>("res://assets/backgrounds/game_background_4/layers/clouds_1.png"),
			GD.Load<CompressedTexture2D>("res://assets/backgrounds/game_background_4/layers/clouds_2.png"),
			GD.Load<CompressedTexture2D>("res://assets/backgrounds/game_background_4/layers/ground.png"),
			GD.Load<CompressedTexture2D>("res://assets/backgrounds/game_background_4/layers/rocks.png")
		};

		bgFiles.Add(l1);
		bgFiles.Add(l2);
		bgFiles.Add(l3);
		bgFiles.Add(l4);

		this.AddBackgrounds();
	}

	private int GetLevelIndex()
	{
		return this.levelNumber - 1;
	}

	private double GetIncrement()
	{
		return 1.0 / this.bgFiles[this.GetLevelIndex()].Count;
	}

	private Sprite2D GetSprite(Texture2D texture2d)
	{
		Sprite2D sprite = new()
		{
			Texture = texture2d,
			Scale = this.spriteScale,
			Offset = this.spriteOffset
		};

		return sprite;
	}

	private void AddLayer(Texture2D texture2d, double timeOffset)
	{
		Sprite2D sprite = this.GetSprite(texture2d);
		ParallaxLayer parallaxLayer = new()
		{
			MotionMirroring = new Vector2((float)this.mirrorX, 0),
			MotionScale = new Vector2((float)timeOffset, 1)
		};

		parallaxLayer.AddChild(sprite);
		this.AddChild(parallaxLayer);
	}

	private void AddBackgrounds()
	{
		double increment = this.GetIncrement();
		double timeOffset = increment;
		List<CompressedTexture2D> filesList = this.bgFiles[this.GetLevelIndex()];

		int levelIndex = 0;
		foreach (CompressedTexture2D bgFile in filesList)
		{
			if (levelIndex == 0)
			{
				this.AddLayer(bgFile, 1);
			}
			else
			{
				this.AddLayer(bgFile, timeOffset);
				timeOffset += increment;
			}
			levelIndex++;
		}
	}
}
