using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float speed = 5;

    private void Update()
    {
        float distance = Vector2.Distance(transform.position, target.position);
        transform.position = Vector2.MoveTowards(transform.position, target.position, Mathf.Sqrt(distance) * speed * Time.deltaTime);
    }
}
