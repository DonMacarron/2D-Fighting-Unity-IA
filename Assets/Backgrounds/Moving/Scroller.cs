using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Scroller : MonoBehaviour
{
    public float speed;

    [SerializeField] new private Renderer renderer;

    void Update() {
        renderer.material.mainTextureOffset += new Vector2(speed * Time.deltaTime,0);
    }
}