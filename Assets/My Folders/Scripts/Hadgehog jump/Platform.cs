using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    HadgehogControlJump hadgehogControl;
    new Transform transform;
    private void Start()
    {
        transform = GetComponent<Transform>();
        hadgehogControl = GameObject.Find("Hadgehog").GetComponent<HadgehogControlJump>();
    }
    private void Update()
    {
        if(transform.position.y<hadgehogControl.maxY-7)
        {
            Destroy(gameObject);
        }
    }
}
