using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsButton : MonoBehaviour
{
    public GameObject CreditsPanel;
    public void openCreditsPanel()
    {
        if (CreditsPanel != null)
        {
            bool isActive = CreditsPanel.activeSelf;
            CreditsPanel.SetActive(!isActive);
        }
        else
        {
            Debug.LogError("CreditsPanel is not assigned in the inspector.");
        }
    }
    
}
