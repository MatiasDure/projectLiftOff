using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;
using TiledMapParser;

public class Floor:Sprite
{
    string _type;

    public string Type { get => _type; private set => _type = value; }

    public Floor(TiledObject obj = null) : base("snowPath.png")
    {
        alpha = 0;
        collider.isTrigger = true;
        if(obj != null)
        {
            Type = obj.GetStringProperty("type","fast");
        }
    }
}
