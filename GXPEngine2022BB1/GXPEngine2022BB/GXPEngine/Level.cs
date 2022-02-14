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
    Scroller scrollerObject;
    Parallax parallaxScroller;
    SpriteBatch pillars;
    SpriteBatch mount;

    public Level(string fileName)
    {
        loader = new TiledLoader(fileName);
        Map levelData = MapParser.ReadMap(fileName);
        currentLevel = fileName;
        Console.WriteLine(currentLevel);
        parallaxScroller = new Parallax();
        pillars = new SpriteBatch();
        mount = new SpriteBatch();
    }

    public void CreateLevel()
    {
        float speedForScroller;
        loader.addColliders = false;
        loader.LoadImageLayers();

        for (int i = 0; i < 2; i++)
        {
            if (i == 0) loader.rootObject = mount;
            else loader.rootObject = pillars;
            loader.LoadTileLayers(i);
        }

        mount.Freeze(); 
        pillars.Freeze();
        parallaxScroller.AddChild(mount); //adding sprite batch for parallax effect
        AddChild(pillars); //adding sprite batch as child of level for normal scrolling

        loader.addColliders = true;
        loader.LoadTileLayers(2);
        loader.autoInstance = true;
        loader.LoadObjectGroups();

        loader.autoInstance = false;
        loader.addColliders = false;
        loader.LoadTileLayers(3);

        switch (currentLevel)
        {
            case "projectLevel1.tmx":
                speedForScroller = 2f;
                break;
            case "projectLevel2.tmx":
                speedForScroller = 4f;
                break;
            case "projectLevel3.tmx":
                speedForScroller = 6f;
                break;
            case "projectLevel4.tmx":
                speedForScroller = 8f;
                break;
            default:
                speedForScroller = 2f;
                break;
        }

        game.AddChild(parallaxScroller);
        scrollerObject = new Scroller(speedForScroller);
        scrollerObject.SetXY(game.width/2,game.height/2);
        AddChild(scrollerObject);
        parallaxScroller.scroller = scrollerObject;
        players = FindObjectsOfType<Player>();

        foreach(Player player in players)
        {
            player.currentLvlVelocityX = speedForScroller;
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
        if (scrollerObject != null)
        {
            scrolling(scrollerObject);
            for(int i = 0; i < players.Length; i++)
            {
                players[i].scrollerPositionX = scrollerObject.x;
            }
        }
    }

    void CreateHUD(Player pTarget)
    {
        if (pTarget == null) return;
        game.AddChild(new HUD(pTarget));
    }

}
