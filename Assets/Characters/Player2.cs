using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : PlayerMoves
{
    protected override void Start()
    {
        base.Start();
        Horizontal = "Horizontalp2";
        Vertical = "Verticalp2";
        Jump = "Jumpp2";
        Fire1 = "Fire1p2";

    }
    
}
