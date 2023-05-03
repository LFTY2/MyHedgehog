using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField] private int saturation;
    [SerializeField] private string foodName;
    public int foodId;

    [SerializeField] private bool isHadgehog; //remove SF
    private Vector3 startPosition;
    private float distanceFromCamera;

    private void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        startPosition = transform.position;
    }

    private void OnMouseDown()
    {
        distanceFromCamera = Camera.main.WorldToScreenPoint(transform.position).z;
    }

    private void OnMouseDrag()
    {
        Vector3 currentScreenPosition = new(Input.mousePosition.x, Input.mousePosition.y, distanceFromCamera);
        Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenPosition);
        transform.position = new Vector3(currentPosition.x, currentPosition.y, startPosition.z);
    }

    private void OnMouseUp()
    {
        if(isHadgehog)
        {
            if(gameManager.Eat(saturation, foodId))
                Destroy(gameObject);
            else
                transform.position = startPosition;
        }
        else
        {
            transform.position = startPosition;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isHadgehog = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isHadgehog = false;
    }
}
