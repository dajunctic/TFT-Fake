using UnityEngine;
using System;
using System.Collections;

namespace Dajunctic.SkillSystem.Graph
{
    public abstract class SkillNode : ScriptableObject
    {
        [HideInInspector] public string guid;
        [HideInInspector] public Vector2 position;

        protected SkillExecutionContext _context;
        private Action _onComplete;

        /// <summary>
        /// Khởi tạo node với context và callback trước mỗi lần chạy.
        /// </summary>
        public void Init(SkillExecutionContext context, Action onComplete)
        {
            _context = context;
            _onComplete = onComplete;
            OnInit();
        }

        /// <summary>
        /// Override để thực hiện logic khởi tạo riêng của từng node.
        /// </summary>
        protected virtual void OnInit() { }

        /// <summary>
        /// Thực thi node. Khi xong việc, gọi TriggerComplete().
        /// </summary>
        public virtual void Execute()
        {
            TriggerComplete();
        }

        /// <summary>
        /// Gọi khi node hoàn thành, kích hoạt các node tiếp theo trong graph.
        /// </summary>
        public void TriggerComplete()
        {
            _onComplete?.Invoke();
        }

        /// <summary>
        /// Đặt lại trạng thái của node (gọi khi graph được reset hoặc chạy lại).
        /// </summary>
        public virtual void Reset()
        {
            _context = null;
            _onComplete = null;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class NodeInputAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Field)]
    public class NodeOutputAttribute : Attribute { }
}
