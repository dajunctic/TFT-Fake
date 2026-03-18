using TMPro;
using UnityEngine;

namespace Dajunctic
{
    public class ChatMessageUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text messageText;

        public void Setup(ChatMessage message)
        {
            string color = GetColorByType(message.Type);
            string timeStr = message.Timestamp.ToString("HH:mm");
            
            messageText.text = $"<color=#888888>[{timeStr}]</color> <color={color}><b>{message.Sender}:</b></color> {message.Content}";
        }

        private string GetColorByType(ChatMessageType type)
        {
            return type switch
            {
                ChatMessageType.System => "#FFD700", // Gold
                ChatMessageType.Whisper => "#FF00FF", // Magenta
                _ => "#00FFFF" // Cyan/Global
            };
        }
    }
}
