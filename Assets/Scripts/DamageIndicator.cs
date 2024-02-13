using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageIndicator : MonoBehaviour {
    protected TMP_Text[] damageTexts;
    [SerializeField] protected GameObject damageTextPrefab;
    protected Canvas _myCanvas;
    protected Camera _mainCam;

    protected void Awake() {
        _myCanvas = GetComponentInParent<Canvas>();
        _mainCam = FindObjectOfType<Camera>();
        Init();
    }
    public void Init() {
        damageTexts = new TMP_Text[10];
        for (int i = 0; i < 10; ++i) {
            GameObject obj = Instantiate(damageTextPrefab, transform);
            damageTexts[i] = obj.GetComponent<TMP_Text>();
            obj.SetActive(false);
        }
    }
    public void ShowDamage(Vector3 worldPosition, int damage) {
        Vector3 indicatePosition = _mainCam.WorldToScreenPoint(worldPosition) / _myCanvas.scaleFactor;
        for (int i = 0; i < 10; ++i) {
            if (damageTexts[i].gameObject.activeInHierarchy)
                continue;
            StartCoroutine(ProcessShowDamage(i, damage, indicatePosition));
            return;
        } 
    }

    IEnumerator ProcessShowDamage(int index, int damage, Vector2 position) {
        damageTexts[index].gameObject.SetActive(true);
        damageTexts[index].text = damage.ToString();
        damageTexts[index].transform.position = position;
        Vector2 targetPosition = new Vector2(position.x, position.y+30f);
        float t = 0f;
        yield return new WaitForEndOfFrame();
        while (t < 1f) {
            t += Time.deltaTime * 2;
            if (t > 1f)
                t = 1f;
            damageTexts[index].transform.position = Vector2.Lerp(position,targetPosition, t);
            yield return new WaitForEndOfFrame();
        }
        damageTexts[index].gameObject.SetActive(false);

    }
}
