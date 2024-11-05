using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FPSCounter : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float updateInterval = 5f;
    private float timePassed = 0f;
    private int frames = 0;
    private float averageFPS = 0f;

    void Update()
    {
        frames++;
        timePassed += Time.deltaTime;

        averageFPS = frames / timePassed;
        text.text = $"FPS : {(int)averageFPS}";

        if (timePassed >= updateInterval)
        {
            frames = 0;
            timePassed = 0f;
        }
    }
}