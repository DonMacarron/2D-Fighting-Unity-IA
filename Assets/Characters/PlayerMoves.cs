using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMoves : MonoBehaviour
{
    public Game_manager gameManager;

    public string Horizontal;
    public string Vertical;
    public string Jump;
    public string Fire1;
    public Animator animator;

    public float movimientoHorizontal;
    public float movimientoVertical;
    public string nombre;

    //0=derecha  1= arriba  2 = izquierda  3 = derecha
    public byte mirandoHacia;

    public float dañoInicial = 40f;
    public float dañoAcumulado;
    public float ataqueDePersonaje = 1f;
    public float velocidadMovimiento = 17f; // Velocidad de movimiento lateral.
    public float fuerzaSalto = 33f; // Fuerza del salto.
    public Transform p_transform; // Punto de verificación para detectar el suelo.
    public LayerMask capasDeSuelo;
    public LayerMask capasDeJugador;
    public int maxSaltos = 2; // Número máximo de saltos.
    public float maxAttackCoolDown = 1f;
    public GameObject porrazoPrefab;
    public float untouchableCoolDown;
    public float minUntouchableCoolDown=0.35f;
    public float attackCoolDown;
    public int jumpsLeft;
    public bool enSuelo = false;
    public Rigidbody2D rb;
    public float jumpCoolDown;
    public Collider2D otherPlayerCollider;
    public bool isFacingRight;
    public bool lastFaced;
    public Vector3 initialPosition;
    public Vector3 deathPosition;
    public TextMeshProUGUI healthText;
    protected virtual void Start()
    {
        jumpCoolDown = 0.02f;
        dañoAcumulado = dañoInicial;
        rb = GetComponent<Rigidbody2D>();
        p_transform = GetComponent<Transform>();
        capasDeSuelo = LayerMask.GetMask("Ground");
        capasDeJugador = LayerMask.GetMask("Player");
        jumpsLeft = maxSaltos;
        attackCoolDown = 0;
        minUntouchableCoolDown = 0.35f;
        //inicializar contador de vidas
        gameManager.playerJoins(nombre);

        //posicion inicial
        p_transform.position = initialPosition;
        healthText.text = "0";
}

    private void Update()
    {

        // a ver si se mueve lateralmente
        movimientoHorizontal = Input.GetAxisRaw(Horizontal);
        if (movimientoHorizontal == 1) { mirandoHacia = 0; }
        if (movimientoHorizontal == -1) { mirandoHacia = 2; }
        movimientoVertical = Input.GetAxisRaw(Vertical);
        if (movimientoVertical == 1) { mirandoHacia = 1; }
        if (movimientoVertical == -1) { mirandoHacia = 3; }
        if (movimientoHorizontal == 0 && movimientoVertical == 0) {
            if (lastFaced) { mirandoHacia = 0; }
            else { mirandoHacia = 2; }
        }

        //a ver si esta en el suelo
        enSuelo =  Physics2D.OverlapBox(
        new Vector2(p_transform.position.x, p_transform.position.y - 0.06f),
        new Vector2(Math.Abs(p_transform.localScale.x) + 0.03f, Math.Abs(p_transform.localScale.y) + 0.03f),
        0, 
        capasDeSuelo);

        Vector2 raycastOrigin = new Vector2(p_transform.position.x, p_transform.position.y - (p_transform.localScale.y / 2) - 3f);
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
            jumpsLeft = maxSaltos;
        }
        else { if (jumpsLeft > 1) { jumpsLeft = 1; } }

        if (Input.GetButtonDown(Jump) && jumpCoolDown<0)
        {
            Saltar();
        }
        if (Input.GetButtonDown(Fire1) && attackCoolDown<0 && untouchableCoolDown - (untouchableCoolDown / 2f) <= 0) { 
            Atacar1();
        }

        if (attackCoolDown > -1)
            attackCoolDown -= Time.deltaTime;
        if (untouchableCoolDown > -1)
            untouchableCoolDown -= Time.deltaTime;
        if (jumpCoolDown > -1)
            jumpCoolDown -= Time.deltaTime;

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
        if (jumpsLeft >= 2)
        {
            rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);
            animator.SetTrigger("Jump_ing");
        }
        if (jumpsLeft == 1)
        {
            rb.velocity = new Vector2(rb.velocity.x, (float)(fuerzaSalto));
            animator.SetTrigger("Double_Jump_ing");
        }
        jumpsLeft--;
        jumpCoolDown = 0.05f;
    }


    //pegar
    private void Atacar1() {
        
            Ataque1();
            attackCoolDown = maxAttackCoolDown;
        
    }
    protected virtual void Ataque1() {
        int rotateWithAttack = 0;
        if (mirandoHacia == 1) { rotateWithAttack = 90;
            if (!isFacingRight) { rotateWithAttack = -rotateWithAttack; }
            transform.Rotate(0f, 0f, rotateWithAttack);
        }
        if (mirandoHacia == 3)
        {
            rotateWithAttack = 270;
            if (!isFacingRight) { rotateWithAttack = -rotateWithAttack; }
            transform.Rotate(0f, 0f, rotateWithAttack);
        }

        animator.SetBool("face_vertical_ing",mirandoHacia == 1 || mirandoHacia == 3);
        animator.SetBool("On_Attack", true);
        animator.SetTrigger("Attack_ing");
        GameObject proyectil = Instantiate(porrazoPrefab, p_transform.position, p_transform.rotation);
        PorrazoBehaviour proyectilScript = proyectil.GetComponent<PorrazoBehaviour>();
        proyectilScript.deQuienEsAtaque = this.gameObject;
        proyectilScript.scriptPlayer = this;

        
        StartCoroutine(DestroyProjectile(proyectil, -rotateWithAttack));
    }

    protected IEnumerator DestroyProjectile(GameObject proyectilToDestroy, int antiRotate)
    {
        float startTime = Time.time; // Guarda el tiempo actual

        while (Time.time - startTime < 0.2f)
        {
            // Verifica si stunned
            if (untouchableCoolDown > 0)
            {
                // Si la variable es verdadera, destruye el proyectil y sale del bucle
                transform.Rotate(0f, 0f, antiRotate);
                animator.SetBool("On_Attack", false);
                Destroy(proyectilToDestroy);
                yield break; // Sale de la corutina
            }

            // Espera hasta la siguiente iteración del bucle
            yield return null;
        }
        transform.Rotate(0f, 0f, antiRotate);
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

        //vida en pantalla
        healthText.text = ""+ (dañoAcumulado - dañoInicial);

    }
    public void Restart() {
        transform.position = deathPosition;
        rb.velocity = new Vector2(0, 0);
        dañoAcumulado = dañoInicial;
        if (nombre.Equals("p2")) { mirandoHacia = 2; }
        //cancela animacion cuando muere
        animator.SetTrigger("Death");

        //vida en pantalla
        healthText.text = "0";
    }

    public virtual void perderVida() {
        BotMoves enemybot = otherPlayerCollider.gameObject.GetComponent<BotMoves>();

        if ( enemybot!= null) {
            enemybot.enemyKilled();
        }

        gameManager.vidaMenos(nombre);
        Restart();
    }



    public void FlipSprite()
    {  
        Vector2 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

    }
}

