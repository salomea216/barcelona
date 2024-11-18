using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChangeDevice : MonoBehaviour
{
    public void Operate()
    {
        Color random = Random.ColorHSV();
        GetComponent<Renderer>().material.color = random;
    }
}