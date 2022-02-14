using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;

public class Scroller:GameObject
{
    float speedX;

    public Scroller(float pSpeedX):base()
    {
        speedX = pSpeedX; 
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        x += speedX;
    }
}
