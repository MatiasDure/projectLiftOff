using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;
using TiledMapParser;

public class Greek:Player
{
    public Greek(TiledObject obj = null):base("greekChar.png",1,1)
    {

    }

    protected override void Update()
    {
        Jumping(Key.W);
        Ability(Key.SPACE);
        base.Update();
    }
}
