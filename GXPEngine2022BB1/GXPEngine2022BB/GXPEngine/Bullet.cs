using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;

public class Bullet:Sprite
{
    public Bullet(float pPosX, float pPosY) : base("portal.png")
    {
        SetXY(pPosX, pPosY);
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
    
    void Death()
    {
        Console.WriteLine("hey");
        this.LateDestroy();
    }
}