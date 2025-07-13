using System;
using EasyButtons;
using ScheduleOne.Economy;
using ScheduleOne.NPCs;
using ScheduleOne.NPCs.Relation;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ScheduleOne.UI.Relations
{
	// Token: 0x02000ACA RID: 2762
	public class RelationCircle : MonoBehaviour
	{
		// Token: 0x06004A0E RID: 18958 RVA: 0x00137120 File Offset: 0x00135320
		private void Awake()
		{
			this.LoadNPCData();
			if (this.AssignedNPC != null)
			{
				this.AssignNPC(this.AssignedNPC);
			}
			else if (this.AssignedNPC_ID != string.Empty)
			{
				Console.LogWarning("Failed to find NPC with ID '" + this.AssignedNPC_ID + "'", null);
			}
			this.Button.onClick.AddListener(new UnityAction(this.ButtonClicked));
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = 0;
			entry.callback.AddListener(delegate(BaseEventData <p0>)
			{
				this.HoverStart();
			});
			this.Trigger.triggers.Add(entry);
			EventTrigger.Entry entry2 = new EventTrigger.Entry();
			entry2.eventID = 1;
			entry2.callback.AddListener(delegate(BaseEventData <p0>)
			{
				this.HoverEnd();
			});
			this.Trigger.triggers.Add(entry2);
		}

		// Token: 0x06004A0F RID: 18959 RVA: 0x00137204 File Offset: 0x00135404
		private void OnValidate()
		{
			if (this.AssignedNPC != null)
			{
				this.AssignedNPC_ID = this.AssignedNPC.ID;
				this.HeadshotImg.sprite = this.AssignedNPC.MugshotSprite;
			}
			if (this.AutoSetName && this.AssignedNPC != null)
			{
				base.gameObject.name = this.AssignedNPC_ID;
			}
		}

		// Token: 0x06004A10 RID: 18960 RVA: 0x00137270 File Offset: 0x00135470
		public void AssignNPC(NPC npc)
		{
			if (npc != null)
			{
				this.UnassignNPC();
			}
			this.AssignedNPC = npc;
			NPCRelationData relationData = this.AssignedNPC.RelationData;
			relationData.onRelationshipChange = (Action<float>)Delegate.Combine(relationData.onRelationshipChange, new Action<float>(this.RelationshipChange));
			NPCRelationData relationData2 = this.AssignedNPC.RelationData;
			relationData2.onUnlocked = (Action<NPCRelationData.EUnlockType, bool>)Delegate.Combine(relationData2.onUnlocked, new Action<NPCRelationData.EUnlockType, bool>(this.SetUnlocked));
			foreach (NPC npc2 in this.AssignedNPC.RelationData.Connections)
			{
				NPCRelationData relationData3 = npc2.RelationData;
				relationData3.onUnlocked = (Action<NPCRelationData.EUnlockType, bool>)Delegate.Combine(relationData3.onUnlocked, new Action<NPCRelationData.EUnlockType, bool>(delegate(NPCRelationData.EUnlockType <p0>, bool <p1>)
				{
					this.UpdateBlackout();
				}));
			}
			if (npc.RelationData.Unlocked)
			{
				this.SetUnlocked(npc.RelationData.UnlockType, false);
			}
			else
			{
				this.SetLocked();
			}
			if (npc is Dealer)
			{
				(npc as Dealer).onRecommended.AddListener(new UnityAction(this.UpdateBlackout));
			}
			this.HeadshotImg.sprite = this.AssignedNPC.MugshotSprite;
			this.RefreshNotchPosition();
			this.RefreshDependenceDisplay();
			this.UpdateBlackout();
		}

		// Token: 0x06004A11 RID: 18961 RVA: 0x001373CC File Offset: 0x001355CC
		private void UnassignNPC()
		{
			if (this.AssignedNPC != null)
			{
				NPCRelationData relationData = this.AssignedNPC.RelationData;
				relationData.onRelationshipChange = (Action<float>)Delegate.Remove(relationData.onRelationshipChange, new Action<float>(this.RelationshipChange));
				NPCRelationData relationData2 = this.AssignedNPC.RelationData;
				relationData2.onUnlocked = (Action<NPCRelationData.EUnlockType, bool>)Delegate.Remove(relationData2.onUnlocked, new Action<NPCRelationData.EUnlockType, bool>(this.SetUnlocked));
			}
		}

		// Token: 0x06004A12 RID: 18962 RVA: 0x0013743F File Offset: 0x0013563F
		private void RelationshipChange(float change)
		{
			this.RefreshNotchPosition();
		}

		// Token: 0x06004A13 RID: 18963 RVA: 0x00137447 File Offset: 0x00135647
		public void SetNotchPosition(float relationshipDelta)
		{
			this.NotchPivot.localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(90f, -90f, relationshipDelta / 5f));
		}

		// Token: 0x06004A14 RID: 18964 RVA: 0x00137479 File Offset: 0x00135679
		private void RefreshNotchPosition()
		{
			this.SetNotchPosition(this.AssignedNPC.RelationData.RelationDelta);
		}

		// Token: 0x06004A15 RID: 18965 RVA: 0x00137494 File Offset: 0x00135694
		private void RefreshDependenceDisplay()
		{
			Customer component = this.AssignedNPC.GetComponent<Customer>();
			if (component == null)
			{
				this.PortraitBackground.color = RelationCircle.PortraitColor_ZeroDependence;
				return;
			}
			this.PortraitBackground.color = Color.Lerp(RelationCircle.PortraitColor_ZeroDependence, RelationCircle.PortraitColor_MaxDependence, component.CurrentAddiction);
		}

		// Token: 0x06004A16 RID: 18966 RVA: 0x001374E7 File Offset: 0x001356E7
		[Button]
		public void SetLocked()
		{
			this.Locked.gameObject.SetActive(true);
			this.NotchPivot.gameObject.SetActive(false);
		}

		// Token: 0x06004A17 RID: 18967 RVA: 0x0013750B File Offset: 0x0013570B
		[Button]
		public void SetUnlocked(NPCRelationData.EUnlockType unlockType, bool notify = true)
		{
			this.Locked.gameObject.SetActive(false);
			this.NotchPivot.gameObject.SetActive(true);
			this.SetBlackedOut(false);
		}

		// Token: 0x06004A18 RID: 18968 RVA: 0x00137536 File Offset: 0x00135736
		[Button]
		public void LoadNPCData()
		{
			this.AssignedNPC = NPCManager.GetNPC(this.AssignedNPC_ID);
		}

		// Token: 0x06004A19 RID: 18969 RVA: 0x0013754C File Offset: 0x0013574C
		private void UpdateBlackout()
		{
			bool blackedOut = false;
			if (!this.AssignedNPC.RelationData.Unlocked)
			{
				if (this.AssignedNPC is Dealer)
				{
					blackedOut = !(this.AssignedNPC as Dealer).HasBeenRecommended;
				}
				else if (this.AssignedNPC is Supplier)
				{
					blackedOut = true;
				}
				else if (this.AssignedNPC.GetComponent<Customer>() != null)
				{
					blackedOut = (!this.AssignedNPC.RelationData.Unlocked && !this.AssignedNPC.RelationData.IsMutuallyKnown());
				}
			}
			this.SetBlackedOut(blackedOut);
		}

		// Token: 0x06004A1A RID: 18970 RVA: 0x001375E4 File Offset: 0x001357E4
		public void SetBlackedOut(bool blackedOut)
		{
			this.HeadshotImg.color = (blackedOut ? Color.black : Color.white);
		}

		// Token: 0x06004A1B RID: 18971 RVA: 0x00137600 File Offset: 0x00135800
		private void ButtonClicked()
		{
			if (this.onClicked != null)
			{
				this.onClicked();
			}
		}

		// Token: 0x06004A1C RID: 18972 RVA: 0x00137615 File Offset: 0x00135815
		private void HoverStart()
		{
			if (this.onHoverStart != null)
			{
				this.onHoverStart();
			}
		}

		// Token: 0x06004A1D RID: 18973 RVA: 0x0013762A File Offset: 0x0013582A
		private void HoverEnd()
		{
			if (this.onHoverEnd != null)
			{
				this.onHoverEnd();
			}
		}

		// Token: 0x04003664 RID: 13924
		public const float NotchMinRot = 90f;

		// Token: 0x04003665 RID: 13925
		public const float NotchMaxRot = -90f;

		// Token: 0x04003666 RID: 13926
		public static Color PortraitColor_ZeroDependence = new Color32(60, 60, 60, byte.MaxValue);

		// Token: 0x04003667 RID: 13927
		public static Color PortraitColor_MaxDependence = new Color32(120, 15, 15, byte.MaxValue);

		// Token: 0x04003668 RID: 13928
		public string AssignedNPC_ID = string.Empty;

		// Token: 0x04003669 RID: 13929
		public NPC AssignedNPC;

		// Token: 0x0400366A RID: 13930
		public Action onClicked;

		// Token: 0x0400366B RID: 13931
		public Action onHoverStart;

		// Token: 0x0400366C RID: 13932
		public Action onHoverEnd;

		// Token: 0x0400366D RID: 13933
		public bool AutoSetName;

		// Token: 0x0400366E RID: 13934
		[Header("References")]
		public RectTransform Rect;

		// Token: 0x0400366F RID: 13935
		public Image PortraitBackground;

		// Token: 0x04003670 RID: 13936
		public Image HeadshotImg;

		// Token: 0x04003671 RID: 13937
		public RectTransform NotchPivot;

		// Token: 0x04003672 RID: 13938
		public RectTransform Locked;

		// Token: 0x04003673 RID: 13939
		public Button Button;

		// Token: 0x04003674 RID: 13940
		public EventTrigger Trigger;
	}
}
