using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;

public struct Vec2
{
    public float x;
    public float y;

    public Vec2(float pPositionX, float pPositionY)
    {
        this.x = pPositionX;
        this.y = pPositionY;
    }

    public void SetXY(Vec2 pOther)
    {
        this.SetXY(pOther.x, pOther.y);
    }

    public void SetXY(float pX, float pY)
    {
        this.x = pX;
        this.y = pY;
    }

    public void Add(Vec2 pOther)
    {
        this.Add(pOther.x, pOther.y);
    }

    public void Add(float pX, float pY)
    {
        this.x += pX;
        this.y += pY;
    }

    public void Sub(Vec2 pOther)
    {
        this.Sub(pOther.x, pOther.y);
    }

    public void Sub(float pX, float pY)
    {
        this.x -= pX;
        this.y -= pY;
    }

    public void Mult(Vec2 pOther)
    {
        this.Mult(pOther.x, pOther.y);
    }

    public void Mult(float pScale)
    {
        this.Mult(pScale, pScale);
    }

    public void Mult(float pScaleX, float pScaleY)
    {
        this.x *= pScaleX;
        this.y *= pScaleY;
    }

    public void Div(Vec2 pOther)
    {
        this.Div(pOther.x, pOther.y);
    }

    public void Div(float pNum)
    {
        this.Div(pNum, pNum);
    }

    public void Div(float pX, float pY)
    {
        if (pX == 0 || pY == 0) return;
        this.x /= pX;
        this.y /= pY;
    }


    public float Mag() => (float)Math.Sqrt(this.x * this.x + this.y * this.y);

    public void Normalize()
    {
        float length = this.Mag();

        if (length == 0) return;
        //Given a magnitud/length of 5
        //Using reciprocal of 5 which is 1/5 we can go from dividing 200/5 to multiplying 1/5*200
        //ex: 1 / 5 = 0.2 -> 0.2 * 200 = 40 --- 200 / 5 = 40
        this.Mult(1 / length);
    }

    public void SetMag(float pMag)
    {
        this.Normalize();
        this.Mult(pMag);
    }

    public void LimitMag(float pLimit)
    {
        float magnitude = this.Mag();
        if (magnitude > pLimit)
        {
            this.Normalize();
            this.Mult(pLimit);
        }
    }

    public float DistanceTo(Vec2 pOther)
    {
        float distanceX = pOther.x - this.x;
        float distanceY = pOther.y - this.y;
        return (float)Math.Sqrt(distanceX * distanceX + distanceY * distanceY);
    }

    static int OneOrNeg() => Utils.Random(-1, 2) >= 0 ? 1 : -1;

    public static Vec2 AddVectors(Vec2 pFirstPos, Vec2 pSecondPos) => new Vec2(pFirstPos.x + pSecondPos.x, pFirstPos.y + pSecondPos.y);

    public static Vec2 SubVectors(Vec2 pFirstPos, Vec2 pSecondPos) => new Vec2(pFirstPos.x - pSecondPos.x, pFirstPos.y - pSecondPos.y);

    public static Vec2 MultVectors(Vec2 pFirst, Vec2 pSecond) => new Vec2(pFirst.x * pSecond.x, pFirst.y * pSecond.y);

    public static Vec2 MultVectors(Vec2 pFirst, float pSecond) => new Vec2(pFirst.x * pSecond, pFirst.y * pSecond);

    public static Vec2 RandomVector() => new Vec2(OneOrNeg(), OneOrNeg());

    public static Vec2 operator +(Vec2 pLeft, Vec2 pRight) => new Vec2(pLeft.x + pRight.x, pLeft.y + pRight.y);

    public static Vec2 operator -(Vec2 pLeft, Vec2 pRight) => new Vec2(pLeft.x - pRight.x, pLeft.y - pRight.y);

}