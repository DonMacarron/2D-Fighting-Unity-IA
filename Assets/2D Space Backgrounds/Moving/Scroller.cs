using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroller : MonoBehaviour
{
    public float speed;

    [SerializeField] private Renderer bgRenderer;

    void Update() {
        bgRenderer.material.mainTextureOffset += new Vector2(speed * Time.deltaTime, Time.deltaTime * (speed/5));
    }
}