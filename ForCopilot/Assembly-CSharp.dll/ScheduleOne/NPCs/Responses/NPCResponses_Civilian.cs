using System;
using System.Collections.Generic;
using FishNet;
using ScheduleOne.Combat;
using ScheduleOne.Dialogue;
using ScheduleOne.Law;
using ScheduleOne.Noise;
using ScheduleOne.PlayerScripts;
using ScheduleOne.VoiceOver;
using UnityEngine;

namespace ScheduleOne.NPCs.Responses
{
	// Token: 0x020004C3 RID: 1219
	public class NPCResponses_Civilian : NPCResponses
	{
		// Token: 0x06001AC9 RID: 6857 RVA: 0x00073FA1 File Offset: 0x000721A1
		protected override void Update()
		{
			base.Update();
			this.timeSinceLastThreat += Time.deltaTime;
			if (this.timeSinceLastThreat > 30f)
			{
				this.currentThreatResponse = NPCResponses_Civilian.EAttackResponse.None;
			}
		}

		// Token: 0x06001ACA RID: 6858 RVA: 0x00073FD0 File Offset: 0x000721D0
		public override void GunshotHeard(NoiseEvent gunshotSound)
		{
			base.GunshotHeard(gunshotSound);
			if (this.currentThreatResponse == NPCResponses_Civilian.EAttackResponse.None)
			{
				Player player = (gunshotSound.source != null) ? gunshotSound.source.GetComponent<Player>() : null;
				this.timeSinceLastThreat = 0f;
				this.currentThreatResponse = this.GetThreatResponse(NPCResponses_Civilian.EThreatType.GunshotHeard, player);
				this.ExecuteThreatResponse(this.currentThreatResponse, player, gunshotSound.origin, new DischargeFirearm());
			}
		}

		// Token: 0x06001ACB RID: 6859 RVA: 0x0007403C File Offset: 0x0007223C
		public override void ExplosionHeard(NoiseEvent explosionSound)
		{
			base.ExplosionHeard(explosionSound);
			Console.Log("Explosion heard by " + base.npc.fullName, null);
			if (this.currentThreatResponse == NPCResponses_Civilian.EAttackResponse.None)
			{
				Player threatSource = (explosionSound.source != null) ? explosionSound.source.GetComponent<Player>() : null;
				this.timeSinceLastThreat = 0f;
				this.currentThreatResponse = this.GetThreatResponse(NPCResponses_Civilian.EThreatType.ExplosionHeard, threatSource);
				this.ExecuteThreatResponse(this.currentThreatResponse, null, explosionSound.origin, null);
			}
		}

		// Token: 0x06001ACC RID: 6860 RVA: 0x000740C0 File Offset: 0x000722C0
		public override void PlayerFailedPickpocket(Player player)
		{
			base.PlayerFailedPickpocket(player);
			string line = base.npc.dialogueHandler.Database.GetLine(EDialogueModule.Reactions, "noticed_pickpocket");
			base.npc.dialogueHandler.ShowWorldspaceDialogue(line, 3f);
			base.npc.Avatar.EmotionManager.AddEmotionOverride("Angry", "noticed_pickpocket", 20f, 3);
			if (base.npc.Aggression > 0.5f && UnityEngine.Random.value < base.npc.Aggression)
			{
				base.npc.behaviour.CombatBehaviour.SetTarget(null, player.NetworkObject);
				base.npc.behaviour.CombatBehaviour.Enable_Networked(null);
				base.npc.VoiceOverEmitter.Play(EVOLineType.Angry);
				return;
			}
			float value = UnityEngine.Random.value;
			if (value > 0.3f && this.CanCallPolice)
			{
				base.actions.SetCallPoliceBehaviourCrime(new Theft());
				base.actions.CallPolice_Networked(player);
				base.npc.PlayVO(EVOLineType.Alerted);
				return;
			}
			if (value > 0.1f)
			{
				base.npc.PlayVO(EVOLineType.Alerted);
				base.npc.behaviour.FacePlayerBehaviour.SetTarget(player.NetworkObject, 5f);
				base.npc.behaviour.FacePlayerBehaviour.SendEnable();
				return;
			}
			base.npc.PlayVO(EVOLineType.Alerted);
			base.npc.behaviour.FleeBehaviour.SetEntityToFlee(player.NetworkObject);
			base.npc.behaviour.FleeBehaviour.Enable_Networked(null);
		}

