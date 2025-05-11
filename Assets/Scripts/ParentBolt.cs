using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class ParentBolt : MonoBehaviour
{
    [SerializeField] public List<Bolt> boltList = new List<Bolt>();
    [SerializeField] public TextMeshProUGUI boltText;
    [SerializeField] public GameObject shavingsPrefab;

    [HideInInspector] public int currentCountBolt = 0;
    [HideInInspector] public int boltAllCount = 0;

    private Transform targetObject;
    private Vector3 targetWorldPos;
    private AudioSource audioSource;
    private HolesManager holesManager;

    private void Start()
    {
        InitializeManagers();
        PopulateBoltList();
    }

    private void Update()
    {
        CheckBoltAnimations();
    }

    private void InitializeManagers()
    {
        holesManager = FindObjectOfType<HolesManager>();
        audioSource = GetComponent<AudioSource>();
        GameObject textObject = GameObject.FindGameObjectWithTag("Bolt Count");
        boltText = textObject.GetComponent<TextMeshProUGUI>();
    }

    private void PopulateBoltList()
    {
        foreach (Transform child in transform)
        {
            foreach (Transform grandChild in child)
            {
                Bolt bolt = grandChild.GetComponent<Bolt>();
                if (bolt != null)
                {
                    boltList.Add(bolt);
                }
            }
        }

        boltAllCount = boltList.Count;
        UpdateBoltText();
    }

    private void CheckBoltAnimations()
    {
        for (int i = boltList.Count - 1; i >= 0; i--)
        {
            Bolt bolt = boltList[i];
            if (!bolt.isEndAnimation)
                continue;

            bolt.isEndAnimation = false;
            ProcessBoltAnimation(bolt);
        }
    }

    private void ProcessBoltAnimation(Bolt bolt)
    {
        BoxesManager boxManager = FindObjectOfType<BoxesManager>();
        Box box = boxManager.GetBoxByColor(bolt.ToNameString(bolt.mesh.material.color));

        targetObject = GetTargetTransform(box, out Transform holeUsed);

        if(targetObject == null)
            targetObject = GetTargetTransform(box, out holeUsed);

        // Сохраняем дырку, если используется
        bolt.targetHole = holeUsed;

        bolt.GetComponent<Collider>().enabled = false;

        bolt.transform.DOMove(targetWorldPos + new Vector3(0f, 0f, 5f), 1f)
            .SetEase(Ease.InOutSine)
            .OnComplete(() => OnBoltAnimationComplete(bolt, box));
    }

    private Transform GetTargetTransform(Box box, out Transform usedHole)
    {
        if (box != null)
        {
            usedHole = null;
            return box.GetTargetFromBox(box) as RectTransform;
        }

        usedHole = holesManager.GetfreeHole() as Transform;
        return usedHole;
    }

    private void OnBoltAnimationComplete(Bolt bolt, Box box)
    {
        boltList.Remove(bolt);
        currentCountBolt++;
        UpdateBoltText();

        FindObjectOfType<TaskManager>().ProgressBoltTask(bolt.mesh.material.color);
        AnimateBoltRotation(bolt);
        SpawnShavings(bolt);

        if (box != null)
        {
            bolt.transform.SetParent(box.transform);
            box.AddBoltToBox(bolt);
        }
        else if (bolt.targetHole != null)
        {
            bolt.transform.SetParent(bolt.targetHole);
            bolt.transform.localPosition = new Vector3(0f, 0f, 0.05f); // Смещение к игроку

            if (holesManager != null)
            {
                holesManager.bolts.Add(bolt);
                holesManager.AnimateHoleBoltRotation(bolt);
            }
        }

        audioSource.Play();
    }


    private void AnimateBoltRotation(Bolt bolt)
    {
        Quaternion startRotation = bolt.transform.localRotation;
        Quaternion targetRotation = new Quaternion(-0.541675329f, -0.454519421f, -0.454519421f, 0.541675329f);

        DOVirtual.Float(0f, 1f, 0.5f, value =>
        {
            bolt.transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, value);
        }).SetEase(Ease.OutSine);
    }

    private void SpawnShavings(Bolt bolt)
    {
        GameObject shavings = Instantiate(shavingsPrefab, bolt.transform);
        shavings.transform.localPosition = new Vector3(0f, 0f, 0.05f);
        Destroy(shavings, 0.7f);
    }

    private void UpdateBoltText()
    {
        boltText.text = $"{currentCountBolt} / {boltAllCount}";
    }

    public int GetBoltCount()
    {
        return boltList.Count;
    }
}
