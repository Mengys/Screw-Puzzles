using System.Collections.Generic;
using UnityEngine;

public class ParentBolt : MonoBehaviour
{
    [SerializeField] private List<Transform> boltList = new List<Transform>();

    private void Start()
    {
        foreach (Transform child in transform)
        {
            boltList.Add(child);
        }
    }

}
