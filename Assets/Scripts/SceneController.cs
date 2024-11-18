using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab; //es como si pusieras public, pero de esta manera otros scripts no pueden modificarla
    private GameObject _enemy;

    public List<GameObject> spawnPoints;

    void Update()
    {
        if (_enemy == null)
        { // ¡OJO!
            _enemy = Instantiate<GameObject>(enemyPrefab); // instanciar si no había un enemigo

            int i = Random.Range(0, spawnPoints.Count);
            GameObject sp= spawnPoints[i];
            _enemy.transform.position = sp.transform.position;

            float angle = Random.Range(0, 360);
            _enemy.transform.Rotate(0, angle, 0);
        }
    }
}