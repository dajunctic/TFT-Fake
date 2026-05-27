using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;

namespace Dajunctic
{
    public class AISystem : MonoBehaviour, IGameSystem
    {
        private GameSystemManager _manager;

        public async Task LoadDataAsync()
        {
            await Task.CompletedTask;
        }

        public void Initialize(GameSystemManager manager)
        {
            _manager = manager;
            this.RegisterListener<GameplayPhaseChangedEvent>(OnPhaseChanged);
        }

        private void OnPhaseChanged(GameplayPhaseChangedEvent evt)
        {
            if (evt.Phase == GameplayPhase.Planning)
            {
                PerformAILogic();
            }
        }

        private void PerformAILogic()
        {
            Debug.Log("<color=red>AISystem: AI is performing logic (buying, leveling)...</color>");
            
        }

        public void Shutdown()
        {
            this.RemoveListener<GameplayPhaseChangedEvent>(OnPhaseChanged);
        }
    }
}
