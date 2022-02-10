using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;
using TiledMapParser;

public class Level: GameObject
{
    TiledLoader loader;
    Player[] players;
    string currentLevel;
    Scroller scroller;

    public Level(string fileName)
    {
        loader = new TiledLoader(fileName);
        Map levelData = MapParser.ReadMap(fileName);
        currentLevel = fileName;
    }

    public void CreateLevel()
    {
        float speedForScroller;
        loader.addColliders = false;
        loader.LoadImageLayers();

        loader.addColliders = true;
        loader.rootObject = this;
        loader.LoadTileLayers();
        loader.autoInstance = true;
        loader.LoadObjectGroups();

        switch(currentLevel)
        {
            case "1":
                speedForScroller = 2f;
                break;
            case "2":
                speedForScroller = 4f;
                break;
            case "3":
                speedForScroller = 6f;
                break;
            case "4":
                speedForScroller = 8f;
                break;
            default:
                speedForScroller = 2f;
                break;
        }

        scroller = new Scroller(speedForScroller);
        scroller.SetXY(game.width/2,game.height/2);
        AddChild(scroller);
        players = FindObjectsOfType<Player>();
        int i = 0;
        foreach(Player player in players)
        {
            if (i == 0)
            {
                //player.Attributes[1] = 100;
                i++;
            }
            CreateHUD(player);
        }
    }

    void scrolling(GameObject pTarget)
    {
        int boundary = 150;
        if (pTarget.x + x < boundary)
        {
            x = boundary - pTarget.x;
        }
        if (pTarget.x + x > game.width - boundary)
        {
            x = game.width - boundary - pTarget.x;
        }
    }

    void Update()
    { 
        if(scroller != null) scrolling(scroller);
    }

    void CreateHUD(Player pTarget)
    {
        if (pTarget == null) return;
        game.AddChild(new HUD(pTarget));
    }
}
