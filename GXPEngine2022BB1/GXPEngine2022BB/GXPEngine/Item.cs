using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;
using TiledMapParser;

public class Item:Sprite
{
    string type;
    Sprite itemImg;

    public Item(TiledObject obj = null):base("hitboxItems.jpg")
    {
        if(obj != null)
        {
            type = obj.GetStringProperty("type","shield");
        }

        if (type != null)
        {
            string filePath = "";
            switch (type)
            {
                case "greek":
                    filePath = "shield";
                    break;
                case "egypt":
                    filePath = "hitBox";
                    break;
            }
            Console.WriteLine(filePath);
            try 
            { 
                itemImg = new Sprite(filePath+".png", false, false);
                itemImg.SetOrigin(width/2,height/2);
                AddChild(itemImg);
                alpha = 0;
            } 
            catch (Exception e) { Console.WriteLine(e.Message); }                 
        }
    }
}