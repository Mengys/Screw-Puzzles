using System.Collections.Generic;
using UnityEngine;

public class HolesManager : MonoBehaviour
{
    public List<GameObject> holes = new List<GameObject>();

    [SerializeField] private GameObject gameOver;

    private int nextFreeIndex = 0;

    public Transform GetfreeHole()
    {
        if (nextFreeIndex < holes.Count)
        {
            Transform holeTransform = holes[nextFreeIndex].transform;
            nextFreeIndex++;
            return holeTransform;
        }
        else
        {
            gameOver.SetActive(true);
            return null;
        }
    }
}