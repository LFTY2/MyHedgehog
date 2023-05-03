using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovment : MonoBehaviour
{
    static int speed = 2;
    public int speedBoost;
    new Transform transform; 
    void Start()
    {
        transform = GetComponent<Transform>();
        speedBoost = Random.Range(0, 2);
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.position += transform.up * (speed + speedBoost) * Time.deltaTime;
        if (transform.position.y < -15 || transform.position.y > 15)
            Destroy(gameObject);
        
    }

    public static void IncreaseSpeed(int add)
    {
        speed += add;
    }
}
