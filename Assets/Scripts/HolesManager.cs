using System.Collections.Generic;
using UnityEngine;

public class HolesManager : MonoBehaviour
{
    public List<GameObject> holes = new List<GameObject>();

    private int freeHoles = 5;

    public Vector3 GetfreeHole()
    {
        if (freeHoles >= 0)
        {
            freeHoles--;
        }
        return holes[freeHoles].transform.position;
    }
}
