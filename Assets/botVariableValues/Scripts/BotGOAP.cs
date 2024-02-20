using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class BotGOAP : MonoBehaviour
{
    public bool onEvolution = true;

    public const int FactNumber = 12;

    public PlayerMoves enemigo;
    public BotMoves bot;


    /*factores de decision:
     -posicion bot  0 y 1
    -posicion enemigo  2 y 3
     -distancia x  : 4
     -distancia y  :  5
     -Stunned (ambos):  6 y 7
     -Attacking (ambos) : 8 y 9
     -Jumping (ambos):  10 y 11

    GOALS:
    -Incrementar health enemigo
    -QUitar una vida al enemigo
    
    Heuristicas:
    -Estar por el centro del escenario
    -No estar en el suelo

    NO-GOALS:
    -Incrementar mi Health
    -Perder mi vida
     */


    //GOAP randomnes
    public float randHorizontal;
    public float randVertical;
    public float randJump;
    public float randAttack;

    public float[] factArray;
    
    //GOAP weight facts
    public float[] horizontalWeights = new float[FactNumber];
    public float[] verticalWeights = new float[FactNumber];
    public float[] jumpWeights = new float[FactNumber];
    public float[] attackWeights = new float[FactNumber];



    public void Awake()
    {
        factArray = new float[FactNumber];
        for (int i = 0; i < FactNumber; i++) {
            verticalWeights[i] = 1;
        }
        for (int i = 0; i < FactNumber; i++)
        {
            horizontalWeights[i] = 1;
        }
    }
    public void Update()
    {
        //facts   
        factArray[0] = bot.p_transform.localPosition.x;
        factArray[1] = bot.p_transform.localPosition.y;
        factArray[2] = enemigo.p_transform.localPosition.x;
        factArray[3] = enemigo.p_transform.localPosition.y;

        factArray[4] = factArray[0] - factArray[2];
        factArray[5] = factArray[1] - factArray[3];
        factArray[6] = bot.untouchableCoolDown;
        factArray[7] = enemigo.untouchableCoolDown;
        factArray[8] = bot.attackCoolDown;
        factArray[9] = enemigo.attackCoolDown;
        factArray[10] = bot.saltosRestantes;
        factArray[11] = enemigo.saltosRestantes;
        string x = "";
        for (int i = 0;i< FactNumber;i++) { x += factArray[i] + "xxxxx"; }
        Debug.Log(x);
    }

    public float getHorizontalMovement() {
        float x = 0;

        //pesos de likelihood  
        for (int i = 0; i < FactNumber; i++)
        {
            x += factArray[i] * horizontalWeights[i];
        }

        //factor de randomness
        float randomBeta = Random.Range(-randHorizontal, randHorizontal);
        x += randomBeta;
        float xNorm = x / Mathf.Abs(x);
        return xNorm;
    }
    public float getVerticaltalMovement()
    {
        float y = 0;

        //pesos de likelihood  
        for (int i = 0; i < FactNumber; i++) {
            y += factArray[i] * verticalWeights[i];            
        }
        //factor de randomness
        float randomBeta = Random.Range(-randVertical, randVertical);
        y += randomBeta;
        float yNorm = y / Mathf.Abs(y);
        return yNorm;
    }
    public bool getJump() {
        return true;
    }
    public bool getAttack() {
        return true;
    }

}