		// Token: 0x06001ACD RID: 6861 RVA: 0x0007425C File Offset: 0x0007245C
		protected override void RespondToFirstNonLethalAttack(Player perpetrator, Impact impact)
		{
			base.RespondToFirstNonLethalAttack(perpetrator, impact);
			if (base.npc.Aggression > 0.5f && UnityEngine.Random.value < base.npc.Aggression)
			{
				base.npc.behaviour.CombatBehaviour.SetTarget(null, perpetrator.NetworkObject);
				base.npc.behaviour.CombatBehaviour.Enable_Networked(null);
				base.npc.VoiceOverEmitter.Play(EVOLineType.Angry);
				return;
			}
			base.npc.dialogueHandler.PlayReaction("hurt", 2.5f, false);
			base.npc.Avatar.EmotionManager.AddEmotionOverride("Annoyed", "hurt", 20f, 3);
			if (InstanceFinder.IsServer)
			{
				base.npc.behaviour.FacePlayerBehaviour.SetTarget(perpetrator.NetworkObject, 5f);
				base.npc.behaviour.FacePlayerBehaviour.SendEnable();
			}
		}

		// Token: 0x06001ACE RID: 6862 RVA: 0x00074358 File Offset: 0x00072558
		protected override void RespondToAnnoyingImpact(Player perpetrator, Impact impact)
		{
			base.RespondToAnnoyingImpact(perpetrator, impact);
			if (base.npc.Aggression > 0.6f && UnityEngine.Random.value * 1.5f < base.npc.Aggression)
			{
				base.npc.behaviour.CombatBehaviour.SetTarget(null, perpetrator.NetworkObject);
				base.npc.behaviour.CombatBehaviour.Enable_Networked(null);
				base.npc.VoiceOverEmitter.Play(EVOLineType.Angry);
				return;
			}
			base.npc.VoiceOverEmitter.Play(EVOLineType.Annoyed);
			base.npc.dialogueHandler.PlayReaction("annoyed", 2.5f, false);
			base.npc.Avatar.EmotionManager.AddEmotionOverride("Annoyed", "annoyed", 20f, 3);
			if (InstanceFinder.IsServer)
			{
				base.npc.behaviour.FacePlayerBehaviour.SetTarget(perpetrator.NetworkObject, 5f);
				base.npc.behaviour.FacePlayerBehaviour.SendEnable();
			}
		}

		// Token: 0x06001ACF RID: 6863 RVA: 0x0007446A File Offset: 0x0007266A
		protected override void RespondToLethalAttack(Player perpetrator, Impact impact)
		{
			base.RespondToLethalAttack(perpetrator, impact);
			this.RespondToLethalOrRepeatedAttack(perpetrator, impact);
		}

		// Token: 0x06001AD0 RID: 6864 RVA: 0x0007447C File Offset: 0x0007267C
		protected override void RespondToRepeatedNonLethalAttack(Player perpetrator, Impact impact)
		{
			base.RespondToRepeatedNonLethalAttack(perpetrator, impact);
			this.RespondToLethalOrRepeatedAttack(perpetrator, impact);
		}

