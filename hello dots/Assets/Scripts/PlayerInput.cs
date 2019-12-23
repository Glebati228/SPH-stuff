using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventAggregation;

public class PlayerInput : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            EventAggregator.Publish<SpaceClicked>(new SpaceClicked() { });
        }
    }
}
