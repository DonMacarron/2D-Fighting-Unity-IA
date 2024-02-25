using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LearnBot1 : BotMoves
{
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
    public override void enemyKilled() { Agent.enemyKillReward(); }
    public override void enemyHit() {  Agent.hitReward(); }
    public override int getJumpsLeft() { return jumpsLeft; }
}