		// Token: 0x06001AD1 RID: 6865 RVA: 0x00074490 File Offset: 0x00072690
		private void RespondToLethalOrRepeatedAttack(Player perpetrator, Impact impact)
		{
			float value = UnityEngine.Random.value;
			float aggression = base.npc.Aggression;
			if (aggression > 0.5f && value < aggression)
			{
				base.npc.behaviour.CombatBehaviour.SetTarget(null, perpetrator.NetworkObject);
				base.npc.behaviour.CombatBehaviour.Enable_Networked(null);
				base.npc.VoiceOverEmitter.Play(EVOLineType.Angry);
				return;
			}
			if (value > 0.5f && this.CanCallPolice)
			{
				if (Impact.IsLethal(impact.ImpactType))
				{
					base.actions.SetCallPoliceBehaviourCrime(new DeadlyAssault());
				}
				else
				{
					base.actions.SetCallPoliceBehaviourCrime(new Assault());
				}
				base.actions.CallPolice_Networked(perpetrator);
				return;
			}
			base.npc.SetPanicked();
			base.npc.dialogueHandler.PlayReaction("panic_start", 3f, false);
			if (value > 0.2f)
			{
				base.npc.behaviour.FleeBehaviour.SetEntityToFlee(perpetrator.NetworkObject);
				base.npc.behaviour.FleeBehaviour.Enable_Networked(null);
			}
		}

		// Token: 0x06001AD2 RID: 6866 RVA: 0x000745AC File Offset: 0x000727AC
		public override void RespondToAimedAt(Player player)
		{
			base.RespondToAimedAt(player);
			player.VisualState.ApplyState("aiming_at_npc", PlayerVisualState.EVisualState.Brandishing, 2.5f);
			if (this.currentThreatResponse == NPCResponses_Civilian.EAttackResponse.None)
			{
				this.timeSinceLastThreat = 0f;
				this.currentThreatResponse = this.GetThreatResponse(NPCResponses_Civilian.EThreatType.AimedAt, player);
				this.ExecuteThreatResponse(this.currentThreatResponse, player, player.transform.position, new BrandishingWeapon());
			}
		}

