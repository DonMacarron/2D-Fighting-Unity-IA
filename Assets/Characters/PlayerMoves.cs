using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerMoves : MonoBehaviour
{
    public string Horizontal;
    public string Vertical;
    public string Jump;
    public string Fire1;
    public Animator animator;

    private float movimientoHorizontal;
    private float movimientoVertical;
    public string nombre;

    //0=derecha  1= arriba  2 = izquierda  3 = derecha
    public byte mirandoHacia;
    private byte lastIzqDer;

    public int vidasIniciales = 3;
    public int vidasRestantes;
    public float dañoAcumulado;
    public float ataqueDePersonaje = 1f;
    public float velocidadMovimiento = 17f; // Velocidad de movimiento lateral.
    public float fuerzaSalto = 33f; // Fuerza del salto.
    public Transform puntoVerificador; // Punto de verificación para detectar el suelo.
    public LayerMask capasDeSuelo;
    public LayerMask capasDeJugador;
    public int maxSaltos = 2; // Número máximo de saltos.
    public float maxAttackCoolDown = 1f;
    public GameObject porrazoPrefab;
    public float untouchableCoolDown;
    public float minUntouchableCoolDown=0.2f;
    private float attackCoolDown;
    private int saltosRestantes;
    private bool enSuelo = false;
    private Rigidbody2D rb;
    private float jumpCoolDown;
    private Collider2D otherPlayerCollider;
    private SpriteRenderer spriteRenderer;
    public bool isFacingRight;
    protected bool lastFaced;

    protected virtual void Start()
    {
        jumpCoolDown = 0.02f;
        dañoAcumulado = 40;
        mirandoHacia = 0;
        rb = GetComponent<Rigidbody2D>();
        puntoVerificador = GetComponent<Transform>();
        capasDeSuelo = LayerMask.GetMask("Ground");
        capasDeJugador = LayerMask.GetMask("Player");
        saltosRestantes = maxSaltos;
        nombre = "elbueno";
        vidasRestantes = vidasIniciales;
        attackCoolDown = 0;
        spriteRenderer = GetComponent<SpriteRenderer>();

}

private void Update()
    {
      
        
        // a ver si se mueve lateralmente
        movimientoHorizontal = Input.GetAxisRaw(Horizontal);
        if (movimientoHorizontal == 1) { mirandoHacia = 0; lastIzqDer = 0; }
        if (movimientoHorizontal == -1) { mirandoHacia = 2; lastIzqDer = 2; }
        movimientoVertical = Input.GetAxisRaw(Vertical);
        if (movimientoVertical == 1) { mirandoHacia = 1; }
        if (movimientoVertical == -1) { mirandoHacia = 3; }
        if (movimientoHorizontal == 0 && movimientoVertical == 0) { mirandoHacia = lastIzqDer; }

        //a ver si esta en el suelo
        enSuelo =  Physics2D.OverlapBox(
        new Vector2(puntoVerificador.position.x, puntoVerificador.position.y - 0.06f),
        new Vector2(Math.Abs(puntoVerificador.localScale.x) + 0.03f, Math.Abs(puntoVerificador.localScale.y) + 0.03f),
        0, 
        capasDeSuelo);

        Vector2 raycastOrigin = new Vector2(puntoVerificador.position.x, puntoVerificador.position.y - (puntoVerificador.localScale.y / 2) - 3f);
        Vector2 raycastDirection = -Vector2.up;
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, raycastDirection, 0.01f, capasDeJugador);
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player"))
            {
                enSuelo = true;
            }
        }


        if (enSuelo)
        {
            saltosRestantes = maxSaltos;
        }
        else { if (saltosRestantes > 1) { saltosRestantes = 1; } }

        if (Input.GetButtonDown(Jump) && untouchableCoolDown - (untouchableCoolDown / 2f) <= 0  && jumpCoolDown<0)
        {
            Saltar();
        }
        if (Input.GetButtonDown(Fire1) && untouchableCoolDown - (untouchableCoolDown / 2f) <= 0  &&  attackCoolDown<0) { Atacar1(); }

        attackCoolDown -= Time.deltaTime;
        untouchableCoolDown -= Time.deltaTime;
        jumpCoolDown -= Time.deltaTime;
        if (Input.GetKeyDown("r")) { Restart(); }


        //animaciones
        if (movimientoHorizontal > 0.5)
        {
            animator.SetBool("IsFacingLeft", false);
            animator.SetBool("Run_ing", true);
            isFacingRight = true;
            if (lastFaced != isFacingRight) { FlipSprite(); }
            lastFaced = true;
        }
        else
        {
            if (movimientoHorizontal < -0.5)
            {
                animator.SetBool("IsFacingLeft", true);
                animator.SetBool("Run_ing", true);
                isFacingRight = false;
                if (lastFaced != isFacingRight) { FlipSprite(); }
                lastFaced = false;
            }
            else { animator.SetBool("Run_ing", false); }
        }
        animator.SetFloat("Up_Down", rb.velocity.y);
        animator.SetBool("On_Air", !enSuelo);

        
        animator.SetBool("Stunned_ing", untouchableCoolDown > 0);
        

        
    }

    private void FixedUpdate()
    {
        //Mathf.Abs(rb.velocity.x)-0.5f > velocidadMovimiento  ||
        if((untouchableCoolDown)>0) { rb.AddForce(new Vector3(movimientoHorizontal*(velocidadMovimiento * 0.3f * (dañoAcumulado * 0.001f)), 0, 0), ForceMode2D.Impulse);}
        else { 
            rb.velocity = new Vector2(movimientoHorizontal * velocidadMovimiento, rb.velocity.y);
        }
        

    }

    //salto
    private void Saltar()
    {
        if (saltosRestantes >= 2)
        {
            rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);
            animator.SetTrigger("Jump_ing");
        }
        if (saltosRestantes == 1)
        {
            rb.velocity = new Vector2(rb.velocity.x, (float)(fuerzaSalto));
            animator.SetTrigger("Double_Jump_ing");
        }
        saltosRestantes--;
        jumpCoolDown = 0.05f;
    }


    //pegar
    private void Atacar1() {
        
            Ataque1();
            attackCoolDown = maxAttackCoolDown;
        
    }
    private void Ataque1() {
        animator.SetBool("On_Attack", true);
        animator.SetTrigger("Attack_ing");
        GameObject proyectil = Instantiate(porrazoPrefab, puntoVerificador.position, puntoVerificador.rotation);
        PorrazoBehaviour proyectilScript = proyectil.GetComponent<PorrazoBehaviour>();
        proyectilScript.deQuienEsAtaque = this.gameObject;
        proyectilScript.scriptPlayer = this;

        
        StartCoroutine(DestroyProjectile(proyectil));
    }

    IEnumerator DestroyProjectile(GameObject proyectilToDestroy)
    {
        float startTime = Time.time; // Guarda el tiempo actual

        while (Time.time - startTime < 0.2f)
        {
            // Verifica si stunned
            if (untouchableCoolDown > 0)
            {
                // Si la variable es verdadera, destruye el proyectil y sale del bucle
                animator.SetBool("On_Attack", false);
                Destroy(proyectilToDestroy);
                yield break; // Sale de la corutina
            }

            // Espera hasta la siguiente iteración del bucle
            yield return null;
        }
        animator.SetBool("On_Attack", false);
        Destroy(proyectilToDestroy);
    }
    public void recibirGolpe(float daño, Vector2 direccion) {
        attackCoolDown = 0;
        rb.velocity = new Vector2(0, 0);

        dañoAcumulado += daño;
        direccion.Normalize();
        rb.AddForce(direccion * dañoAcumulado, ForceMode2D.Impulse);
        untouchableCoolDown = minUntouchableCoolDown + (dañoAcumulado*0.0008f);

        //animator
        animator.SetTrigger("Hit");

    }
    public void Restart() {
        transform.position = new Vector3(0,0,0);
        FlipSprite();
        Start();
    }

    public void eliminarVida() {
        vidasRestantes -= 1;
        if (vidasRestantes <= 0) {
            perder();
        }
        Restart();
    }
    public void perder() { }



    public void FlipSprite()
    {  
        Vector2 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

    }
}

