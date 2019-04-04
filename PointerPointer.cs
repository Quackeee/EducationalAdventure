using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerPointer : MonoBehaviour
{
  
    void Update()
    {
        if (Input.GetButtonDown("Point"))
        {
            Move();
        }
       
     
    }

    void Move()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
