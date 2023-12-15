using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_movement : MonoBehaviour
{
    public GameObject p1;
    public GameObject p2; 
    private Transform p1_trans;
    private Transform p2_trans;
    public Transform transform;
    public Camera camera;
    private bool over;
    private float distancia;
    public float no_biggerx;
    private float no_biggery;
    private float norma;
    public float peso_funciones = 5f;
    public float threshold_size = 5f;
    public bool kike = true;

    public float gabi_Threshold = 5.0f;
    public float gabi_cambioF = 18.0f;
    public float gabi_base = 1.8f;
    public float gabi_exp = 2.0f;

    public float kike_f = 1.3f;
    public float kike_g = 4.0f;

    void Start()
    {
        over = false;
        p1_trans = p1.transform;
        p2_trans = p2.transform;
        transform = GetComponent<Transform>();
        camera = GetComponent<Camera>();
        transform.localPosition = new Vector3(0, 0,-1);
        no_biggerx = 7.0f;
        no_biggery = 1;
    }


    void Update()
    {
        if (!over)
        {

            Vector3 new_pos = (p1_trans.position + p2_trans.position) / 2.0f + new Vector3(0,0,-1);

            float d1 = p1_trans.position.x - p2_trans.position.x;
            d1 = d1 * d1;

            float d2 = p1_trans.position.y - p2_trans.position.y;
            d2 = d2 * d2;
            d2 = d2 + d1;
            norma = Mathf.Sqrt(d2);


            if (kike)
            { 
                float x = norma / no_biggerx;

                if (x > no_biggery)
                {
                    distancia = ggg(x);
                }
                else { distancia = fff(x); }


                distancia = threshold_size + (distancia * peso_funciones);
            }
            else {
                if (norma > gabi_cambioF)
                {
                    distancia = gabi_f(norma);
                }
                else
                {
                    if (norma < gabi_Threshold) { distancia = gabi_Threshold; }
                    else { distancia = gabi_g(norma); }
                }
                
                
            
            }
            

            camera.orthographicSize = distancia ;
            transform.localPosition = new_pos;


        }
        else { transform.localPosition = new Vector3(0, 0, -1); }
    }

    public void isOver()
    {
        over = true;
    }


    //funciones de tamaño de camara
    private float fff(float x) {
        return Mathf.Pow(x, kike_f);
    }
    private float ggg(float x) {
        return Mathf.Log(x, kike_g) + 1;
    }
    private float gabi_g(float x) {
        return (Mathf.Pow(x - gabi_Threshold, 2) / (gabi_cambioF - gabi_Threshold)) + gabi_Threshold;
    }
    private float gabi_f(float x) {
        return Mathf.Log(x - gabi_cambioF + 1, gabi_base) + gabi_cambioF;
    }
}
