using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject particle;

    private void Start()
    {
        StartCoroutine(SpawnParticle());
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(player.transform.position.x, player.transform.position.y);
    }

    private IEnumerator SpawnParticle()
    {
        while (true)
        {
            float rand = Random.Range(0.05f, 0.2f);
            Vector3 randPos = new Vector2(Random.Range(-6f, 6f), Random.Range(-4.5f, 4.5f));
            Instantiate(particle, transform.position + randPos, Quaternion.Euler(0, 0, -35f + Random.Range(-10f, 10f)));
            yield return new WaitForSeconds(rand);
        }
    }
}
