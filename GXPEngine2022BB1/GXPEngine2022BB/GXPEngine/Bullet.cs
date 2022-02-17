using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;

public class Bullet:Sprite
{
    Sound bullet;
    public Bullet(float pPosX, float pPosY) : base("daggers.png")
    {
        SetXY(pPosX, pPosY);
        bullet = new Sound("ping.wav");
        bullet.Play();
        collider.isTrigger = true;
    }

    void Update()
    {
        Move();
        if (this.y > game.height - 50) Death();
    }

    void Move()
    {
        x -= 2f;
        y += 1.2f;
    }
    
    public void Death()
    {
        this.LateDestroy();
    }
}