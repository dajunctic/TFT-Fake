

namespace ShaderCrew.TheToonShader
{
    public static class TheToonShaderConstants
    {
        public const string THETOONSHADER_VERSION_3D = "1.3.2";
        public const string THETOONSHADER_VERSION_2D = "1.0.0";


        public const string THETOONSHADER_IDENTIFIER_PROPERTY = "_TheToonShaderIdentifier";
        public const string THETOONSHADER2D_IDENTIFIER_PROPERTY = "_TheToonShader2DIdentifier"; //temp
        public static readonly string THETOONSHADER_INSTANCE_PREFIX = "TTS - Instance";

        public static readonly string TTS_SHADER_DEFAULT_KEY = "default";

        public static readonly string TTS_URP2D_SHADER_STANDARD_KEY = "TTS_URP2D";


        public static readonly string TTS_SHADER_STANDARD_WITH_OUTLINE_KEY = "TTS_StandardWithOutline";
        public static readonly string TTS_SHADER_STANDARD_KEY = "TTS_Standard";

        public static readonly string TTS_SHADER_STS_WITH_OUTLINE_KEY = "TTS_STSWithOutline";
        public static readonly string TTS_SHADER_STS_KEY = "TTS_STS";

        public static readonly string TTS_SHADER_NAME_WITH_OUTLINE_SUFFIX = "_WithOutline";

        public static readonly string TTS_SHADER_NAME_WITH_STS_SUFFIX = "_WithSTS";

        public static readonly string TTS_SHADER_NAME_FOLDER = "TheToonShader";
        public static readonly string TTS_SHADER_NAME = "/TheToonShader";


        public static readonly string TTS_SHADER_FILENAME_2D_SHADERGRAPH = TTS_SHADER_NAME_FOLDER + "/URP2D" + "/TheToonShader_URP2D" ;


        /// BiRP ///
        public static readonly string TTS_SHADER_NAME_BIRP_FOLDER = TTS_SHADER_NAME_FOLDER + "/BiRP";
        public static readonly string TTS_SHADER_NAME_BIRP = TTS_SHADER_NAME_BIRP_FOLDER + "/TheToonShader";
        public static readonly string TTS_SHADER_NAME_BIRP_WITH_OUTLINE = TTS_SHADER_NAME_BIRP + TTS_SHADER_NAME_WITH_OUTLINE_SUFFIX;

        public static readonly string TTS_SHADER_NAME_BIRP_WITH_STS = TTS_SHADER_NAME_BIRP + TTS_SHADER_NAME_WITH_STS_SUFFIX;
        public static readonly string TTS_SHADER_NAME_BIRP_WITH_OUTLINE_AND_STS = TTS_SHADER_NAME_BIRP_WITH_OUTLINE + TTS_SHADER_NAME_WITH_STS_SUFFIX;

        /// URP ///
        public static readonly string TTS_SHADER_NAME_URP_FOLDER = TTS_SHADER_NAME_FOLDER + "/URP";

        /// URP 2020///
        public static readonly string TTS_SHADER_NAME_URP_2020 = TTS_SHADER_NAME_URP_FOLDER + "/2020" + TTS_SHADER_NAME;
        public static readonly string TTS_SHADER_NAME_URP_2020_WITH_OUTLINE = TTS_SHADER_NAME_URP_2020 + TTS_SHADER_NAME_WITH_OUTLINE_SUFFIX;

        public static readonly string TTS_SHADER_NAME_URP_2020_WITH_STS = TTS_SHADER_NAME_URP_2020 + TTS_SHADER_NAME_WITH_STS_SUFFIX;
        public static readonly string TTS_SHADER_NAME_URP_2020_WITH_OUTLINE_AND_STS = TTS_SHADER_NAME_URP_2020_WITH_OUTLINE + TTS_SHADER_NAME_WITH_STS_SUFFIX;


        /// URP 2021///
        public static readonly string TTS_SHADER_NAME_URP_2021 = TTS_SHADER_NAME_URP_FOLDER + "/2021" + TTS_SHADER_NAME;
        public static readonly string TTS_SHADER_NAME_URP_2021_WITH_OUTLINE = TTS_SHADER_NAME_URP_2021 + TTS_SHADER_NAME_WITH_OUTLINE_SUFFIX;

        public static readonly string TTS_SHADER_NAME_URP_2021_WITH_STS = TTS_SHADER_NAME_URP_2021 + TTS_SHADER_NAME_WITH_STS_SUFFIX;
        public static readonly string TTS_SHADER_NAME_URP_2021_WITH_OUTLINE_AND_STS = TTS_SHADER_NAME_URP_2021_WITH_OUTLINE + TTS_SHADER_NAME_WITH_STS_SUFFIX;


        /// URP 2022///
        public static readonly string TTS_SHADER_NAME_URP_2022 = TTS_SHADER_NAME_URP_FOLDER + "/2022" + TTS_SHADER_NAME;
        public static readonly string TTS_SHADER_NAME_URP_2022_WITH_OUTLINE = TTS_SHADER_NAME_URP_2022 + TTS_SHADER_NAME_WITH_OUTLINE_SUFFIX;

        public static readonly string TTS_SHADER_NAME_URP_2022_WITH_STS = TTS_SHADER_NAME_URP_2022 + TTS_SHADER_NAME_WITH_STS_SUFFIX;
        public static readonly string TTS_SHADER_NAME_URP_2022_WITH_OUTLINE_AND_STS = TTS_SHADER_NAME_URP_2022_WITH_OUTLINE + TTS_SHADER_NAME_WITH_STS_SUFFIX;


