using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SplashScreenManager : MonoBehaviour
{
    public GameObject splashPanel;
    public GameObject furnitureButtonsPanel;
    public GameObject transformPanel;
    public Image blackCover; // full-screen black image that hides AR init

    private void Start()
    {
        // AR always runs — never disabled
        // Black cover hides the ugly AR initialization flash
        if (blackCover != null) blackCover.gameObject.SetActive(true);
        if (splashPanel != null) splashPanel.SetActive(true);
        if (furnitureButtonsPanel != null) furnitureButtonsPanel.SetActive(false);
        if (transformPanel != null) transformPanel.SetActive(false);

        // After 0.8s AR has initialized — fade out the black cover
        StartCoroutine(FadeOutCover());
    }

private IEnumerator FadeOutCover()
    {
        yield return new WaitForSeconds(2f);
        if (blackCover != null) blackCover.gameObject.SetActive(false);
    }

    public void OnStartScanning()
    {
        if (splashPanel != null) splashPanel.SetActive(false);
        if (furnitureButtonsPanel != null) furnitureButtonsPanel.SetActive(true);
        if (transformPanel != null) transformPanel.SetActive(false);
    }
}
