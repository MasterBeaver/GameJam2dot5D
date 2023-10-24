using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Character : MonoBehaviour
{
    [SerializeField] private Character _character;
    [SerializeField] private Combat _combat;
    [SerializeField] private TextMeshProUGUI hp;
    [SerializeField] private TextMeshProUGUI dmg;

    [SerializeField] Transform objTransform;
    [SerializeField] Canvas canvas;
   

    private void Awake()
    {
        canvas.worldCamera = Camera.main;
        
    }

    // Update is called once per frame
    void Update()
    {
        hp.text = $"{_character.currentHealth}";
        dmg.text = $"{_combat.damage}";

        if (objTransform != null)
        {
            canvas.transform.position = objTransform.position;
           
        }
    }
}
