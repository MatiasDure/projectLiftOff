using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;
using TiledMapParser;

public class Obstacle:Sprite
{
    string type;
    AnimationSprite itemImg;

    public Obstacle(TiledObject obj = null) : base("hitboxItems.jpg")
    {
        collider.isTrigger = true;
        if (obj != null)
        {
            type = obj.GetStringProperty("type", "vase");
        }

        try
        {
            itemImg = new AnimationSprite("obstacles/"+type + ".png", 5, 2, -1, false, false);
            itemImg.SetOrigin(width / 2, height / 2);
            AddChild(itemImg);
            itemImg.SetCycle(0,7);
            alpha = 0;
        }
        catch (Exception e) { Console.WriteLine(e.Message); }
        
    }

    void Update()
    {
        itemImg.Animate(0.15f);
    }
}