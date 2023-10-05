using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorrazoBehaviour : MonoBehaviour
{
    public GameObject deQuienEsAtaque;
    public float daño;

    private void Update() { transform.position = deQuienEsAtaque.transform.position; }
}
