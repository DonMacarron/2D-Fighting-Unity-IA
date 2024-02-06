using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LivesMenu : MonoBehaviour
{
    public TextMeshProUGUI vidas;
    public SceneLoader loader;
    public Scrollbar scroll;
    public int nn;

    public void Start()
    {
        scroll.onValueChanged.AddListener(livesUpdate);
        scroll.value = 0.222222222f;
        nn = 3;
    }
    public void Play() {
        PlayerPrefs.SetInt("initial_lives", nn);
        PlayerPrefs.Save();
        loader.LoadScene("Fight_pvp");
        
    }
    public void Back() {
        loader.LoadSceneInstant("INDEXSCENE");
    }

    public void livesUpdate(float n) {
        nn = (int)((n*10)-n) + 1;
        if (nn < 10) { vidas.text = "Lives: " + nn; }
        else { vidas.text = "Lives: INFINITE";
            nn = 999999999;
        }
        
    }
    
}
