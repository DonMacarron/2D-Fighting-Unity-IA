using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorrazoBehaviour : MonoBehaviour
{
    public GameObject deQuienEsAtaque;
    public float da�o;

    private void Update() { transform.position = deQuienEsAtaque.transform.position; }
}
