using GXPEngine;
using TiledMapParser;

public class Portal : AnimationSprite
{
    string target;
    public GameObject targetObj;
    
    //portal sound
    bool soundPlaying;
    Sound sound;

    public Portal(TiledObject obj = null) : base("spritesheets/portalSpritesheet.png", 5, 2)
    {
        Initialize(obj);
    }

    void Initialize(TiledObject obj = null)
    {
        collider.isTrigger = true;
        sound = new Sound("sounds/portal.mp3");
        if (obj != null)
        {
            target = obj.GetStringProperty("target", "1");
        }
    }

    void Update()
    {
        Animate(0.12f);
        Sound();
    }

    void Sound()
    {
        if(DistanceTo(targetObj) < 300 && !soundPlaying)
        {
            soundPlaying = true;
            sound.Play();
        }
    }

    void OnCollision(GameObject other)
    {
        if (other is Player p)
        {
            ((MyGame)game).LoadLevel(target);
        }
    }
}