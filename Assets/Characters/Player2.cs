using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player2 : PlayerMoves
{
    protected override void Start()
    {
        base.Start();
        Horizontal = "Horizontalp2";
        Vertical = "Verticalp2";
        Jump = "Jumpp2";
        Fire1 = "Fire1p2";

        //empieza mirando a la izquierda
        FlipSprite();
        isFacingRight = false;
        lastFaced = false;

    }
    
}
