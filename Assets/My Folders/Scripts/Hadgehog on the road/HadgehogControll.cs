using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HadgehogControll : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Camera mainCamera;
    private new Transform transform;
    private Transform cameraTransform;
    [SerializeField ]private Vector3 cameraOffset;
    [SerializeField] HadgehogRoadGameManager gameManager;
    public bool isAlive = true;
    public float maxX;
    void Start()
    {
        transform = GetComponent<Transform>();
        mainCamera = Camera.main;
        cameraTransform = mainCamera.GetComponent<Transform>();

        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CameraControl();
        if (transform.position.x > maxX)
        {
            maxX = transform.position.x;
        }
    }

    private void Move()
    {
        if (!isAlive) return;
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(speed * Time.deltaTime * Vector2.up);
        }
        else if (Input.GetKey(KeyCode.A)&& transform.position.x>maxX-3)
        {
            transform.Translate(speed * Time.deltaTime * Vector2.down);
        }
    }

    private void CameraControl()
    {
        cameraTransform.position = new Vector3(maxX, 0, -10);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("End Trigger"))
        {
            Destroy(collision.gameObject);
            gameManager.AddRoad();
        }
        if(collision.CompareTag("Car")&&isAlive)
        {
            gameManager.StopGame();
        }
    }

    
}
