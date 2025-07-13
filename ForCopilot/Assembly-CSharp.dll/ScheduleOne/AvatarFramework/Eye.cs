using System;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.AvatarFramework
{
	// Token: 0x020009A3 RID: 2467
	public class Eye : MonoBehaviour
	{
		// Token: 0x17000965 RID: 2405
		// (get) Token: 0x060042F4 RID: 17140 RVA: 0x00119C5D File Offset: 0x00117E5D
		// (set) Token: 0x060042F5 RID: 17141 RVA: 0x00119C65 File Offset: 0x00117E65
		public Eye.EyeLidConfiguration CurrentConfiguration { get; protected set; }

		// Token: 0x17000966 RID: 2406
		// (get) Token: 0x060042F6 RID: 17142 RVA: 0x00119C6E File Offset: 0x00117E6E
		public bool IsBlinking
		{
			get
			{
				return this.blinkRoutine != null;
			}
		}

		// Token: 0x060042F7 RID: 17143 RVA: 0x00119C79 File Offset: 0x00117E79
		private void Awake()
		{
			this.avatar = base.GetComponentInParent<Avatar>();
			this.EyeLight.Enabled = false;
		}

		// Token: 0x060042F8 RID: 17144 RVA: 0x00119C93 File Offset: 0x00117E93
		public void SetSize(float size)
		{
			this.Container.localScale = Eye.defaultScale * size;
		}

		// Token: 0x060042F9 RID: 17145 RVA: 0x00119CAB File Offset: 0x00117EAB
		public void SetLidColor(Color color)
		{
			this.TopLidRend.material.color = color;
			this.BottomLidRend.material.color = color;
		}

		// Token: 0x060042FA RID: 17146 RVA: 0x00119CCF File Offset: 0x00117ECF
		public void SetEyeballMaterial(Material mat, Color col)
		{
			this.EyeBallRend.material = mat;
		}

		// Token: 0x060042FB RID: 17147 RVA: 0x00119CDD File Offset: 0x00117EDD
		public void SetEyeballColor(Color col, float emission = 0.115f, bool writeDefault = true)
		{
			this.EyeBallRend.material.color = col;
			this.EyeBallRend.material.SetColor("_EmissionColor", col * emission);
			if (writeDefault)
			{
				this.defaultEyeColor = col;
			}
		}

		// Token: 0x060042FC RID: 17148 RVA: 0x00119D16 File Offset: 0x00117F16
		public void ResetEyeballColor()
		{
			this.EyeBallRend.material.color = this.defaultEyeColor;
			this.EyeBallRend.material.SetColor("_EmissionColor", this.defaultEyeColor * 0.115f);
		}

		// Token: 0x060042FD RID: 17149 RVA: 0x00119D54 File Offset: 0x00117F54
		public void ConfigureEyeLight(Color color, float intensity)
		{
			if (this.EyeLight == null || this.EyeLight._Light == null)
			{
				return;
			}
			this.EyeLight._Light.color = color;
			this.EyeLight._Light.intensity = intensity;
			this.EyeLight.Enabled = (intensity > 0f);
		}

		// Token: 0x060042FE RID: 17150 RVA: 0x00119DB8 File Offset: 0x00117FB8
		public void SetDilation(float dil)
		{
			this.PupilRend.SetBlendShapeWeight(0, dil * 100f);
		}

		// Token: 0x060042FF RID: 17151 RVA: 0x00119DD0 File Offset: 0x00117FD0
		public void SetEyeLidState(Eye.EyeLidConfiguration config, float time)
		{
			Eye.<>c__DisplayClass34_0 CS$<>8__locals1 = new Eye.<>c__DisplayClass34_0();
			CS$<>8__locals1.config = config;
			CS$<>8__locals1.time = time;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.startConfig = this.CurrentConfiguration;
			this.StopExistingRoutines();
			if (!Singleton<CoroutineService>.InstanceExists)
			{
				return;
			}
			this.stateRoutine = Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<SetEyeLidState>g__Routine|0());
		}

		// Token: 0x06004300 RID: 17152 RVA: 0x00119E28 File Offset: 0x00118028
		private void StopExistingRoutines()
		{
			if (this.blinkRoutine != null)
			{
				base.StopCoroutine(this.blinkRoutine);
			}
			if (this.stateRoutine != null)
			{
				base.StopCoroutine(this.stateRoutine);
			}
		}

		// Token: 0x06004301 RID: 17153 RVA: 0x00119E54 File Offset: 0x00118054
		public void SetEyeLidState(Eye.EyeLidConfiguration config, bool debug = false)
		{
			if (this.TopLidContainer == null || this.BottomLidContainer == null)
			{
				return;
			}
			if (debug)
			{
				string str = "Setting eye lid state: ";
				Eye.EyeLidConfiguration eyeLidConfiguration = config;
				Console.Log(str + eyeLidConfiguration.ToString(), null);
			}
			this.TopLidContainer.localRotation = Quaternion.Lerp(Quaternion.Euler(0f, 0f, 0f), Quaternion.Euler(-90f, 0f, 0f), config.topLidOpen);
			this.BottomLidContainer.localRotation = Quaternion.Lerp(Quaternion.Euler(0f, 0f, 0f), Quaternion.Euler(90f, 0f, 0f), config.bottomLidOpen);
			this.CurrentConfiguration = config;
		}

		// Token: 0x06004302 RID: 17154 RVA: 0x00119F24 File Offset: 0x00118124
		public void LookAt(Vector3 position, bool instant = false)
		{
			Vector3 vector = (position - this.EyeLookOrigin.position).normalized;
			vector = this.EyeLookOrigin.InverseTransformDirection(vector);
			vector.z = Mathf.Clamp(vector.z, 0.1f, float.MaxValue);
			vector = this.EyeLookOrigin.TransformDirection(vector);
			Vector3 vector2 = this.EyeLookOrigin.InverseTransformDirection(vector);
			vector2.x = 0f;
			vector2 = this.EyeLookOrigin.TransformDirection(vector2);
			float num = Vector3.SignedAngle(this.EyeLookOrigin.forward, vector2, this.EyeLookOrigin.right);
			Vector3 vector3 = this.EyeLookOrigin.InverseTransformDirection(vector);
			vector3.y = 0f;
			vector3 = this.EyeLookOrigin.TransformDirection(vector3);
			float num2 = Vector3.SignedAngle(this.EyeLookOrigin.forward, vector3, this.EyeLookOrigin.up);
			Vector3 vector4 = new Vector3(Mathf.Clamp(num + this.AngleOffset.x, Eye.minRotation.y, Eye.maxRotation.y), Mathf.Clamp(num2 + this.AngleOffset.y, Eye.minRotation.x, Eye.maxRotation.x), 0f);
			if (instant)
			{
				string str = "instant: ";
				Vector3 vector5 = vector4;
				Debug.Log(str + vector5.ToString());
				this.PupilContainer.localRotation = Quaternion.Euler(vector4);
				return;
			}
			this.PupilContainer.localRotation = Quaternion.Lerp(this.PupilContainer.localRotation, Quaternion.Euler(vector4), Time.deltaTime * 10f);
		}

		// Token: 0x06004303 RID: 17155 RVA: 0x0011A0C8 File Offset: 0x001182C8
		public void Blink(float blinkDuration, Eye.EyeLidConfiguration endState, bool debug = false)
		{
			Eye.<>c__DisplayClass38_0 CS$<>8__locals1 = new Eye.<>c__DisplayClass38_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.blinkDuration = blinkDuration;
			CS$<>8__locals1.debug = debug;
			CS$<>8__locals1.endState = endState;
			this.StopExistingRoutines();
			if (this.avatar == null || this.avatar.EmotionManager == null)
			{
				return;
			}
			if (this.avatar.EmotionManager.IsSwitchingEmotion)
			{
				return;
			}
			this.blinkRoutine = Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<Blink>g__Routine|0());
		}

		// Token: 0x04002FAF RID: 12207
		public const float PupilLookSpeed = 10f;

		// Token: 0x04002FB0 RID: 12208
		private static Vector3 defaultScale = new Vector3(0.03f, 0.03f, 0.015f);

		// Token: 0x04002FB1 RID: 12209
		private static Vector3 maxRotation = new Vector3(40f, 35f, 0f);

		// Token: 0x04002FB2 RID: 12210
		private static Vector3 minRotation = new Vector3(-40f, -90f, 0f);

		// Token: 0x04002FB4 RID: 12212
		[Header("References")]
		public Transform Container;

		// Token: 0x04002FB5 RID: 12213
		public Transform TopLidContainer;

		// Token: 0x04002FB6 RID: 12214
		public Transform BottomLidContainer;

		// Token: 0x04002FB7 RID: 12215
		public Transform PupilContainer;

		// Token: 0x04002FB8 RID: 12216
		public MeshRenderer TopLidRend;

		// Token: 0x04002FB9 RID: 12217
		public MeshRenderer BottomLidRend;

		// Token: 0x04002FBA RID: 12218
		public MeshRenderer EyeBallRend;

		// Token: 0x04002FBB RID: 12219
		public Transform EyeLookOrigin;

		// Token: 0x04002FBC RID: 12220
		public OptimizedLight EyeLight;

		// Token: 0x04002FBD RID: 12221
		public SkinnedMeshRenderer PupilRend;

		// Token: 0x04002FBE RID: 12222
		private Coroutine blinkRoutine;

		// Token: 0x04002FBF RID: 12223
		private Coroutine stateRoutine;

		// Token: 0x04002FC0 RID: 12224
		private Avatar avatar;

		// Token: 0x04002FC1 RID: 12225
		private Color defaultEyeColor = Color.white;

		// Token: 0x04002FC2 RID: 12226
		public Vector2 AngleOffset = Vector2.zero;

		// Token: 0x020009A4 RID: 2468
		[Serializable]
		public struct EyeLidConfiguration
		{
			// Token: 0x06004306 RID: 17158 RVA: 0x0011A1C0 File Offset: 0x001183C0
			public override string ToString()
			{
				return "Top: " + this.topLidOpen.ToString() + ", Bottom: " + this.bottomLidOpen.ToString();
			}

			// Token: 0x06004307 RID: 17159 RVA: 0x0011A1E8 File Offset: 0x001183E8
			public static Eye.EyeLidConfiguration Lerp(Eye.EyeLidConfiguration start, Eye.EyeLidConfiguration end, float lerp)
			{
				return new Eye.EyeLidConfiguration
				{
					topLidOpen = Mathf.Lerp(start.topLidOpen, end.topLidOpen, lerp),
					bottomLidOpen = Mathf.Lerp(start.bottomLidOpen, end.bottomLidOpen, lerp)
				};
			}

			// Token: 0x04002FC3 RID: 12227
			[Range(0f, 1f)]
			public float topLidOpen;

			// Token: 0x04002FC4 RID: 12228
			[Range(0f, 1f)]
			public float bottomLidOpen;
		}
	}
}
