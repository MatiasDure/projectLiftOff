using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;
using TiledMapParser;

public class Floor:Sprite
{
    public string type;

    public Floor(TiledObject obj = null) : base("snowPath.png")
    {
        collider.isTrigger = true;
        if(obj != null)
        {
            type = obj.GetStringProperty("type","fast");
        }
    }
}
