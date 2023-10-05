using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player2Moves1 : MonoBehaviour
{
    private float movimientoHorizontal;

    public float velocidadMovimiento = 17f; // Velocidad de movimiento lateral.
    public float fuerzaSalto = 60f; // Fuerza del salto.
    public Transform puntoVerificador; // Punto de verificación para detectar el suelo.
    public LayerMask capasDeSuelo; // Las capas que considerarás como suelo.
    public int maxSaltos = 2; // Número máximo de saltos.
    public float maxAttackCoolDown = 0.5f;
    private float attackCoolDown=0;
    private int saltosRestantes;
    private bool enSuelo = false;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        puntoVerificador = GetComponent<Transform>();
        capasDeSuelo = LayerMask.GetMask("Ground");
        saltosRestantes = maxSaltos;

    }

    private void Update()
    {
        // a ver si se mueve lateralmente
        movimientoHorizontal = Input.GetAxisRaw("Horizontalp2");


        //a ver si esta en el suelo
        enSuelo = Physics2D.OverlapCircle((puntoVerificador.position - (new Vector3(0, puntoVerificador.localScale.y / 2, 0))), 0.1f, capasDeSuelo);

        if (enSuelo)
        {
            saltosRestantes = maxSaltos;
        }

        if (Input.GetButtonDown("Jumpp2"))
        {
            Saltar();
        }
        if (Input.GetButtonDown("Fire1p2")) { Atacar1(); }

        attackCoolDown -= Time.deltaTime;
    }

    private void FixedUpdate()
    {

        // Establece la velocidad de manera instantánea en función de la entrada del 
        rb.velocity = new Vector2(movimientoHorizontal * velocidadMovimiento, rb.velocity.y);

    }

    //salto
    private void Saltar()
    {
        if (saltosRestantes >= 2)
        {
            rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);
            saltosRestantes--;
        }
        if (saltosRestantes == 1)
        {
            rb.velocity = new Vector2(rb.velocity.x, (float)(fuerzaSalto / 2.0));
            saltosRestantes--;
        }
    }


    //pegar
    private void Atacar1() {
        //IMPLEMENTAR
        if (attackCoolDown <= 0) {
            Ataque1();
            attackCoolDown = maxAttackCoolDown;
        }
    }
    private void Ataque1() {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.AddComponent<Rigidbody>();
        cube.transform.position = new Vector3(0, 0, 0);
    }
}

