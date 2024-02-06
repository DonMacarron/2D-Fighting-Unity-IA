using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BotGOAP : MonoBehaviour
{
    public PlayerMoves jugador;
    public BotMoves bot;

    public float distanceX;
    public float distanceY;

    //GOAP data
    public void Update()
    {
        distanceX = jugador.p_transform.localPosition.x - bot.p_transform.localPosition.x;
        distanceY = jugador.p_transform.localPosition.y - bot.p_transform.localPosition.y;
    }

    public float getHorizontalMovement() {
        int x = Random.Range(-1, 2);
        Debug.Log(x);
        return x ;
    }
    public float getVerticaltalMovement()
    {
        int x = Random.Range(-1, 2);
        Debug.Log(x);
        return x;
    }
    public bool getJump() {
        return true;
    }
    public bool getAtack() {
        return true;
    }

}
