using DG.Tweening;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoltBox : MonoBehaviour
{
    [HideInInspector] public List<Bolt> bolts = new List<Bolt>();

    public void AddBolt(Bolt bolt) {
        bolts.Add(bolt);
    }
}
