using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;

public class HUD:GameObject
{
    Player player;
    EasyDraw health;
    EasyDraw mana;
    public HUD(Player pPlayer)
    {
        player = pPlayer;
        health = new EasyDraw(25, 10, false);
        mana = new EasyDraw(50, 10, false);
        mana.SetXY(player.playerType > 0 ? game.width - 70:0, 20);
        health.SetXY(player.playerType > 0 ? game.width - 70 : 0, 0);
        AddChild(health);   
        AddChild(mana);
    }

    void Update()
    {
        Health();
        Mana();
    }

    void Health()
    {
        health.Clear(255, 0, 0);
        health.Fill(0, 255, 0);
        health.Rect(0, 0, player.Attributes[0] * 17, 20);
    }

    void Mana()
    {
        mana.Clear(255, 0, 0);
        mana.Fill(0, 0, 255);
        mana.Rect(0, 0, player.Attributes[1], 20);
    }
}