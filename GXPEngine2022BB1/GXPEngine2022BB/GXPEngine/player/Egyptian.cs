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
            new Sound("sounds/greekAbility.wav"),
            new Sound("sounds/egyptHurt.wav")
        };
    }
    protected override void Update()
    {
        CheckMana(30);
        Sliding(Key.O);
        Jumping(Key.I);
        Ability(Key.SPACE, 30);
        base.Update();
    }

    protected override void Ability(int pKey, int pAmountManaCost)
    {
        if (!AbilitySet(pKey, pAmountManaCost) || Attributes[0] == 3) return;
        Attributes[0]++; //get hp back
    }

    protected override void AnimCycleSetter()
    {
        if (isInjured)
        {
            animationSpeed = 0.08f;
            playerImg.SetCycle(12, 2); //injured animation
            if (playerImg.currentFrame == 13)
            {
                isInjured = false;
                animationSpeed = 0.2f;
            }
        }
        else if (isSliding)
        {
            if (playerImg.currentFrame == 33) playerImg.SetCycle(33, 1);
            else playerImg.SetCycle(30, 4); //sliding animation
        }

        else if (isFalling) playerImg.SetCycle(31, 1); //falling animation
        else if (isJumping) playerImg.SetCycle(30, 1); //jumping animation        
        else if (readyToUseItem) playerImg.SetCycle(20, 10); //Has ability at hand
        else playerImg.SetCycle(0, 10); //normal running animation
    }

}
