using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;
using TiledMapParser;

public class Obstacle:AnimationSprite
{
    public Obstacle(TiledObject obj = null):base("danger.png",1,1)
    {
        collider.isTrigger = true;
    }
}