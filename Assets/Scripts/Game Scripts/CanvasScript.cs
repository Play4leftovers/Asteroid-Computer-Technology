using System;
using System.Collections;
using System.Collections.Generic;
using Game_Scripts;
using Scriptable_Objects.Code;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CanvasScript : MonoBehaviour
{
    public AsteroidData astData;
    public ShipData shipData;

    public TextMeshProUGUI score;
    public TextMeshProUGUI life;
    public TextMeshProUGUI time;
    
    private int _seconds;
    private void Start()
    {
        astData.asteroidScore = 0;
        InvokeRepeating(nameof(TimeUpdate), 1,1);
    }

    private void ScoreUpdate()
    {
        if (int.Parse(score.text) == astData.asteroidScore) { return; }
        score.SetText(astData.asteroidScore.ToString());
    }

    private void HealthUpdate()
    {
        if (int.Parse(life.text) == shipData.shipCurrentHealth) { return; }
        life.SetText(shipData.shipCurrentHealth.ToString());
    }

    private void TimeUpdate()
    {
        _seconds++;
        time.SetText(_seconds.ToString());
    }

    void Update()
    {
        ScoreUpdate();
        HealthUpdate();
    }
}
