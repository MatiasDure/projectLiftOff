using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;

public class Boss:AnimationSprite
{
    bool turn;
    bool shoot;
    bool hasShot;
    public Boss(float pPosX, float pPosY):base("boss.png",5,6)
    {
        x = pPosX;
        y = pPosY;
        collider.isTrigger = true;
    }

    void Update()
    {
        ControlPosition();
        Move();
        Shoot();
        Animate(0.2f);
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
            shoot = true;
        }
        else if (y >= game.height - 150) turn = false;
    }

    void Shoot()
    {
        
        if(shoot)
        {
            SetCycle(10, 17);
            if (currentFrame == 14 && !hasShot)
            {
                game.AddChild(new Bullet(this.x - 28, this.y + height / 2 + 20));
                hasShot = true;
            }
            if (currentFrame == 26)
            {
                
                shoot = hasShot = false;
                SetCycle(0, 10);
            }
        }
        
    }
}