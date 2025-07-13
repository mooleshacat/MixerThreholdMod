using System;
using ScheduleOne.Noise;
using ScheduleOne.NPCs.Responses;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Vehicles;
using ScheduleOne.Vision;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.NPCs
{
	// Token: 0x02000492 RID: 1170
	public class NPCAwareness : MonoBehaviour
	{
		// Token: 0x060017F9 RID: 6137 RVA: 0x00069848 File Offset: 0x00067A48
		protected virtual void Awake()
		{
			this.npc = base.GetComponentInParent<NPC>();
			if (this.Responses == null)
			{
				Console.LogError("NPCAwareness doesn't have a reference to NPCResponses - responses won't be automatically connected.", null);
			}
			VisionCone visionCone = this.VisionCone;
			visionCone.onVisionEventFull = (VisionCone.EventStateChange)Delegate.Combine(visionCone.onVisionEventFull, new VisionCone.EventStateChange(this.VisionEvent));
			Listener listener = this.Listener;
			listener.onNoiseHeard = (Listener.HearingEvent)Delegate.Combine(listener.onNoiseHeard, new Listener.HearingEvent(this.NoiseEvent));
		}

		// Token: 0x060017FA RID: 6138 RVA: 0x000698C8 File Offset: 0x00067AC8
		public void SetAwarenessActive(bool active)
		{
			this.Listener.enabled = active;
			this.VisionCone.enabled = active;
			base.enabled = active;
		}

		// Token: 0x060017FB RID: 6139 RVA: 0x000698EC File Offset: 0x00067AEC
		public void VisionEvent(VisionEventReceipt vEvent)
		{
			if (!base.enabled)
			{
				return;
			}
			switch (vEvent.State)
			{
			case PlayerVisualState.EVisualState.Visible:
			case PlayerVisualState.EVisualState.SearchedFor:
				break;
			case PlayerVisualState.EVisualState.Suspicious:
				if (this.onNoticedSuspiciousPlayer != null)
				{
					this.onNoticedSuspiciousPlayer.Invoke(vEvent.TargetPlayer.GetComponent<Player>());
				}
				if (this.Responses != null)
				{
					this.Responses.NoticedSuspiciousPlayer(vEvent.TargetPlayer.GetComponent<Player>());
					return;
				}
				break;
			case PlayerVisualState.EVisualState.DisobeyingCurfew:
				if (this.onNoticedPlayerViolatingCurfew != null)
				{
					this.onNoticedPlayerViolatingCurfew.Invoke(vEvent.TargetPlayer.GetComponent<Player>());
				}
				if (this.Responses != null)
				{
					this.Responses.NoticedViolatingCurfew(vEvent.TargetPlayer.GetComponent<Player>());
					return;
				}
				break;
			case PlayerVisualState.EVisualState.Vandalizing:
				if (this.Responses != null)
				{
					this.Responses.NoticedVandalism(vEvent.TargetPlayer.GetComponent<Player>());
					return;
				}
				break;
			case PlayerVisualState.EVisualState.PettyCrime:
				if (this.onNoticedPettyCrime != null)
				{
					this.onNoticedPettyCrime.Invoke(vEvent.TargetPlayer.GetComponent<Player>());
				}
				if (this.onNoticedGeneralCrime != null)
				{
					this.onNoticedGeneralCrime.Invoke(vEvent.TargetPlayer.GetComponent<Player>());
				}
				if (this.Responses != null)
				{
					this.Responses.NoticedPettyCrime(vEvent.TargetPlayer.GetComponent<Player>());
					return;
				}
				break;
			case PlayerVisualState.EVisualState.DrugDealing:
				if (this.onNoticedDrugDealing != null)
				{
					this.onNoticedDrugDealing.Invoke(vEvent.TargetPlayer.GetComponent<Player>());
				}
				if (this.onNoticedGeneralCrime != null)
				{
					this.onNoticedGeneralCrime.Invoke(vEvent.TargetPlayer.GetComponent<Player>());
				}
				if (this.Responses != null)
				{
					this.Responses.NoticedDrugDeal(vEvent.TargetPlayer.GetComponent<Player>());
					return;
				}
				break;
			case PlayerVisualState.EVisualState.Wanted:
				if (this.Responses != null)
				{
					this.Responses.NoticedWantedPlayer(vEvent.TargetPlayer.GetComponent<Player>());
					return;
				}
				break;
			case PlayerVisualState.EVisualState.Pickpocketing:
				if (this.Responses != null)
				{
					this.Responses.SawPickpocketing(vEvent.TargetPlayer.GetComponent<Player>());
					return;
				}
				break;
			case PlayerVisualState.EVisualState.DischargingWeapon:
				if (this.Responses != null)
				{
					this.Responses.NoticePlayerDischargingWeapon(vEvent.TargetPlayer.GetComponent<Player>());
				}
				break;
			case PlayerVisualState.EVisualState.Brandishing:
				if (this.Responses != null)
				{
					this.Responses.NoticePlayerBrandishingWeapon(vEvent.TargetPlayer.GetComponent<Player>());
					return;
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x060017FC RID: 6140 RVA: 0x00069B50 File Offset: 0x00067D50
		public void NoiseEvent(NoiseEvent nEvent)
		{
			if (!base.enabled)
			{
				return;
			}
			if (nEvent.type == ENoiseType.Gunshot)
			{
				if (this.onGunshotHeard != null)
				{
					this.onGunshotHeard.Invoke(nEvent);
				}
				if (this.Responses != null)
				{
					this.Responses.GunshotHeard(nEvent);
				}
			}
			if (nEvent.type == ENoiseType.Explosion)
			{
				if (this.onExplosionHeard != null)
				{
					this.onExplosionHeard.Invoke(nEvent);
				}
				if (this.Responses != null)
				{
					this.Responses.ExplosionHeard(nEvent);
				}
			}
		}

		// Token: 0x060017FD RID: 6141 RVA: 0x00069BD4 File Offset: 0x00067DD4
		public void HitByCar(LandVehicle vehicle)
		{
			if (this.onHitByCar != null)
			{
				this.onHitByCar.Invoke(vehicle);
			}
			if (this.Responses != null)
			{
				this.Responses.HitByCar(vehicle);
			}
		}

		// Token: 0x04001570 RID: 5488
		public const float PLAYER_AIM_DETECTION_RANGE = 15f;

		// Token: 0x04001571 RID: 5489
		[Header("References")]
		public VisionCone VisionCone;

		// Token: 0x04001572 RID: 5490
		public Listener Listener;

		// Token: 0x04001573 RID: 5491
		public NPCResponses Responses;

		// Token: 0x04001574 RID: 5492
		public UnityEvent<Player> onNoticedGeneralCrime;

		// Token: 0x04001575 RID: 5493
		public UnityEvent<Player> onNoticedPettyCrime;

		// Token: 0x04001576 RID: 5494
		public UnityEvent<Player> onNoticedDrugDealing;

		// Token: 0x04001577 RID: 5495
		public UnityEvent<Player> onNoticedPlayerViolatingCurfew;

		// Token: 0x04001578 RID: 5496
		public UnityEvent<Player> onNoticedSuspiciousPlayer;

		// Token: 0x04001579 RID: 5497
		public UnityEvent<NoiseEvent> onGunshotHeard;

		// Token: 0x0400157A RID: 5498
		public UnityEvent<NoiseEvent> onExplosionHeard;

		// Token: 0x0400157B RID: 5499
		public UnityEvent<LandVehicle> onHitByCar;

		// Token: 0x0400157C RID: 5500
		private NPC npc;
	}
}
