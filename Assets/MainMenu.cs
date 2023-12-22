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
        loader.LoadSceneInstant("LIVESNUMBER");
    }
    public void BotVsBot() {
        loader.LoadSceneInstant("LIVESNUMBER");
    }
    public void Settings() {
        loader.LoadSceneInstant("LIVESNUMBER");
    }

    public void Exit() {
        Application.Quit(); 
    }
}
