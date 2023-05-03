using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowerControl : MonoBehaviour
{
    [SerializeField] GameObject water;
    private GameManager gameManager;
    private float distanceFromCamera;
    private Vector3 startPosition;
    private void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        startPosition = transform.position;
    }
    private void OnMouseDown()
    {
        distanceFromCamera = Camera.main.WorldToScreenPoint(transform.position).z;
        StartCoroutine(WaterTimer());
    }

    private void OnMouseDrag()
    {
        Vector3 currentScreenPosition = new(Input.mousePosition.x, Input.mousePosition.y, distanceFromCamera);
        Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenPosition);
        transform.position = new Vector3(currentPosition.x, currentPosition.y + 1, startPosition.z);
        
        
    }
    private void OnMouseUp()
    {
        transform.position = startPosition;
        StopAllCoroutines();
    }
    IEnumerator WaterTimer()
    {
        Water();
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(WaterTimer());
    }
    void Water()
    {
        Instantiate(water, transform.position - new Vector3(0, 1.5f, 0), Quaternion.identity);
    }


    void Update()
    {
        
    }
}
