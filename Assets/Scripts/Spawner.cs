using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject wall;
    public GameObject spawn;
    BoxCollider2D rBodySpawn;

    // Start is called before the first frame update
    void Start()
    {
        rBodySpawn = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            wall.SetActive(true);
        }
    }
}
