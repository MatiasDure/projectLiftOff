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
    const int HEALTH = 1;

    //animation sprite
    string itemImgFile;
    int[] amountColsRows;
    AnimationSprite itemAnim;

    //type of collectable health || mana
    int _type;

    public int Type { get => _type; private set => _type = value; }



    //item mana
    int _mana;
    int _health;

    public int Mana { get => _mana; private set => _mana = value; }
    public int Health { get => _health; private set => _health = value; }

    public Collectable(TiledObject obj = null) : base("hitbox.jpg")
    {
        Initialize(obj);
    }

    void Initialize(TiledObject obj = null)
    {
        if(obj != null)
        {
            itemImgFile = obj.GetStringProperty("fileName","hitbox") + ".jpg";
            amountColsRows = new int[] {obj.GetIntProperty("columns",1), obj.GetIntProperty("rows",1)};
            Type = obj.GetIntProperty("type",0);

            _ = Type == Mana ? Mana = obj.GetIntProperty("amountMana", 5) : Health = obj.GetIntProperty("amountHealth", 1); 
            itemAnim = new AnimationSprite(itemImgFile,amountColsRows[0],amountColsRows[1],-1,false,false);
            itemAnim.SetOrigin(this.width/2,this.height/2);
            AddChild(itemAnim);
        }

        this.alpha = 100;
        this.collider.isTrigger = true;
    }

    void Update()
    {
        itemAnim.Animate(0.2f);
    }

    public void Disappear()
    {
        this.LateDestroy();
    }
}