using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class SliderController : MonoBehaviour
{
    public Slider slider;
    public Text sliderHandlerText;
    public string unit;
    
    public int Value =>  (int) slider.value;
    
    public void UpdateSliderHandlerText()
    {
        sliderHandlerText.text = Value + " " + unit;
    }
}
