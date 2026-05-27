using UnityEngine;

namespace DTT.AreaOfEffectRegions
{
    /// <summary>
    /// A region that exists of spread out line regions.
    /// </summary>
    [ExecuteAlways]
    public class ScatterLineRegion : MonoBehaviour
    {
        /// <summary>
        /// The amount the line is filled.
        /// </summary>
        public float FillProgress
        {
            get => _fillProgress;
            set => _fillProgress = Mathf.Clamp01(value);
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
            set => _arc = Mathf.Repeat(value, 360f);
        }


        /// <summary>
        /// The width of the lines.
        /// </summary>
        public float Width
        {
            get => _width;
            set => _width = Mathf.Max(value, 0f);
        }
        
        /// <summary>
        /// The prefab to instantiate from.
        /// </summary>
        [SerializeField]
        private LineRegionBase _linePrefab;
        
        /// <summary>
        /// Regions used te show the scattering.
        /// </summary>
        private LineRegionBase[] _regions;

        /// <summary>
        /// The progress of filling the line.
        /// </summary>
        [SerializeField]
        [Range(0, 1)]
        private float _fillProgress;
        
        /// <summary>
        /// The length of the line.
        /// </summary>
        [SerializeField]
        [Range(0, 5)]
        private float _length = 3;

        /// <summary>
        /// The width of the line.
        /// </summary>
        [SerializeField]
        [Range(0, 1)]
        private float _width = 1;

        /// <summary>
        /// The angle for the arc of all the regions.
        /// </summary>
        [Range(0, 360)]
        [SerializeField]
        private float _arc;

        /// <summary>
        /// The previous amount of children. Used for checking for new lines.
        /// </summary>
        private int _prevCount;

        /// <summary>
        /// The prefab used for creating the lines.
        /// </summary>
        internal LineRegionBase LinePrefab => _linePrefab;
        
        /// <summary>
        /// Gets the lines.
        /// </summary>
        private void OnEnable()
        {
            _regions = GetComponentsInChildren<LineRegionBase>();
            _prevCount = transform.childCount;
        }

        /// <summary>
        /// Updates and instantiates the lines.
        /// </summary>
        private void Update()
        {
            if(_prevCount != transform.childCount)
                _regions = GetComponentsInChildren<LineRegionBase>();

            for (int i = 0; i < _regions.Length; i++)
            {
                if (_regions.Length > 1)
                    _regions[i].Angle = _arc / (_regions.Length - 1) * i - _arc / 2;
                else
                    _regions[i].Angle = 0;
                _regions[i].Width = _width;
                _regions[i].Length = _length;
                _regions[i].FillProgress = _fillProgress;
            }
            
            _prevCount = transform.childCount;
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
                if (transform.childCount == 0)
                    continue;
                
                GameObject toDestroy = transform.GetChild(transform.childCount - 1).gameObject;
                if(Application.isPlaying)
                    Destroy(toDestroy);
                else
                    DestroyImmediate(toDestroy);
            }
        }
    }
}