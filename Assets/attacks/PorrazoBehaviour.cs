using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorrazoBehaviour : MonoBehaviour
{
    public GameObject deQuienEsAtaque;
    public PlayerMoves scriptPlayer;
    public float daño;
    private int dondeMira;
    private Vector3 aSumar;
    private Vector2 direc;

    private void Start() { dondeMira = scriptPlayer.mirandoHacia;

        //derecha
        if (dondeMira == 0) { aSumar =  new Vector3(1, 0, 0); direc = new Vector3(1, 0.7f); }
        //arriba
        if (dondeMira == 1) { aSumar =  new Vector3(0, 1f, 0);
            if (scriptPlayer.isFacingRight) { direc = new Vector3(0.3f, 1); }
            else { direc = new Vector3(-0.3f,1); }
             }
        //izquierda
        if (dondeMira == 2) { aSumar = new Vector3(-1, 0, 0); direc = new Vector3(-1, 0.7f); }
        //abajo
        if (dondeMira == 3) { aSumar =  new Vector3(0, -1f, 0);
            if (scriptPlayer.isFacingRight) { direc = new Vector3(0.6f, -1.5f); }
            else { direc = new Vector3(-0.6f, -1.5f); }
        }

        daño = 20f;
        daño = scriptPlayer.ataqueDePersonaje * daño;
    }

    private void Update()
    { 
        transform.position = deQuienEsAtaque.transform.position + aSumar;
    
    }
    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {

            PlayerMoves personaje = collision.gameObject.GetComponent<PlayerMoves>();
            if (personaje != null && collision.gameObject != deQuienEsAtaque)
            {
                personaje.recibirGolpe(daño, direc);
                Destroy(this);
            }
        }
    }

    
}
