using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public float scrollSpeed = 1.0f;

    void Update()
    {
        // Mueve el objeto en el eje Y basado en el tiempo y la velocidad establecida
        float newPosition = Mathf.Repeat(Time.time * scrollSpeed, 1);
        transform.position = new Vector2(0, newPosition);
    }
}
