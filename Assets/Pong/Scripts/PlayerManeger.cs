using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerManeger : NetworkBehaviour
{
    public GameObject[] cards;
    public GameObject PlayerArea;
    public GameObject EnemyArea;

    public Transform player1Area;
    public Transform player2Area;

    private Transform playArea;
    public override void OnStartClient()
    {
        base.OnStartClient();

        PlayerArea = GameObject.Find("CardsOnHand");
        EnemyArea = GameObject.Find("CardsOnEnemy");
        player1Area = GameObject.Find("spawn1").GetComponent<Transform>();
        player2Area = GameObject.Find("spawn2").GetComponent<Transform>();
        
        if (isLocalPlayer)
        {
            if (isServer)
            {
                playArea = player1Area;
            }
            else
            {
                playArea = player2Area;
            }
        }
        else
        {
            if (isServer)
            {
                playArea = player2Area;
            }
            else
            {
                playArea = player1Area;
            }
        }
    }

    [Command]
    public void CmdDealCards()
    {
        //(5x) Spawn a random card from the cards deck on the Server, assigning authority over it to the Client that requested the Command. Then run RpcShowCard() and indicate that this card was "Dealt"
        for (int i = 0; i < 5; i++)
        {
            GameObject card = Instantiate(cards[Random.Range(0, cards.Length)], new Vector2(0, 0), Quaternion.identity);
            NetworkServer.Spawn(card, connectionToClient);
            RpcShowCard(card, "Dealt");
        }
    }

    [Command]
    public void CmdDrawCard()
    {
        GameObject card = Instantiate(cards[Random.Range(0, cards.Length)], new Vector2(0, 0), Quaternion.identity);
        NetworkServer.Spawn(card, connectionToClient);
        RpcShowCard(card, "Dealt");
    }

    //PlayCard() is called by the DragDrop script when a card is placed in the DropZone, and requests CmdPlayCard() from the Server
    public void PlayCard(GameObject card)
    {
        CmdPlayCard(card);
    }

    //CmdPlayCard() uses the same logic as CmdDealCards() in rendering cards on all Clients, except that it specifies that the card has been "Played" rather than "Dealt"
    [Command]
    void CmdPlayCard(GameObject card)
    {
        RpcShowCard(card, "Played");
        
        //If this is the Server, trigger the UpdateTurnsPlayed() method to demonstrate how to implement game logic on card drop
        if (isServer)
        {
            UpdateTurnsPlayed();
        }
    }

    //UpdateTurnsPlayed() is run only by the Server, finding the Server-only GameManager game object and incrementing the relevant variable
    [Server]
    void UpdateTurnsPlayed()
    {
        // RpcLogToClients("Turns Played: " + gm.TurnsPlayed);
    }

    //RpcLogToClients demonstrates how to request all clients to log a message to their respective consoles
    [ClientRpc]
    void RpcLogToClients(string message)
    {
        Debug.Log(message);
    }

    //ClientRpcs are methods requested by the Server to run on all Clients, and require the [ClientRpc] attribute immediately preceding them
    [ClientRpc]
    void RpcShowCard(GameObject card, string type)
    {
        //if the card has been "Dealt," determine whether this Client has authority over it, and send it either to the PlayerArea or EnemyArea, accordingly. For the latter, flip it so the player can't see the front!
        if (type == "Dealt")
        {
            if (isOwned)
            {
                card.transform.SetParent(PlayerArea.transform, false);
            }
            else
            {
                card.transform.SetParent(EnemyArea.transform, false);
                card.GetComponent<CardManager>().Flip();
            }
        }
        //if the card has been "Played," send it to the DropZone. If this Client doesn't have authority over it, flip it so the player can now see the front!
        else if (type == "Played")
        {
            // card.transform.SetParent(DropZone.transform, false);
            // if (!isOwned)
            // {
            //     card.GetComponent<CardManager>().Flip();
            // }
        }
    }
}
