using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;
using GXPEngine.Core;
using TiledMapParser;

public class Player:Sprite
{

    static int TypePlayer = 0;

    public int playerType = 0;

    //animated character
    protected AnimationSprite playerImg;

    //character's sounds
    protected Sound[] charSounds;
    Sound[] objectSounds;

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
    protected bool isJumping;
    protected bool isFalling;
    protected bool isInjured;
    protected bool isBoosting;
    protected bool isSliding;
    protected bool readyToUseItem;
    bool isSlowing;
    bool isCollidingWall;
    

    //injured anim
    int lastTimeCollided;

    //animation speed
    protected float animationSpeed;

    //sliding
    int lastTimeSlide = 0;

    //affect speed
    protected int timeBoost;
    float lastSpeed;
    protected int lastTimeBoosted;

    //mana
    int lastTimeManaIncreased = 0;
    int lastTimeManaUsed = 0;

    //health
    int lastTimeHit = 0;

    public Player(string pFileName,TiledObject obj = null):base(pFileName)
    {
        playerType = TypePlayer; //Determine which player each instance is, so we can manipulate them individually
        TypePlayer = TypePlayer > 0 ? 0 : +1; //Determines the amount of players in game, max 2
        _attributes = new int[] { 3, 0 }; //attributes = health, mana
        jumpForce = -9.25f; 
        gravity = 0.35f;
        collider.isTrigger = true;
        animationSpeed = 0.2f;
        objectSounds = new Sound[]
        {
            new Sound("sounds/fastFloor.wav"), //when colliding with fast floor 0
            new Sound("sounds/slowFloor.wav"), //when colliding with slow floor 1
            new Sound("sounds/manaGain.wav"), //when colliding with health object 2
            new Sound("sounds/lifeGain.wav"), //when colliding with mana object 3
        };
    }

    virtual protected void Update()
    {
        Edges();
        CheckHP();
        if (isCollidingWall) CollidedWall();
        else HorizontalMovement();
        IncreaseMana();
        AnimCycleSetter();
        playerImg.Animate(animationSpeed);
    }
    protected void HorizontalMovement()
    {
        velocityX = currentLvlVelocityX;
        if (isBoosting)
        {
            velocityX *= 1.5f;
            isBoosting = RestoreSpeed();
        }
        else if (isSlowing)
        {
            velocityX *= 0.5f;
            isSlowing = RestoreSpeed();
        }
        MoveUntilCollision(velocityX,0);
    }

    protected void Sliding(int pKey)
    {
        if(Input.GetKeyDown(pKey) && !isSliding)
        {
            isSliding = true;
            playerImg.rotation = 90;
            playerImg.x = 30;
            this.rotation =  -90f;
            lastTimeSlide = Time.time + 1000;
        }
        if (isSliding && lastTimeSlide < Time.time )
        {
            y = game.height - 50;
            isSliding = false;
            rotation = 0;
            playerImg.x = 0;
            playerImg.rotation = 0;
        }
    }

    protected void IncreaseMana()
    {
        if(_attributes[1] < 100 && lastTimeManaIncreased < Time.time)
        {
            lastTimeManaIncreased = Time.time + 500;
            Attributes[1] += 2;
        }
    }

    protected bool AbilitySet(int pKey, int pAmountManaCost)
    {
        if (!Input.GetKeyDown(pKey) || 
            lastTimeManaUsed > Time.time ||
            Attributes[1] < (pAmountManaCost - 1)) return false;

        Attributes[1] -= pAmountManaCost;
        lastTimeManaUsed = Time.time + 2000;
        
        return true;
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
        isFalling = velocityY > 0 && isJumping;
        Collision col = MoveUntilCollision(0, velocityY);
        if(col != null)
        {
            if (col.normal.y < 0) isJumping = isFalling = false;
            velocityY = 0;
        }

        if (Input.GetKeyDown(pToJump) && !isJumping && !isSliding)
        {
            velocityY += jumpForce;
            isJumping = true;
        }
    }

    virtual protected void AnimCycleSetter()
    {
        if (isInjured)
        {
            animationSpeed = 0.08f;
            playerImg.SetCycle(12, 2); //injured animation
            if (playerImg.currentFrame == 13)
            {
                isInjured = false;
                animationSpeed = 0.2f;
            }
        }
        else if (isSliding)
        {
            if (playerImg.currentFrame == 33) playerImg.SetCycle(33, 1);
            else playerImg.SetCycle(30, 4); //sliding animation
        }

        else if (isFalling) playerImg.SetCycle(31, 1); //falling animation
        else if (isJumping) playerImg.SetCycle(30, 1); //jumping animation        
        else if (readyToUseItem) playerImg.SetCycle(20, 10); //Has ability at hand
        else playerImg.SetCycle(0, 10); //normal running animation
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
        lastTimeCollided = Time.time + 400;
        ReceiveDamage();
        isInjured = true;
        lastTimeSlide = 0;
        charSounds[1].Play(); //injured sound
    }

    void CheckHP()
    {
        if (_attributes[0] <= 0)
        {
            ((MyGame)game).LoadLevel("1");
        } 
    }

    protected void CheckMana(int pAmountMana)
    {
        readyToUseItem = Attributes[1] >= pAmountMana; 
    }

    void ReceiveDamage()
    {
        Attributes[0]--;
    }

    protected void OnCollision(GameObject pOther)
    {
        if(pOther is Collectable c)
        {
            switch(c.Type)
            {
                case 0:
                    Attributes[1] += c.Mana;
                    objectSounds[2].Play();
                    break;
                case 1:
                    Attributes[0] += c.Health;
                    objectSounds[3].Play();
                    break;
                default:
                    Console.WriteLine("type not found");
                    break;
            }
            
            c.Disappear();
        }

        if(pOther is Floor f)
        {
            if(lastTimeBoosted < Time.time)
            {
                switch(f.Type)
                {
                    case "fast":
                        isBoosting = true;
                        objectSounds[0].Play(); 
                        break;
                    case "slow":
                        isSlowing = true;
                        objectSounds[1].Play();
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

    virtual protected void Ability(int pKey, int pAmountManaCost) { }
    
}
