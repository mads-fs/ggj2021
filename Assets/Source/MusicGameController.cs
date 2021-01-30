using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicGameController : MonoBehaviour
{
    [Tooltip("Line that renders points.")]
    public LineRenderer Line;
    [Tooltip("Put sliders in order.")]
    public List<Slider> Sliders;
    public float LowestY = 1f;
    public float HighestY = 6.5f;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Slider slider in Sliders)
        {
            slider.onValueChanged.AddListener((value) =>
            {
                HandleValueChange(value, slider);
            });
        }
    }

    private void HandleValueChange(float value, Slider slider)
    {
        int index = Sliders.IndexOf(slider);
        float newY = Mathf.Clamp(HighestY * value, LowestY, HighestY);
        Vector3 position = Line.GetPosition(index + 1);
        Vector3 newPosition = new Vector3(position.x, newY, position.z);
        Line.SetPosition(index + 1, newPosition);
    }
}
