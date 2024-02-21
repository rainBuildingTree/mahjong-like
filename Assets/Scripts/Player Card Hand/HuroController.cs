using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuroController : MonoBehaviour
{
    private PlayerDeckManager _manager;
    private PonButton _ponButton;
    private ChiButton _chiButton;
    private bool _isChiable = true;
    private bool _isPonable = false;

    public void Init()
    {
        _manager = GetComponentInParent<PlayerDeckManager>();
        _ponButton = FindObjectOfType<PonButton>();
        _chiButton = FindObjectOfType<ChiButton>();
        _ponButton.gameObject.SetActive(false);
        _chiButton.gameObject.SetActive(false);
    }


}
