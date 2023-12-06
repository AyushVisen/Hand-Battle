using System;
using Ayush;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HandElementCard : MonoBehaviour
{
    [SerializeField] private Image _bg;

    [SerializeField] private Image _icon;

    [SerializeField] private TextMeshProUGUI _cardName;

    private MoveType _moveType;
    private Action<MoveType> CardClicked;
    private AudioService _audioService;
    private AudioClip _clickSfx;

    public void Init(MoveType moveType, HandElementDetail handElementDetail, AudioService audioService,
        Action<MoveType> onCardClick)
    {
        _moveType = moveType;
        _clickSfx = handElementDetail.SelectAudioClip;
        _bg.color = handElementDetail.CardBgColor;
        _icon.sprite = handElementDetail.Icon;
        _cardName.SetText(_moveType.ToString());
        CardClicked = onCardClick;
        _audioService = audioService;
    }

    public void OnClick()
    {
        _audioService.PlaySfx(_clickSfx);
        CardClicked?.Invoke(_moveType);
    }

    private void OnDestroy()
    {
        CardClicked = null;
    }
}