        /// URP 6///
        public static readonly string TTS_SHADER_NAME_URP_UNITY6 = TTS_SHADER_NAME_URP_FOLDER + "/Unity6" + TTS_SHADER_NAME;
        public static readonly string TTS_SHADER_NAME_URP_UNITY6_WITH_OUTLINE = TTS_SHADER_NAME_URP_UNITY6 + TTS_SHADER_NAME_WITH_OUTLINE_SUFFIX;

        public static readonly string TTS_SHADER_NAME_URP_UNITY6_WITH_STS = TTS_SHADER_NAME_URP_UNITY6 + TTS_SHADER_NAME_WITH_STS_SUFFIX;
        public static readonly string TTS_SHADER_NAME_URP_UNITY6_WITH_OUTLINE_AND_STS = TTS_SHADER_NAME_URP_UNITY6_WITH_OUTLINE + TTS_SHADER_NAME_WITH_STS_SUFFIX;



        /// BiRP ///
        public static readonly string TTS_SHADER_FILENAME_BIRP = "/BiRP" + "/TheToonShader" + "_BiRP";
        public static readonly string TTS_SHADER_FILENAME_BIRP_WITH_OUTLINE = "/BiRP" + "/TheToonShader" + "_WithOutline" + "_BiRP";

        public static readonly string TTS_SHADER_FILENAME_BIRP_WITH_STS = "/BiRP" + "/TheToonShader" + TTS_SHADER_NAME_WITH_STS_SUFFIX + "_BiRP";
        public static readonly string TTS_SHADER_FILENAME_BIRP_WITH_OUTLINE_AND_STS = "/BiRP" + "/TheToonShader" + "_WithOutline" + TTS_SHADER_NAME_WITH_STS_SUFFIX + "_BiRP";

        /// URP 2020///
        public static readonly string TTS_SHADER_FILENAME_URP_2020 = "/URP" + "/2020" + "/TheToonShader" + "_URP2020";
        public static readonly string TTS_SHADER_FILENAME_URP_2020_WITH_OUTLINE = "/URP" + "/2020" + "/TheToonShader" + "_WithOutline" + "_URP2020";

        public static readonly string TTS_SHADER_FILENAME_URP_2020_WITH_STS = "/URP" + "/2020" + "/TheToonShader" + TTS_SHADER_NAME_WITH_STS_SUFFIX + "_URP2020";
        public static readonly string TTS_SHADER_FILENAME_URP_2020_WITH_OUTLINE_AND_STS = "/URP" + "/2020" + "/TheToonShader" + "_WithOutline" + TTS_SHADER_NAME_WITH_STS_SUFFIX + "_URP2020";

        /// URP 2021///
        public static readonly string TTS_SHADER_FILENAME_URP_2021 = "/URP" + "/2021" + "/TheToonShader" + "_URP2021";
        public static readonly string TTS_SHADER_FILENAME_URP_2021_WITH_OUTLINE = "/URP" + "/2021" + "/TheToonShader" + "_WithOutline" + "_URP2021";

        public static readonly string TTS_SHADER_FILENAME_URP_2021_WITH_STS = "/URP" + "/2021" + "/TheToonShader" + TTS_SHADER_NAME_WITH_STS_SUFFIX + "_URP2021";
        public static readonly string TTS_SHADER_FILENAME_URP_2021_WITH_OUTLINE_AND_STS = "/URP" + "/2021" + "/TheToonShader" + "_WithOutline" + TTS_SHADER_NAME_WITH_STS_SUFFIX + "_URP2021";

        /// URP 2022///
        public static readonly string TTS_SHADER_FILENAME_URP_2022 = "/URP" + "/2022" + "/TheToonShader" + "_URP2022";
        public static readonly string TTS_SHADER_FILENAME_URP_2022_WITH_OUTLINE = "/URP" + "/2022" + "/TheToonShader" + "_WithOutline" + "_URP2022";

        public static readonly string TTS_SHADER_FILENAME_URP_2022_WITH_STS = "/URP" + "/2022" + "/TheToonShader" + TTS_SHADER_NAME_WITH_STS_SUFFIX + "_URP2022";
        public static readonly string TTS_SHADER_FILENAME_URP_2022_WITH_OUTLINE_AND_STS = "/URP" + "/2022" + "/TheToonShader" + "_WithOutline" + TTS_SHADER_NAME_WITH_STS_SUFFIX + "_URP2022";

        /// URP 6///
        public static readonly string TTS_SHADER_FILENAME_URP_UNITY6 = "/URP" + "/Unity6" + "/TheToonShader" + "_URPUnity6";
        public static readonly string TTS_SHADER_FILENAME_URP_UNITY6_WITH_OUTLINE = "/URP" + "/Unity6" + "/TheToonShader" + "_WithOutline" + "_URPUnity6";

        public static readonly string TTS_SHADER_FILENAME_URP_UNITY6_WITH_STS = "/URP" + "/Unity6" + "/TheToonShader" + TTS_SHADER_NAME_WITH_STS_SUFFIX + "_URPUnity6";
        public static readonly string TTS_SHADER_FILENAME_URP_UNITY6_WITH_OUTLINE_AND_STS = "/URP" + "/Unity6" + "/TheToonShader" + "_WithOutline" + TTS_SHADER_NAME_WITH_STS_SUFFIX + "_URPUnity6";

        //public static readonly string TTS_SHADER_NAME_URP = TTS_SHADER_NAME_URP_FOLDER + "/TheToonShader";
        //public static readonly string TTS_SHADER_NAME_URP_WITH_OUTLINE = TTS_SHADER_NAME_URP + TTS_SHADER_NAME_WITH_OUTLINE_SUFFIX;

    }
}