using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Tuple<String,String> location; // left right or middle
    public String currentPortalLocation;
    public String nextPortalLocation;

    private void Start()
    {
        location = new Tuple<String,String>(currentPortalLocation, nextPortalLocation);
    }
}
