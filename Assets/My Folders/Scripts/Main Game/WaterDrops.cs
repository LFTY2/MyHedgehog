using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDrops : MonoBehaviour
{
    new Transform transform;
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y<-10)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Dirt"))
        {
            Color dirtColor = collision.gameObject.GetComponent<SpriteRenderer>().color;
            dirtColor = new Color(0,0,0, dirtColor.a - 0.1f);
            collision.gameObject.GetComponent<SpriteRenderer>().color = dirtColor;
            if(dirtColor.a <= 0)
            {
                collision.gameObject.SetActive(false);
                gameManager.RemovingDirt();
            }
        }
    }
    
}
