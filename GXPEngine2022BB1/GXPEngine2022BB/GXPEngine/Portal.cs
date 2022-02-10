using GXPEngine;
using TiledMapParser;


//------------------------Gate-----------------------------------//
// Inherits from Sprite to use a white hitbox image
// Contains an AnimationSprite to create the portal animation
// Creates Gate objects
// Contains a target variable which points to the next fileName which has to be loaded
// Used to travel between levels
//------------------------------------------------------------------------//

public class Portal : Sprite
{
    string target;

    AnimationSprite gateAnimation;

    public Portal(TiledObject obj = null) : base("square.png")
    {
        Initialize(obj);
    }

    void Initialize(TiledObject obj = null)
    {
        alpha = 0;
        SetOrigin(width / 2, height / 2);
        gateAnimation = new AnimationSprite("portal.png", 4, 1, -1, false, false);
        gateAnimation.SetOrigin(gateAnimation.width, gateAnimation.height / 2);
        gateAnimation.scaleX = 2f;
        AddChild(gateAnimation);
        collider.isTrigger = true;
        if (obj != null)
        {
            target = obj.GetStringProperty("target", "1");
        }
    }

    void Update()
    {
        gateAnimation.Animate(0.12f);
    }
    void OnCollision(GameObject other)
    {
        if (other is Player p)
        {
            ((MyGame)game).LoadLevel(target);
        }
    }
}