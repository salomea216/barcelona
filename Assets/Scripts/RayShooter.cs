using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RayShooter : MonoBehaviour
{
    private Camera _camera;
    public Texture2D crosshair;

    private float timeSinceLastShoot = 1;
    public float coolDownSeconds = 1;
        
    void Start()
    {
        _camera = GetComponent<Camera>();
        Cursor.lockState = CursorLockMode.Locked; // deja el ratón en el centro de la ventana
        Cursor.visible = false; // pulsa Esc para desbloquearlo
    }

    IEnumerator SphereIndicator(Vector3 position)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = position;
        //sphere.tag = "NonShootable";
        Destroy(sphere.GetComponent<SphereCollider>());

        yield return new WaitForSeconds(1);

        Destroy(sphere);
    }

    void Update()
    {

        timeSinceLastShoot += Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && timeSinceLastShoot >= coolDownSeconds )
        {
            timeSinceLastShoot = 0;
            Vector3 point = new Vector3(_camera.pixelWidth / 2, _camera.pixelHeight / 2, 0);
            Ray ray = _camera.ScreenPointToRay(point);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.transform.gameObject;
                ReactiveTarget target = hitObject.GetComponent<ReactiveTarget>();
                if (target != null)
                {
                    //Debug.Log("Take that!");
                    target.ReactToHit();
                }
                else
                {
                    StartCoroutine(SphereIndicator(hit.point));
                }
            }
            //if (!hit.collider.CompareTag("NonShootable")){
            //Debug.Log("Hit " + hit.point + " (" + hit.transform.gameObject.name + ")");                

            
        }
    }

    void OnGUI()
    { // se ejecuta después de dibujar el frame del juego
        int size = 20;
        float posX = _camera.pixelWidth / 2 - size / 2;
        float posY = _camera.pixelHeight / 2 - size / 2;
        GUI.Label(new Rect(posX, posY, size, size), crosshair); // puede mostrar texto e imágenes
    }


}