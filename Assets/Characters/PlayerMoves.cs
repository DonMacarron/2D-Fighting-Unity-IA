using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float movimientoHorizontal;

    public float velocidadMovimiento = 17f; // Velocidad de movimiento lateral.
    public float fuerzaSalto = 60f; // Fuerza del salto.
    public Transform puntoVerificador; // Punto de verificación para detectar el suelo.
    public LayerMask capasDeSuelo; // Las capas que considerarás como suelo.
    public int maxSaltos = 2; // Número máximo de saltos.

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
        //a ver si esta en el suelo
        enSuelo = Physics2D.OverlapCircle((puntoVerificador.position - (new Vector3(0,puntoVerificador.localScale.y/2, 0))), 0.1f, capasDeSuelo);

        if (enSuelo)
        {
            saltosRestantes = maxSaltos;
        }

        if (Input.GetButtonDown("Jump"))
        { 
            Saltar();
        }


        // a ver si se mueve lateralmente
        movimientoHorizontal = Input.GetAxis("Horizontal");

        
    }

    private void FixedUpdate() {
        // Establece la velocidad de manera instantánea en función de la entrada del jugador.
        if (movimientoHorizontal != 0)
        {
            puntoVerificador.position += new Vector3(movimientoHorizontal * velocidadMovimiento * Time.deltaTime, 0, 0);
        }
        else { puntoVerificador.position = new Vector3(puntoVerificador.position.x, puntoVerificador.position.y, puntoVerificador.position.z); }
    }

    //salto
    private void Saltar()
    {
        if (saltosRestantes >= 2) {
            rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);
            saltosRestantes--;
        }
        if (saltosRestantes == 1) {
            rb.velocity = new Vector2(rb.velocity.x, (float)(fuerzaSalto/2.0));
            saltosRestantes--;
        }
        
        
    }
}
