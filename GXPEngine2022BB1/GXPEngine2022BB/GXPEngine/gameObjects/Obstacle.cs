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
    Sprite itemImg;

    public Obstacle(TiledObject obj = null) : base("hitboxItems.jpg")
    {
        collider.isTrigger = true;
        if (obj != null)
        {
            type = obj.GetStringProperty("type", "shield");
        }

        try
        {
            itemImg = new Sprite(type + ".png", false, false);
            itemImg.SetOrigin(width / 2, height / 2);
            AddChild(itemImg);
            alpha = 0;
        }
        catch (Exception e) { Console.WriteLine(e.Message); }
        
    }
}