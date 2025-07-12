using System;
using FishNet;
using ScheduleOne.Combat;
using ScheduleOne.Law;
using ScheduleOne.Noise;
using ScheduleOne.NPCs.Responses;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Vehicles;
using ScheduleOne.VoiceOver;

namespace ScheduleOne.Police
{
	// Token: 0x02000349 RID: 841
	public class NPCResponses_Police : NPCResponses
	{
		// Token: 0x06001273 RID: 4723 RVA: 0x0004F92B File Offset: 0x0004DB2B
		protected override void Awake()
		{
			base.Awake();
			this.officer = (base.npc as PoliceOfficer);
		}

		// Token: 0x06001274 RID: 4724 RVA: 0x0004F944 File Offset: 0x0004DB44
		public override void HitByCar(LandVehicle vehicle)
		{
			base.HitByCar(vehicle);
			base.npc.PlayVO(EVOLineType.Angry);
			if (vehicle.DriverPlayer != null && vehicle.DriverPlayer.IsOwner)
			{
				vehicle.DriverPlayer.CrimeData.AddCrime(new VehicularAssault(), 1);
				if (vehicle.DriverPlayer.CrimeData.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.None)
				{
					vehicle.DriverPlayer.CrimeData.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.NonLethal);
					return;
				}
				vehicle.DriverPlayer.CrimeData.Escalate();
			}
		}

