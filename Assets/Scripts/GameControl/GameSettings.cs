using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    [SerializeField] private Toggle _leftHandedToggle;
    [SerializeField] private Toggle _aimPathToggle;
    [SerializeField] GameObject _warningText;
    void Start()
    {
        _leftHandedToggle.isOn = GlobalVariables.leftHanded;
        _aimPathToggle.isOn = GlobalVariables.showAimPath;
    }
    public void SetLeftHanded(bool isLeftHanded)
    {
        GlobalVariables.leftHanded = isLeftHanded;
        Debug.Log("Left Handed: " + isLeftHanded);
    }

    public void SetAimPath(bool showAimPath)
    {
        GlobalVariables.showAimPath = showAimPath;
        Debug.Log("Show Aim Path: " + showAimPath);
        _warningText.SetActive(showAimPath);
        Debug.Log("Warning Text Active: " + _warningText.activeSelf);
    }
}
