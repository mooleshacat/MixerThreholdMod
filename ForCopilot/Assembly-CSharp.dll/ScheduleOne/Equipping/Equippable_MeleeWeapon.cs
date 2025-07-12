using System;
using ScheduleOne.Audio;
using ScheduleOne.Combat;
using ScheduleOne.DevUtilities;
using ScheduleOne.FX;
using ScheduleOne.ItemFramework;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using UnityEngine;

namespace ScheduleOne.Equipping
{
	// Token: 0x0200095D RID: 2397
	public class Equippable_MeleeWeapon : Equippable_AvatarViewmodel
	{
		// Token: 0x170008EB RID: 2283
		// (get) Token: 0x0600409E RID: 16542 RVA: 0x00111243 File Offset: 0x0010F443
		public bool IsLoading
		{
			get
			{
				return this.load > 0f;
			}
		}

		// Token: 0x170008EC RID: 2284
		// (get) Token: 0x0600409F RID: 16543 RVA: 0x00111252 File Offset: 0x0010F452
		// (set) Token: 0x060040A0 RID: 16544 RVA: 0x0011125A File Offset: 0x0010F45A
		public bool IsAttacking { get; private set; }

		// Token: 0x060040A1 RID: 16545 RVA: 0x00111263 File Offset: 0x0010F463
		protected override void Update()
		{
			base.Update();
			if (Singleton<PauseMenu>.Instance.IsPaused)
			{
				return;
			}
			this.UpdateInput();
			this.UpdateCooldown();
		}

		// Token: 0x060040A2 RID: 16546 RVA: 0x00111284 File Offset: 0x0010F484
		public override void Equip(ItemInstance item)
		{
			base.Equip(item);
		}

		// Token: 0x060040A3 RID: 16547 RVA: 0x0011128D File Offset: 0x0010F48D
		public override void Unequip()
		{
			base.Unequip();
			PlayerSingleton<PlayerCamera>.Instance.Animator.SetFloat("Load", 0f);
		}

		// Token: 0x060040A4 RID: 16548 RVA: 0x001112B0 File Offset: 0x0010F4B0
		private void UpdateCooldown()
		{
			if (this.remainingCooldown > 0f && !this.IsLoading && !this.IsAttacking)
			{
				this.remainingCooldown -= Time.deltaTime;
				this.remainingCooldown = Mathf.Clamp(this.remainingCooldown, 0f, this.MaxCooldown);
			}
		}

		// Token: 0x060040A5 RID: 16549 RVA: 0x00111308 File Offset: 0x0010F508
		private void UpdateInput()
		{
			if (GameInput.GetButton(GameInput.ButtonCode.PrimaryClick))
			{
				if (this.load == 0f)
				{
					if (!GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick) && (!this.loadQueued || !GameInput.GetButton(GameInput.ButtonCode.PrimaryClick)))
					{
						return;
					}
					if (this.CanStartLoading())
					{
						this.StartLoad();
					}
					else if (this.clickReleased)
					{
						this.loadQueued = true;
					}
				}
				if (this.load >= 0.0001f)
				{
					this.load += Time.deltaTime;
					if (this.load < this.MaxLoadTime)
					{
						PlayerSingleton<PlayerMovement>.Instance.ChangeStamina(-(this.MaxStaminaCost - this.MinStaminaCost) * Time.deltaTime / this.MaxLoadTime, true);
					}
					else
					{
						PlayerSingleton<PlayerMovement>.Instance.ChangeStamina(-1E-07f, true);
					}
				}
				this.clickReleased = false;
				Singleton<ViewmodelAvatar>.Instance.Animator.SetFloat("Load", Mathf.Clamp01(this.load / this.MaxLoadTime));
				PlayerSingleton<PlayerCamera>.Instance.Animator.SetFloat("Load", Mathf.Clamp01(this.load / this.MaxLoadTime));
				if (this.IsLoading && PlayerSingleton<PlayerMovement>.Instance.CurrentStaminaReserve <= 0f)
				{
					this.Release();
					return;
				}
			}
			else
			{
				this.clickReleased = true;
				this.loadQueued = false;
				if (this.load > 0f)
				{
					this.Release();
				}
			}
		}

		// Token: 0x060040A6 RID: 16550 RVA: 0x00111460 File Offset: 0x0010F660
		private bool CanStartLoading()
		{
			return this.remainingCooldown <= 0f && !this.IsAttacking && base.equipAnimDone && PlayerSingleton<PlayerMovement>.Instance.CurrentStaminaReserve >= this.MinStaminaCost && !GameManager.IS_TUTORIAL;
		}

		// Token: 0x060040A7 RID: 16551 RVA: 0x001114B0 File Offset: 0x0010F6B0
		private void StartLoad()
		{
			this.loadQueued = false;
			this.load = 0.001f;
			PlayerSingleton<PlayerMovement>.Instance.ChangeStamina(-this.MinStaminaCost, true);
			Singleton<ViewmodelAvatar>.Instance.Animator.SetFloat("Load", 0f);
			PlayerSingleton<PlayerCamera>.Instance.Animator.SetFloat("Load", 0f);
		}

