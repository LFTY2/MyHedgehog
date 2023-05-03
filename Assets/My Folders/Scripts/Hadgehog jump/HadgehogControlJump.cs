using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HadgehogControlJump : MonoBehaviour
{
    Rigidbody2D hadgehogRB;
    new Transform transform; 
    [SerializeField] float jumpForce;
    [SerializeField] float speed;

    public float maxY = 0;
    private Camera mainCamera;
    private Transform cameraTransform;
    private bool isGameStoped;
    [SerializeField] Text score, coins;


    [SerializeField] GameObject gameOverMenu;
    void Start()
    {
        mainCamera = Camera.main;
        cameraTransform = mainCamera.GetComponent<Transform>();
        hadgehogRB = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameStoped) return;
        if(Input.GetKey(KeyCode.A)&&transform.position.x>-3)
        {
            transform.position += transform.right * -1 * speed * Time.deltaTime; 
        }
        else if(Input.GetKey(KeyCode.D) && transform.position.x < 3)
        {
            transform.position += transform.right * speed * Time.deltaTime;
        }
        if(transform.position.y>maxY)
        {
            maxY = transform.position.y;
            UpdateScore();
        }
        if(transform.position.y < maxY - 5)
        {
            StopGame();
        }
        CameraControl();
    }
    void Jump()
    {
        hadgehogRB.velocity = Vector2.zero;
        hadgehogRB.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        float playerBottom = transform.position.y - GetComponent<BoxCollider2D>().bounds.extents.y+0.2f;
        float platformTop = collision.GetComponent<BoxCollider2D>().bounds.max.y;
        if (playerBottom >= platformTop)
        {
            Jump();
        }
    }
    void CameraControl()
    {
        cameraTransform.position = new Vector3(0,maxY,-10);
    }

    void StopGame()
    {
        int coinsCount = (int)(maxY / 10) + 2;
        coins.text = "Coins: " + coinsCount.ToString();
        isGameStoped = true;

        StopAllCoroutines();
        gameOverMenu.SetActive(true);
        PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + coinsCount);
        if (PlayerPrefs.GetInt("Mood") + 10 <= 100)
            PlayerPrefs.SetInt("Mood", PlayerPrefs.GetInt("Mood") + 10);
        else
            PlayerPrefs.SetInt("Mood", 100);
        PlayerPrefs.SetInt("Energy", PlayerPrefs.GetInt("Energy") - 5);
    }

    private void UpdateScore()
    {
        score.text ="Score: " + ((int)maxY).ToString();
    }
}
