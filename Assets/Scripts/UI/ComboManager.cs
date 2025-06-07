using UnityEngine;
using UnityEngine.UI;

public class ComboManager : MonoBehaviour
{
    public int combo;
    private float comboTimer = 3f;
    private float lastHitTime;
    public Text comboText; // ������ �� UI Text

    void Update()
    {
        if (Time.time - lastHitTime > comboTimer)
        {
            combo = 0;
            UpdateComboUI();
        }
    }

    public void RegisterHit()
    {
        combo++;
        lastHitTime = Time.time;
        UpdateComboUI();
    }

    void UpdateComboUI()
    {
        if (comboText != null)
            comboText.text = "Combo: " + combo.ToString();
    }
}