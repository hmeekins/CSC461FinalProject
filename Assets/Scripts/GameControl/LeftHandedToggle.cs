using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftHandedToggle : MonoBehaviour
{
    [SerializeField] private Toggle toggle;
    void Start()
    {
        toggle.isOn = GlobalVariables.leftHanded;
    }
    public void SetLeftHanded(bool isLeftHanded)
    {
        GlobalVariables.leftHanded = isLeftHanded;
    }
}
