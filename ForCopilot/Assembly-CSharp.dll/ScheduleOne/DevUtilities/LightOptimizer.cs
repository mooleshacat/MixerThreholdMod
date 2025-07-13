using System;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x02000721 RID: 1825
	public class LightOptimizer : MonoBehaviour
	{
		// Token: 0x0600315F RID: 12639 RVA: 0x000CE275 File Offset: 0x000CC475
		public void Awake()
		{
			this.lights = base.GetComponentsInChildren<OptimizedLight>();
		}

		// Token: 0x06003160 RID: 12640 RVA: 0x000CE284 File Offset: 0x000CC484
		public void FixedUpdate()
		{
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				return;
			}
			OptimizedLight[] array;
			if (Vector3.Distance(base.transform.position, PlayerSingleton<PlayerCamera>.Instance.transform.position) > this.checkRange)
			{
				array = this.lights;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].DisabledForOptimization = true;
				}
				return;
			}
			if (this.activationZones.Length == 0 && this.viewPoints.Length == 0)
			{
				this.ApplyLights();
				return;
			}
			BoxCollider[] array2 = this.activationZones;
			for (int i = 0; i < array2.Length; i++)
			{
				if (array2[i].bounds.Contains(PlayerSingleton<PlayerCamera>.Instance.transform.position))
				{
					this.ApplyLights();
					return;
				}
			}
			GeometryUtility.CalculateFrustumPlanes(PlayerSingleton<PlayerCamera>.Instance.Camera);
			foreach (Transform transform in this.viewPoints)
			{
				if (this.PointInCameraView(transform.position))
				{
					this.ApplyLights();
					return;
				}
			}
			array = this.lights;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].DisabledForOptimization = true;
			}
		}

		// Token: 0x06003161 RID: 12641 RVA: 0x000CE398 File Offset: 0x000CC598
		public void ApplyLights()
		{
			OptimizedLight[] array = this.lights;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].DisabledForOptimization = false;
			}
		}

		// Token: 0x06003162 RID: 12642 RVA: 0x000CE3C4 File Offset: 0x000CC5C4
		public bool PointInCameraView(Vector3 point)
		{
			Camera camera = PlayerSingleton<PlayerCamera>.Instance.Camera;
			bool flag = camera.WorldToViewportPoint(point).z > -1f;
			bool flag2 = false;
			Vector3 normalized = (point - camera.transform.position).normalized;
			float num = Vector3.Distance(camera.transform.position, point);
			RaycastHit raycastHit;
			if (Physics.Raycast(camera.transform.position, normalized, ref raycastHit, num + 0.05f, 1 << LayerMask.NameToLayer("Default")) && raycastHit.point != point)
			{
				flag2 = true;
			}
			return flag && !flag2;
		}

		// Token: 0x06003163 RID: 12643 RVA: 0x000A33B1 File Offset: 0x000A15B1
		public bool Is01(float a)
		{
			return a > 0f && a < 1f;
		}

		// Token: 0x06003164 RID: 12644 RVA: 0x000CE463 File Offset: 0x000CC663
		public void LightsEnabled_True()
		{
			this.LightsEnabled = true;
		}

		// Token: 0x06003165 RID: 12645 RVA: 0x000CE46C File Offset: 0x000CC66C
		public void LightsEnabled_False()
		{
			this.LightsEnabled = false;
		}

		// Token: 0x040022C5 RID: 8901
		public bool LightsEnabled = true;

		// Token: 0x040022C6 RID: 8902
		[Header("References")]
		[SerializeField]
		protected BoxCollider[] activationZones;

		// Token: 0x040022C7 RID: 8903
		[SerializeField]
		protected Transform[] viewPoints;

		// Token: 0x040022C8 RID: 8904
		[Header("Settings")]
		public float checkRange = 50f;

		// Token: 0x040022C9 RID: 8905
		protected OptimizedLight[] lights;
	}
}
