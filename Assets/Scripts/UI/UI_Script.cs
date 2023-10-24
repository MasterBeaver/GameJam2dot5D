using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Script : MonoBehaviour
{
    public static UI_Script Instane { get; set; }

    [SerializeField] private TextMeshProUGUI point;
    [SerializeField] private TextMeshProUGUI round;
    private const int _maxPoint = 20;
    public int currentPoint;
    public int roundNum = 0;
    private void Awake()
    {
        Instane = this;

        point.text = _maxPoint.ToString();
        currentPoint = _maxPoint;
    }

    // Update is called once per frame
    void Update()
    {
        round.text = $"Round {roundNum}";
        point.text = currentPoint.ToString();
    }

    public void LostPoint(int num)
    {
        currentPoint -= num;
       
    }
}
