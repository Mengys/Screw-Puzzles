using System.Collections.Generic;
using UnityEngine;

public class BoltDetection : MonoBehaviour
{
    [SerializeField] private List<Bolt> holdingBolts = new List<Bolt>();
    [SerializeField] private float detachForce = 200f;
    [SerializeField] private float destroyDelay = 5f;

    private bool isDetached = false;
    private Renderer objectRenderer;
    private Color originalColor;
    private bool isTransparent = false;

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color;
            SetMaterialTransparent(); // готовим материал к прозрачности
        }
    }

    private void Update()
    {
        TryDetach();

        Vector2 screenPos = Input.mousePosition;

#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
            screenPos = Input.GetTouch(0).position;
#endif

        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject == gameObject)
            {
                if (Input.GetMouseButton(0))
                {
                    SetTransparent();
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    ResetTransparency();
                }
            }
            else
            {
                // Если клик был вне объекта — вернуть прозрачность обратно
                if (isTransparent) ResetTransparency();
            }
        }
        else
        {
            // Если никуда не попали — вернуть прозрачность обратно
            if (isTransparent) ResetTransparency();
        }
    }

    private void SetTransparent()
    {
        if (objectRenderer == null || isTransparent) return;

        Color newColor = originalColor;
        newColor.a = 0.5f;
        objectRenderer.material.color = newColor;
        isTransparent = true;
    }

    private void ResetTransparency()
    {
        if (objectRenderer == null || !isTransparent) return;

        objectRenderer.material.color = originalColor;
        isTransparent = false;
    }


    private void ToggleTransparency()
    {
        if (!isTransparent)
        {
            Color newColor = originalColor;
            newColor.a = 0.5f;
            objectRenderer.material.color = newColor;
            isTransparent = true;
        }
        else
        {
            objectRenderer.material.color = originalColor;
            isTransparent = false;
        }
    }

    private void SetMaterialTransparent()
    {
        if (objectRenderer == null) return;

        Material mat = objectRenderer.material;
        mat.shader = Shader.Find("Standard");

        mat.SetFloat("_Mode", 3); // 3 = Transparent
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 1);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = 3000;
    }

    private void TryDetach()
    {
        if (isDetached)
            return;

        holdingBolts.RemoveAll(bolt => bolt == null || bolt.gameObject == null);

        if (holdingBolts.Count == 0)
        {
            Detach();
            return;
        }

        bool allBoltsDone = true;
        foreach (var bolt in holdingBolts)
        {
            if (bolt != null && !bolt.isScrewing)
            {
                allBoltsDone = false;
                break;
            }
        }

        if (allBoltsDone)
        {
            Detach();
        }
    }

    private void Detach()
    {
        isDetached = true;

        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody>();

        rb.isKinematic = false;

        Vector3 randomDirection = (transform.position - transform.parent.position).normalized + Random.insideUnitSphere * 0.3f;
        rb.AddForce(randomDirection * detachForce);
        transform.parent = null;

        Destroy(gameObject, destroyDelay);
    }
}
