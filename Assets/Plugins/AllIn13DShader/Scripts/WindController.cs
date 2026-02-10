using UnityEngine;

namespace AllIn13DShader
{
	[ExecuteInEditMode]
	public class WindController : MonoBehaviour
	{
		private readonly int MATPROP_GLOBAL_WIND_NOISE_TEX	= Shader.PropertyToID("global_windNoiseTex");
		private readonly int MATPROP_GLOBAL_WIND_FORCE		= Shader.PropertyToID("global_windForce");
		private readonly int MATPROP_GLOBAL_NOISE_SPEED		= Shader.PropertyToID("global_noiseSpeed");
		private readonly int MATPROP_GLOBAL_WIND_DIR		= Shader.PropertyToID("global_windDir");
		private readonly int MATPROP_GLOBAL_USE_WIND_DIR	= Shader.PropertyToID("global_useWindDir");
		private readonly int MATPROP_GLOBAL_MIN_WIND_VALUE	= Shader.PropertyToID("global_minWindValue");
		private readonly int MATPROP_GLOBAL_MAX_WIND_VALUE	= Shader.PropertyToID("global_maxWindValue");
		private readonly int MATPROP_GLOBAL_WIND_WORLD_SIZE	= Shader.PropertyToID("global_windWorldSize");

		[Header("General")]
		[Tooltip("Overall strength of the wind effect. Higher values create stronger wind displacement.")]
		[Range(0f, 3f)] public float windForce = 1f;
		
		[Tooltip("Speed of noise texture scrolling (X,Y). Higher values create faster wind movement.")]
		public Vector2 noiseSpeed = new Vector2(12f, 6f);
		
		[Tooltip("When enabled, wind can push objects in both positive and negative directions. When disabled, wind only pushes in the positive direction.")]
		public bool bidirectionalWind = true;
		
		[Tooltip("When enabled, wind direction is determined by this GameObject's forward direction. When disabled, wind affects all directions equally.")]
		public bool useWindDir = false;

		[Header("World Size")]
		[Tooltip("Scale of the noise texture in world space. Larger values spread the wind pattern across a bigger area.")]
		public float worldSize = 50f;

		[Header("Noise")]
		[Tooltip("Texture used to generate wind displacement patterns. RGB channels control displacement in respective directions.")]
		public Texture2D windNoise;

		public void Update()
		{
			ApplyWindValues();
		}

		public void ApplyWindValues()
		{
			if(windNoise != null)
			{
				Shader.SetGlobalTexture(MATPROP_GLOBAL_WIND_NOISE_TEX, windNoise);
			}

			Shader.SetGlobalFloat(MATPROP_GLOBAL_WIND_FORCE, windForce);
			Shader.SetGlobalVector(MATPROP_GLOBAL_NOISE_SPEED, noiseSpeed);

			Vector3 correctedWindDir = useWindDir ? transform.forward : Vector3.one;
			float useWindDirFloat = useWindDir ? 1f : 0f;
			Shader.SetGlobalFloat(MATPROP_GLOBAL_USE_WIND_DIR, useWindDirFloat);
			Shader.SetGlobalVector(MATPROP_GLOBAL_WIND_DIR, correctedWindDir);

			float minWindValue = bidirectionalWind ? -1f : 0f;
			Shader.SetGlobalFloat(MATPROP_GLOBAL_MIN_WIND_VALUE, minWindValue);
			Shader.SetGlobalFloat(MATPROP_GLOBAL_MAX_WIND_VALUE, 1f);
			Shader.SetGlobalFloat(MATPROP_GLOBAL_WIND_WORLD_SIZE, worldSize);
		}

#if UNITY_EDITOR
		public void OnDrawGizmos()
		{
			string gizmosFolderPath = UnityEditor.SessionState.GetString(ConstantsRuntime.SESSION_KEY_ROOT_PLUGIN_PATH, string.Empty);
			if (!string.IsNullOrEmpty(gizmosFolderPath))
			{
				string iconPath = System.IO.Path.Combine(gizmosFolderPath, "Editor\\Gizmos\\GizmoIcon_Wind.png");
				iconPath = iconPath.Replace(System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar);
				Gizmos.DrawIcon(transform.position, iconPath, false, Color.cyan);
			}
		}

		private void OnDrawGizmosSelected()
		{
			if(!useWindDir) { return; }

			Color rayColor = Color.cyan;
			rayColor.a = 0.25f;

			int numRays = 12;
			float angle = 360f / numRays;
			float radius = 0.5f;
			float rayLength = 3f;
			Vector3 rayInit = transform.position + transform.up * radius;

			Gizmos.color = rayColor;
			for (int i = 0; i < numRays; i++)
			{
				Gizmos.DrawRay(rayInit, transform.forward * rayLength);

				Vector3 vec = rayInit - transform.position;
				Vector3 rotatedVec = Quaternion.AngleAxis(angle, transform.forward) * vec;

				rayInit = transform.position + rotatedVec;
			}
		}
#endif
	}
}