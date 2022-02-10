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

    Vector2 screenPos;

    static int TypePlayer = 0;

    public int playerType = 0;

    //game physics
    float jumpForce;
    float gravity;

    //player attributes 
    int[] _attributes; //health, mana
    float velocityY;

    public int[] Attributes{ get => _attributes; set => _attributes = value; } 

    //player states
    bool isJumping;
    bool isFalling;
    bool isBoosting;
    bool isSlowing;

    //animation speed
    float animationSpeed;

    //affect speed
    int lastTimeChangedAnimSpeed = 0;
    int timeBoost = 0; 
    int lastTimeBoost = 0;
    float lastSpeed;

    //mana
    int lastTimeManaIncreased = 0;
    int lastTimeManaUsed = 0;

    //health
    int lastTimeHit = 0;

    public Player(string pFileName, int pCol, int pRow, TiledObject obj = null):base(pFileName,pCol,pRow)
    {
        playerType = TypePlayer;
        TypePlayer++;
        _attributes = new int[] { 3, 0 }; //attributes = health, mana
        jumpForce = -9.25f;
        gravity = 0.25f;
        velocityY = 0;
        collider.isTrigger = true;
    }

    virtual protected void Update()
    {
        CheckHP();
        HorizontalMovement();
        IncreaseMana();
        AnimSpeed();
        this.Animate(0.25f);
    }
    protected void HorizontalMovement()
    {
        float velocityX;
        if (isBoosting) velocityX = 3;
        else if (isSlowing) velocityX = 1;
        else velocityX = 2;

        MoveUntilCollision(velocityX,0);
    }

    protected void IncreaseMana()
    {
        if(Attributes[1] < 100 && lastTimeManaIncreased < Time.time)
        {
            lastTimeManaIncreased = Time.time + 1000;
            Attributes[1] += 2;
            Console.WriteLine(Attributes[1]);
        }
    }

    virtual protected void Ability(int pKey)
    {
        if (Input.GetKey(pKey) && lastTimeManaUsed < Time.time && Attributes[1] > 4)
        {
            Attributes[1] -= 5;
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
            if (Time.time >= timeBoost)
            {
                isBoosting = false;
                animationSpeed = lastSpeed;
            }
        }
        else if (isSlowing)
        {
            animationSpeed = 0.05f;
            if (Time.time >= timeBoost)
            {
                isSlowing = false;
                animationSpeed = lastSpeed;
            }
        }
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
        if (screenPos.x < 0 || 
            screenPos.x >= game.width - width ||
            screenPos.y < 0 || 
            screenPos.y >= game.height - height) SetCollided();
    }

    protected void SetCollided()
    {
        screenPos.x = game.width / 2;
        Console.WriteLine("hey");
    }

    void CheckHP()
    {
        if (Attributes[0] <= 0)
        {
            Console.WriteLine("You");
            ((MyGame)game).LoadLevel("1");
        } 

    }

    protected void RespawnMiddle()
    {

    }

    protected void OnCollision(GameObject pOther)
    {
        if(pOther is Collectable c)
        {
            this._attributes[1] += c.Mana;
            c.Disappear();
        }

        if(pOther is Floor f)
        {
            if(timeBoost < Time.time)
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
                lastSpeed = animationSpeed;
            }
        }

        if(pOther is Obstacle)
        {
            if(lastTimeHit < Time.time)
            {
                Attributes[0]--;
                lastTimeHit = Time.time + 1000;
            }
        }
    }
}
