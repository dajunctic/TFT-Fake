using System;
using UnityEngine;
using UnityEngine.UI;

namespace DTT.AreaOfEffectRegions.Demo
{
    public class ScatterLineSettings : MonoBehaviour
    {
        /// <summary>
        /// Slider that handle the fill value of the circle projector.
        /// </summary>
        [SerializeField]
        private Slider _fillProgress;
        
        /// <summary>
        /// Slider that handle the fade value of the circle projector.
        /// </summary>
        [SerializeField]
        private Slider _fadeAmount;
        
        /// <summary>
        /// Slider that handle the arc value of the circle projector.
        /// </summary>
        [SerializeField]
        private Slider _arc;

        /// <summary>
        /// Slider that handle the length value of the circle projector.
        /// </summary>
        [SerializeField]
        private Slider _length;
        
        /// <summary>
        /// Slider that handle the width value of the circle projector.
        /// </summary>
        [SerializeField]
        private Slider _width;

        /// <summary>
        /// Button that handle adding a line.
        /// </summary>
        [SerializeField]
        private Button _plusButton;

        /// <summary>
        /// Button that handle removing a line.
        /// </summary>
        [SerializeField]
        private Button _minusButton;

        /// <summary>
        /// Display the number of lines in the scatter indicator.
        /// </summary>
        [SerializeField]
        private Text _countInfo;

        /// <summary>
        /// Count of lines.
        /// </summary>
        private int _count;

        /// <summary>
        /// Current projector.
        /// </summary>
        private ScatterLineRegionProjector _projector;

        /// <summary>
        /// Initialize sliders values to match the projectors value.
        /// </summary>
        private void InitializeValues()
        {
            _fillProgress.value = _projector.FillProgress;
            _fadeAmount.value = _projector.FadeAmount;
            _arc.value = _projector.Arc;
            _length.value = _projector.Length;
            _width.value = _projector.Width;
        }
        
        /// <summary>
        /// Initialize the sliders min and max values.
        /// </summary>
        private void InitializeSlider()
        {
            _fillProgress.maxValue = 1;
            _fillProgress.minValue = 0;
            _fadeAmount.maxValue = 1;
            _fadeAmount.minValue = 0;
            _arc.maxValue = 360;
            _arc.minValue = 0;
            
            _length.maxValue = 100;
            _length.minValue = 25;
            
            _width.maxValue = 25;
            _width.minValue = 1;
        }

        /// <summary>
        /// Add listeners for slider value change.
        /// </summary>
        private void AddListener()
        {
            _fillProgress.onValueChanged.AddListener(UpdateFill);
            _fadeAmount.onValueChanged.AddListener(UpdateFade);
            _arc.onValueChanged.AddListener(UpdateArc);
            _plusButton.onClick.AddListener(AddLine);
            _minusButton.onClick.AddListener(RemoveLine);
            
            _length.onValueChanged.AddListener(UpdateLength);
            _width.onValueChanged.AddListener(UpdateWidth);
        }

        /// <summary>
        /// Remove listener for slider value change.
        /// </summary>
        private void RemoveListerner()
        {
            _fillProgress.onValueChanged.RemoveListener(UpdateFill);
            _fadeAmount.onValueChanged.RemoveListener(UpdateFade);
            _arc.onValueChanged.RemoveListener(UpdateArc);
            _plusButton.onClick.RemoveListener(AddLine);
            _minusButton.onClick.RemoveListener(RemoveLine);
            
            _length.onValueChanged.RemoveListener(UpdateLength);
            _width.onValueChanged.RemoveListener(UpdateWidth);
        }

        /// <summary>
        /// Add a line to the scatter.
        /// </summary>
        private void AddLine()
        {
            _projector.Add(1);
            _countInfo.text = (_projector.NumberOfLines+1).ToString();
        } 

        /// <summary>
        /// Remove a line to the scatter.
        /// </summary>
        private void RemoveLine()
        {
            _projector.Remove(1);
            _countInfo.text = _projector.NumberOfLines == 1 ? "1" : (_projector.NumberOfLines-1).ToString();
        }

        /// <summary>
        /// Update the fade value.
        /// </summary>
        /// <param name="fade">New ade value.</param>
        private void UpdateFade(float fade)
        {
            _projector.FadeAmount = fade;
            _projector.UpdateLines();
        }
        
        /// <summary>
        /// Update the arc value.
        /// </summary>
        /// <param name="arc">New arc value.</param>
        private void UpdateArc(float arc)
        {
            _projector.Arc = arc;
            _projector.UpdateLines();
        }
        
        /// <summary>
        /// Update the length value.
        /// </summary>
        /// <param name="length">New length value.</param>
        private void UpdateLength(float length)
        {
            _projector.Length = length;
            _projector.UpdateLines();
        }
        
        /// <summary>
        /// Update the width value.
        /// </summary>
        /// <param name="width">New width value.</param>
        private void UpdateWidth(float width)
        {
            _projector.Width = width;
            _projector.UpdateLines();
        }
        
        /// <summary>
        /// Update the fill value.
        /// </summary>
        /// <param name="arc">New fill value.</param>
        private void UpdateFill(float arc)
        {
            _projector.FillProgress = arc;
            _projector.UpdateLines();
        }

        /// <summary>
        /// Change the current projector.
        /// </summary>
        /// <param name="indic">New scatter line projector.</param>
        public void ChangeIndicator(ScatterLineRegionProjector indic)
        {
            RemoveListerner();
            InitializeSlider();
            _count = indic.NumberOfLines;
            _countInfo.text = _count.ToString();
            _projector = indic;
            InitializeIndicator();
            InitializeValues();
            AddListener();
        }

        /// <summary>
        /// Initialize the projector length value.
        /// </summary>
        private void InitializeIndicator() => _projector.Length = 15;
    }
}