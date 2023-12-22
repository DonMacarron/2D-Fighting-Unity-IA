using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Game_manager : MonoBehaviour
{
    private bool isOver = false;
    public int vidasIniciales;
    public camera_movement cameraScript;
    public Dictionary<string, int> vidasJugadores = new Dictionary<string, int>();
    public Text winText;
    public Animator animator;

    public void Start()
    {
        vidasIniciales = PlayerPrefs.GetInt("initial_lives");
    }
    public void playerJoins(string nombre) {
        vidasJugadores[nombre] = -5;
    }
    public void vidaMenos(string nombreJugador) {
        if (!isOver) {
            if (vidasJugadores[nombreJugador] != -5)
            {
                vidasJugadores[nombreJugador] = vidasJugadores[nombreJugador] - 1;
                if (vidasJugadores[nombreJugador] <= 0) { vidasJugadores.Remove(nombreJugador); }
                if (vidasJugadores.Count == 1)
                {
                    foreach (var ganador in vidasJugadores.Keys)
                    {
                        terminaPartida(ganador);
                    }
                }
            }
            else { vidasJugadores[nombreJugador] = vidasIniciales;
                vidasJugadores[nombreJugador] = vidasJugadores[nombreJugador] - 1;
                if (vidasJugadores[nombreJugador] <= 0) { vidasJugadores.Remove(nombreJugador); }
                if (vidasJugadores.Count == 1)
                {
                    foreach (var ganador in vidasJugadores.Keys)
                    {
                        terminaPartida(ganador);
                    }
                }
            }
        }
    }   
    public void terminaPartida(string ganador) {
        cameraScript.isOver();

        winText.text = ganador + "  WINS !!";
        Outline contorno = winText.GetComponent<Outline>();
        if (contorno == null)
        {
            contorno = winText.gameObject.AddComponent<Outline>();
        }
        contorno.effectDistance = new Vector2(5f, -5f);

        winText.fontStyle = FontStyle.Bold;

        contorno.effectColor = Color.black;

        isOver = true;

        StartCoroutine(animatorChange());

        StartCoroutine(sceneChange("PlayMenu"));

    }

    IEnumerator animatorChange() {
        float startTime = Time.time;

        while (Time.time - startTime < 2.5f) { 
            yield return null;
        }

        animator.SetTrigger("nextTrans");
    }

    IEnumerator sceneChange(string scene)
    {
        float startTime = Time.time;

        while (Time.time - startTime < 4f)
        {
            yield return null;
        }

        SceneManager.LoadScene("LIVESNUMBER");
    }
}
