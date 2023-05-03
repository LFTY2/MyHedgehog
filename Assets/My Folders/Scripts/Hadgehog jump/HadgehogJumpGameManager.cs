using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HadgehogJumpGameManager : MonoBehaviour
{
    [SerializeField] GameObject platform;
    HadgehogControlJump hadgehogControl;
    int step;
    int platformsCreated;
    int platformsNeed;
    void Start()
    {
        hadgehogControl = GameObject.Find("Hadgehog").GetComponent<HadgehogControlJump>();
    }

    // Update is called once per frame
    void Update()
    {
        platformsNeed = ((int)hadgehogControl.maxY + 12) / 2;
        if(platformsCreated <  platformsNeed)
        {
            Instantiate(platform, new Vector2(Random.Range(-2f, 2f), 2.5f * platformsCreated - 3), Quaternion.identity);
            platformsCreated++;
        }
    }
}
