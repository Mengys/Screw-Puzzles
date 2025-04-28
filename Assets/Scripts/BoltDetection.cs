using UnityEngine;

public class BoltDetection : MonoBehaviour
{
    private bool isDetached = false;
    private float detachForce = 200f;

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log(collision.gameObject);

        if (isDetached)
            return;

        if (collision.gameObject.GetComponentInChildren<Bolt>())
        {
            Detach();
        }
    }

    private void Detach()
    {
        isDetached = true;

        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody>();

        rb.isKinematic = false;

        Vector3 randomDirection = (transform.position - transform.parent.position).normalized + Random.insideUnitSphere * 0.3f;
        rb.AddForce(randomDirection * detachForce);

        Debug.Log($"Деталь {gameObject.name} отсоединилась!");
    }
}
