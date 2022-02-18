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
    bool disappearAnimGone;
    Sound[] appearDis;
    AnimationSprite disappear;
    int lifeTime;


    public Boss(float pPosX, float pPosY):base("boss.png",5,6)
    {
        appearDis = new Sound[] { new Sound("sounds/bossAppear.wav"), new Sound("sounds/bossDisappear.wav") };
        x = pPosX;
        y = pPosY;
        collider.isTrigger = true;
        lifeTime = Time.time + 15000;
        disappear = new AnimationSprite("disappearParticle.png", 4, 2, -1, false, false);
        AddChild(disappear);
        disappear.alpha = 0;
    }

    void Update()
    {
        ControlPosition();
        Move();
        Shoot();
        Animate(0.2f);
        if(Time.time > lifeTime) Death();
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
            appearDis[0].Play();
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

    void Death()
    {
        if (!disappearAnimGone)
        {
            appearDis[1].Play();
            disappear.alpha = 1;
            this.alpha = 0;
            disappearAnimGone = true;
        }
        disappear.Animate(0.15f);
        if(disappear.currentFrame == 7) this.Destroy(); 
    }
}