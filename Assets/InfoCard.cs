using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Card 
{   
    public string name;
    public string level;
    public string health;

    public Card(string name, string level , string health)
    {
        this.name = name;
        this.level = level;
        this.health = health;
    } 
}

public class InfoCard : MonoBehaviour
{
    public TMP_InputField nameInput;
    public TMP_InputField levelInput;
    public TMP_InputField healthInput;

    public Card ReturnClass()
    {
        return new Card(nameInput.text, levelInput.text, healthInput.text);
        
    }

    public void SetUI(Card card)
    {
        nameInput.name = card.name;
    }
}

