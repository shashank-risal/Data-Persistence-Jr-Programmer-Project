using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerName : MonoBehaviour
{
    public static string playerName;
    public TMP_InputField nameOfPlayer;

    public string GetName()
    {
        playerName = nameOfPlayer.GetComponent<TMP_InputField>().text;
        return playerName;
    }

}
