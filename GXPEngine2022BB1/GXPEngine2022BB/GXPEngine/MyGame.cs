using System;                                   // System contains a lot of default C# libraries 
using System.Collections.Generic;
using GXPEngine;                                // GXPEngine contains the engine


//-------What--to--work--on--------
// HUD (its not appearing) Maybe behind the sky
// Find a more efficient way to make create the boost and slow down paths
// Create main screen where people can choose their characters
// Create disappearing environtment name appear at the start of every environment
// Create SelectChar class to select characters in main screen
// You can see whos player 1 and player 2 depending on ID in tiled
// Make floors not be a trigger

public class MyGame : Game
{
	string levelName = null;
	string startName = "1"; 
	public MyGame() : base(854, 480, false, false, 1366, 768) 		
	{
		OnAfterStep += CheckLevel;
		LoadLevel(startName);
	}

	void Update()
	{
		if(Input.GetKeyDown(Key.F)) LoadLevel(startName);
        if (Input.GetKeyDown(Key.D)) Console.WriteLine(currentFps);
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
        level.CreateLevel();
        AddChild(level);	
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
/*
  using System;

using System.IO.Ports;

using System.Threading;



class Program

{

   public static void Main()

   {

      SerialPort port = new SerialPort();

      port.PortName = "COM4"; 

      port.BaudRate = 9600;

      port.RtsEnable = true;

      port.DtrEnable = true;

      port.Open();

      while (true)

      {

         string a = port.ReadExisting();

         if (a!="") 

            Console.WriteLine("Read from port: "+a);



         if (Console.KeyAvailable) {

            ConsoleKeyInfo key = Console.ReadKey ();

            port.Write (key.KeyChar.ToString());

         }

         Thread.Sleep(30);

      }

   }

}
 */
/*
 * Ports = SerialPort.GetPortNames();

           foreach (String portName in Ports)

           {

               port = new SerialPort(portName);



               port.BaudRate = 9600;

               port.ReadTimeout = 1000000;





               if (_useUno == false)

               {

                   port.RtsEnable = true;

                   port.DtrEnable = true;

               }

               else

               {

                   port.RtsEnable = false;

                   port.DtrEnable = false;

               }



               if (port.IsOpen)

               {

                   port.Close();

                   try { port.Open(); }

                   catch (System.IO.IOException e) { continue; }

               }

               else

               {

                   try { port.Open(); }

                   catch (System.IO.IOException e) { continue; }

               }



               port.DiscardOutBuffer();

               port.DiscardInBuffer();



               Console.WriteLine("Send Data please");

               SendString("GIVE HANDSHAKE");



               Console.WriteLine("Gimme Data please");

               String Accept = null;

               bool accepted = false;



               while (!accepted)

                   try

                   {

                       Accept = port.ReadLine();

                       accepted = true;

                   }

                   catch (TimeoutException e)

                   {

                       Console.WriteLine("rip");

                   }



               Console.WriteLine(Accept);

               if (Accept.Contains("HANDSHAKE"))

               {

                   found = true;

                   port.Write("FOUND");

                   break;

               }



               port.DiscardInBuffer();

               Console.WriteLine("blih");

           }

           Console.WriteLine("bloh");

       }
*/