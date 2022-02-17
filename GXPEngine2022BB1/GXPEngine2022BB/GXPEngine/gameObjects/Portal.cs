using GXPEngine;
using TiledMapParser;


//------------------------Gate-----------------------------------//
// Inherits from Sprite to use a white hitbox image
// Contains an AnimationSprite to create the portal animation
// Creates Gate objects
// Contains a target variable which points to the next fileName which has to be loaded
// Used to travel between levels
//------------------------------------------------------------------------//

public class Portal : AnimationSprite
{
    string target;

    public Portal(TiledObject obj = null) : base("spritesheets/portalSpritesheet.png", 5, 2)
    {
        Initialize(obj);
    }

    void Initialize(TiledObject obj = null)
    {
        collider.isTrigger = true;
        if (obj != null)
        {
            target = obj.GetStringProperty("target", "1");
        }
    }

    void Update()
    {
        Animate(0.12f);
    }
    void OnCollision(GameObject other)
    {
        if (other is Player p)
        {
            ((MyGame)game).LoadLevel(target);
        }
    }
}