using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Player1 : PlayerMoves
{
    protected override void Start()
    {   
        
        Horizontal = "Horizontal";
        Vertical = "Vertical";
        Jump = "Jump";
        Fire1 = "Fire1";
        isFacingRight = true;
        lastFaced = true;
        initialPosition = new Vector3(-9,0,0);
        deathPosition = new Vector3(-3,10,0);
        base.Start();
    }
}
