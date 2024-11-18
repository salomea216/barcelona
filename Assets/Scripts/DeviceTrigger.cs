using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceTrigger : MonoBehaviour
{
    [SerializeField] private GameObject[] targets; //define un array de gameObjects
    void OnTriggerEnter(Collider other) //cuando alguien entra en mi collider trigger se ejecuta el codigo
    {
        foreach (GameObject target in targets) //y recorre uno a uno y le manda el mensaje de activate
        {
            target.SendMessage("Activate");
        }
    }
    void OnTriggerExit(Collider other)
    {
        foreach (GameObject target in targets)
        {
            target.SendMessage("Deactivate");
        }
    }
}
