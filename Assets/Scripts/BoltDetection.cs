using System;
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

    public bool IsDetached => isDetached;

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color;
        }
    }

    private void Update() {
        TryDetach();
        ApplyTransparency();
    }

    private void ApplyTransparency() {

        Vector2 screenPos = Vector2.zero;
        if (Input.GetMouseButton(0)) {
            screenPos = Input.mousePosition;
        } else if (Input.touchCount > 0) {
            screenPos = Input.GetTouch(0).position;
        } else {
            if (isTransparent) ResetTransparency();
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out RaycastHit hit)) {
            if (hit.collider.gameObject == gameObject) {
                if (Input.GetMouseButton(0) || Input.touchCount > 0) {
                    //SetTransparent();
                    //SetTransparent1();
                } else if (Input.GetMouseButtonUp(0) || Input.touchCount == 0) {
                    //ResetTransparency();
                    //SetOpaque1();
                }
            } else {
                // Если клик был вне объекта — вернуть прозрачность обратно
                //if (isTransparent) SetOpaque1();
            }
        } else {
            // Если никуда не попали — вернуть прозрачность обратно
            //if (isTransparent) SetOpaque1();
        }
    }

    private void SetTransparent()
    {
        if (objectRenderer == null || isTransparent) return;

        SetMaterialTransparent(); // готовим материал к прозрачности

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

    void SetTransparent1() {
        if (objectRenderer == null) return;

        Material mat = objectRenderer.material;

        mat.SetFloat("_Surface", 1); // Transparent
        Color c = mat.color;
        c.a = 0.3f;
        mat.color = c;
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
    }

    void SetOpaque1() {
        if (objectRenderer == null) return;
        
        Material mat = objectRenderer.material;

        mat.SetFloat("_Surface", 0); // Opaque
        Color c = mat.color;
        c.a = 1f;
        mat.color = c;
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        mat.SetInt("_ZWrite", 1);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.DisableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;
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

        Vector3 randomDirection = (transform.position - transform.parent.position).normalized + UnityEngine.Random.insideUnitSphere * 0.3f;
        rb.AddForce(randomDirection * detachForce);
        transform.parent = null;

        Destroy(gameObject, destroyDelay);
    }
}
