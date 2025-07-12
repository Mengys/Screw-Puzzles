using System.Collections.Generic;
using UnityEngine;

public class Fireworks : MonoBehaviour
{
    [SerializeField] private List<GameObject> _confetties;

    private void OnEnable() {
        foreach(var confetti in _confetties) {
            confetti.SetActive(true);
        }
    }
}
