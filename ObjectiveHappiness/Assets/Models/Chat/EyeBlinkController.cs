using UnityEngine;

public class EyeBlinkController : MonoBehaviour
{
    public Renderer characterRenderer;
    public Texture openEyesTexture;
    public Texture closedEyesTexture;

    // Appelée par l'Animation Event
    public void CloseEyes()
    {
        characterRenderer.material.SetTexture("_MainTex", closedEyesTexture);
    }

    // Si tu veux rouvrir les yeux sur un autre event
    public void OpenEyes()
    {
        characterRenderer.material.SetTexture("_MainTex", openEyesTexture);
    }
}
