using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public Slider bgmSlider; // BGM音量調整スライダー
    public Slider seSlider;  // SE音量調整スライダー

    private void Start()
    {
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
