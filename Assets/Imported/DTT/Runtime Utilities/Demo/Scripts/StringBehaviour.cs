
using UnityEngine;

namespace DTT.Utils.Extensions.Demo
{
    public class StringBehaviour : MonoBehaviour
    {
        private void Awake()
        {
            /* Use string extension methods to modify text for display. */
            string spaced = "MyString".AddSpacesBeforeCapitals(); //: My String
            string readable = "MY_CONSTANT".FromAllCapsToReadableFormat(); //: My Constant
            string constant = "My Readable".FromReadableFormatToAllCaps(); //: MY_READABLE

            /* Use string extensions to strip html tags from your text. */
            string stripped = "<p>Paragraph text</p>".StripHtmlTags(); //: Paragraph text
        }

        private void OnGUI()
        {
            /* Use string extensions to truncate your string after a certain length and at a character to indicate its end. */
            string ellipsis = "This text is to long".Ellipsis(5, GUI.skin.font); //: This text is...
        }
    }
}
