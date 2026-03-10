using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;

namespace Dajunctic
{
    public class CarouselSystem : MonoBehaviour, IGameSystem
    {
        private GameSystemManager _manager;
        private List<ChampionActor> _unitsInCarousel = new List<ChampionActor>();
        private Transform _center;
        
        [SerializeField] private float rotationSpeed = 30f;
        [SerializeField] private float radius = 5f;

        public async Task LoadDataAsync()
        {
            await Task.CompletedTask;
        }

        public void Initialize(GameSystemManager manager)
        {
            _manager = manager;
            // Center could be found by tag or assigned via a Binder
        }

        public void SetupCarousel(List<ChampionData> availableUnits)
        {
            Debug.Log("<color=yellow>CarouselSystem: Setting up Carousel!</color>");
            // Spawn units in a circle
        }

        public void StartRotation()
        {
            // Rotate the _center transform
        }

        private void Update()
        {
            if (_center != null)
            {
                _center.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            }
        }

        public void Shutdown()
        {
            _unitsInCarousel.Clear();
        }
    }
}
