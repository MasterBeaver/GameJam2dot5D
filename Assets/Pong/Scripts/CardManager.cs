using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public Sprite CardFront;
    public Sprite CardBack;

    public void Flip()
    {
        //when Flip() is called, store the value of the current sprite attached to this gameobject
        Sprite currentSprite = gameObject.GetComponent<SpriteRenderer>().sprite;

        //conditional logic to determine whether to display the card front or back sprite
        if (currentSprite == CardFront)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = CardBack;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = CardFront;
        }
    }
}
