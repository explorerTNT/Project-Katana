using UnityEngine;
using UnityEngine.UI;

public class ComboManager : MonoBehaviour
{
    public int combo;
    private float comboTimer = 3f;
    private float lastHitTime;
    public Text comboText; // —сылка на UI Text

    void Update()
    {
        if (Time.unscaledTime - lastHitTime > comboTimer)
        {
            combo = 0;
            UpdateComboUI();
        }
    }

    public void RegisterHit()
    {
        combo++;
        lastHitTime = Time.unscaledTime;
        UpdateComboUI();
    }

    void UpdateComboUI()
    {
        if (comboText != null)
            comboText.text = "Combo: " + combo.ToString();
    }
}