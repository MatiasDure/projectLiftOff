using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;
using TiledMapParser;
public class Egyptian : Player
{
    public Egyptian(TiledObject obj = null):base("greekChar.png",1,1)
    {

    }
    protected override void Update()
    {
        Jumping(Key.UP);
        Ability(Key.K,20);
        base.Update();
    }

}
