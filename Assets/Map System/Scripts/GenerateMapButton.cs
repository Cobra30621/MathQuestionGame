using System;
using System.Collections;
using System.Collections.Generic;
using Map;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class GenerateMapButton : MonoBehaviour
{
    [Required]
    public Button button;


    private void Awake()
    {
        button.onClick.AddListener(GenerateMap);
    }

    public void GenerateMap()
    {
        MapManager.Instance.InitializedMap();
    }
}
