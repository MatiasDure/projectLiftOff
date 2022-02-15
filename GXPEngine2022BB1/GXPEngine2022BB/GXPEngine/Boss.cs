using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;

public class Boss:AnimationSprite
{
    bool turn;
    public Boss(float pPosX, float pPosY):base("hitbox.jpg",1,1)
    {
        x = pPosX;
        y = pPosY;
    }

    void Update()
    {
        ControlPosition();
        Move();
    }

    void Move()
    {
        if (!turn) y -= 2.0f;
        else y += 2.0f;
    }

    void ControlPosition()
    {
        if (y <= 100)
        {
            turn = true;
            Shoot();
        }
        else if (y >= game.height - 150) turn = false;
    }

    void Shoot()
    {
        game.AddChild(new Bullet(this.x,this.y));
    }
}