using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;
using TiledMapParser;

public class Collectable:Sprite
{
    //animation sprite
    string itemImgFile;
    int[] amountColsRows;
    AnimationSprite itemAnim;

    //item mana
    int _mana;

    public int Mana { get => _mana; private set => _mana = value; }
    

    public Collectable(TiledObject obj = null) : base("square.png")
    {
        Initialize(obj);
    }

    void Initialize(TiledObject obj = null)
    {
        if(obj != null)
        {
            itemImgFile = obj.GetStringProperty("fileName","square") + ".png";
            amountColsRows = new int[] {obj.GetIntProperty("columns",0), obj.GetIntProperty("rows",0)};
            Mana = obj.GetIntProperty("amountMana", 5);
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