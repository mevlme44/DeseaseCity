using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-20)]
public class BarChart : MonoBehaviour
{
    public Image Health, Invisible, Infected, Recovered, Dead;
    public TMP_Text HealthText, InvisibleText, InfectedText, RecoveredText, DeadText;

    void Awake()
    {
        Citizen.ChangeStatus += OnChangeStatus;
    }

    void OnChangeStatus() {
        StartCoroutine(DoUpdateGraph());
    }

    IEnumerator DoUpdateGraph() {
        yield return new WaitForSeconds(0.1f);

        var count = City.Citizens.Count;
        var health = City.Citizens.FindAll(c => c.CurrentStatus == Citizen.Status.Health).Count;
        var invisible = City.Citizens.FindAll(c => c.CurrentStatus == Citizen.Status.InvisibleInfected).Count;
        var infected = City.Citizens.FindAll(c => c.CurrentStatus == Citizen.Status.Infected).Count;
        var recovered = City.Citizens.FindAll(c => c.CurrentStatus == Citizen.Status.Recovered).Count;
        var dead = City.Citizens.FindAll(c => c.CurrentStatus == Citizen.Status.Dead).Count;
        Health.fillAmount = (float)health / count;
        HealthText.text = $"{health}/{count}";

        Invisible.fillAmount = (float)invisible / count;
        InvisibleText.text = $"{invisible}/{count}";

        Infected.fillAmount = (float)infected / count;
        InfectedText.text = $"{infected}/{count}";

        Recovered.fillAmount = (float)recovered / count;
        RecoveredText.text = $"{recovered}/{count}";

        Dead.fillAmount = (float)dead / count;
        DeadText.text = $"{dead}/{count}";
    }
}
