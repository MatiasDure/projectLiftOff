using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;
using TiledMapParser;
public class Egyptian : Player
{
    public Egyptian(TiledObject obj = null):base("hitbox.jpg")
    {
        playerImg = new AnimationSprite("greekCharSpritesheet.png", 5, 5);
    }
    protected override void Update()
    {
        Jumping(Key.UP);
        Ability(Key.K,5);
        base.Update();
    }

    protected override void Ability(int pKey, int pAmountManaCost)
    {
        if (!AbilitySet(pKey, pAmountManaCost) || Attributes[0] == 3) return;
        Attributes[0]++; //get hp back
    }

}
