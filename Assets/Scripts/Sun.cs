using UnityEngine;

public class Sun : MonoBehaviour
{
    void Update()
    {
        transform.localRotation = Quaternion.Euler((float)City.Instance.CurrentDate.TimeOfDay.TotalSeconds / 24, 90, 0);
    }
}
