using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cooldown : MonoBehaviour {
    protected PlayerDeckManager _manager;
    protected TMP_Text _text;
    protected Slider _slider;
    protected float _cooldownTime = 4f;

    protected void Awake() {
        _manager = GetComponentInParent<PlayerDeckManager>();
        _text = GetComponentInChildren<TMP_Text>();
        _slider = GetComponentInChildren<Slider>();
        _text.enabled = false;
        _slider.gameObject.SetActive(false);
    }
    public void Activate() {
        StartCoroutine(ActivateCoolDown());
    }
    IEnumerator ActivateCoolDown() {
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < _manager.Hand.Cards.Length; ++i) {
            if (_manager.Hand.Cards[i] == null)
                continue;
            _manager.Hand.Cards[i].SetLockActive(true);
        }
        _text.enabled = true;
        _slider.gameObject.SetActive(true);
        StartCoroutine(ActivateTimer());
        yield return new WaitForSeconds(_cooldownTime);
        for (int i = 0; i < _manager.Hand.Cards.Length; ++i) {
            if (_manager.Hand.Cards[i] == null)
                continue;
            _manager.Hand.Cards[i].SetLockActive(false);
        }
        _text.enabled = false;
        _slider.gameObject.SetActive(false);

        _manager.Hand.GetRequiredCards();
        _manager.RiichiController.ProceedRiichi();
    }

    IEnumerator ActivateTimer() {
        float time = 0f;
        _slider.value = 0f;
        yield return new WaitForEndOfFrame();
        while (time < _cooldownTime) {
            time += Time.deltaTime;
            if (time > _cooldownTime)
                time = _cooldownTime;
            _slider.value = time / _cooldownTime;
            yield return new WaitForEndOfFrame();
        }
    }


}
