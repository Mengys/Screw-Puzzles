using System.Collections.Generic;
using UnityEngine;

public class HolesManager : MonoBehaviour
{
    public List<GameObject> holes = new List<GameObject>();

    [SerializeField] private GameObject gameOver;
    private int freeHoles = 5;

    public Transform GetfreeHole()
    {
        if (freeHoles >= 0)
        {
            freeHoles--;
            return holes[freeHoles].transform;
        }
        else
        {
            gameOver.SetActive(true);
            return null;
        }
    }
}
