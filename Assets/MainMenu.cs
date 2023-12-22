using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public SceneLoader loader;
    public void PlayerVsPlayer() {
        loader.LoadSceneInstant("LIVESNUMBER");
    }
    public void PlayerVsBot()
    {
        loader.LoadSceneInstant("");
    }
    public void BotVsBot() {
        loader.LoadSceneInstant("");
    }
    public void Settings() {
        loader.LoadSceneInstant("");
    }

    public void Exit() {
        Application.Quit(); 
    }
}
