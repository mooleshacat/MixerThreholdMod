using System;
using ScheduleOne.Combat;
using ScheduleOne.Law;
using ScheduleOne.Noise;
using ScheduleOne.NPCs.Actions;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Vehicles;
using ScheduleOne.VoiceOver;
using UnityEngine;

namespace ScheduleOne.NPCs.Responses
{
	// Token: 0x020004C2 RID: 1218
	public class NPCResponses : MonoBehaviour
	{
		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x06001AB0 RID: 6832 RVA: 0x00073CE8 File Offset: 0x00071EE8
		// (set) Token: 0x06001AB1 RID: 6833 RVA: 0x00073CF0 File Offset: 0x00071EF0
		private protected NPC npc { protected get; private set; }

		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x06001AB2 RID: 6834 RVA: 0x00073CF9 File Offset: 0x00071EF9
		protected NPCActions actions
		{
			get
			{
				return this.npc.actions;
			}
		}

		// Token: 0x06001AB3 RID: 6835 RVA: 0x00073D06 File Offset: 0x00071F06
		protected virtual void Awake()
		{
			this.npc = base.GetComponentInParent<NPC>();
		}

		// Token: 0x06001AB4 RID: 6836 RVA: 0x00073D14 File Offset: 0x00071F14
		protected virtual void Update()
		{
			this.timeSinceLastImpact += Time.deltaTime;
			this.timeSinceAimedAt += Time.deltaTime;
		}

		// Token: 0x06001AB5 RID: 6837 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void GunshotHeard(NoiseEvent gunshotSound)
		{
		}

		// Token: 0x06001AB6 RID: 6838 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void ExplosionHeard(NoiseEvent explosionSound)
		{
		}

		// Token: 0x06001AB7 RID: 6839 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void NoticedPettyCrime(Player player)
		{
		}

		// Token: 0x06001AB8 RID: 6840 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void NoticedVandalism(Player player)
		{
		}

		// Token: 0x06001AB9 RID: 6841 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void SawPickpocketing(Player player)
		{
		}

		// Token: 0x06001ABA RID: 6842 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void NoticePlayerBrandishingWeapon(Player player)
		{
		}

		// Token: 0x06001ABB RID: 6843 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void NoticePlayerDischargingWeapon(Player player)
		{
		}

		// Token: 0x06001ABC RID: 6844 RVA: 0x00073D3A File Offset: 0x00071F3A
		public virtual void PlayerFailedPickpocket(Player player)
		{
			if (this.npc.RelationData.Unlocked)
			{
				this.npc.RelationData.ChangeRelationship(0.25f, true);
			}
		}

		// Token: 0x06001ABD RID: 6845 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void NoticedDrugDeal(Player player)
		{
		}

		// Token: 0x06001ABE RID: 6846 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void NoticedViolatingCurfew(Player player)
		{
		}

		// Token: 0x06001ABF RID: 6847 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void NoticedWantedPlayer(Player player)
		{
		}

		// Token: 0x06001AC0 RID: 6848 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void NoticedSuspiciousPlayer(Player player)
		{
		}

		// Token: 0x06001AC1 RID: 6849 RVA: 0x00073D64 File Offset: 0x00071F64
		public virtual void HitByCar(LandVehicle vehicle)
		{
			if (vehicle.DriverPlayer != null && this.npc.Movement.timeSinceHitByCar > 2f)
			{
				if (vehicle.DriverPlayer.CrimeData.CurrentPursuitLevel > PlayerCrimeData.EPursuitLevel.None)
				{
					vehicle.DriverPlayer.CrimeData.AddCrime(new VehicularAssault(), 1);
				}
				else
				{
					vehicle.DriverPlayer.CrimeData.RecordVehicleCollision(this.npc);
				}
				this.npc.Avatar.EmotionManager.AddEmotionOverride("Angry", "hitbycar", 5f, 1);
				this.npc.Avatar.EmotionManager.AddEmotionOverride("Annoyed", "hitbycar1", 20f, 0);
				this.npc.PlayVO(EVOLineType.Hurt);
			}
		}

		// Token: 0x06001AC2 RID: 6850 RVA: 0x00073E34 File Offset: 0x00072034
		public virtual void ImpactReceived(Impact impact)
		{
			if (!this.npc.IsConscious)
			{
				this.timeSinceLastImpact = 0f;
				return;
			}
			this.npc.VoiceOverEmitter.Play(EVOLineType.Hurt);
			Player perpetrator2;
			if (impact.ImpactForce > 50f || impact.ImpactDamage > 10f)
			{
				Player perpetrator;
				if (impact.IsPlayerImpact(out perpetrator))
				{
					if (Impact.IsLethal(impact.ImpactType))
					{
						this.RespondToLethalAttack(perpetrator, impact);
					}
					else if (this.timeSinceLastImpact < 20f)
					{
						this.RespondToRepeatedNonLethalAttack(perpetrator, impact);
					}
					else
					{
						this.RespondToFirstNonLethalAttack(perpetrator, impact);
					}
				}
			}
			else if (impact.IsPlayerImpact(out perpetrator2))
			{
				this.RespondToAnnoyingImpact(perpetrator2, impact);
			}
			this.timeSinceLastImpact = 0f;
		}

		// Token: 0x06001AC3 RID: 6851 RVA: 0x00073EE4 File Offset: 0x000720E4
		protected virtual void RespondToFirstNonLethalAttack(Player perpetrator, Impact impact)
		{
			if (this.timeSinceLastImpact > 20f)
			{
				this.npc.RelationData.ChangeRelationship(0.25f, true);
			}
		}

		// Token: 0x06001AC4 RID: 6852 RVA: 0x00073F09 File Offset: 0x00072109
		protected virtual void RespondToRepeatedNonLethalAttack(Player perpetrator, Impact impact)
		{
			if (this.timeSinceLastImpact > 20f)
			{
				this.npc.RelationData.ChangeRelationship(-0.25f, true);
			}
		}

		// Token: 0x06001AC5 RID: 6853 RVA: 0x00073F2E File Offset: 0x0007212E
		protected virtual void RespondToLethalAttack(Player perpetrator, Impact impact)
		{
			if (this.timeSinceLastImpact > 20f)
			{
				this.npc.RelationData.ChangeRelationship(-1f, true);
			}
		}

		// Token: 0x06001AC6 RID: 6854 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void RespondToAnnoyingImpact(Player perpetrator, Impact impact)
		{
		}

		// Token: 0x06001AC7 RID: 6855 RVA: 0x00073F53 File Offset: 0x00072153
		public virtual void RespondToAimedAt(Player player)
		{
			if (this.timeSinceAimedAt > 20f)
			{
				this.npc.RelationData.ChangeRelationship(-0.5f, true);
			}
			this.timeSinceAimedAt = 0f;
		}

		// Token: 0x040016AA RID: 5802
		public const float ASSAULT_RELATIONSHIPCHANGE = -0.25f;

		// Token: 0x040016AB RID: 5803
		public const float DEADLYASSAULT_RELATIONSHIPCHANGE = -1f;

		// Token: 0x040016AC RID: 5804
		public const float AIMED_AT_RELATIONSHIPCHANGE = -0.5f;

		// Token: 0x040016AD RID: 5805
		public const float PICKPOCKET_RELATIONSHIPCHANGE = -0.25f;

		// Token: 0x040016AF RID: 5807
		protected float timeSinceLastImpact = 100f;

		// Token: 0x040016B0 RID: 5808
		protected float timeSinceAimedAt = 100f;
	}
}
