using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;

public class Parallax:GameObject
{
    public Scroller scroller;
    public Parallax()
    {
        
    }

    void Update()
    {
        if (scroller.x > 700) x -= 0.8f;
    }
}
