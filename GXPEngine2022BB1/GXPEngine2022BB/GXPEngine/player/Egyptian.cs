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
        playerImg = new AnimationSprite("egyptCharSpriteSheet.png", 5 , 7, -1, false, false);
        playerImg.SetOrigin(this.width / 2 + 25, this.height / 2 + 5);
        AddChild(playerImg);
        charSounds = new Sound[]
        {
            new Sound("sounds/lifeGain.wav"),
            new Sound("sounds/egyptHurt.wav")
        };
    }

    protected override void Update()
    {
        CheckMana(30);
        Sliding(Key.O);
        Jumping(Key.P);
        Ability(Key.I, 30);
        base.Update();
    }

    protected override void Ability(int pKey, int pAmountManaCost)
    {
        if (!AbilitySet(pKey, pAmountManaCost, Attributes[0] >= 3)) return;
        charSounds[0].Play();
        Attributes[0]++; //get hp back
    }

}
