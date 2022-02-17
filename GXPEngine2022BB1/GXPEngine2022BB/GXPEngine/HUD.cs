using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;

public class HUD:GameObject
{
    Player player;
    AnimationSprite lives;
    //EasyDraw health;
    EasyDraw mana;
    int currentHp, framesToAnimate;
    public HUD(Player pPlayer)
    {
        player = pPlayer;
        currentHp = pPlayer.Attributes[0];
        //health = new EasyDraw(50, 10, false);
        mana = new EasyDraw(50, 10, false);
        mana.SetXY(player.playerType > 0 ? game.width - 70 : 20, 40);
        //health.SetXY(player.playerType > 0 ? game.width - 70 : 20, 0);
        //AddChild(health);   
        AddChild(mana);
        lives = new AnimationSprite("heartsAnimation.png",1,13);
        lives.SetXY(player.playerType > 0 ? game.width - 70 : 20, 20);
        lives.SetScaleXY(0.15f,0.15f);
        AddChild(lives);
    }

    void Update()
    {
        Health();
        Mana();
    }

    void Health()
    {
        if (framesToAnimate > 12) framesToAnimate = 0;
        if (currentHp > player.Attributes[0])
        {
            currentHp = player.Attributes[0];
            framesToAnimate += 4;
        }
        if (framesToAnimate > lives.currentFrame)
        {
            lives.Animate(0.12f);
        }
        //health.Clear(255, 0, 0);
        //health.Fill(0, 255, 0);
        //health.Rect(0, 0, player.Attributes[0] * 33, 20);
    }

    void Mana()
    {
        mana.Clear(255, 0, 0);
        mana.Fill(0, 0, 255);
        mana.Rect(0, 0, player.Attributes[1], 20);
    }
}