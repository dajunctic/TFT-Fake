using System.IO;
using UnityEngine;

namespace AllIn13DShader
{
	[ExecuteInEditMode]
	public class FastLightConfigurator : MonoBehaviour
	{
		private readonly int global_lightDirection = Shader.PropertyToID("global_lightDirection");
		private readonly int global_lightColor = Shader.PropertyToID("global_lightColor");

		public Color lightColor = Color.white;

		private void Update()
		{
			Shader.SetGlobalVector(global_lightDirection, -transform.forward);
			Shader.SetGlobalColor(global_lightColor, lightColor);
		}

#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			string gizmosFolderPath = UnityEditor.SessionState.GetString(ConstantsRuntime.SESSION_KEY_ROOT_PLUGIN_PATH, string.Empty);
			if (!string.IsNullOrEmpty(gizmosFolderPath))
			{
				string iconPath = Path.Combine(gizmosFolderPath, "Editor\\Gizmos\\Fast_Light_Icon.png");
				iconPath = iconPath.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
				Gizmos.DrawIcon(transform.position, iconPath, false, lightColor);
			}
		}

		private void OnDrawGizmosSelected()
		{
			Color rayColor = lightColor;
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

		private void Reset()
		{
			Light thisLight = GetComponent<Light>();
			if(thisLight != null) lightColor = thisLight.color;
		}
#endif
	}
}