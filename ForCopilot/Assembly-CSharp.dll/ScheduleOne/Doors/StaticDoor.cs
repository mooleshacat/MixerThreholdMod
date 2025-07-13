using System;
using System.Collections;
using System.Runtime.CompilerServices;
using FluffyUnderware.DevTools.Extensions;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.Interaction;
using ScheduleOne.Map;
using ScheduleOne.NPCs;
using ScheduleOne.UI;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Doors
{
	// Token: 0x020006CD RID: 1741
	public class StaticDoor : MonoBehaviour
	{
		// Token: 0x06002FEB RID: 12267 RVA: 0x000C9474 File Offset: 0x000C7674
		protected virtual void Awake()
		{
			this.IntObj.onHovered.AddListener(new UnityAction(this.Hovered));
			this.IntObj.onInteractStart.AddListener(new UnityAction(this.Interacted));
			if (this.Building == null)
			{
				this.Building = base.GetComponentInParent<NPCEnterableBuilding>();
				if (this.Building == null && (this.Usable || this.CanKnock))
				{
					Console.LogWarning("StaticDoor " + base.name + " has no NPCEnterableBuilding!", null);
					this.Usable = false;
					this.CanKnock = false;
				}
			}
		}

		// Token: 0x06002FEC RID: 12268 RVA: 0x000C951C File Offset: 0x000C771C
		protected virtual void OnValidate()
		{
			if (this.Building == null)
			{
				this.Building = base.GetComponentInParent<NPCEnterableBuilding>();
			}
			if (this.Building != null && !base.transform.IsChildOf(this.Building.transform))
			{
				Console.LogWarning("StaticDoor " + base.name + " is not a child of " + this.Building.BuildingName, null);
			}
		}

		// Token: 0x06002FED RID: 12269 RVA: 0x000C958F File Offset: 0x000C778F
		protected virtual void Update()
		{
			if (this.timeSinceLastKnock < 2f)
			{
				this.timeSinceLastKnock += Time.deltaTime;
			}
		}

		// Token: 0x06002FEE RID: 12270 RVA: 0x000C95B0 File Offset: 0x000C77B0
		protected virtual void Hovered()
		{
			if (!this.CanKnockNow())
			{
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
				return;
			}
			string message;
			if (this.IsKnockValid(out message))
			{
				this.IntObj.SetMessage("Knock");
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
				return;
			}
			this.IntObj.SetMessage(message);
			this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Invalid);
		}

		// Token: 0x06002FEF RID: 12271 RVA: 0x000C9611 File Offset: 0x000C7811
		protected virtual bool CanKnockNow()
		{
			return this.CanKnock && this.timeSinceLastKnock >= 2f && this.Building != null;
		}

		// Token: 0x06002FF0 RID: 12272 RVA: 0x00075416 File Offset: 0x00073616
		protected virtual bool IsKnockValid(out string message)
		{
			message = string.Empty;
			return true;
		}

		// Token: 0x06002FF1 RID: 12273 RVA: 0x000C9636 File Offset: 0x000C7836
		protected virtual void Interacted()
		{
			this.Knock();
		}

		// Token: 0x06002FF2 RID: 12274 RVA: 0x000C963E File Offset: 0x000C783E
		protected virtual void Knock()
		{
			this.timeSinceLastKnock = 0f;
			if (this.KnockSound != null)
			{
				this.KnockSound.Play();
			}
			base.StartCoroutine(this.<Knock>g__knockRoutine|16_0());
		}

		// Token: 0x06002FF3 RID: 12275 RVA: 0x000C9674 File Offset: 0x000C7874
		protected virtual void NPCSelected(NPC npc)
		{
			npc.behaviour.Summon(this.Building.GUID.ToString(), ArrayExt.IndexOf<StaticDoor>(this.Building.Doors, this), 8f);
		}

		// Token: 0x06002FF5 RID: 12277 RVA: 0x000C96DC File Offset: 0x000C78DC
		[CompilerGenerated]
		private IEnumerator <Knock>g__knockRoutine|16_0()
		{
			yield return new WaitForSeconds(0.7f);
			if (this.Building.OccupantCount > 0)
			{
				if (this.Building.OccupantCount == 1)
				{
					this.NPCSelected(this.Building.GetSummonableNPCs()[0]);
				}
				else
				{
					Singleton<NPCSummonMenu>.Instance.Open(this.Building.GetSummonableNPCs(), new Action<NPC>(this.NPCSelected));
				}
			}
			else
			{
				Console.Log("Building is empty!", null);
			}
			yield break;
		}

		// Token: 0x040021BD RID: 8637
		public const float KNOCK_COOLDOWN = 2f;

		// Token: 0x040021BE RID: 8638
		public const float SUMMON_DURATION = 8f;

		// Token: 0x040021BF RID: 8639
		[Header("References")]
		public Transform AccessPoint;

		// Token: 0x040021C0 RID: 8640
		public InteractableObject IntObj;

		// Token: 0x040021C1 RID: 8641
		public AudioSourceController KnockSound;

		// Token: 0x040021C2 RID: 8642
		public NPCEnterableBuilding Building;

		// Token: 0x040021C3 RID: 8643
		[Header("Settings")]
		public bool Usable = true;

		// Token: 0x040021C4 RID: 8644
		public bool CanKnock = true;

		// Token: 0x040021C5 RID: 8645
		private float timeSinceLastKnock = 2f;
	}
}
