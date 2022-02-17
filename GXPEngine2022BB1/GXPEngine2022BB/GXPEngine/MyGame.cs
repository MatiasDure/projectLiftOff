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
	string startName = "1";
	int _loserNr;
	string _loserName;

	EasyDraw declareWinner;
	Sprite winnerImg;
	public int LoserNr { get => _loserNr; set => _loserNr = value; }
	public string LoserName { get => _loserName; private set => _loserName = value; }

	HUD[] huds;
	public MyGame() : base(854, 480, false, false, 1366, 768) 		
	{
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

		if (levelName == "projectLevel4.tmx")
		{
			Sound whoWon;
			declareWinner = new EasyDraw(200, 200, false);
			string playerWon;

			playerWon = LoserNr == 0 ? "greek" : "egypt";
			whoWon = LoserNr == 0 ? new Sound("sounds/player2Wins.wav") : new Sound("sounds/player1Wins.wav");
			whoWon.Play();
			winnerImg = new Sprite(playerWon + ".png",false,false);
			winnerImg.SetXY(width/2,height/2);
			declareWinner.Text(playerWon);
			AddChild(winnerImg);
            try { AddChild(declareWinner); } catch (Exception e) { Console.WriteLine("this is the line that kills"); }
		}
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
