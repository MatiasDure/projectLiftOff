using System;                                   // System contains a lot of default C# libraries 
using System.Collections.Generic;
using GXPEngine;                                // GXPEngine contains the engine

//-------What--to--work--on--------
// HUD (make it look nicer)
// Find a more efficient way to make create the boost and slow down paths
// Create main screen where people can choose their characters
// Create disappearing environtment name appear at the start of every environment
// You can see whos player 1 and player 2 depending on ID in tiled

public class MyGame : Game
{
	string levelName = null;
	string startName = "0";
	int _loserNr;
	string _loserName;

	public int LoserNr { get => _loserNr; set => _loserNr = value; }
	public string LoserName { get => _loserName; private set => _loserName = value; }

	HUD[] huds;

	Sound[] soundTracks;
	SoundChannel channel;
	public MyGame() : base(854, 480, false, false, 1366, 768) 		
	{
		soundTracks = new Sound[]
		{
			new Sound("sounds/mainMenuSoundtrack.mp3", true, true),
			new Sound("sounds/greekSoundtrack.mp3", true, true),
			new Sound("sounds/egyptSoundtrack.mp3", true, true),
			new Sound("sounds/player2Wins.wav"),
			new Sound("sounds/player1Wins.wav"),
			new Sound("sounds/victorySound.mp3", true, true)
		};
		OnAfterStep += CheckLevel;
		LoadLevel(startName);
	}

	void Update()
	{
		if (Input.GetKeyDown(Key.F)) LoadLevel(startName);
        if (Input.GetKeyDown(Key.D)) Console.WriteLine(game.GetDiagnostics());
	}

	static void Main()							
	{
		MyGame game = new MyGame();
		game.Start();
	}
	
	public void LoadLevel(string pLevelNum = null)
    {
		levelName = "projectLevel" + pLevelNum + ".tmx";	
    }

	void CheckLevel()
    {
		if (levelName == null) return;
		if(channel != null) channel.Stop();
		DestroyAll();
		Level level = new Level(levelName);
		huds = level.CreateLevel();
   
		AddChild(level);

		if (huds != null)
        {
            foreach (HUD h in huds)
            {
                if (h != null) AddChild(h);
            }
        }

		int sound = -1;
		switch (levelName)
        {
			case "projectLevel0.tmx":
				sound = 0;
				break;
			case "projectLevel1.tmx":
				sound = 1;
				break;
			case "projectLevel2.tmx":
				sound = 2;
				break;
			case "projectLevel4.tmx":
				_ = LoserNr == 0 ? soundTracks[3].Play() : soundTracks[4].Play();
				sound = 5;
				break;
		}

		if(sound != -1) channel = soundTracks[sound].Play();
		levelName = null;
    }

	void DestroyAll()
    {
		List<GameObject> children = GetChildren();
		foreach(GameObject child  in children)
        {
			child.Destroy();
        }
    }

}
