using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public List<Bolt> bolts = new List<Bolt>();

    [HideInInspector] public bool isComplete = false;

    public void AddBoltToBox(Box box, Bolt bolt)
    {
        if (box.bolts.Count < 3)
        {
            box.bolts.Add(bolt);

            if (box.bolts.Count == 3)
            {
                box.isComplete = true;
                BoxesManager boxManger = GetComponentInParent<BoxesManager>();
                boxManger.ChangeBox(this);
            }
        }
    }
}
