using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public struct Notification : NetworkMessage
{
    public string content;
}

public class MessageSender : MonoBehaviour
{
    [SerializeField] private TMP_Text notificationText;

    private void Start() 
    {
        if(!NetworkClient.active){ return; }

        NetworkClient.RegisterHandler<Notification>(OnNotication);    
    }


    private void OnNotication(Notification msg) 
    {
        notificationText.text += $"\n{msg.content}";
    }
}
