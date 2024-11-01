using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class ParticleMoment : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float speed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        speed = Random.Range(-0.2f, -1f);
    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g,
        spriteRenderer.color.b, spriteRenderer.color.a - Time.deltaTime);
        if (spriteRenderer.color.a <= 0) Destroy(gameObject);
        transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y + speed * Time.deltaTime);
    }
}
