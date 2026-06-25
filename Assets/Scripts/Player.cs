using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rb.WakeUp();
    }

    public void ResetPlayer(Transform spawnPoint)
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.position = spawnPoint.position;
        rb.rotation = spawnPoint.rotation;

        rb.WakeUp();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            GameManager.Instance.EndGame();
        }
    }
}