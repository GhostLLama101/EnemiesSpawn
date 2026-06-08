using UnityEngine;

public class VolumeSlideController : MonoBehaviour
{
   public GameObject slider;
   public GameObject soundManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        soundManager.GetComponent<SoundManager>().volume = slider.GetComponent<UnityEngine.UI.Slider>().value;
    }


}
