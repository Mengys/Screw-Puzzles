using UnityEngine;

public class TransparentOnMouse : MonoBehaviour
{
    private Material mat;
    private Color _originalColor;
    private bool _isMouseOver = false;

    void Start() {
        mat = GetComponent<Renderer>().material;
        _originalColor = mat.color;
    }

    void OnMouseEnter() {
        Debug.Log("enter");
        _isMouseOver = true;
    }

    void OnMouseExit() {
        Debug.Log("exit");
        _isMouseOver = false;
        mat.color = _originalColor;
    }

    void Update() {
        if (_isMouseOver && Input.GetMouseButton(0) && !GetComponent<BoltDetection>().IsDetached) {
            SetTransparent();
        } else if (!_isMouseOver || !Input.GetMouseButton(0)) {
            SetOpaque();
        }
    }

    void SetTransparent() {
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

    void SetOpaque() {
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
}
