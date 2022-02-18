using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;
using TiledMapParser;

public class Spawner:Sprite
{
    public GameObject target;
    public Spawner(TiledObject obj = null) : base("hitboxItems.jpg",false,false)
    {
        alpha = 0;
    }

    void Update()
    {
        if(DistanceTo(target) < 100) CreateBoss();   
    }

    void CreateBoss()
    {
        game.AddChild(new Boss(this.x,this.y));
        SelfDestruct();
    }

    void SelfDestruct()
    {
        this.Destroy();
    }
}