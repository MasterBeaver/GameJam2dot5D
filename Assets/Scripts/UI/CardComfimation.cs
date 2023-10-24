using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardComfimation : MonoBehaviour
{
    public static CardComfimation Instane { get; private set; }

    [SerializeField] private GameObject ComfimationUI;
    private TextMeshProUGUI textMeshPro;

    public Button _confirm;
    public Button _cancel;
    private void Awake()
    {
        Instane = this;

        ComfimationUI = this.gameObject;
        _confirm = gameObject.transform.Find("Confirm").GetComponent<Button>();
        _cancel = gameObject.transform.Find("Cancel").GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayComfirmaitonUI(Action confirmAction, Action cancelAction)
    {
        _confirm.gameObject.SetActive(true);
        _cancel.gameObject.SetActive(true);


        _confirm.onClick.AddListener(() => {
            confirmAction();
            Hide();
        });

        _cancel.onClick.AddListener(() => {
            cancelAction();
            Hide();
        });

    }

    private void Hide()
    {
        _confirm.gameObject.SetActive(false);
        _cancel.gameObject.SetActive(false);
    }

}
