using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    public Player1 jugador;
    public Player2 bot;
    public string saveFile;
    public string countPath;
    public BotData botData;
    public BotCount botCount;
    public Button saveButton;
    public Button loadButton;

    public void Awake()
    {
        //fichero que almacena el numero de bots
        countPath = Application.dataPath + "/botVariableValues/Values/BotCount.json";
        if (File.Exists(countPath))
        {

            string content = File.ReadAllText(countPath);
            botCount = JsonUtility.FromJson<BotCount>(content);

            Debug.Log("numero bots: " + botCount.count);
        }
        else
        {
            botCount = new BotCount() { count = 0 };
            string newCountFileJSON = JsonUtility.ToJson(botCount);
            File.WriteAllText(countPath, newCountFileJSON);
            Debug.Log("numero bots: 0");
        }


        saveFile = Application.dataPath + "/botVariableValues/Values/values";
    }

    public void SaveData()
    {
        BotData newData = new BotData()
        {
            enemyHealth = jugador.dañoAcumulado
        };
        //crea nuevo bot
        string newJSON = JsonUtility.ToJson(newData);
        File.WriteAllText(saveFile+""+(botCount.count++)+".json", newJSON);
        //actualiza contador
        File.WriteAllText(countPath, JsonUtility.ToJson(new BotCount(){count = botCount.count }));
        Debug.Log(newJSON);
    }

    public void LoadData()
    {
        if (File.Exists(saveFile))
        {
            string content = File.ReadAllText(saveFile);
            botData = JsonUtility.FromJson<BotData>(content);

            Debug.Log("Vida  =  " + botData.enemyHealth);
        }
        else {
            Debug.Log("El archivo de guardado no existe");
        }
    }
}