		// Token: 0x060040A8 RID: 16552 RVA: 0x00111514 File Offset: 0x0010F714
		private void Release()
		{
			this.loadQueued = false;
			float num = Mathf.Clamp01(this.load / this.MaxLoadTime);
			this.remainingCooldown = Mathf.Lerp(this.MinCooldown, this.MaxCooldown, num);
			this.Hit(num);
			PlayerSingleton<PlayerMovement>.Instance.SetResidualVelocity(Player.Local.transform.forward, Mathf.Lerp(0f, 300f, num), Mathf.Lerp(0.05f, 0.15f, num));
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
			if (this.SwingAnimationTrigger != string.Empty)
			{
				Player.Local.SendAnimationTrigger(this.SwingAnimationTrigger);
			}
			this.load = 0f;
		}

		// Token: 0x060040A9 RID: 16553 RVA: 0x0011161C File Offset: 0x0010F81C
		private void Hit(float power)
		{
			Equippable_MeleeWeapon.<>c__DisplayClass37_0 CS$<>8__locals1 = new Equippable_MeleeWeapon.<>c__DisplayClass37_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.power = power;
			this.IsAttacking = true;
			this.WhooshSound.VolumeMultiplier = Mathf.Lerp(0.4f, 1f, CS$<>8__locals1.power);
			this.WhooshSound.PitchMultiplier = Mathf.Lerp(1f, 0.8f, CS$<>8__locals1.power) * this.WhooshSoundPitch;
			this.WhooshSound.Play();
			this.hitRoutine = base.StartCoroutine(CS$<>8__locals1.<Hit>g__HitRoutine|0());
		}

		// Token: 0x060040AA RID: 16554 RVA: 0x001116A8 File Offset: 0x0010F8A8
		private void ExecuteHit(float power)
		{
			RaycastHit hit;
			if (PlayerSingleton<PlayerCamera>.Instance.LookRaycast(this.Range, out hit, NetworkSingleton<CombatManager>.Instance.MeleeLayerMask, true, this.HitRadius))
			{
				IDamageable componentInParent = hit.collider.GetComponentInParent<IDamageable>();
				if (componentInParent != null)
				{
					float impactDamage = Mathf.Lerp(this.MinDamage, this.MaxDamage, power);
					float impactForce = Mathf.Lerp(this.MinForce, this.MaxForce, power);
					Impact impact = new Impact(hit, hit.point, PlayerSingleton<PlayerCamera>.Instance.transform.forward, impactForce, impactDamage, this.ImpactType, Player.Local, UnityEngine.Random.Range(int.MinValue, int.MaxValue));
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
					this.ImpactSound.Play();
					PlayerSingleton<PlayerCamera>.Instance.StartCameraShake(Mathf.Lerp(0.1f, 0.4f, power), 0.2f, true);
					if (componentInParent is NPC)
					{
						Player.Local.VisualState.ApplyState("melee_attack", PlayerVisualState.EVisualState.Brandishing, 2.5f);
					}
				}
			}
		}

		// Token: 0x04002E05 RID: 11781
		[Header("Basic Settings")]
		public EImpactType ImpactType;

		// Token: 0x04002E06 RID: 11782
		public float Range = 1.25f;

		// Token: 0x04002E07 RID: 11783
		public float HitRadius = 0.2f;

		// Token: 0x04002E08 RID: 11784
		[Header("Timing")]
		public float MaxLoadTime = 1f;

		// Token: 0x04002E09 RID: 11785
		public float MinCooldown = 0.1f;

		// Token: 0x04002E0A RID: 11786
		public float MaxCooldown = 0.2f;

		// Token: 0x04002E0B RID: 11787
		public float MinHitDelay = 0.1f;

		// Token: 0x04002E0C RID: 11788
		public float MaxHitDelay = 0.2f;

		// Token: 0x04002E0D RID: 11789
		[Header("Damage")]
		public float MinDamage = 20f;

		// Token: 0x04002E0E RID: 11790
		public float MaxDamage = 60f;

		// Token: 0x04002E0F RID: 11791
		public float MinForce = 100f;

		// Token: 0x04002E10 RID: 11792
		public float MaxForce = 300f;

		// Token: 0x04002E11 RID: 11793
		[Header("Stamina Settings")]
		public float MinStaminaCost = 10f;

		// Token: 0x04002E12 RID: 11794
		public float MaxStaminaCost = 40f;

		// Token: 0x04002E13 RID: 11795
		[Header("Sound")]
		public AudioSourceController WhooshSound;

		// Token: 0x04002E14 RID: 11796
		public float WhooshSoundPitch = 1f;

		// Token: 0x04002E15 RID: 11797
		public AudioSourceController ImpactSound;

		// Token: 0x04002E16 RID: 11798
		[Header("Animation")]
		public string SwingAnimationTrigger;

		// Token: 0x04002E17 RID: 11799
		private float load;

		// Token: 0x04002E18 RID: 11800
		private float remainingCooldown;

		// Token: 0x04002E19 RID: 11801
		private Coroutine hitRoutine;

		// Token: 0x04002E1A RID: 11802
		private bool loadQueued;

		// Token: 0x04002E1B RID: 11803
		private bool clickReleased;
	}
}
