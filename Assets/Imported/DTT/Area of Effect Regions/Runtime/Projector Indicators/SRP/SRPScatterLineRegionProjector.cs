using System.Collections;
using System.Collections.Generic;
using DTT.AreaOfEffectRegions;
using UnityEngine;

namespace DTT.AreaOfEffectRegions
{
    /// <summary>
    /// A scattered line region that uses projectors for effects.
    /// </summary>
    [ExecuteAlways]
    public class SRPScatterLineRegionProjector : MonoBehaviour
    {
#if UNIVERSAL_PACKAGE
        /// <summary>
        /// The prefab to instantiate from.
        /// </summary>
        [SerializeField]
        private SRPLineRegionProjector _linePrefab;

        /// <summary>
        /// Regions used te show the scattering.
        /// </summary>
        private SRPLineRegionProjector[] _regions;

        /// <summary>
        /// The number of spawned <see cref="SRPLineRegionProjector"/> for this scattered line region.
        /// </summary>
        public int NumberOfLines => _regions.Length;

        /// <summary>
        /// The fill amount of the lines.
        /// </summary>
        public float FillProgress
        {
            get => _fillProgress;
            set => _fillProgress = Mathf.Clamp01(value);
        }

        /// <summary>
        /// The fade amount of the lines.
        /// </summary>
        public float FadeAmount
        {
            get => _fadeAmount;
            set => _fadeAmount = Mathf.Clamp01(value);
        }

        /// <summary>
        /// The length of the lines.
        /// </summary>
        public float Length
        {
            get => _length;
            set => _length = Mathf.Max(value, 0f);
        }

        /// <summary>
        /// The length of the lines.
        /// </summary>
        public float Arc
        {
            get => _arc;
            set => _arc = Mathf.Clamp(value, 0f, 360f);
        }

        /// <summary>
        /// The width of the lines.
        /// </summary>
        public float Width
        {
            get => _width;
            set => _width = Mathf.Max(value, 0f);
        }

        [Header("Properties")]
        /// <summary>
        /// The progress of filling the line.
        /// </summary>
        [SerializeField]
        [Range(0, 1)]
        private float _fillProgress;

        /// <summary>
        /// The fading amount of the line.
        /// </summary>
        [SerializeField]
        [Range(0, 1)]
        private float _fadeAmount;

        /// <summary>
        /// The length of the line.
        /// </summary>
        [SerializeField]
        [Min(0)]
        private float _length = 3;

        /// <summary>
        /// The width of the line.
        /// </summary>
        [SerializeField]
        [Min(0)]
        private float _width = 1;

        /// <summary>
        /// The angle for the arc of all the regions.
        /// </summary>
        [Range(0, 360)]
        [SerializeField]
        private float _arc;
        
        /// <summary>
        /// Depth to project the object.
        /// </summary>
        [SerializeField]
        [Min(0)]
        private float _depth = 10;

        /// <summary>
        /// The previous amount of children. Used for checking for new lines.
        /// </summary>
        private int _prevCount = 0;

        /// <summary>
        /// The prefab used for creating the lines.
        /// </summary>
        internal SRPLineRegionProjector LinePrefab => _linePrefab;

        [Header("Colors")]
        /// <summary>
        /// The base color for the lines.
        /// </summary>
        [SerializeField]
        private Color _lineBaseColor;

        /// <summary>
        /// The fill color for the lines.
        /// </summary>
        [SerializeField]
        private Color _lineFillColor;

        /// <summary>
        /// Generate the SRP decal projector.
        /// </summary>
        public void GenerateProjector()
        {
            foreach (var line in _regions)
            {
                line.GenerateProjector();
            }
        }
        /// <summary>
        /// Gets the lines.
        /// </summary>
        private void OnEnable()
        {
            _regions = GetComponentsInChildren<SRPLineRegionProjector>();
            _prevCount = transform.childCount;
        }

        /// <summary>
        /// Updates and instantiates the lines.
        /// </summary>
        private void Update()
        {
            if (_prevCount != transform.childCount)
                _regions = GetComponentsInChildren<SRPLineRegionProjector>();

            UpdateLines();
        }

        /// <summary>
        /// Update the line projectors if there are any changes made in the inspector.
        /// </summary>
        private void OnValidate()
        {
            if (_prevCount != transform.childCount || _prevCount == 0)
                _regions = GetComponentsInChildren<SRPLineRegionProjector>();

            UpdateLines();
        }

        /// <summary>
        /// Adds lines to the scatter.
        /// </summary>
        /// <param name="amount">The amount of lines to add to the scatter.</param>
        public void Add(int amount)
        {
            for (int i = 0; i < amount; i++)
                Instantiate(_linePrefab, transform);
        }

        /// <summary>
        /// Removes lines from the scatter.
        /// </summary>
        /// <param name="amount">The amount of lines to remove.</param>
        public void Remove(int amount)
        {
            amount = Mathf.Clamp(amount, 0, transform.childCount);
            for (int i = 0; i < amount; i++)
            {
                if (transform.childCount == 1)
                    break;

                GameObject toDestroy = transform.GetChild(transform.childCount - 1).gameObject;
                if (Application.isPlaying)
                    Destroy(toDestroy);
                else
                    DestroyImmediate(toDestroy);
            }
        }
        /// <summary>
        /// Updates every line and its projector properties.
        /// </summary>
        public void UpdateLines()
        {
            if (_regions.Length == 0)
                return;

            for (int i = 0; i < _regions.Length; i++)
            {
                if (_regions.Length > 1)
                    _regions[i].Angle = _arc / (_regions.Length - 1) * i - _arc / 2;
                else
                    _regions[i].Angle = 0;

                _regions[i].Color = _lineBaseColor;
                _regions[i].FillColor = _lineFillColor;
                _regions[i].Width = _width;
                _regions[i].Length = _length;
                _regions[i].Depth = _depth;
                _regions[i].FillProgress = _fillProgress;
                _regions[i].FadeAmount = _fadeAmount;
                _regions[i].UpdateProjectors();
            }

            _prevCount = transform.childCount;
        }
#endif

    }
}