using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;
using TiledMapParser;

public class Collectable:Sprite
{

    //collectable types
    const int MANA = 0;

    AnimationSprite itemAnim;

    //type of collectable health || mana && level
    int _type;
    int level;

    public int Type { get => _type; private set => _type = value; }

    //item mana
    int _mana;
    int _health;

    public int Mana { get => _mana; private set => _mana = value; }
    public int Health { get => _health; private set => _health = value; }

    public Collectable(TiledObject obj = null) : base("hitboxItems.jpg")
    {
        Initialize(obj);
    }

    void Initialize(TiledObject obj = null)
    {
        if(obj != null)
        {
            string fileName;

            Type = obj.GetIntProperty("type", 0);
            level = obj .GetIntProperty("level", 1);

            if (Type == MANA)
            {
                fileName = "mana" + level;
                Mana = obj.GetIntProperty("amountMana", 5);
            }
            else
            {
                fileName = "health";
                Health = obj.GetIntProperty("amountHealth", 1);
            }

            fileName = "spriteSheets/" + fileName + "Spritesheet.png";

            try { itemAnim = new AnimationSprite(fileName, 5, 4, -1,false,false); } catch (Exception ex) { Console.WriteLine(fileName); }
            itemAnim.SetOrigin(this.width/2,this.height/2);
            AddChild(itemAnim);
            itemAnim.SetCycle(0, 16);
        }

        this.alpha = 0;
        this.collider.isTrigger = true;
    }

    void Update()
    {
        itemAnim.Animate(0.45f);
    }

    public void Disappear()
    {
        this.LateDestroy();
    }
}