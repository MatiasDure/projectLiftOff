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
    AnimationSprite anim;
    Sprite winnerScreen;

    public Level(string fileName)
    {
        loader = new TiledLoader(fileName);
        Map levelData = MapParser.ReadMap(fileName);
        currentLevel = fileName;
        parallaxScroller = new Parallax();
        pillars = new SpriteBatch();
        mount = new SpriteBatch();
    }

    public HUD[] CreateLevel()
    {
        if (currentLevel == "projectLevel0.tmx" || currentLevel == "projectLevel4.tmx")
        {           
            loader.addColliders = false;
            loader.LoadImageLayers();
            if(currentLevel == "projectLevel0.tmx")
            {
                anim = new AnimationSprite("menuScreenAnim.png",2,1,-1,false,false);
                anim.SetXY(game.width/2 - 160,game.height - 120);
            }
            else
            {
                string animName = null;
                if (((MyGame)game).LoserNr == 0) animName = "egypt";
                else animName = "greek";
                if (animName != null)
                {
                    anim = new AnimationSprite(animName + "WinnerAnim.png", 2, 1, -1, false, false);
                    anim.SetXY(game.width / 2 - 50, game.height / 2 - 30);
                    winnerScreen = new Sprite(animName+"WinnerScreen.png",false,false);
                }                
                AddChild(winnerScreen);
            }
            AddChild(anim);
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



        speedForScroller = ((MyGame)game).CurrentLvlIteration * 1.2f;//currentLevel == "projectLevel1.tmx" ? 2f : 3f;

        game.AddChild(parallaxScroller);

        //creating scroller object 
        scrollerObject = new Scroller(speedForScroller);
        scrollerObject.SetXY(game.width / 2, game.height / 2);
        AddChild(scrollerObject);

        parallaxScroller.scroller = scrollerObject; //setting target to parallex scrolling

        players = FindObjectsOfType<Player>();

        Portal[] portals = FindObjectsOfType<Portal>();

        foreach (Portal p in portals) p.targetObj = scrollerObject;

        if (currentLevel == "projectLevel2.tmx")
        {
            Spawner spawner = FindObjectOfType<Spawner>();
            spawner.target = scrollerObject;
        }

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
        int boundary = 300;
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
                anim.Animate(0.06f);
                if (Input.GetKeyDown(Key.H)) NextLevel(1);
                break;
            case "projectLevel4.tmx":
                anim.Animate(0.06f);
                if (Input.GetKeyDown(Key.H)) NextLevel(0);
                break;
        }

    }

    HUD CreateHUD(Player pTarget)
    {
        if (pTarget == null) return null;
        return new HUD(pTarget);
    }

    void NextLevel(int pLevel)
    {
        ((MyGame)game).LoadLevel(""+pLevel);
    }

}
