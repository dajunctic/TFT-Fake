using UnityEngine;

namespace DTT.Utils.Extensions.Demo
{
    public class RichTextBehaviour : MonoBehaviour
    {
        private void Awake()
        {
            // Add bold styling to your strings before logging it to the console.
            string boldText = "This text needs to be logged in bold.";
            Debug.Log(boldText.Bold());

            // Add italic styling to your strings
            string italicText = "This text needs to be logged in italic styling.";
            Debug.Log(italicText.Italics());

            // Add color to your strings.
            string coloredText = "This text needs to be logged in a nice red color.";
            Debug.Log(coloredText.Color(new Color32(230, 84, 64, 255)));
        }
    }
}
