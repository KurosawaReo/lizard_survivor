using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public GameObject settingsPanel; // 設定パネル
    public Slider bgmSlider; // BGM音量調整スライダー
    public Slider seSlider;  // SE音量調整スライダー
    public float slideSpeed = 10f; // スライド速度

    private bool isSettingsOpen = false; // 設定パネルの状態
    private Vector2 hiddenPosition; // 非表示位置（画面上部）
    private Vector2 visiblePosition; // 表示位置（画面中央）

    private RectTransform panelRect;

    private void Start()
    {
        panelRect = settingsPanel.GetComponent<RectTransform>();

        // 設定パネルを画面外（上）に配置
        hiddenPosition = new Vector2(0, Screen.height); // 画面上部外に配置
        visiblePosition = new Vector2(0, 80); // 画面内に移動

        panelRect.anchoredPosition = hiddenPosition; // 初期状態でパネルを非表示

        // スライダーの初期値をSoundManagerの現在の音量に設定
        if (SoundManager.Instance != null)
        {
            bgmSlider.value = SoundManager.Instance.bgmSource.volume;
            seSlider.value = SoundManager.Instance.audioSource.volume;
        }

        // スライダーの値が変更されたときのイベント登録
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        seSlider.onValueChanged.AddListener(SetSEVolume);
    }

    private void Update()
    {
        // エスケープキーを押すと設定パネルを開閉
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleSettings();
        }

        // パネルのスライド
        if (isSettingsOpen)
        {
            panelRect.anchoredPosition = Vector2.Lerp(panelRect.anchoredPosition, visiblePosition, Time.deltaTime * slideSpeed);
        }
        else
        {
            panelRect.anchoredPosition = Vector2.Lerp(panelRect.anchoredPosition, hiddenPosition, Time.deltaTime * slideSpeed);
        }
    }

    public void ToggleSettings()
    {
        isSettingsOpen = !isSettingsOpen; // 状態反転
    }

    public void SetBGMVolume(float volume)
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.bgmSource.volume = volume;
        }
    }

    public void SetSEVolume(float volume)
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.audioSource.volume = volume;
        }
    }
}
