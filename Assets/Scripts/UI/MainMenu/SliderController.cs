using System.Text;
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
        StringBuilder sb = new StringBuilder()
            .Append(Value)
            .Append(" ")
            .Append(unit);
        sliderHandlerText.text = sb.ToString();
    }
}
