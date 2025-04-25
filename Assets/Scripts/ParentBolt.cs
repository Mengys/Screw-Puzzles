using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class ParentBolt : MonoBehaviour
{
    [SerializeField] private List<Bolt> boltList = new List<Bolt>();
    [SerializeField] private RectTransform targetObject;
    [SerializeField] public TextMeshProUGUI boltText;

    [HideInInspector] public int currentCountBolt = 0;
    [HideInInspector] public int boltAllCount = 0;

    private void Start()
    {
        foreach (Transform child in transform)
        {
            foreach (Transform grandChild in child)
            {
                Bolt bolt = grandChild.GetComponent<Bolt>();
                boltList.Add(bolt);

                GameObject target = GameObject.FindGameObjectWithTag("Bolt Indicator");
                targetObject = target.transform as RectTransform;

                GameObject text = GameObject.FindGameObjectWithTag("Bolt Count");
                boltText = text.GetComponent<TextMeshProUGUI>();

                boltAllCount = boltList.Count;
                boltText.text = currentCountBolt.ToString() + " / " + boltAllCount.ToString();
            }
        }
    }

    private void Update()
    {
        foreach (Bolt bolt in boltList)
        {
            if (bolt.isEndAnimation)
            {
                bolt.isEndAnimation = false;

                Vector3 targetWorldPos = targetObject.position;

                bolt.transform.DOMove(targetWorldPos + new Vector3(0f, 0f, 5f), 1f)
                    .SetEase(Ease.InOutSine)
                    .OnComplete(() =>
                    {
                        Destroy(bolt.gameObject);
                        boltList.Remove(bolt);
                        currentCountBolt++;
                        boltText.text = currentCountBolt.ToString() + " / " + boltAllCount.ToString();
                    });
            }
        }
    }

    public int GetBoltCount()
    {
        return boltList.Count;
    }
}
