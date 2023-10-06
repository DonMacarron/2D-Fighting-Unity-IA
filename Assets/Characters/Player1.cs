using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : PlayerMoves
{
    protected override void Start()
    {
        base.Start();
        Horizontal = "Horizontal";
        Vertical = "Vertical";
        Jump = "Jump";
        Fire1 = "Fire1";
    }
}
