using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class VolumeManager : MonoBehaviour
{
    public Slider volumeSlider;
    public float masterVolume;
    public TextMeshProUGUI volume_text_number;

    private void Start()
    {
        AudioListener.volume = volumeSlider.value;
    }

    public void adj_volume()
    {
        masterVolume = volumeSlider.value;
        AudioListener.volume = masterVolume;
    }

    private void Update()
    {
        float numHold = volumeSlider.value * 100;
        volume_text_number.text = numHold.ToString("F0") + "%";
    }
}
