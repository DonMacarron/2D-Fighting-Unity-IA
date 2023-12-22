using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player2 : PlayerMoves
{
    protected override void Start()
    {
        Horizontal = "Horizontalp2";
        Vertical = "Verticalp2";
        Jump = "Jumpp2";
        Fire1 = "Fire1p2";
        nombre = "Player 2";
        initialPosition = new Vector3(9, 0, 0);
        deathPosition = new Vector3 (3, 10, 0);
        mirandoHacia = 2;

        FlipSprite();
        isFacingRight = false;
        lastFaced = false;


        base.Start();
    }
    
}