		// Token: 0x06001275 RID: 4725 RVA: 0x0004F9CC File Offset: 0x0004DBCC
		public override void NoticedDrugDeal(Player player)
		{
			base.NoticedDrugDeal(player);
			base.npc.PlayVO(EVOLineType.Command);
			if (player.IsOwner)
			{
				player.CrimeData.AddCrime(new DrugTrafficking(), 1);
				player.CrimeData.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.Arresting);
				(base.npc as PoliceOfficer).BeginFootPursuit_Networked(player.NetworkObject, true);
			}
		}

		// Token: 0x06001276 RID: 4726 RVA: 0x0004FA28 File Offset: 0x0004DC28
		public override void NoticedPettyCrime(Player player)
		{
			base.NoticedPettyCrime(player);
			base.npc.PlayVO(EVOLineType.Command);
			if (player.IsOwner)
			{
				player.CrimeData.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.Arresting);
				(base.npc as PoliceOfficer).BeginFootPursuit_Networked(player.NetworkObject, true);
			}
		}

		// Token: 0x06001277 RID: 4727 RVA: 0x0004FA68 File Offset: 0x0004DC68
		public override void NoticedVandalism(Player player)
		{
			base.NoticedVandalism(player);
			base.npc.PlayVO(EVOLineType.Command);
			if (player.IsOwner)
			{
				player.CrimeData.AddCrime(new Vandalism(), 1);
				player.CrimeData.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.Arresting);
				(base.npc as PoliceOfficer).BeginFootPursuit_Networked(player.NetworkObject, true);
			}
		}

		// Token: 0x06001278 RID: 4728 RVA: 0x0004FAC4 File Offset: 0x0004DCC4
		public override void SawPickpocketing(Player player)
		{
			base.SawPickpocketing(player);
			base.npc.PlayVO(EVOLineType.Command);
			if (player.IsOwner)
			{
				player.CrimeData.AddCrime(new Theft(), 1);
				player.CrimeData.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.Arresting);
				(base.npc as PoliceOfficer).BeginFootPursuit_Networked(player.NetworkObject, true);
			}
		}

		// Token: 0x06001279 RID: 4729 RVA: 0x0004FB20 File Offset: 0x0004DD20
		public override void NoticePlayerBrandishingWeapon(Player player)
		{
			base.NoticePlayerBrandishingWeapon(player);
			base.npc.PlayVO(EVOLineType.Command);
			if (player.IsOwner)
			{
				player.CrimeData.AddCrime(new BrandishingWeapon(), 1);
				player.CrimeData.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.NonLethal);
				(base.npc as PoliceOfficer).BeginFootPursuit_Networked(player.NetworkObject, true);
			}
		}

		// Token: 0x0600127A RID: 4730 RVA: 0x0004FB7C File Offset: 0x0004DD7C
		public override void NoticePlayerDischargingWeapon(Player player)
		{
			base.NoticePlayerDischargingWeapon(player);
			base.npc.PlayVO(EVOLineType.Command);
			if (player.IsOwner)
			{
				player.CrimeData.AddCrime(new DischargeFirearm(), 1);
				player.CrimeData.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.NonLethal);
				(base.npc as PoliceOfficer).BeginFootPursuit_Networked(player.NetworkObject, true);
			}
		}

		// Token: 0x0600127B RID: 4731 RVA: 0x0004FBD8 File Offset: 0x0004DDD8
		public override void NoticedWantedPlayer(Player player)
		{
			base.NoticedWantedPlayer(player);
			base.npc.PlayVO(EVOLineType.Command);
			if (player.IsOwner)
			{
				player.CrimeData.RecordLastKnownPosition(true);
				if (base.npc.CurrentVehicle != null)
				{
					(base.npc as PoliceOfficer).BeginFootPursuit_Networked(player.NetworkObject, false);
					(base.npc as PoliceOfficer).BeginVehiclePursuit_Networked(player.NetworkObject, base.npc.CurrentVehicle.NetworkObject, true);
					return;
				}
				(base.npc as PoliceOfficer).BeginFootPursuit_Networked(player.NetworkObject, true);
			}
		}

		// Token: 0x0600127C RID: 4732 RVA: 0x0004FC75 File Offset: 0x0004DE75
		public override void NoticedSuspiciousPlayer(Player player)
		{
			base.NoticedSuspiciousPlayer(player);
			if (player.IsOwner)
			{
				(base.npc as PoliceOfficer).BeginBodySearch_Networked(player.NetworkObject);
			}
		}

		// Token: 0x0600127D RID: 4733 RVA: 0x0004FC9C File Offset: 0x0004DE9C
		public override void NoticedViolatingCurfew(Player player)
		{
			base.NoticedViolatingCurfew(player);
			base.npc.PlayVO(EVOLineType.Command);
			if (player.IsOwner)
			{
				player.CrimeData.AddCrime(new ViolatingCurfew(), 1);
				player.CrimeData.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.Arresting);
				if (base.npc.CurrentVehicle != null)
				{
					(base.npc as PoliceOfficer).BeginFootPursuit_Networked(player.NetworkObject, false);
					(base.npc as PoliceOfficer).BeginVehiclePursuit_Networked(player.NetworkObject, base.npc.CurrentVehicle.NetworkObject, true);
					return;
				}
				(base.npc as PoliceOfficer).BeginFootPursuit_Networked(player.NetworkObject, true);
			}
		}

		// Token: 0x0600127E RID: 4734 RVA: 0x0004FD50 File Offset: 0x0004DF50
		protected override void RespondToFirstNonLethalAttack(Player perpetrator, Impact impact)
		{
			base.RespondToFirstNonLethalAttack(perpetrator, impact);
			perpetrator.CrimeData.AddCrime(new Assault(), 1);
			if (perpetrator.CrimeData.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.None)
			{
				perpetrator.CrimeData.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.Arresting);
				this.officer.BeginFootPursuit_Networked(perpetrator.NetworkObject, true);
				return;
			}
			perpetrator.CrimeData.Escalate();
			this.officer.BeginFootPursuit_Networked(perpetrator.NetworkObject, true);
		}

		// Token: 0x0600127F RID: 4735 RVA: 0x0004FDC0 File Offset: 0x0004DFC0
		protected override void RespondToLethalAttack(Player perpetrator, Impact impact)
		{
			base.RespondToLethalAttack(perpetrator, impact);
			perpetrator.CrimeData.AddCrime(new DeadlyAssault(), 1);
			if (perpetrator.CrimeData.CurrentPursuitLevel < PlayerCrimeData.EPursuitLevel.Lethal)
			{
				perpetrator.CrimeData.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.Lethal);
				this.officer.BeginFootPursuit_Networked(perpetrator.NetworkObject, true);
			}
		}

		// Token: 0x06001280 RID: 4736 RVA: 0x0004FE14 File Offset: 0x0004E014
		protected override void RespondToRepeatedNonLethalAttack(Player perpetrator, Impact impact)
		{
			base.RespondToRepeatedNonLethalAttack(perpetrator, impact);
			if (!perpetrator.CrimeData.IsCrimeOnRecord(typeof(Assault)))
			{
				perpetrator.CrimeData.AddCrime(new Assault(), 1);
			}
			if (perpetrator.CrimeData.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.None)
			{
				perpetrator.CrimeData.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.Arresting);
				this.officer.BeginFootPursuit_Networked(perpetrator.NetworkObject, true);
				return;
			}
			perpetrator.CrimeData.Escalate();
			this.officer.BeginFootPursuit_Networked(perpetrator.NetworkObject, true);
		}

		// Token: 0x06001281 RID: 4737 RVA: 0x0004FE9C File Offset: 0x0004E09C
		protected override void RespondToAnnoyingImpact(Player perpetrator, Impact impact)
		{
			base.RespondToAnnoyingImpact(perpetrator, impact);
			base.npc.VoiceOverEmitter.Play(EVOLineType.Annoyed);
			base.npc.dialogueHandler.PlayReaction("annoyed", 2.5f, false);
			base.npc.Avatar.EmotionManager.AddEmotionOverride("Annoyed", "annoyed", 20f, 3);
			if (InstanceFinder.IsServer)
			{
				base.npc.behaviour.FacePlayerBehaviour.SetTarget(perpetrator.NetworkObject, 5f);
				base.npc.behaviour.FacePlayerBehaviour.Enable_Networked(null);
			}
		}

		// Token: 0x06001282 RID: 4738 RVA: 0x0004FF40 File Offset: 0x0004E140
		public override void RespondToAimedAt(Player player)
		{
			base.RespondToAimedAt(player);
			if (player.CrimeData.CurrentPursuitLevel < PlayerCrimeData.EPursuitLevel.Lethal)
			{
				player.CrimeData.AddCrime(new Assault(), 1);
				player.CrimeData.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.Lethal);
			}
		}

		// Token: 0x06001283 RID: 4739 RVA: 0x0004FF74 File Offset: 0x0004E174
		public override void ImpactReceived(Impact impact)
		{
			base.ImpactReceived(impact);
			if (this.officer.PursuitBehaviour.Active)
			{
				this.officer.PursuitBehaviour.ResetArrestProgress();
			}
		}

		// Token: 0x06001284 RID: 4740 RVA: 0x0004FFA0 File Offset: 0x0004E1A0
		public override void GunshotHeard(NoiseEvent gunshotSound)
		{
			base.GunshotHeard(gunshotSound);
			if (gunshotSound.source != null && gunshotSound.source.GetComponent<Player>() != null)
			{
				this.officer.behaviour.FacePlayerBehaviour.SetTarget(gunshotSound.source.GetComponent<Player>().NetworkObject, 5f);
				this.officer.behaviour.FacePlayerBehaviour.SendEnable();
			}
		}

		// Token: 0x040011C5 RID: 4549
		private PoliceOfficer officer;
	}
}
