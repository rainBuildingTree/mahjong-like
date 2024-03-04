using System;
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
    public void CheckHuroability(int bonusCardCode)
    {
        _isChiable = false;
        _isPonable = false;

        int[] handCardCodes = new int[_manager.Hand.Size];
        for (int i = 0; i < handCardCodes.Length; ++i)
        {
            if (_manager.Hand.Cards[i] is MergedCard)
            {
                handCardCodes[i] = -1;
                continue;
            }
            handCardCodes[i] = _manager.Hand.Cards[i].Code;
        }

        for (int i = 0; i < handCardCodes.Length-1; ++i)
        {
            if (handCardCodes[i] < 0) continue;
            int[] codes = new int[3] { handCardCodes[i], handCardCodes[i + 1], bonusCardCode };
            Array.Sort(codes);
            _isChiable = _isChiable || IsChiable(codes);
            _isPonable = _isPonable || IsPonable(codes);
            // TODO: save chiable or ponable indice of hand
        }
        _chiButton.gameObject.SetActive(_isChiable);
        _ponButton.gameObject.SetActive(_isPonable);
    }
    private bool IsChiable(int[] codes)
    {
        if (!((codes[0] / 9 == codes[1] / 9) && (codes[1] / 9 == codes[2] / 9)))
            return false;
        if (!(codes[2] - codes[1] == 1) || !(codes[1] - codes[0] == 1))
            return false;
        return true;
    }
    private bool IsPonable(int[] codes)
    {
        if (!((codes[0] / 9 == codes[1] / 9) && (codes[1] / 9 == codes[2] / 9)))
            return false;
        if (!(codes[0] == codes[1]) || !(codes[1] == codes[2]))
            return false;
        return true;
    }





}
