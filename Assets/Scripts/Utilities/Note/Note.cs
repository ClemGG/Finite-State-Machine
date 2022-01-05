using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public string text = "Type your note here";

    public void Awake()
    {
        if (!Application.isEditor)
        {
            Destroy(this);
        }
    }
}
