    using UnityEngine;
    using UnityEngine.UI; // Required for UI elements

    public class MyToggleScript : MonoBehaviour
    {
        public void OnToggleValueChanged(bool isOn)
        {
            if (isOn)
            {
                GlobalVariables.leftHanded = true;
            }
            else
            {
                GlobalVariables.leftHanded = false;
            }
        }
    }