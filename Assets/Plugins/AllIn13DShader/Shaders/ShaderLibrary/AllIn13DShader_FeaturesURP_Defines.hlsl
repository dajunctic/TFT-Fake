#ifndef ALLIN13DSHADER_FEATURESURP_DEFINES
#define ALLIN13DSHADER_FEATURESURP_DEFINES

//---------------------------------------------------------------------
// Features to enable disable. You will want to disable features you don't use to speed up compilation and build times
// You can also enable/disable these features in the Asset Window, in the URP Settings tab

// Core rendering features
//#define ALLIN1_GPU_INSTANCING_SUPPORT           // Enables GPU Instancing for better performance
//#define ALLIN1_DOTS_INSTANCING_SUPPORT          // Supports Unity ECS instancing
//#define ALLIN1_FOG_SUPPORT                      // Unity fog system integration

// Lighting and shadows
#define ALLIN1_LIGHTMAPS_SUPPORT                // Enables baked lightmap support
#define ALLIN1_ADDITIONAL_LIGHTS_SUPPORT        // Support for additional real-time lights beyond main directional
#define ALLIN1_CAST_SHADOWS_SUPPORT             // Enables shadow casting from this material
#define ALLIN1_SHADOW_MASK_SUPPORT              // Mixed lighting shadowmask support

// Advanced rendering (Unity 6+)
#define ALLIN1_FORWARD_PLUS_SUPPORT_UNITY6      // Forward+ rendering path (Unity 6+)
#define ALLIN1_REFLECTIONS_PROBES_SUPPORT_UNITY6 // Probe blending (Unity 6+ only, enable if using probe blending)
#define ALLIN1_ADAPTATIVE_PROBE_VOLUMES_UNITY6 //Adaptative probe volumes support (Unity 6+ only)


// Screen-space effects
#define ALLIN1_SSO_SUPPORT                      // Screen Space Ambient Occlusion support

//Light Cookies
#define ALLIN1_LIGHT_COOKIES_SUPPORT

//Decals
#define ALLIN1_DECALS_SUPPORT

// Specialized features
//#define ALLIN1_LIGHT_LAYERS_SUPPORT             // Unity light layer system (if using light layers)
//---------------------------------------------------------------------

#endif //ALLIN13DSHADER_FEATURESURP_DEFINES