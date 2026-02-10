using UnityEngine;

namespace AllIn13DShader
{
    [CreateAssetMenu(fileName = "RGBAPackerValues", menuName = "AllIn1 3D Shader/RGBA Packer Values")]
    public class RGBAPackerValues : ScriptableObject
    {
        [SerializeField] public Texture2D rChannelTexture;
        [SerializeField] public bool rChannelDefaultWhite = false;
        [SerializeField] public Texture2D gChannelTexture;
        [SerializeField] public bool gChannelDefaultWhite = false;
        [SerializeField] public Texture2D bChannelTexture;
        [SerializeField] public bool bChannelDefaultWhite = false;
        [SerializeField] public Texture2D aChannelTexture;
        [SerializeField] public bool aChannelDefaultWhite = false;
		
        [SerializeField] public TextureSizes textureSizes = TextureSizes._512;
        [SerializeField] public FilterMode filtering = FilterMode.Bilinear;
		
        [System.NonSerialized] public Texture2D createdRGBATexture;
    }
}