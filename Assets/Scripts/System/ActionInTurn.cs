using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionInTurn : MonoBehaviour
{
    public static ActionInTurn Instance { get; set; }
 

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
