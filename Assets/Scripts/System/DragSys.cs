using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragSys : MonoBehaviour
{
    //public GameObject correctForm;
    public bool moving;

    private float starPosX;
    private float starPosY;
    public Vector3 resetPosition;

    private bool finish;
    public bool hover = false;
   
    void Start()
    {
        resetPosition = this.gameObject.transform.position;
        moving = false;
        //card = gameObject.GetComponent<Cards>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!finish)
        {
            if (moving)
            {
                //Vector3 mousePos;
                //mousePos = Input.mousePosition;
                //mousePos = Camera.main.ScreenToWorldPoint(mousePos);
                //mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z - Camera.main.transform.position.z));

                //this.gameObject.transform.localPosition = new Vector3(mousePos.x - starPosX, mousePos.y - starPosY, this.gameObject.transform.localPosition.z);
            }
        }

        

    }


    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0) && !moving)
        {
            
            //Vector3 mousePos;
            //mousePos = Input.mousePosition;
            //mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.y, Input.mousePosition.x, transform.position.x - Camera.main.transform.position.z));

            //starPosX = mousePos.x - transform.localPosition.x;
            //starPosY = mousePos.y - transform.localPosition.y;

            /*board.Instance._type = card.type;
            
            board.Instance.idCard = card.idCrad;*/

            Vector3 dragPos = new Vector3(0, 0.05f, 0.05f) + resetPosition;
            this.transform.position = dragPos;

            board.Instance.spawnMonster = false;
            moving = true;
        }

    }

    private void OnMouseUp()
    {
        moving = false;
        board.Instance.spawnMonster = true;
        this.transform.position = new Vector3(resetPosition.x, resetPosition.y, resetPosition.z);
    }

    public void holdingCard()
    {

    }

}
