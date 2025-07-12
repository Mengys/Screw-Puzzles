using System.Collections.Generic;
using UnityEngine;

public class TutorialTargets : MonoBehaviour
{
    [SerializeField] private List<GameObject> _tutorialTargets;

    private int number = 0;

    public GameObject GetNextTarget() {
        if (number >= _tutorialTargets.Count) return null;
        return _tutorialTargets[number++];
    }
}
