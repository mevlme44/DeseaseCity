using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TMP_Text Text;

    void Update() {
        if (!City.Instance) return;

        Text.text = City.Instance.CurrentDate.ToString();
    }
}
