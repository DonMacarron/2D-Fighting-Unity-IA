using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BotMoves : PlayerMoves
{
    bool jumpOrder = false;
    bool attackOrder = false;

    public FirstAgent Agent;

    private void Update()
    {

        // a ver si se mueve lateralmente
        
        if (movimientoHorizontal == 1) { mirandoHacia = 0; }
        if (movimientoHorizontal == -1) { mirandoHacia = 2; }
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


        if (jumpOrder && jumpCoolDown<0 && jumpsLeft>0)
        {
            Saltar();
        }
        if (attackOrder && attackCoolDown<0 && untouchableCoolDown - (untouchableCoolDown / 2f) <= 0) { 
            Atacar1();
        }
        if(attackCoolDown>-1)
            attackCoolDown -= Time.deltaTime;
        if(untouchableCoolDown>-1)
            untouchableCoolDown -= Time.deltaTime;
        if(jumpCoolDown>-1)
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
    protected override void Ataque1() {
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
        BotPorrazoBehaviour proyectilScript = proyectil.GetComponent<BotPorrazoBehaviour>();
        proyectilScript.deQuienEsAtaque = this.gameObject;
        proyectilScript.scriptPlayer = this;

        
        StartCoroutine(base.DestroyProjectile(proyectil, -rotateWithAttack));
    }

    public override void perderVida() {
        gameManager.vidaMenos(nombre);

        //IA
        Debug.Log("Pierdo  vida");
        Agent.lifeLostReward();

        Restart();
    }
    public void enemyKilled() { Agent.enemyKillReward(); }
    public void setHorizontal(int h)
    {
        if (h == 0) { movimientoHorizontal = -1; }
        else { if (h == 2) { movimientoHorizontal = 1; }
            else { movimientoHorizontal = 0;  } }
    }
    public void setVertical(int v)
    {
        if (v == 0) { movimientoVertical = -1; }
        else
        {
            if (v == 2) { movimientoVertical = 1; }
            else { movimientoVertical = 0; }
        }
    }
    public void setJump(bool j)
    {
        jumpOrder = j; 
    }
    public void setAttack(bool a) {  attackOrder = a;}


}

