using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BoxList : MonoBehaviour
{
    public List<GameObject> boxes;
    public int number = 0;
    public Queue<GameObject> queue = new Queue<GameObject>();

    private void Awake() {
        number = 0;
    }

    public GameObject GetNext() {
        if (number >=  boxes.Count) return null;
        return boxes[number++];
    }
}
