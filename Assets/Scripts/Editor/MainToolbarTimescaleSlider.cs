using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;


// public class MainToolbarTimescaleSlider
// {
//     const float MinTimeScale = 0.0f;
//     const float MaxTimeScale = 5.0f;
//     const float Padding = 10.0f;
//
//                     [MainToolbarElement("Timescale/Slider", defaultDockPosition = MainToolbarDockPosition.Middle)]
//     public static MainToolbarElement TimeSlider()
//     {
//         var content = new MainToolbarContent("Time Scale", "Time Scale Slider");
//         var slider = new MainToolbarSlider(content, Time.time, MinTimeScale, MaxTimeScale, OnSliderValueChanged);
//         return slider;
//     }
//     
//     
//     private static void OnSliderValueChanged(float newValue)
//     {
//         Time.timeScale = newValue;
//     }
// }
