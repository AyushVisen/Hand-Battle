using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Ayush
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private AudioClip _bgmClip;

        [SerializeField] private AudioClip _clickClip;

        [SerializeField] private TextMeshProUGUI _highScoreText;

        [SerializeField] private Toggle _sfxToogle;

        [SerializeField] private Toggle _musicToggle;

        private PlayerDataService _playerDataService;
        private AudioService _audioService;

        void Start()
        {
            if (GameManager.Instance == null)
                return;

            GameManager.Instance.TryGetService(out _playerDataService);
            GameManager.Instance.TryGetService(out _audioService);

            _highScoreText.SetText(_playerDataService.PlayerData.WinningStreak.ToString());
            _sfxToogle.isOn = _playerDataService.PlayerData.SFX_On;
            _musicToggle.isOn = _playerDataService.PlayerData.Music_On;
            _audioService.PlayBGM(true, _bgmClip);
        }

        public void OnSfxToggle(bool value)
        {
            _audioService.SetSfxVolume(value);
            _playerDataService.PlayerData.SFX_On = value;
        }

        public void OnMusicToggle(bool value)
        {
            _audioService.SetMusicVolume(value);
            _playerDataService.PlayerData.Music_On = value;
        }

        public void OnPlayClick()
        {
            _audioService.PlaySfx(_clickClip);
            SceneManager.LoadScene(Constants.GamePlayScene);
        }
    }
}