using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodDrop : MonoBehaviour
{
    private FoodDropGameManager gameManager;
    private Rigidbody2D foodRigidbody;
    [SerializeField] bool isBad;
    [SerializeField] private int foodScore;
    private float force;
    
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<FoodDropGameManager>();
        foodRigidbody = GetComponent<Rigidbody2D>();
        force = Random.Range(10f, 15f);
        AddForceToFood();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Border"))
        {
            if(!isBad)
            {
                gameManager.ReduceLives();
            }
            Destroy(gameObject);
        }
    }

    void AddForceToFood()
    {
        foodRigidbody.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }

    void OnMouseDown()
    {
        gameManager.UpdateScore(foodScore);
        if (isBad)
            gameManager.ReduceLives();
        Destroy(gameObject);
    }
}
