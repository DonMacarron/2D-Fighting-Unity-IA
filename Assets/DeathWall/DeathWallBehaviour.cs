using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathWallBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnCollisionEnter2D(Collision2D coll) {
        if (coll.gameObject.CompareTag("Player"))
        {

            PlayerMoves personaje = coll.gameObject.GetComponent<PlayerMoves>();
            if (personaje != null)
            {
                personaje.eliminarVida();
            }
        }
    }
}
