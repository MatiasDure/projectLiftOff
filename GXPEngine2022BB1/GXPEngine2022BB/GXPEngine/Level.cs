using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;
using TiledMapParser;

public class Level : GameObject
{
    TiledLoader loader;
    Player[] players;
    string currentLevel;
    Scroller scrollerObject;
    Parallax parallaxScroller;
    SpriteBatch pillars;
    SpriteBatch mount;

    EasyDraw mainMenu;

    public Level(string fileName)
    {
        loader = new TiledLoader(fileName);
        Map levelData = MapParser.ReadMap(fileName);
        currentLevel = fileName;
        parallaxScroller = new Parallax();
        pillars = new SpriteBatch();
        mount = new SpriteBatch();
        mainMenu = new EasyDraw(400, 400, false);
        mainMenu.SetXY(game.width / 2 - mainMenu.width / 2, game.height / 2 - mainMenu.height / 2);
        AddChild(mainMenu);
    }

    public void CreateLevel(int pLoser)
    {
        AnimationSprite winnerAnim;
        winnerAnim = pLoser == 0 ? new AnimationSprite("egyptCharSpriteSheet.png", 5, 2) : new AnimationSprite("greekCharSpriteSheet.png", 5, 2);
        winnerAnim.SetXY(game.width/2,game.height/2);
        AddChild(winnerAnim);
    }

    public HUD[] CreateLevel()
    {
        if (currentLevel == "projectLevel0.tmx" || currentLevel == "projectLevel4.tmx")
        {            
            loader.addColliders = false;
            loader.LoadImageLayers();
            return null;
        }

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

        loader.addColliders = false;
        loader.LoadTileLayers(3); //no collision tiles

        loader.addColliders = true;
        loader.LoadTileLayers(2); //collision tiles
        loader.autoInstance = true;
        loader.LoadObjectGroups();

        

        speedForScroller = currentLevel == "projectLevel1.tmx" ? 2f : 3f;

        game.AddChild(parallaxScroller);

        //creating scroller object 
        scrollerObject = new Scroller(speedForScroller);
        scrollerObject.SetXY(game.width / 2, game.height / 2);
        AddChild(scrollerObject);

        parallaxScroller.scroller = scrollerObject; //setting target to parallex scrolling

        players = FindObjectsOfType<Player>();

        Portal[] portals = FindObjectsOfType<Portal>();

        foreach (Portal p in portals) p.targetObj = scrollerObject;

        Spawner spawner = FindObjectOfType<Spawner>();
        spawner.target = scrollerObject;

        HUD[] huds = new HUD[2];
        int j = 0;
        foreach (Player player in players)
        {
            player.currentLvlVelocityX = speedForScroller;
            huds[j] = CreateHUD(player);
            j++;
        }

        return huds;

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
            for (int i = 0; i < players.Length; i++)
            {
                players[i].scrollerPositionX = scrollerObject.x;
            }
            if (Input.GetKeyDown(Key.H)) NextLevel(0);
            return;
        }
        switch(currentLevel)
        {
            case "projectLevel0.tmx":
                if (Input.GetKeyDown(Key.H)) NextLevel(1);
                break;
            case "projectLevel4.tmx":
                if (Input.GetKeyDown(Key.H)) NextLevel(0);
                break;
        }

    }

    HUD CreateHUD(Player pTarget)
    {
        if (pTarget == null) return null;
        return new HUD(pTarget);
    }

    bool turn;

    void NextLevel(int pLevel)
    {
        ((MyGame)game).LoadLevel(""+pLevel);
    }

}
