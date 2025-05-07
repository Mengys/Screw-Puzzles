using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class ParentBolt : MonoBehaviour
{
    [SerializeField] public List<Bolt> boltList = new List<Bolt>();
    [SerializeField] public TextMeshProUGUI boltText;

    [HideInInspector] public int currentCountBolt = 0;
    [HideInInspector] public int boltAllCount = 0;

    private Transform targetObject;
    private Vector3 targetWorldPos;

    private void Start()
    {
        foreach (Transform child in transform)
        {
            foreach (Transform grandChild in child)
            {
                Bolt bolt = grandChild.GetComponent<Bolt>();
                boltList.Add(bolt);

                GameObject text = GameObject.FindGameObjectWithTag("Bolt Count");
                boltText = text.GetComponent<TextMeshProUGUI>();

                boltAllCount = boltList.Count;
                boltText.text = currentCountBolt.ToString() + " / " + boltAllCount.ToString();
            }
        }
    }

    private void Update()
    {
        for (int i = boltList.Count - 1; i >= 0; i--)
        {
            Bolt bolt = boltList[i];
            if (bolt.isEndAnimation)
            {
                bolt.isEndAnimation = false;

                BoxesManager boxManager = FindObjectOfType<BoxesManager>();
                Box box = boxManager.GetBoxByColor(bolt.ToNameString(bolt.mesh.material.color));

                if(box != null)
                {
                    targetObject = box.GetTargetFromBox(box) as RectTransform;
                }
                else
                {
                    targetObject = boltText.gameObject.transform as RectTransform;
                }
                targetWorldPos = targetObject.position;

                bolt.transform.DOMove(targetWorldPos + new Vector3(0f, 0f, 5f), 1f)
                    .SetEase(Ease.InOutSine)
                    .OnComplete(() =>
                    {
                        Destroy(bolt.gameObject);
                        Destroy(bolt.transform.parent.gameObject);

                        boltList.Remove(bolt);
                        currentCountBolt++;
                        boltText.text = currentCountBolt.ToString() + " / " + boltAllCount.ToString();

                        FindObjectOfType<TaskManager>().ProgressBoltTask(bolt.mesh.material.color);

                        box.AddBoltToBox(bolt);
                    });
            }
        }
    }

    public int GetBoltCount()
    {
        return boltList.Count;
    }
}
