using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;
using GXPEngine.Core;
using TiledMapParser;

public class Player:AnimationSprite
{

    static int TypePlayer = 0;

    public int playerType = 0;

    //game physics
    float jumpForce;
    float gravity;

    //player attributes 
    int[] _attributes; //health, mana
    float velocityY;
    float velocityX;
    public float currentLvlVelocityX;

    public int[] Attributes { get => _attributes; private set => _attributes = value; }

    //scrollers position
    public float scrollerPositionX;

    //player states
    bool isJumping;
    bool isFalling;
    bool isBoosting;
    bool isSlowing;
    bool isCollidingWall;

    //injured anim
    int lastTimeCollided;
    int[] rgb = new int[] { 255,255,255 }; //curent as hit anim

    //animation speed
    float animationSpeed;
    int lastTimeChangedAnimSpeed;

    //affect speed
    int timeBoost;
    float lastSpeed;
    int lastTimeBoosted;

    //mana
    int lastTimeManaIncreased = 0;
    int lastTimeManaUsed = 0;

    //health
    int lastTimeHit = 0;

    public Player(string pFileName, int pCol, int pRow, TiledObject obj = null):base(pFileName,pCol,pRow)
    {
        playerType = TypePlayer; //Determine which player each instance is, so we can manipulate them individually
        TypePlayer = TypePlayer > 0 ? 0 : +1; //Determines the amount of players in game, max 2
        _attributes = new int[] { 3, 0 }; //attributes = health, mana
        jumpForce = -9.25f; 
        gravity = 0.35f;
        collider.isTrigger = true;
    }

    virtual protected void Update()
    {
        Edges();
        CheckHP();
        if(lastTimeCollided <= Time.time)
        {
            rgb[1] = rgb[2] = 255;
            SetColor(rgb[0],rgb[1],rgb[2]);
        }
        if (isCollidingWall) CollidedWall();
        else HorizontalMovement();
        IncreaseMana();
        AnimSpeed();
        this.Animate(0.25f);
    }
    protected void HorizontalMovement()
    {
        velocityX = currentLvlVelocityX;
        if (isBoosting) velocityX *= 1.5f;
        else if (isSlowing) velocityX *= 0.5f;

        MoveUntilCollision(velocityX,0);
    }

    protected void IncreaseMana()
    {
        if(_attributes[1] < 100 && lastTimeManaIncreased < Time.time)
        {
            lastTimeManaIncreased = Time.time + 500;
            Attributes[1] += 2;
        }
    }

    virtual protected void Ability(int pKey, int pAmountManaCost)
    {
        if (Input.GetKey(pKey) && lastTimeManaUsed < Time.time && _attributes[1] > 4)
        {
            Attributes[1] -= pAmountManaCost;
            lastTimeManaUsed = Time.time + 2000;
        }
    }

    protected void AnimSpeed()
    {
        if (Time.time > lastTimeChangedAnimSpeed && !isBoosting && !isSlowing)
        {
            animationSpeed += 0.02f;
            lastTimeChangedAnimSpeed = Time.time + 5000;
        }
        else if (isBoosting)
        {
            animationSpeed = 0.1f;
            isBoosting = RestoreSpeed();
        }
        else if (isSlowing)
        {
            animationSpeed = 0.05f;
            isSlowing = RestoreSpeed();
        }
    }

    bool RestoreSpeed()
    {
        if(Time.time >= timeBoost)
        {
            animationSpeed = lastSpeed;
            return false;
        }
        return true;
    }

    protected void Jumping(int pToJump)
    {
        velocityY += gravity;
        Collision col = MoveUntilCollision(0, velocityY);
        if(col != null)
        {
            if (col.normal.y < 0) isJumping = isFalling = false;
            velocityY = 0;
        }

        if (Input.GetKey(pToJump) && !isJumping)
        {
            velocityY += jumpForce;
            isJumping = true;
        }
    }

    protected float SpeedTransitionManager(double pNum1, double pNum2)
    {
        return (float)Math.Min(pNum1, pNum2);
    }

    protected void Edges()
    {
        Vector2 screenPos = TransformPoint(0, 0);
        isCollidingWall = screenPos.x < 0; 
    }

    protected void CollidedWall()
    {
        SetInjured();
        x = scrollerPositionX;
        y = 0;
    }

    void SetInjured()
    {
        lastTimeCollided = Time.time + 300;
        //ReceiveDamage();
        rgb[1] = rgb[2] = 0; //sets red
        SetColor(rgb[0],rgb[1],rgb[2]); 
    }

    void CheckHP()
    {
        if (_attributes[0] <= 0)
        {
            ((MyGame)game).LoadLevel("1");
        } 
    }

    void ReceiveDamage()
    {
        Attributes[0]--;
    }

    protected void OnCollision(GameObject pOther)
    {
        if(pOther is Collectable c)
        {
            Attributes[1] += c.Mana;
            c.Disappear();
        }

        if(pOther is Floor f)
        {
            if(lastTimeBoosted < Time.time)
            {
                switch(f.type)
                {
                    case "fast":
                        isBoosting = true;
                        break;
                    case "slow":
                        isSlowing = true;
                        break;
                }
                timeBoost = Time.time + 1000;
                lastTimeBoosted = timeBoost + 1000;
                lastSpeed = animationSpeed;
            }
        }

        if(pOther is Obstacle)
        {
            if(lastTimeHit < Time.time)
            {
                SetInjured();
                lastTimeHit = Time.time + 1500;
            }
        }
    }
}
