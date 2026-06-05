using UnityEngine;
using UnityEngine.UI;

//[ExecuteInEditMode] // Runs in the Unity Editor without pressing Play
public class SyncSprites : MonoBehaviour
{
    private Image uiImage;
    private SpriteRenderer spriteRenderer;

    void Update()
    {
        if (uiImage == null) uiImage = GetComponent<Image>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

        if (uiImage != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = uiImage.sprite;
        }
    }
}