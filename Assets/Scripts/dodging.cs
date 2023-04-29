using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dodging : MonoBehaviour 
{
    //Can change movement speed in Unity just 
    //set it to something random for now
    [SerializeField] private float moveSpeed = 4f;

    private void Move(float distance) 
    {
        //input Vector for A and D key
        Vector2 inputVector = new Vector2(0,0);

        if (Input.GetKey(KeyCode.A)) {
            inputVector.x = -1;
        }

        if (Input.GetKey(KeyCode.D)) {
            inputVector.x = +1;
        }
    

        //move horizontally and not vertically
        Vector3 moveDir = new Vector3(inputVector.x, 0f, 0f );
        //move at a decent speed and a specific distance
        transform.position += moveDir * moveSpeed * distance * Time.deltaTime;
    }   
}


