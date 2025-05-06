using UnityEngine;

public class Box : MonoBehaviour
{
    public int countBox = 0;
    public string color;

    [HideInInspector] public bool isComplete = false;

    public void AddBoltToBox(Box box, Bolt bolt)
    {
        if (box.countBox < 3)
        {
            box.countBox++;

            if (box.countBox == 3)
            {
                box.isComplete = true;

                BoxesManager boxesManager = GetComponentInParent<BoxesManager>();
                boxesManager.ChangeBox(this);
            }
        }
    }
}
