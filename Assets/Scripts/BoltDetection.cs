using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoltDetection : MonoBehaviour
{
    [SerializeField] private List<Bolt> holdingBolts = new List<Bolt>();
    [SerializeField] private float detachForce = 200f;

    private bool isDetached = false;

    private void Update()
    {
        TryDetach();
    }

    private void TryDetach()
    {
        if (isDetached)
            return;

        holdingBolts.RemoveAll(bolt => bolt == null || bolt.gameObject == null);

        if (holdingBolts.Count == 0)
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
    }
}
