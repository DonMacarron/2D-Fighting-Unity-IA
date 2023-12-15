using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Game_manager : MonoBehaviour
{
    Dictionary<string, int> miDiccionario = new Dictionary<string, int>();
    public int vidasIniciales;
    public camera_movement cameraScript;

    public void jugadorSeUne(string nombreJugador) {
        miDiccionario.Add(nombreJugador,vidasIniciales);
    }

    public void vidaMenos(string nombreJugador) {
        miDiccionario[nombreJugador] = miDiccionario[nombreJugador] - 1;
        if (miDiccionario[nombreJugador] <= 0) { miDiccionario.Remove(nombreJugador); } 
        if (miDiccionario.Count == 1) {
            foreach (var ganador in miDiccionario)
            terminaPartida(ganador.Key); }
    }
    public void terminaPartida(string ganador) {
        cameraScript.isOver();
    }
}
