using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.EntityFramework;
using ScheduleOne.Tools;
using ScheduleOne.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace ScheduleOne.PlayerScripts
{
	// Token: 0x0200061C RID: 1564
	public class PlayerCamera : PlayerSingleton<PlayerCamera>
	{
		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x060027B2 RID: 10162 RVA: 0x000A1E66 File Offset: 0x000A0066
		// (set) Token: 0x060027B3 RID: 10163 RVA: 0x000A1E6D File Offset: 0x000A006D
		public static ScheduleOne.DevUtilities.GraphicsSettings.EAntiAliasingMode AntiAliasingMode { get; private set; }

		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x060027B4 RID: 10164 RVA: 0x000A1E75 File Offset: 0x000A0075
		// (set) Token: 0x060027B5 RID: 10165 RVA: 0x000A1E7D File Offset: 0x000A007D
		public bool canLook { get; protected set; } = true;

		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x060027B6 RID: 10166 RVA: 0x000A1E86 File Offset: 0x000A0086
		public int activeUIElementCount
		{
			get
			{
				return this.activeUIElements.Count;
			}
		}

		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x060027B7 RID: 10167 RVA: 0x000A1E93 File Offset: 0x000A0093
		// (set) Token: 0x060027B8 RID: 10168 RVA: 0x000A1E9B File Offset: 0x000A009B
		public bool transformOverriden { get; protected set; }

		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x060027B9 RID: 10169 RVA: 0x000A1EA4 File Offset: 0x000A00A4
		// (set) Token: 0x060027BA RID: 10170 RVA: 0x000A1EAC File Offset: 0x000A00AC
		public bool fovOverriden { get; protected set; }

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x060027BB RID: 10171 RVA: 0x000A1EB5 File Offset: 0x000A00B5
		// (set) Token: 0x060027BC RID: 10172 RVA: 0x000A1EBD File Offset: 0x000A00BD
		public bool FreeCamEnabled { get; private set; }

		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x060027BD RID: 10173 RVA: 0x000A1EC6 File Offset: 0x000A00C6
		// (set) Token: 0x060027BE RID: 10174 RVA: 0x000A1ECE File Offset: 0x000A00CE
		public bool ViewingAvatar { get; private set; }

		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x060027BF RID: 10175 RVA: 0x000A1ED7 File Offset: 0x000A00D7
		// (set) Token: 0x060027C0 RID: 10176 RVA: 0x000A1EDF File Offset: 0x000A00DF
		public PlayerCamera.ECameraMode CameraMode { get; protected set; }

		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x060027C1 RID: 10177 RVA: 0x000A1EE8 File Offset: 0x000A00E8
		// (set) Token: 0x060027C2 RID: 10178 RVA: 0x000A1EF0 File Offset: 0x000A00F0
		public bool MethVisuals { get; set; }

		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x060027C3 RID: 10179 RVA: 0x000A1EF9 File Offset: 0x000A00F9
		// (set) Token: 0x060027C4 RID: 10180 RVA: 0x000A1F01 File Offset: 0x000A0101
		public bool CocaineVisuals { get; set; }

		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x060027C5 RID: 10181 RVA: 0x000A1F0A File Offset: 0x000A010A
		// (set) Token: 0x060027C6 RID: 10182 RVA: 0x000A1F12 File Offset: 0x000A0112
		public float FovJitter { get; private set; }

		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x060027C7 RID: 10183 RVA: 0x000A1F1B File Offset: 0x000A011B
		// (set) Token: 0x060027C8 RID: 10184 RVA: 0x000A1F23 File Offset: 0x000A0123
		public List<string> activeUIElements { get; protected set; } = new List<string>();

		// Token: 0x060027C9 RID: 10185 RVA: 0x000A1F2C File Offset: 0x000A012C
		protected override void Awake()
		{
			base.Awake();
			Player.onLocalPlayerSpawned = (Action)Delegate.Remove(Player.onLocalPlayerSpawned, new Action(this.PlayerSpawned));
			Player.onLocalPlayerSpawned = (Action)Delegate.Combine(Player.onLocalPlayerSpawned, new Action(this.PlayerSpawned));
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 100);
			this.ApplyAASettings();
		}

		// Token: 0x060027CA RID: 10186 RVA: 0x000A1F98 File Offset: 0x000A0198
		public override void OnStartClient(bool IsOwner)
		{
			base.OnStartClient(IsOwner);
			if (!IsOwner)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			this.Camera.enabled = true;
		}

		// Token: 0x060027CB RID: 10187 RVA: 0x000A1FBC File Offset: 0x000A01BC
		protected override void Start()
		{
			base.Start();
			if (Singleton<Settings>.InstanceExists)
			{
				this.Camera.fieldOfView = Singleton<Settings>.Instance.CameraFOV;
			}
			if (GameObject.Find("GlobalVolume") != null)
			{
				this.globalVolume = GameObject.Find("GlobalVolume").GetComponent<Volume>();
				this.globalVolume.sharedProfile.TryGet<DepthOfField>(ref this.DoF);
				this.DoF.active = false;
			}
			this.cameralocalPos_PriorOverride = base.transform.localPosition;
			this.FoVChangeSmoother.Initialize();
			this.FoVChangeSmoother.SetDefault(0f);
			this.SmoothLookSmoother.Initialize();
			this.SmoothLookSmoother.SetDefault(0f);
			this.SmoothLookSmoother.SetSmoothingSpeed(0.5f);
			this.LockMouse();
		}

		// Token: 0x060027CC RID: 10188 RVA: 0x000A2092 File Offset: 0x000A0292
		private void PlayerSpawned()
		{
			Player.Local.onTased.AddListener(delegate()
			{
				this.StartCameraShake(1f, 2f, true);
			});
			Player.Local.onTasedEnd.AddListener(new UnityAction(this.StopCameraShake));
		}

		// Token: 0x060027CD RID: 10189 RVA: 0x000A20CA File Offset: 0x000A02CA
		public static void SetAntialiasingMode(ScheduleOne.DevUtilities.GraphicsSettings.EAntiAliasingMode mode)
		{
			PlayerCamera.AntiAliasingMode = mode;
			if (PlayerSingleton<PlayerCamera>.Instance != null)
			{
				PlayerSingleton<PlayerCamera>.Instance.ApplyAASettings();
			}
		}

		// Token: 0x060027CE RID: 10190 RVA: 0x000A20EC File Offset: 0x000A02EC
		public void ApplyAASettings()
		{
			AntialiasingMode antialiasing;
			switch (PlayerCamera.AntiAliasingMode)
			{
			case ScheduleOne.DevUtilities.GraphicsSettings.EAntiAliasingMode.Off:
				antialiasing = 0;
				break;
			case ScheduleOne.DevUtilities.GraphicsSettings.EAntiAliasingMode.FXAA:
				antialiasing = 1;
				break;
			case ScheduleOne.DevUtilities.GraphicsSettings.EAntiAliasingMode.SMAA:
				antialiasing = 2;
				break;
			default:
				antialiasing = 0;
				break;
			}
			this.Camera.GetComponent<UniversalAdditionalCameraData>().antialiasing = antialiasing;
		}

		// Token: 0x060027CF RID: 10191 RVA: 0x000A2134 File Offset: 0x000A0334
		protected virtual void Update()
		{
			this.UpdateCameraBob();
			if (this.canLook)
			{
				this.RotateCamera();
			}
			if (this.MethVisuals)
			{
				this.MethRumble.VolumeMultiplier = Mathf.MoveTowards(this.MethRumble.VolumeMultiplier, 1f, Time.deltaTime * 0.5f);
				if (!this.MethRumble.isPlaying)
				{
					this.MethRumble.Play();
				}
			}
			else
			{
				this.MethRumble.VolumeMultiplier = Mathf.MoveTowards(this.MethRumble.VolumeMultiplier, 0f, Time.deltaTime * 0.5f);
				if (this.MethRumble.VolumeMultiplier == 0f && this.MethRumble.isPlaying)
				{
					this.MethRumble.Stop();
				}
			}
			if (this.FreeCamEnabled)
			{
				this.RotateFreeCam();
				this.UpdateFreeCamInput();
				this.MoveFreeCam();
			}
			if (Player.Local.Schizophrenic)
			{
				this.timeUntilNextSchizoVoice -= Time.deltaTime;
				if (this.timeUntilNextSchizoVoice <= 0f)
				{
					this.timeUntilNextSchizoVoice = UnityEngine.Random.Range(5f, 20f);
					this.SchizoVoices.VolumeMultiplier = UnityEngine.Random.Range(0.5f, 1f);
					this.SchizoVoices.PitchMultiplier = UnityEngine.Random.Range(0.4f, 1f);
					this.SchizoVoices.transform.localPosition = UnityEngine.Random.insideUnitSphere * 1f;
					this.SchizoVoices.Play();
				}
			}
			if (GameInput.GetButton(GameInput.ButtonCode.ViewAvatar))
			{
				if (!this.ViewingAvatar && this.activeUIElementCount == 0 && this.canLook && !GameInput.IsTyping)
				{
					this.ViewAvatar();
				}
				if (this.ViewingAvatar)
				{
					Vector3 worldPos = this.ViewAvatarCameraPosition.position;
					Vector3 vector = PlayerSingleton<PlayerMovement>.Instance.transform.TransformPoint(new Vector3(0f, this.GetTargetLocalY(), 0f));
					RaycastHit raycastHit;
					if (Physics.Raycast(vector, (this.ViewAvatarCameraPosition.position - vector).normalized, ref raycastHit, Vector3.Distance(vector, this.ViewAvatarCameraPosition.position), 1 << LayerMask.NameToLayer("Default"), 1))
					{
						worldPos = raycastHit.point;
					}
					this.OverrideTransform(worldPos, this.ViewAvatarCameraPosition.rotation, 0f, true);
					base.transform.LookAt(Player.Local.Avatar.LowestSpine.transform);
				}
			}
			else if (this.ViewingAvatar)
			{
				this.StopViewingAvatar();
			}
			if ((this.FreeCamEnabled || Application.isEditor) && Input.GetKeyDown(KeyCode.F12))
			{
				this.Screenshot();
			}
			this.UpdateMovementEvents();
		}

		// Token: 0x060027D0 RID: 10192 RVA: 0x000A23D9 File Offset: 0x000A05D9
		private void Screenshot()
		{
			base.StartCoroutine(PlayerCamera.<Screenshot>g__Routine|96_0());
		}

		// Token: 0x060027D1 RID: 10193 RVA: 0x000A23E8 File Offset: 0x000A05E8
		protected virtual void LateUpdate()
		{
			if (this.Camera == null || base.transform == null)
			{
				return;
			}
			if (!PlayerSingleton<PlayerMovement>.InstanceExists)
			{
				return;
			}
			if (!this.transformOverriden && this.ILerpCamera_Coroutine == null)
			{
				base.transform.localPosition = new Vector3(0f, this.GetTargetLocalY(), 0f);
			}
			if (!this.fovOverriden && this.ILerpCameraFOV_Coroutine == null)
			{
				float num = Singleton<Settings>.Instance.CameraFOV * (PlayerSingleton<PlayerMovement>.Instance.isSprinting ? this.SprintFoVBoost : 1f);
				if (this.MethVisuals)
				{
					this.FovJitter = Mathf.Lerp(this.FovJitter, UnityEngine.Random.Range(0f, 1f), Time.deltaTime * 10f);
				}
				else if (this.CocaineVisuals)
				{
					this.FovJitter = Mathf.Lerp(this.FovJitter, 1f, Time.deltaTime * 0.5f);
				}
				else
				{
					this.FovJitter = Mathf.Lerp(this.FovJitter, 0f, Time.deltaTime * 3f);
				}
				if (Player.Local.Schizophrenic)
				{
					this.schizoFoV = -Mathf.Lerp(this.schizoFoV, Mathf.Sin(Time.time * 0.5f) * 20f, Time.deltaTime);
				}
				else
				{
					this.schizoFoV = Mathf.Lerp(this.schizoFoV, 0f, Time.deltaTime);
				}
				num += this.FovJitter * 6f;
				num += this.schizoFoV;
				num += this.FoVChangeSmoother.CurrentValue;
				this.Camera.fieldOfView = Mathf.MoveTowards(this.Camera.fieldOfView, num, Time.deltaTime * this.FoVChangeRate);
			}
			this.Camera.transform.localPosition = this.cameraLocalPos;
			this.cameraLocalPos = Vector3.zero;
		}

		// Token: 0x060027D2 RID: 10194 RVA: 0x000A25CC File Offset: 0x000A07CC
		private void Exit(ExitAction action)
		{
			if (action.Used)
			{
				return;
			}
			if (this.FreeCamEnabled && action.exitType == ExitType.Escape)
			{
				action.Used = true;
				this.SetFreeCam(false, true);
			}
			if (this.ViewingAvatar && action.exitType == ExitType.Escape)
			{
				action.Used = true;
				this.StopViewingAvatar();
			}
		}

		// Token: 0x060027D3 RID: 10195 RVA: 0x000A2620 File Offset: 0x000A0820
		public float GetTargetLocalY()
		{
			if (!PlayerSingleton<PlayerMovement>.InstanceExists)
			{
				return 0f;
			}
			return PlayerSingleton<PlayerMovement>.Instance.Controller.height / 2f + this.cameraOffsetFromTop;
		}

		// Token: 0x060027D4 RID: 10196 RVA: 0x000A264B File Offset: 0x000A084B
		public void SetCameraMode(PlayerCamera.ECameraMode mode)
		{
			this.CameraMode = mode;
		}

		// Token: 0x060027D5 RID: 10197 RVA: 0x000A2654 File Offset: 0x000A0854
		private void RotateCamera()
		{
			float num = GameInput.MouseDelta.x * (Singleton<Settings>.InstanceExists ? Singleton<Settings>.Instance.LookSensitivity : 1f);
			float num2 = GameInput.MouseDelta.y * (Singleton<Settings>.InstanceExists ? Singleton<Settings>.Instance.LookSensitivity : 1f);
			if (Player.Local.Disoriented)
			{
				num2 = -num2;
			}
			if (Player.Local.Seizure)
			{
				Vector2 b = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
				this.seizureJitter = Vector2.Lerp(this.seizureJitter, b, Time.deltaTime * 10f);
				num += this.seizureJitter.x;
				num2 += this.seizureJitter.y;
			}
			if (Player.Local.Schizophrenic)
			{
				num += Mathf.Sin(Time.time * 0.4f) * 0.01f;
				num2 += Mathf.Sin(Time.time * 0.3f) * 0.01f;
			}
			if (this.SmoothLook)
			{
				this.mouseX = Mathf.Lerp(this.mouseX, num, this.SmoothLookSpeed * Time.deltaTime);
				this.mouseY = Mathf.Lerp(this.mouseY, num2, this.SmoothLookSpeed * Time.deltaTime);
			}
			else if (this.SmoothLookSmoother.CurrentValue <= 0.01f)
			{
				this.mouseX = num;
				this.mouseY = num2;
			}
			else
			{
				float num3 = Mathf.Lerp(50f, 1f, this.SmoothLookSmoother.CurrentValue);
				this.mouseX = Mathf.Lerp(this.mouseX, num, num3 * Time.deltaTime);
				this.mouseY = Mathf.Lerp(this.mouseY, num2, num3 * Time.deltaTime);
			}
			Vector3 eulerAngles = base.transform.localRotation.eulerAngles;
			Vector3 eulerAngles2 = Player.Local.transform.rotation.eulerAngles;
			if (Singleton<Settings>.InstanceExists && Singleton<Settings>.Instance.InvertMouse)
			{
				this.mouseY = -this.mouseY;
			}
			this.mouseX += this.focusMouseX;
			this.mouseY += this.focusMouseY;
			eulerAngles.x -= Mathf.Clamp(this.mouseY, -89f, 89f);
			eulerAngles2.y += this.mouseX;
			eulerAngles.z = 0f;
			if (eulerAngles.x >= 180f)
			{
				if (eulerAngles.x < 271f)
				{
					eulerAngles.x = 271f;
				}
			}
			else if (eulerAngles.x > 89f)
			{
				eulerAngles.x = 89f;
			}
			base.transform.localRotation = Quaternion.Euler(eulerAngles);
			base.transform.localEulerAngles = new Vector3(base.transform.localEulerAngles.x, 0f, 0f);
			Player.Local.transform.rotation = Quaternion.Euler(eulerAngles2);
		}

		// Token: 0x060027D6 RID: 10198 RVA: 0x000A295B File Offset: 0x000A0B5B
		public void LockMouse()
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			if (Singleton<HUD>.InstanceExists)
			{
				Singleton<HUD>.Instance.SetCrosshairVisible(true);
			}
		}

		// Token: 0x060027D7 RID: 10199 RVA: 0x000A297B File Offset: 0x000A0B7B
		public void FreeMouse()
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			if (Singleton<HUD>.InstanceExists)
			{
				Singleton<HUD>.Instance.SetCrosshairVisible(false);
			}
		}

		// Token: 0x060027D8 RID: 10200 RVA: 0x000A299C File Offset: 0x000A0B9C
		public bool LookRaycast(float range, out RaycastHit hit, LayerMask layerMask, bool includeTriggers = true, float radius = 0f)
		{
			if (radius == 0f)
			{
				return Physics.Raycast(base.transform.position, base.transform.forward, ref hit, range, layerMask, includeTriggers ? 2 : 1);
			}
			return Physics.SphereCast(base.transform.position, radius, base.transform.forward, ref hit, range, layerMask, includeTriggers ? 2 : 1);
		}

		// Token: 0x060027D9 RID: 10201 RVA: 0x000A2A0C File Offset: 0x000A0C0C
		public bool LookRaycast_ExcludeBuildables(float range, out RaycastHit hit, LayerMask layerMask, bool includeTriggers = true)
		{
			RaycastHit[] array = Physics.RaycastAll(base.transform.position, base.transform.forward, range, layerMask, includeTriggers ? 2 : 1);
			RaycastHit raycastHit = default(RaycastHit);
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i].collider.GetComponentInParent<BuildableItem>() && (raycastHit.collider == null || Vector3.Distance(base.transform.position, array[i].point) < Vector3.Distance(base.transform.position, raycastHit.point)))
				{
					raycastHit = array[i];
				}
			}
			if (raycastHit.collider != null)
			{
				hit = raycastHit;
				return true;
			}
			hit = default(RaycastHit);
			return false;
		}

		// Token: 0x060027DA RID: 10202 RVA: 0x000A2AE0 File Offset: 0x000A0CE0
		private void OnDrawGizmosSelected()
		{
			for (int i = 0; i < this.gizmos.Count; i++)
			{
				Gizmos.DrawSphere(this.gizmos[i], 0.05f);
			}
			this.gizmos.Clear();
		}

		// Token: 0x060027DB RID: 10203 RVA: 0x000A2B24 File Offset: 0x000A0D24
		public bool Raycast_ExcludeBuildables(Vector3 origin, Vector3 direction, float range, out RaycastHit hit, LayerMask layerMask, bool includeTriggers = false, float radius = 0f, float maxAngleDifference = 0f)
		{
			RaycastHit[] array;
			if (radius == 0f)
			{
				array = Physics.RaycastAll(origin, direction, range, layerMask, includeTriggers ? 2 : 1);
			}
			else
			{
				array = Physics.SphereCastAll(origin, radius, direction, range, layerMask, includeTriggers ? 2 : 1);
			}
			RaycastHit raycastHit = default(RaycastHit);
			for (int i = 0; i < array.Length; i++)
			{
				if (!(array[i].point == Vector3.zero) && !array[i].collider.GetComponentInParent<BuildableItem>() && (maxAngleDifference == 0f || Vector3.Angle(direction, -array[i].normal) < maxAngleDifference) && (raycastHit.collider == null || Vector3.Distance(base.transform.position, array[i].point) < Vector3.Distance(base.transform.position, raycastHit.point)))
				{
					raycastHit = array[i];
				}
			}
			if (raycastHit.collider != null)
			{
				hit = raycastHit;
				return true;
			}
			hit = default(RaycastHit);
			return false;
		}

		// Token: 0x060027DC RID: 10204 RVA: 0x000A2C58 File Offset: 0x000A0E58
		public bool MouseRaycast(float range, out RaycastHit hit, LayerMask layerMask, bool includeTriggers = true, float radius = 0f)
		{
			Ray ray = PlayerSingleton<PlayerCamera>.Instance.Camera.ScreenPointToRay(Input.mousePosition);
			if (radius == 0f)
			{
				return Physics.Raycast(ray, ref hit, range, layerMask, includeTriggers ? 2 : 1);
			}
			return Physics.SphereCast(ray, radius, ref hit, range, layerMask, includeTriggers ? 2 : 1);
		}

		// Token: 0x060027DD RID: 10205 RVA: 0x000A2CB2 File Offset: 0x000A0EB2
		public bool LookSpherecast(float range, float radius, out RaycastHit hit, LayerMask layerMask)
		{
			return Physics.SphereCast(base.transform.position, radius, base.transform.forward, ref hit, range, layerMask);
		}

		// Token: 0x060027DE RID: 10206 RVA: 0x000A2CDC File Offset: 0x000A0EDC
		public void OverrideTransform(Vector3 worldPos, Quaternion rot, float lerpTime, bool keepParented = false)
		{
			this.canLook = false;
			if (this.ILerpCamera_Coroutine != null)
			{
				base.StopCoroutine(this.ILerpCamera_Coroutine);
				this.ILerpCamera_Coroutine = null;
			}
			else if (!this.transformOverriden)
			{
				this.cameralocalPos_PriorOverride = base.transform.localPosition;
				this.cameraLocalRot_PriorOverride = base.transform.localRotation;
			}
			this.transformOverriden = true;
			if (!keepParented)
			{
				base.transform.SetParent(null);
			}
			this.ILerpCamera_Coroutine = Singleton<CoroutineService>.Instance.StartCoroutine(this.ILerpCamera(worldPos, rot, lerpTime, true, false, false));
		}

		// Token: 0x060027DF RID: 10207 RVA: 0x000A2D6A File Offset: 0x000A0F6A
		protected IEnumerator ILerpCamera(Vector3 endPos, Quaternion endRot, float lerpTime, bool worldSpace, bool returnToRestingPosition = false, bool reenableLook = false)
		{
			Vector3 startPos = base.transform.localPosition;
			Quaternion startRot = base.transform.rotation;
			if (worldSpace)
			{
				startPos = base.transform.position;
			}
			float elapsed = 0f;
			while (elapsed < lerpTime)
			{
				if (returnToRestingPosition)
				{
					base.transform.localPosition = Vector3.Lerp(startPos, new Vector3(0f, this.GetTargetLocalY(), 0f), elapsed / lerpTime);
				}
				else if (worldSpace)
				{
					base.transform.position = Vector3.Lerp(startPos, endPos, elapsed / lerpTime);
				}
				else
				{
					base.transform.localPosition = Vector3.Lerp(startPos, endPos, elapsed / lerpTime);
				}
				base.transform.rotation = Quaternion.Lerp(startRot, endRot, elapsed / lerpTime);
				elapsed += Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
			if (returnToRestingPosition)
			{
				base.transform.localPosition = new Vector3(0f, this.GetTargetLocalY(), 0f);
			}
			else if (worldSpace)
			{
				base.transform.position = endPos;
			}
			else
			{
				base.transform.localPosition = endPos;
			}
			if (reenableLook)
			{
				this.SetCanLook(true);
			}
			base.transform.rotation = endRot;
			this.ILerpCamera_Coroutine = null;
			yield break;
		}

		// Token: 0x060027E0 RID: 10208 RVA: 0x000A2DA8 File Offset: 0x000A0FA8
		public void StopTransformOverride(float lerpTime, bool reenableCameraLook = true, bool returnToOriginalRotation = true)
		{
			if (this.blockNextStopTransformOverride)
			{
				this.blockNextStopTransformOverride = false;
				return;
			}
			if (this.ILerpCamera_Coroutine != null)
			{
				base.StopCoroutine(this.ILerpCamera_Coroutine);
				this.ILerpCamera_Coroutine = null;
			}
			this.transformOverriden = false;
			base.transform.SetParent(PlayerSingleton<PlayerMovement>.Instance.transform);
			if (this.ILerpCamera_Coroutine != null)
			{
				base.StopCoroutine(this.ILerpCamera_Coroutine);
			}
			Quaternion quaternion = PlayerSingleton<PlayerMovement>.Instance.transform.rotation * this.cameraLocalRot_PriorOverride;
			if (!returnToOriginalRotation)
			{
				quaternion = base.transform.rotation;
			}
			if (lerpTime == 0f)
			{
				base.transform.rotation = quaternion;
				base.transform.localPosition = new Vector3(0f, this.GetTargetLocalY(), 0f);
				if (reenableCameraLook)
				{
					this.SetCanLook_True();
					return;
				}
			}
			else
			{
				this.ILerpCamera_Coroutine = base.StartCoroutine(this.ILerpCamera(this.cameralocalPos_PriorOverride, quaternion, lerpTime, false, true, reenableCameraLook));
			}
		}

		// Token: 0x060027E1 RID: 10209 RVA: 0x000A2E98 File Offset: 0x000A1098
		public void LookAt(Vector3 point, float duration = 0.25f)
		{
			PlayerCamera.<>c__DisplayClass118_0 CS$<>8__locals1 = new PlayerCamera.<>c__DisplayClass118_0();
			CS$<>8__locals1.point = point;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.duration = duration;
			if (this.lookRoutine != null)
			{
				base.StopCoroutine(this.lookRoutine);
			}
			base.StartCoroutine(CS$<>8__locals1.<LookAt>g__Look|0());
		}

		// Token: 0x060027E2 RID: 10210 RVA: 0x000A2EE1 File Offset: 0x000A10E1
		private void SetCanLook_True()
		{
			this.SetCanLook(true);
		}

		// Token: 0x060027E3 RID: 10211 RVA: 0x000A2EEA File Offset: 0x000A10EA
		public void SetCanLook(bool c)
		{
			this.canLook = c;
		}

		// Token: 0x060027E4 RID: 10212 RVA: 0x000A2EF3 File Offset: 0x000A10F3
		public void SetDoFActive(bool active, float lerpTime)
		{
			if (this.DoFCoroutine != null)
			{
				base.StopCoroutine(this.DoFCoroutine);
			}
			this.DoFCoroutine = base.StartCoroutine(this.LerpDoF(active, lerpTime));
		}

		// Token: 0x060027E5 RID: 10213 RVA: 0x000A2F1D File Offset: 0x000A111D
		private IEnumerator LerpDoF(bool active, float lerpTime)
		{
			if (active)
			{
				this.DoF.active = true;
			}
			float startFocusDist = this.DoF.focusDistance.value;
			float endFocusDist = 0f;
			if (active)
			{
				endFocusDist = 0.1f;
			}
			else
			{
				endFocusDist = 5f;
			}
			for (float i = 0f; i < lerpTime; i += Time.unscaledDeltaTime)
			{
				this.DoF.focusDistance.value = Mathf.Lerp(startFocusDist, endFocusDist, i / lerpTime);
				yield return new WaitForEndOfFrame();
			}
			this.DoF.focusDistance.value = endFocusDist;
			if (!active)
			{
				this.DoF.active = false;
			}
			this.DoFCoroutine = null;
			yield break;
		}

		// Token: 0x060027E6 RID: 10214 RVA: 0x000A2F3C File Offset: 0x000A113C
		public void OverrideFOV(float fov, float lerpTime)
		{
			if (this.ILerpCameraFOV_Coroutine != null)
			{
				base.StopCoroutine(this.ILerpCameraFOV_Coroutine);
			}
			this.fovOverriden = true;
			if (fov == -1f)
			{
				fov = Singleton<Settings>.Instance.CameraFOV;
			}
			this.ILerpCameraFOV_Coroutine = base.StartCoroutine(this.ILerpFOV(fov, lerpTime));
		}

		// Token: 0x060027E7 RID: 10215 RVA: 0x000A2F8C File Offset: 0x000A118C
		protected IEnumerator ILerpFOV(float endFov, float lerpTime)
		{
			float startFov = this.Camera.fieldOfView;
			for (float i = 0f; i < lerpTime; i += Time.deltaTime)
			{
				this.Camera.fieldOfView = Mathf.Lerp(startFov, endFov, i / lerpTime);
				yield return new WaitForEndOfFrame();
			}
			this.Camera.fieldOfView = endFov;
			this.ILerpCameraFOV_Coroutine = null;
			yield break;
		}

		// Token: 0x060027E8 RID: 10216 RVA: 0x000A2FA9 File Offset: 0x000A11A9
		public void StopFOVOverride(float lerpTime)
		{
			this.OverrideFOV(-1f, lerpTime);
			this.fovOverriden = false;
		}

		// Token: 0x060027E9 RID: 10217 RVA: 0x000A2FBE File Offset: 0x000A11BE
		public void AddActiveUIElement(string name)
		{
			if (!this.activeUIElements.Contains(name))
			{
				this.activeUIElements.Add(name);
			}
		}

		// Token: 0x060027EA RID: 10218 RVA: 0x000A2FDA File Offset: 0x000A11DA
		public void RemoveActiveUIElement(string name)
		{
			if (this.activeUIElements.Contains(name))
			{
				this.activeUIElements.Remove(name);
			}
		}

		// Token: 0x060027EB RID: 10219 RVA: 0x000A2FF8 File Offset: 0x000A11F8
		public void RegisterMovementEvent(int threshold, Action action)
		{
			if (threshold < 1)
			{
				Console.LogWarning("Movement events min. threshold is 1m!", null);
				return;
			}
			if (!this.movementEvents.ContainsKey(threshold))
			{
				this.movementEvents.Add(threshold, new PlayerMovement.MovementEvent());
			}
			this.movementEvents[threshold].actions.Add(action);
		}

		// Token: 0x060027EC RID: 10220 RVA: 0x000A304C File Offset: 0x000A124C
		public void DeregisterMovementEvent(Action action)
		{
			foreach (int key in this.movementEvents.Keys)
			{
				PlayerMovement.MovementEvent movementEvent = this.movementEvents[key];
				if (movementEvent.actions.Contains(action))
				{
					movementEvent.actions.Remove(action);
					break;
				}
			}
		}

		// Token: 0x060027ED RID: 10221 RVA: 0x000A30C8 File Offset: 0x000A12C8
		private void UpdateMovementEvents()
		{
			foreach (int num in this.movementEvents.Keys.ToList<int>())
			{
				PlayerMovement.MovementEvent movementEvent = this.movementEvents[num];
				if (Vector3.Distance(base.transform.position, movementEvent.LastUpdatedDistance) > (float)num)
				{
					movementEvent.Update(base.transform.position);
				}
			}
		}

		// Token: 0x060027EE RID: 10222 RVA: 0x000A3158 File Offset: 0x000A1358
		private void ViewAvatar()
		{
			this.ViewingAvatar = true;
			this.AddActiveUIElement("View avatar");
			Vector3 worldPos = this.ViewAvatarCameraPosition.position;
			Vector3 vector = PlayerSingleton<PlayerMovement>.Instance.transform.TransformPoint(new Vector3(0f, this.GetTargetLocalY(), 0f));
			RaycastHit raycastHit;
			if (Physics.Raycast(vector, (this.ViewAvatarCameraPosition.position - vector).normalized, ref raycastHit, Vector3.Distance(vector, this.ViewAvatarCameraPosition.position), 1 << LayerMask.NameToLayer("Default"), 1))
			{
				worldPos = raycastHit.point;
			}
			this.OverrideTransform(worldPos, this.ViewAvatarCameraPosition.rotation, 0f, true);
			base.transform.LookAt(Player.Local.Avatar.LowestSpine.transform);
			Singleton<HUD>.Instance.canvas.enabled = false;
			PlayerSingleton<PlayerInventory>.Instance.SetViewmodelVisible(false);
			Player.Local.SetVisibleToLocalPlayer(true);
		}

		// Token: 0x060027EF RID: 10223 RVA: 0x000A3254 File Offset: 0x000A1454
		private void StopViewingAvatar()
		{
			this.ViewingAvatar = false;
			this.RemoveActiveUIElement("View avatar");
			this.StopTransformOverride(0f, true, true);
			Singleton<HUD>.Instance.canvas.enabled = true;
			PlayerSingleton<PlayerInventory>.Instance.SetViewmodelVisible(true);
			Player.Local.SetVisibleToLocalPlayer(false);
		}

		// Token: 0x060027F0 RID: 10224 RVA: 0x000A32A8 File Offset: 0x000A14A8
		public void JoltCamera()
		{
			AnimationClip animationClip = this.JoltClips[UnityEngine.Random.Range(0, this.JoltClips.Length)];
			this.Animator.Play(animationClip.name, 0, 0f);
		}

		// Token: 0x060027F1 RID: 10225 RVA: 0x000A32E4 File Offset: 0x000A14E4
		public bool PointInCameraView(Vector3 point)
		{
			Vector3 vector = this.Camera.WorldToViewportPoint(point);
			bool flag = this.Is01(vector.x) && this.Is01(vector.y);
			bool flag2 = vector.z > 0f;
			bool flag3 = false;
			Vector3 normalized = (point - this.Camera.transform.position).normalized;
			float num = Vector3.Distance(this.Camera.transform.position, point);
			RaycastHit raycastHit;
			if (Physics.Raycast(this.Camera.transform.position, normalized, ref raycastHit, num + 0.05f, 1 << LayerMask.NameToLayer("Default")) && raycastHit.point != point)
			{
				flag3 = true;
			}
			return flag && flag2 && !flag3;
		}

		// Token: 0x060027F2 RID: 10226 RVA: 0x000A33B1 File Offset: 0x000A15B1
		public bool Is01(float a)
		{
			return a > 0f && a < 1f;
		}

		// Token: 0x060027F3 RID: 10227 RVA: 0x000A33C5 File Offset: 0x000A15C5
		public void ResetRotation()
		{
			base.transform.localRotation = Quaternion.identity;
		}

		// Token: 0x060027F4 RID: 10228 RVA: 0x000A33D8 File Offset: 0x000A15D8
		public void FocusCameraOnTarget(Transform target)
		{
			PlayerCamera.<>c__DisplayClass139_0 CS$<>8__locals1 = new PlayerCamera.<>c__DisplayClass139_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.target = target;
			if (this.focusRoutine != null)
			{
				base.StopCoroutine(this.focusRoutine);
			}
			this.focusRoutine = base.StartCoroutine(CS$<>8__locals1.<FocusCameraOnTarget>g__FocusRoutine|0());
		}

		// Token: 0x060027F5 RID: 10229 RVA: 0x000A341F File Offset: 0x000A161F
		public void StopFocus()
		{
			if (this.focusRoutine != null)
			{
				base.StopCoroutine(this.focusRoutine);
			}
			this.focusMouseX = 0f;
			this.focusMouseY = 0f;
		}

		// Token: 0x060027F6 RID: 10230 RVA: 0x000A344C File Offset: 0x000A164C
		public void StartCameraShake(float intensity, float duration = -1f, bool decreaseOverTime = true)
		{
			PlayerCamera.<>c__DisplayClass141_0 CS$<>8__locals1 = new PlayerCamera.<>c__DisplayClass141_0();
			CS$<>8__locals1.duration = duration;
			CS$<>8__locals1.intensity = intensity;
			CS$<>8__locals1.decreaseOverTime = decreaseOverTime;
			CS$<>8__locals1.<>4__this = this;
			this.StopCameraShake();
			this.cameraShakeCoroutine = base.StartCoroutine(CS$<>8__locals1.<StartCameraShake>g__Shake|0());
		}

		// Token: 0x060027F7 RID: 10231 RVA: 0x000A3493 File Offset: 0x000A1693
		public void StopCameraShake()
		{
			if (this.cameraShakeCoroutine != null)
			{
				base.StopCoroutine(this.cameraShakeCoroutine);
				this.Camera.transform.localPosition = Vector3.zero;
			}
		}

		// Token: 0x060027F8 RID: 10232 RVA: 0x000A34C0 File Offset: 0x000A16C0
		public void UpdateCameraBob()
		{
			float num = 1f;
			if (PlayerSingleton<PlayerMovement>.InstanceExists)
			{
				num = PlayerSingleton<PlayerMovement>.Instance.CurrentSprintMultiplier - 1f;
			}
			num *= Singleton<Settings>.Instance.CameraBobIntensity;
			this.cameraLocalPos.x = this.cameraLocalPos.x + this.HorizontalBobCurve.Evaluate(Time.time * this.BobRate % 1f) * num * this.HorizontalCameraBob;
			this.cameraLocalPos.y = this.cameraLocalPos.y + this.VerticalBobCurve.Evaluate(Time.time * this.BobRate % 1f) * num * this.VerticalCameraBob;
		}

		// Token: 0x060027F9 RID: 10233 RVA: 0x000A3564 File Offset: 0x000A1764
		public void SetFreeCam(bool enable, bool reenableLook = true)
		{
			this.FreeCamEnabled = enable;
			Singleton<HUD>.Instance.canvas.enabled = !enable;
			PlayerSingleton<PlayerMovement>.Instance.canMove = !enable;
			Player.Local.SetVisibleToLocalPlayer(enable);
			if (enable)
			{
				this.OverrideTransform(base.transform.position, base.transform.rotation, 0f, false);
				PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
				return;
			}
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			this.StopTransformOverride(0f, reenableLook, true);
			this.freeCamMovement = Vector3.zero;
		}

		// Token: 0x060027FA RID: 10234 RVA: 0x000A3604 File Offset: 0x000A1804
		private void RotateFreeCam()
		{
			this.mouseX = Mathf.Lerp(this.mouseX, GameInput.MouseDelta.x * (Singleton<Settings>.InstanceExists ? Singleton<Settings>.Instance.LookSensitivity : 1f), this.SmoothLookSpeed * Time.deltaTime);
			this.mouseY = Mathf.Lerp(this.mouseY, GameInput.MouseDelta.y * (Singleton<Settings>.InstanceExists ? Singleton<Settings>.Instance.LookSensitivity : 1f), this.SmoothLookSpeed * Time.deltaTime);
			Vector3 eulerAngles = base.transform.localRotation.eulerAngles;
			Vector3 eulerAngles2 = base.transform.localRotation.eulerAngles;
			if (Singleton<Settings>.InstanceExists && Singleton<Settings>.Instance.InvertMouse)
			{
				this.mouseY = -this.mouseY;
			}
			eulerAngles.x -= Mathf.Clamp(this.mouseY, -89f, 89f);
			eulerAngles.y += this.mouseX;
			eulerAngles.z = 0f;
			if (eulerAngles.x >= 180f)
			{
				if (eulerAngles.x < 271f)
				{
					eulerAngles.x = 271f;
				}
			}
			else if (eulerAngles.x > 89f)
			{
				eulerAngles.x = 89f;
			}
			base.transform.localRotation = Quaternion.Euler(eulerAngles);
			base.transform.localEulerAngles = new Vector3(base.transform.localEulerAngles.x, base.transform.localEulerAngles.y, 0f);
		}

		// Token: 0x060027FB RID: 10235 RVA: 0x000A37A0 File Offset: 0x000A19A0
		private void UpdateFreeCamInput()
		{
			int num = Mathf.RoundToInt(GameInput.MotionAxis.x);
			int num2 = Mathf.RoundToInt(GameInput.MotionAxis.y);
			int num3 = 0;
			if (GameInput.GetButton(GameInput.ButtonCode.Jump))
			{
				num3 = 1;
			}
			else if (GameInput.GetButton(GameInput.ButtonCode.Crouch))
			{
				num3 = -1;
			}
			if (GameInput.IsTyping)
			{
				num = 0;
				num2 = 0;
				num3 = 0;
			}
			this.freeCamSpeed += Input.mouseScrollDelta.y * Time.deltaTime;
			this.freeCamSpeed = Mathf.Clamp(this.freeCamSpeed, 0f, 10f);
			this.freeCamMovement = new Vector3(Mathf.MoveTowards(this.freeCamMovement.x, (float)num, Time.unscaledDeltaTime * this.FreeCamAcceleration), Mathf.MoveTowards(this.freeCamMovement.y, (float)num3, Time.unscaledDeltaTime * this.FreeCamAcceleration), Mathf.MoveTowards(this.freeCamMovement.z, (float)num2, Time.unscaledDeltaTime * this.FreeCamAcceleration));
		}

		// Token: 0x060027FC RID: 10236 RVA: 0x000A3890 File Offset: 0x000A1A90
		private void MoveFreeCam()
		{
			base.transform.position += base.transform.TransformVector(this.freeCamMovement) * this.FreeCamSpeed * this.freeCamSpeed * Time.unscaledDeltaTime * (GameInput.GetButton(GameInput.ButtonCode.Sprint) ? 3f : 1f);
		}

		// Token: 0x060027FF RID: 10239 RVA: 0x000A39FE File Offset: 0x000A1BFE
		[CompilerGenerated]
		internal static IEnumerator <Screenshot>g__Routine|96_0()
		{
			yield return new WaitForEndOfFrame();
			string text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			text = Path.Combine(text, "Screenshot_" + DateTime.Now.ToString("HH-mm-ss") + ".png");
			Console.Log("Screenshot saved to: " + text, null);
			ScreenCapture.CaptureScreenshot(text, 2);
			yield return new WaitForEndOfFrame();
			yield break;
		}

		// Token: 0x04001C95 RID: 7317
		public const float CAMERA_SHAKE_MULTIPLIER = 0.1f;

		// Token: 0x04001C97 RID: 7319
		[Header("Settings")]
		public float cameraOffsetFromTop = -0.15f;

		// Token: 0x04001C98 RID: 7320
		public float SprintFoVBoost = 1.15f;

		// Token: 0x04001C99 RID: 7321
		public float FoVChangeRate = 4f;

		// Token: 0x04001C9A RID: 7322
		public float HorizontalCameraBob = 1f;

		// Token: 0x04001C9B RID: 7323
		public float VerticalCameraBob = 1f;

		// Token: 0x04001C9C RID: 7324
		public float BobRate = 10f;

		// Token: 0x04001C9D RID: 7325
		public AnimationCurve HorizontalBobCurve;

		// Token: 0x04001C9E RID: 7326
		public AnimationCurve VerticalBobCurve;

		// Token: 0x04001C9F RID: 7327
		public float FreeCamSpeed = 1f;

		// Token: 0x04001CA0 RID: 7328
		public float FreeCamAcceleration = 2f;

		// Token: 0x04001CA1 RID: 7329
		public bool SmoothLook;

		// Token: 0x04001CA2 RID: 7330
		public float SmoothLookSpeed = 1f;

		// Token: 0x04001CA3 RID: 7331
		public FloatSmoother FoVChangeSmoother;

		// Token: 0x04001CA4 RID: 7332
		public FloatSmoother SmoothLookSmoother;

		// Token: 0x04001CA5 RID: 7333
		[Header("References")]
		public Transform CameraContainer;

		// Token: 0x04001CA6 RID: 7334
		public Camera Camera;

		// Token: 0x04001CA7 RID: 7335
		public Camera OverlayCamera;

		// Token: 0x04001CA8 RID: 7336
		public Animator Animator;

		// Token: 0x04001CA9 RID: 7337
		public AnimationClip[] JoltClips;

		// Token: 0x04001CAA RID: 7338
		public UniversalRenderPipelineAsset[] URPAssets;

		// Token: 0x04001CAB RID: 7339
		public Transform ViewAvatarCameraPosition;

		// Token: 0x04001CAC RID: 7340
		public HeartbeatSoundController HeartbeatSoundController;

		// Token: 0x04001CAD RID: 7341
		public ParticleSystem Flies;

		// Token: 0x04001CAE RID: 7342
		public AudioSourceController MethRumble;

		// Token: 0x04001CAF RID: 7343
		public RandomizedAudioSourceController SchizoVoices;

		// Token: 0x04001CB3 RID: 7347
		[HideInInspector]
		public bool blockNextStopTransformOverride;

		// Token: 0x04001CBA RID: 7354
		private Volume globalVolume;

		// Token: 0x04001CBB RID: 7355
		private DepthOfField DoF;

		// Token: 0x04001CBD RID: 7357
		private Coroutine cameraShakeCoroutine;

		// Token: 0x04001CBE RID: 7358
		private Vector3 cameraLocalPos = Vector3.zero;

		// Token: 0x04001CBF RID: 7359
		private Vector3 freeCamMovement = Vector3.zero;

		// Token: 0x04001CC0 RID: 7360
		private Coroutine focusRoutine;

		// Token: 0x04001CC1 RID: 7361
		private float focusMouseX;

		// Token: 0x04001CC2 RID: 7362
		private float focusMouseY;

		// Token: 0x04001CC3 RID: 7363
		private Dictionary<int, PlayerMovement.MovementEvent> movementEvents = new Dictionary<int, PlayerMovement.MovementEvent>();

		// Token: 0x04001CC4 RID: 7364
		private float freeCamSpeed = 1f;

		// Token: 0x04001CC5 RID: 7365
		private float mouseX;

		// Token: 0x04001CC6 RID: 7366
		private float mouseY;

		// Token: 0x04001CC7 RID: 7367
		private Vector2 seizureJitter = Vector2.zero;

		// Token: 0x04001CC8 RID: 7368
		private float schizoFoV;

		// Token: 0x04001CC9 RID: 7369
		private float timeUntilNextSchizoVoice = 15f;

		// Token: 0x04001CCA RID: 7370
		private List<Vector3> gizmos = new List<Vector3>();

		// Token: 0x04001CCB RID: 7371
		private Vector3 cameralocalPos_PriorOverride = Vector3.zero;

		// Token: 0x04001CCC RID: 7372
		private Quaternion cameraLocalRot_PriorOverride = Quaternion.identity;

		// Token: 0x04001CCD RID: 7373
		public Coroutine ILerpCamera_Coroutine;

		// Token: 0x04001CCE RID: 7374
		private Coroutine lookRoutine;

		// Token: 0x04001CCF RID: 7375
		private Coroutine DoFCoroutine;

		// Token: 0x04001CD0 RID: 7376
		private Coroutine ILerpCameraFOV_Coroutine;

		// Token: 0x0200061D RID: 1565
		public enum ECameraMode
		{
			// Token: 0x04001CD2 RID: 7378
			Default,
			// Token: 0x04001CD3 RID: 7379
			Vehicle,
			// Token: 0x04001CD4 RID: 7380
			Skateboard
		}
	}
}
