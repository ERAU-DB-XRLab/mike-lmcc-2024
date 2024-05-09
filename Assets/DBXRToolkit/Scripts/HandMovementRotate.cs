using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandMovementRotate : MonoBehaviour
{

    [Header("Player Movement Method (Pick One)")]
    [SerializeField] private Transform player;
    
    [Space]
    [SerializeField] private InputActionProperty rotate;
    [Space]
    [SerializeField] private float rotationSpeed;

    // Update is called once per frame
    void Update()
    {

        Vector2 input = rotate.action.ReadValue<Vector2>();
        player.Rotate(new Vector3(0, input.x * rotationSpeed * Time.deltaTime, 0));

    }

}
