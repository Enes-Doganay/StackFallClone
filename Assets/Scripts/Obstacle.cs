using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    Rigidbody rb;
    MeshRenderer meshRenderer;
    Collider collider;
    ObstacleController obstacleController;
    private void Awake()
    {
        obstacleController = transform.parent.GetComponent<ObstacleController>();
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        collider = GetComponent<Collider>();
    }

    public void Shatter()
    {
        rb.isKinematic = false;
        collider.enabled = false;
        Vector3 forcePoint = transform.parent.position;
        float parentXPos = transform.parent.position.x;
        float xPos = meshRenderer.bounds.center.x;

        Vector3 subDirection = (parentXPos - xPos < 0) ? Vector3.right : Vector3.left;
        Vector3 direction = (Vector3.up * 1.5f + subDirection).normalized;

        float force = Random.Range(20, 35);
        float torque = Random.Range(110, 180);

        rb.AddForceAtPosition(direction * force , forcePoint,ForceMode.Impulse);
        rb.AddTorque(Vector3.left * torque);
        rb.velocity = Vector3.down;
    }
}
