using System;
using GXPEngine;
using GXPEngine.Core;
using TiledMapParser;

public class Player:Sprite
{

    static int TypePlayer = 0;

    public int playerType = 0;

    //animated character
    protected AnimationSprite playerImg;

    //for hud life
    protected Sprite smallImg;

    //for respawn cloud
    protected Sprite respawnCloud;

    //character's sounds
    protected Sound[] charSounds;
    protected Sound[] objectSounds;
    protected bool collisionSoundPlayed;

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
    protected bool hasJumped;
    protected bool isFalling;
    protected bool isInjured;
    protected bool wasInjured;
    protected bool isBoosting;
    protected bool isSliding;
    protected bool readyToUseItem;
    protected bool addCloud;
    bool isSlowing;
    bool isCollidingWall;

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

    //sound player
    int lastTimePlayedSound = 0;

    //respawn cloud timer
    int timeRespawnCloud = 0;

    public Player(string pFileName,TiledObject obj = null):base(pFileName)
    {
        playerType = TypePlayer; //Determine which player each instance is, so we can manipulate them individually
        TypePlayer = TypePlayer > 0 ? 0 : +1; //Determines the amount of players in game, max 2
        _attributes = new int[] { 3, 0 }; //attributes = health, mana
        jumpForce = -9.25f; 
        gravity = 0.35f;
        collider.isTrigger = true;
        animationSpeed = 0.2f;
        respawnCloud = new Sprite("respawn.png", false, false);
        respawnCloud.alpha = 0.85f;
        respawnCloud.SetOrigin(respawnCloud.width/2,0);
        respawnCloud.SetXY(game.width/2 + 127,-14);
        respawnCloud.SetScaleXY(0.4f,0.4f);

        objectSounds = new Sound[]
        {
            new Sound("sounds/fastFloor.wav"), //when colliding with fast floor 0
            new Sound("sounds/slowFloor.wav"), //when colliding with slow floor 1
            new Sound("sounds/manaGain.wav"), //when colliding with health object 2
            new Sound("sounds/lifeGain.wav"), //when colliding with mana object 3
            new Sound("sounds/vaseBreak.wav"), //colliding with vase 4
            new Sound("sounds/swordSpear.wav"), //colliding with swords 5
            new Sound("sounds/woodPiece.wav") //colliding with wood 6
        };
        alpha = 0;
    }

    virtual protected void Update()
    {
        Edges();
        CheckRespawnCloud();    
        CheckHP();
        if (isCollidingWall || y > game.height - this.height/2) CollidedWall();
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
            animationSpeed = 0.375f;
            isBoosting = RestoreSpeed();
        }
        else if (isSlowing)
        {
            velocityX *= 0.5f;
            animationSpeed = 0.1f;
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
            lastTimeSlide = Time.time + 1500;
        }

        if(isSliding && (lastTimeSlide < Time.time || hasJumped || wasInjured))
        {
            /*if (isSliding)
            {
                hasJumped = isSliding = wasInjured = false;
                y -= 20;
                rotation = 0;
                playerImg.x = 0;
                playerImg.rotation = 0;
                lastTimeSlide = 0;
            }*/
            hasJumped = isSliding = wasInjured = false;
            y -= 20;
            rotation = 0;
            playerImg.x = 0;
            playerImg.rotation = 0;
            lastTimeSlide = 0;
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

    protected bool AbilitySet(int pKey, int pAmountManaCost, bool pAbilityCondition)
    {
        if (!Input.GetKeyDown(pKey) || 
            lastTimeManaUsed > Time.time ||
            Attributes[1] < (pAmountManaCost - 1) || 
            pAbilityCondition) return false;

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
            if (col.normal.y < 0) isJumping = isFalling = hasJumped = false;
            velocityY = 0;
        }

        if (Input.GetKeyDown(pToJump) && !isJumping)
        {
            isJumping = hasJumped = true;
            velocityY += jumpForce;
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
                isInjured = wasInjured = false;
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
        addCloud = true;
        x = scrollerPositionX;
        y = 0;
    }

    protected void CheckRespawnCloud()
    {
        if(addCloud)
        {
            game.AddChild(respawnCloud);
            addCloud = false;
            timeRespawnCloud = Time.time + 1000;
        }
        if (timeRespawnCloud <= Time.time) game.RemoveChild(respawnCloud);
    }

    void SetInjured()
    {       
        ReceiveDamage();
        isInjured = wasInjured = true;
        charSounds[1].Play(); //injured sound
    }

    void CheckHP()
    {
        if (_attributes[0] <= 0)
        {
            ((MyGame)game).LoserNr = this.playerType;
            ((MyGame)game).LoadLevel("4");
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

    virtual protected void OnCollision(GameObject pOther)
    {
        if (pOther is Player) return; 
        if(pOther is Collectable c)
        {
            switch(c.Type)
            {
                case 0:
                    if(!(Attributes[1] >= 100))Attributes[1] += c.Mana;
                    objectSounds[2].Play(); //mana sound
                    break;
                case 1:
                    if (!(Attributes[0] >= 3)) Attributes[0] += c.Health;
                    objectSounds[3].Play(); //health sound
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

        if (pOther is Obstacle o)
        {
            _ = InjuredTimer();

            if (lastTimePlayedSound < Time.time)
            {
                switch (o.Type)
                {
                    case "vase":
                        objectSounds[4].Play();
                        break;
                    case "knife":
                        objectSounds[5].Play();
                        break;
                    case "sarc":
                        objectSounds[5].Play();
                        break;
                    case "spear":
                        objectSounds[5].Play();
                        break;
                    case "weapons":
                        objectSounds[5].Play();
                        break;
                    case "wood":
                        objectSounds[6].Play();
                        break;
                }
                lastTimePlayedSound = Time.time + 1500;
            }
        }

        if (pOther is Bullet b)
        {
            if (InjuredTimer())
            {
                b.Death();
            }
        }

    }

    protected bool InjuredTimer()
    {
        if (lastTimeHit < Time.time)
        {
            SetInjured();
            lastTimeHit = Time.time + 1500;
            return true;
        }
        return false;
    }

    virtual protected void Ability(int pKey, int pAmountManaCost) { }
    
}
