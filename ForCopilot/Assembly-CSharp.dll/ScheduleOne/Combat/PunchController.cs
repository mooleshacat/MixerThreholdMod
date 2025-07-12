using System;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.Dragging;
using ScheduleOne.FX;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using UnityEngine;

namespace ScheduleOne.Combat
{
	// Token: 0x02000779 RID: 1913
	public class PunchController : MonoBehaviour
	{
		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x06003372 RID: 13170 RVA: 0x000D610D File Offset: 0x000D430D
		// (set) Token: 0x06003373 RID: 13171 RVA: 0x000D6115 File Offset: 0x000D4315
		public bool PunchingEnabled { get; set; } = true;

		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x06003374 RID: 13172 RVA: 0x000D611E File Offset: 0x000D431E
		public bool IsLoading
		{
			get
			{
				return this.punchLoad > 0f;
			}
		}

		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x06003375 RID: 13173 RVA: 0x000D612D File Offset: 0x000D432D
		// (set) Token: 0x06003376 RID: 13174 RVA: 0x000D6135 File Offset: 0x000D4335
		public bool IsPunching { get; private set; }

		// Token: 0x06003377 RID: 13175 RVA: 0x000D613E File Offset: 0x000D433E
		private void Awake()
		{
			this.player = base.GetComponentInParent<Player>();
		}

		// Token: 0x06003378 RID: 13176 RVA: 0x000D614C File Offset: 0x000D434C
		private void Start()
		{
			PlayerSingleton<PlayerInventory>.Instance.onPreItemEquipped.AddListener(delegate()
			{
				this.SetPunchingEnabled(false);
			});
		}

		// Token: 0x06003379 RID: 13177 RVA: 0x000D6169 File Offset: 0x000D4369
		private void Update()
		{
			this.SetPunchingEnabled(this.ShouldBeEnabled());
			if (!this.PunchingEnabled || this.timeSincePunchingEnabled < 0.1f)
			{
				return;
			}
			this.UpdateInput();
			this.UpdateCooldown();
			this.itemEquippedLastFrame = PlayerSingleton<PlayerInventory>.Instance.isAnythingEquipped;
		}

		// Token: 0x0600337A RID: 13178 RVA: 0x000D61A9 File Offset: 0x000D43A9
		private void LateUpdate()
		{
			if (this.PunchingEnabled)
			{
				this.timeSincePunchingEnabled += Time.deltaTime;
				return;
			}
			this.timeSincePunchingEnabled = 0f;
		}

		// Token: 0x0600337B RID: 13179 RVA: 0x000D61D4 File Offset: 0x000D43D4
		private void UpdateCooldown()
		{
			if (this.remainingCooldown > 0f && !this.IsLoading && !this.IsPunching)
			{
				this.remainingCooldown -= Time.deltaTime;
				this.remainingCooldown = Mathf.Clamp(this.remainingCooldown, 0f, 0.2f);
			}
		}

		// Token: 0x0600337C RID: 13180 RVA: 0x000D622C File Offset: 0x000D442C
		private void UpdateInput()
		{
			if (GameInput.GetButton(GameInput.ButtonCode.PrimaryClick))
			{
				if (this.punchLoad == 0f)
				{
					if (!this.CanStartLoading() || !GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick))
					{
						return;
					}
					this.StartLoad();
				}
				this.punchLoad += Time.deltaTime;
				Singleton<ViewmodelAvatar>.Instance.Animator.SetFloat("Load", this.punchLoad / 1f);
				PlayerSingleton<PlayerCamera>.Instance.Animator.SetFloat("Load", this.punchLoad / 1f);
				if (this.punchLoad < 1f)
				{
					PlayerSingleton<PlayerMovement>.Instance.ChangeStamina(-(this.MaxStaminaCost - this.MinStaminaCost) * Time.deltaTime / 1f, true);
				}
				else
				{
					PlayerSingleton<PlayerMovement>.Instance.ChangeStamina(-1E-07f, true);
				}
				if (this.IsLoading && PlayerSingleton<PlayerMovement>.Instance.CurrentStaminaReserve <= 0f)
				{
					this.Release();
					return;
				}
			}
			else if (this.punchLoad > 0f)
			{
				this.Release();
			}
		}

