using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;
using TiledMapParser;

public class Greek:Player
{
    
    public Greek(TiledObject obj = null):base("hitbox.jpg")
    {
        playerImg = new AnimationSprite("greekCharSpritesheet.png", 5, 7, -1, false, false);
        playerImg.SetOrigin(this.width / 2 + 25, this.height / 2 + 5);
        AddChild(playerImg);
        charSounds = new Sound[] 
        { 
            new Sound("sounds/greekAbility.wav"),
            new Sound("sounds/greekHurt.wav")
        };
    }

    protected override void Update()
    {
        CheckMana(25);
        Sliding(Key.Q);
        Jumping(Key.W);
        Ability(Key.E,25);
        base.Update();
    }

    protected override void Ability(int pKey, int pAmountManaCost)
    {
        if (!AbilitySet(pKey, pAmountManaCost, isBoosting)) return;
        charSounds[0].Play();
        isBoosting = true;
        timeBoost = Time.time + 800;
    }

}
