using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceOperator : MonoBehaviour
{
    public float radius = 1.5f;
    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius); //calcular todos los colliders que estan en una bola de 1.5m de radio
            foreach (Collider hitCollider in hitColliders)
            {
                // ¿Mirando hacia el dispositivo?
                Vector3 direction = hitCollider.transform.position - transform.position;
                if (Vector3.Dot(transform.forward, direction.normalized) > 0.5f)
                {
                    hitCollider.SendMessage("Operate", SendMessageOptions.DontRequireReceiver); //llama a la función de un script- es mucho más lenta pero no necesita saber el tipo de lo que llamas
                }
            }
        }
    }
}
