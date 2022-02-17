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
    Sprite layout;
    //EasyDraw health;
    EasyDraw mana;
    int currentHp, framesToAnimate;
    public HUD(Player pPlayer)
    {
        player = pPlayer;
        currentHp = pPlayer.Attributes[0];
        mana = new EasyDraw(105, 10, false);
        mana.SetXY(player.playerType > 0 ? game.width - 158 : 55, 13);   
        AddChild(mana);
        lives = new AnimationSprite("heartsAnimation.png",1,13);
        lives.SetXY(player.playerType > 0 ? game.width - 100 : 50, 40);
        lives.SetScaleXY(0.15f,0.15f);
        AddChild(lives);
        if (player.playerType > 0)
        {
            layout = new Sprite("hud.png", false, false);
            AddChild(layout);
        }
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
    }

    void Mana()
    {
        mana.Clear(0, 0, 0, 100);
        mana.Fill(0, 0, 255);
        mana.Rect(0, 0, player.Attributes[1], 20);
    }
}