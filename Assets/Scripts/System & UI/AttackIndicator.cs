using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AttackIndicator : MonoBehaviour {
    protected TMP_Text[] tmpText;

    void Awake() {
        tmpText ??= GetComponentsInChildren<TMP_Text>();
    }

    public void UpdateAttackDamage(MagicModel.ElementalAttribute elementalAttribute, int value) {
        tmpText ??= GetComponentsInChildren<TMP_Text>();
        tmpText[(int)elementalAttribute].text = value.ToString();
    }
}