		// Token: 0x0600337D RID: 13181 RVA: 0x000D6334 File Offset: 0x000D4534
		private bool CanStartLoading()
		{
			return this.remainingCooldown <= 0f && !this.IsPunching && PlayerSingleton<PlayerMovement>.Instance.CurrentStaminaReserve >= this.MinStaminaCost && !this.itemEquippedLastFrame && !GameManager.IS_TUTORIAL;
		}

		// Token: 0x0600337E RID: 13182 RVA: 0x000D6384 File Offset: 0x000D4584
		private void StartLoad()
		{
			PlayerSingleton<PlayerMovement>.Instance.ChangeStamina(-this.MinStaminaCost, true);
			Singleton<ViewmodelAvatar>.Instance.SetVisibility(true);
			Singleton<ViewmodelAvatar>.Instance.SetOffset(this.ViewmodelAvatarOffset);
			Singleton<ViewmodelAvatar>.Instance.SetAnimatorController(this.PunchAnimator);
			Singleton<ViewmodelAvatar>.Instance.Animator.SetFloat("Load", 0f);
			PlayerSingleton<PlayerCamera>.Instance.Animator.SetFloat("Load", 0f);
		}

		// Token: 0x0600337F RID: 13183 RVA: 0x000D6400 File Offset: 0x000D4600
		private void Release()
		{
			float num = Mathf.Clamp01(this.punchLoad / 1f);
			this.Punch(num);
			PlayerSingleton<PlayerMovement>.Instance.SetResidualVelocity(this.player.transform.forward, Mathf.Lerp(0f, 300f, num), Mathf.Lerp(0.05f, 0.15f, num));
			if (num >= 1f)
			{
				Singleton<ViewmodelAvatar>.Instance.Animator.SetTrigger("Release_Heavy");
				PlayerSingleton<PlayerCamera>.Instance.Animator.SetTrigger("Release_Heavy");
			}
			else
			{
				Singleton<ViewmodelAvatar>.Instance.Animator.SetTrigger("Release_Light");
				PlayerSingleton<PlayerCamera>.Instance.Animator.SetTrigger("Release_Light");
			}
			this.punchLoad = 0f;
		}

		// Token: 0x06003380 RID: 13184 RVA: 0x000D64C8 File Offset: 0x000D46C8
		private void Punch(float power)
		{
			PunchController.<>c__DisplayClass39_0 CS$<>8__locals1 = new PunchController.<>c__DisplayClass39_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.power = power;
			this.IsPunching = true;
			this.PunchSound.VolumeMultiplier = Mathf.Lerp(0.4f, 1f, CS$<>8__locals1.power);
			this.PunchSound.PitchMultiplier = Mathf.Lerp(1f, 0.8f, CS$<>8__locals1.power);
			this.PunchSound.Play();
			this.player.SendPunch();
			this.punchRoutine = base.StartCoroutine(CS$<>8__locals1.<Punch>g__PunchRoutine|0());
		}

		// Token: 0x06003381 RID: 13185 RVA: 0x000D6558 File Offset: 0x000D4758
		private void ExecuteHit(float power)
		{
			RaycastHit hit;
			if (PlayerSingleton<PlayerCamera>.Instance.LookRaycast(1.25f, out hit, NetworkSingleton<CombatManager>.Instance.MeleeLayerMask, true, 0.3f))
			{
				IDamageable componentInParent = hit.collider.GetComponentInParent<IDamageable>();
				if (componentInParent != null)
				{
					float impactDamage = Mathf.Lerp(this.MinPunchDamage, this.MaxPunchDamage, power);
					float impactForce = Mathf.Lerp(this.MinPunchForce, this.MaxPunchForce, power);
					Impact impact = new Impact(hit, hit.point, PlayerSingleton<PlayerCamera>.Instance.transform.forward, impactForce, impactDamage, EImpactType.Punch, this.player, UnityEngine.Random.Range(int.MinValue, int.MaxValue));
					string[] array = new string[7];
					array[0] = "Hit ";
					int num = 1;
					IDamageable damageable = componentInParent;
					array[num] = ((damageable != null) ? damageable.ToString() : null);
					array[2] = " with ";
					array[3] = impactDamage.ToString();
					array[4] = " damage and ";
					array[5] = impactForce.ToString();
					array[6] = " force.";
					Console.Log(string.Concat(array), null);
					componentInParent.SendImpact(impact);
					Singleton<FXManager>.Instance.CreateImpactFX(impact);
					PlayerSingleton<PlayerCamera>.Instance.StartCameraShake(Mathf.Lerp(0.1f, 0.4f, power), 0.2f, true);
				}
			}
		}

