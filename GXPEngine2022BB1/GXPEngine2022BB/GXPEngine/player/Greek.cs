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
        Ability(Key.SPACE,1);
        base.Update();
    }

    protected override void Ability(int pKey, int pAmountManaCost)
    {
        if (!AbilitySet(pKey, pAmountManaCost) || isBoosting) return;      
        isBoosting = true;
        Console.WriteLine("boosting");
        timeBoost = Time.time + 500;
    }
}