		// Token: 0x06001AD3 RID: 6867 RVA: 0x00074618 File Offset: 0x00072818
		private void ExecuteThreatResponse(NPCResponses_Civilian.EAttackResponse response, Player target, Vector3 threatOrigin, Crime crime = null)
		{
			Console.Log(string.Concat(new string[]
			{
				base.npc.fullName,
				" executing threat response: ",
				response.ToString(),
				" on target ",
				(target != null) ? target.ToString() : null
			}), null);
			if (response == NPCResponses_Civilian.EAttackResponse.None)
			{
				return;
			}
			switch (response)
			{
			case NPCResponses_Civilian.EAttackResponse.Panic:
				base.npc.SetPanicked();
				base.npc.dialogueHandler.PlayReaction("panic_start", 3f, false);
				return;
			case NPCResponses_Civilian.EAttackResponse.Flee:
				base.npc.SetPanicked();
				base.npc.dialogueHandler.PlayReaction("panic_start", 3f, false);
				if (target != null)
				{
					base.npc.behaviour.FleeBehaviour.SetEntityToFlee(target.NetworkObject);
				}
				else
				{
					base.npc.behaviour.FleeBehaviour.SetPointToFlee(threatOrigin);
				}
				base.npc.behaviour.FleeBehaviour.Enable_Networked(null);
				return;
			case NPCResponses_Civilian.EAttackResponse.CallPolice:
				if (target != null)
				{
					base.actions.SetCallPoliceBehaviourCrime(crime);
					base.actions.CallPolice_Networked(target);
					return;
				}
				break;
			case NPCResponses_Civilian.EAttackResponse.Fight:
				if (target != null)
				{
					base.npc.behaviour.CombatBehaviour.SetTarget(null, target.NetworkObject);
					base.npc.behaviour.CombatBehaviour.Enable_Networked(null);
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06001AD4 RID: 6868 RVA: 0x00074790 File Offset: 0x00072990
		private NPCResponses_Civilian.EAttackResponse GetThreatResponse(NPCResponses_Civilian.EThreatType type, Player threatSource)
		{
			if (base.npc.CurrentVehicle != null)
			{
				return NPCResponses_Civilian.EAttackResponse.Panic;
			}
			switch (type)
			{
			case NPCResponses_Civilian.EThreatType.None:
				break;
			case NPCResponses_Civilian.EThreatType.AimedAt:
				if (UnityEngine.Random.Range(0f, 1f) < base.npc.Aggression)
				{
					return NPCResponses_Civilian.EAttackResponse.Fight;
				}
				if (threatSource != null && threatSource.CrimeData.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.None)
				{
					int num = UnityEngine.Random.Range(0, 2);
					if (num == 0)
					{
						return NPCResponses_Civilian.EAttackResponse.Panic;
					}
					if (num == 1)
					{
						return NPCResponses_Civilian.EAttackResponse.Flee;
					}
					return NPCResponses_Civilian.EAttackResponse.CallPolice;
				}
				else
				{
					if (UnityEngine.Random.value < 0.5f)
					{
						return NPCResponses_Civilian.EAttackResponse.Panic;
					}
					return NPCResponses_Civilian.EAttackResponse.Flee;
				}
				break;
			case NPCResponses_Civilian.EThreatType.GunshotHeard:
				if (threatSource != null && threatSource.CrimeData.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.None)
				{
					List<NPCResponses_Civilian.EAttackResponse> list = new List<NPCResponses_Civilian.EAttackResponse>
					{
						NPCResponses_Civilian.EAttackResponse.Panic,
						NPCResponses_Civilian.EAttackResponse.Flee
					};
					if (this.CanCallPolice)
					{
						list.Add(NPCResponses_Civilian.EAttackResponse.CallPolice);
					}
					int num2 = UnityEngine.Random.Range(0, list.Count);
					if (num2 == 0)
					{
						return NPCResponses_Civilian.EAttackResponse.Panic;
					}
					if (num2 == 1)
					{
						return NPCResponses_Civilian.EAttackResponse.Flee;
					}
					return NPCResponses_Civilian.EAttackResponse.CallPolice;
				}
				else
				{
					if (UnityEngine.Random.value < 0.5f)
					{
						return NPCResponses_Civilian.EAttackResponse.Panic;
					}
					return NPCResponses_Civilian.EAttackResponse.Flee;
				}
				break;
			case NPCResponses_Civilian.EThreatType.ExplosionHeard:
				if (UnityEngine.Random.value < 0.5f)
				{
					return NPCResponses_Civilian.EAttackResponse.Panic;
				}
				return NPCResponses_Civilian.EAttackResponse.Flee;
			default:
				Console.LogError("Unhandled threat type: " + type.ToString(), null);
				break;
			}
			return NPCResponses_Civilian.EAttackResponse.None;
		}

		// Token: 0x040016B1 RID: 5809
		public bool CanCallPolice = true;

		// Token: 0x040016B2 RID: 5810
		private NPCResponses_Civilian.EAttackResponse currentThreatResponse;

		// Token: 0x040016B3 RID: 5811
		private float timeSinceLastThreat;

		// Token: 0x020004C4 RID: 1220
		public enum EAttackResponse
		{
			// Token: 0x040016B5 RID: 5813
			None,
			// Token: 0x040016B6 RID: 5814
			Panic,
			// Token: 0x040016B7 RID: 5815
			Flee,
			// Token: 0x040016B8 RID: 5816
			CallPolice,
			// Token: 0x040016B9 RID: 5817
			Fight
		}

		// Token: 0x020004C5 RID: 1221
		public enum EThreatType
		{
			// Token: 0x040016BB RID: 5819
			None,
			// Token: 0x040016BC RID: 5820
			AimedAt,
			// Token: 0x040016BD RID: 5821
			GunshotHeard,
			// Token: 0x040016BE RID: 5822
			ExplosionHeard
		}
	}
}