		// Token: 0x06003382 RID: 13186 RVA: 0x000D6684 File Offset: 0x000D4884
		private void SetPunchingEnabled(bool enabled)
		{
			if (this.PunchingEnabled == enabled)
			{
				return;
			}
			this.PunchingEnabled = enabled;
			if (!this.PunchingEnabled)
			{
				this.punchLoad = 0f;
				Singleton<ViewmodelAvatar>.Instance.Animator.SetFloat("Load", 0f);
				Singleton<ViewmodelAvatar>.Instance.SetVisibility(false);
				PlayerSingleton<PlayerCamera>.Instance.Animator.SetFloat("Load", 0f);
				if (this.punchRoutine != null)
				{
					base.StopCoroutine(this.punchRoutine);
					this.remainingCooldown = 0.1f;
					this.IsPunching = false;
					this.punchRoutine = null;
				}
			}
		}

		// Token: 0x06003383 RID: 13187 RVA: 0x000D6720 File Offset: 0x000D4920
		private bool ShouldBeEnabled()
		{
			return PlayerSingleton<PlayerInventory>.InstanceExists && PlayerSingleton<PlayerCamera>.InstanceExists && !(Player.Local == null) && Singleton<PauseMenu>.InstanceExists && !PlayerSingleton<PlayerInventory>.Instance.isAnythingEquipped && PlayerSingleton<PlayerCamera>.Instance.activeUIElementCount <= 0 && !Singleton<PauseMenu>.Instance.IsPaused && !(Player.Local.CurrentVehicle != null) && Player.Local.Health.IsAlive && !NetworkSingleton<DragManager>.Instance.IsDragging;
		}

		// Token: 0x04002454 RID: 9300
		public const float MAX_PUNCH_LOAD = 1f;

		// Token: 0x04002455 RID: 9301
		public const float MIN_COOLDOWN = 0.1f;

		// Token: 0x04002456 RID: 9302
		public const float MAX_COOLDOWN = 0.2f;

		// Token: 0x04002457 RID: 9303
		public const float PUNCH_RANGE = 1.25f;

		// Token: 0x04002458 RID: 9304
		public const float PUNCH_DEBOUNCE = 0.1f;

		// Token: 0x0400245B RID: 9307
		[Header("Settings")]
		public Vector3 ViewmodelAvatarOffset = new Vector3(0f, 0f, 0f);

		// Token: 0x0400245C RID: 9308
		public float MinPunchDamage = 20f;

		// Token: 0x0400245D RID: 9309
		public float MaxPunchDamage = 60f;

		// Token: 0x0400245E RID: 9310
		public float MinPunchForce = 100f;

		// Token: 0x0400245F RID: 9311
		public float MaxPunchForce = 300f;

		// Token: 0x04002460 RID: 9312
		[Header("Stamina Settings")]
		public float MinStaminaCost = 10f;

		// Token: 0x04002461 RID: 9313
		public float MaxStaminaCost = 40f;

		// Token: 0x04002462 RID: 9314
		[Header("References")]
		public AudioSourceController PunchSound;

		// Token: 0x04002463 RID: 9315
		public RuntimeAnimatorController PunchAnimator;

		// Token: 0x04002464 RID: 9316
		private float punchLoad;

		// Token: 0x04002465 RID: 9317
		private float remainingCooldown;

		// Token: 0x04002466 RID: 9318
		private Player player;

		// Token: 0x04002467 RID: 9319
		private Coroutine punchRoutine;

		// Token: 0x04002468 RID: 9320
		private bool itemEquippedLastFrame;

		// Token: 0x04002469 RID: 9321
		private float timeSincePunchingEnabled;
	}
}
