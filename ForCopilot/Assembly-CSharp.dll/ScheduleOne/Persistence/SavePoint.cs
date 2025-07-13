using System;
using FishNet;
using ScheduleOne.DevUtilities;
using ScheduleOne.Interaction;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Persistence
{
	// Token: 0x02000398 RID: 920
	public class SavePoint : MonoBehaviour
	{
		// Token: 0x060014EB RID: 5355 RVA: 0x0005CC20 File Offset: 0x0005AE20
		public void Awake()
		{
			this.IntObj.onHovered.AddListener(new UnityAction(this.Hovered));
			this.IntObj.onInteractStart.AddListener(new UnityAction(this.Interacted));
			Singleton<SaveManager>.Instance.onSaveComplete.RemoveListener(new UnityAction(this.OnSaveComplete));
			Singleton<SaveManager>.Instance.onSaveComplete.AddListener(new UnityAction(this.OnSaveComplete));
			Singleton<SaveManager>.Instance.onSaveStart.RemoveListener(new UnityAction(this.OnSaveStart));
			Singleton<SaveManager>.Instance.onSaveStart.AddListener(new UnityAction(this.OnSaveStart));
		}

		// Token: 0x060014EC RID: 5356 RVA: 0x0005CCD4 File Offset: 0x0005AED4
		public void Hovered()
		{
			if (!InstanceFinder.IsServer)
			{
				this.IntObj.SetMessage("Only host can save");
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Invalid);
				return;
			}
			if (Singleton<SaveManager>.Instance.IsSaving)
			{
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
			}
			string message;
			if (this.CanSave(out message))
			{
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
				this.IntObj.SetMessage("Save game");
				return;
			}
			if (Singleton<SaveManager>.Instance.SecondsSinceLastSave < 10f)
			{
				this.IntObj.SetMessage("Game saved!");
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Label);
				return;
			}
			this.IntObj.SetMessage(message);
			this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Invalid);
		}

		// Token: 0x060014ED RID: 5357 RVA: 0x0005CD8C File Offset: 0x0005AF8C
		private bool CanSave(out string reason)
		{
			reason = string.Empty;
			if (Singleton<SaveManager>.Instance.SecondsSinceLastSave < 60f)
			{
				reason = "Wait " + Mathf.Ceil(60f - Singleton<SaveManager>.Instance.SecondsSinceLastSave).ToString() + "s";
				return false;
			}
			if (Singleton<SaveManager>.Instance.SecondsSinceLastSave < 60f)
			{
				reason = "Wait " + Mathf.Ceil(60f - Singleton<SaveManager>.Instance.SecondsSinceLastSave).ToString() + "s";
				return false;
			}
			return true;
		}

		// Token: 0x060014EE RID: 5358 RVA: 0x0005CE24 File Offset: 0x0005B024
		public void Interacted()
		{
			string text;
			if (!this.CanSave(out text))
			{
				return;
			}
			this.Save();
		}

		// Token: 0x060014EF RID: 5359 RVA: 0x0005CE42 File Offset: 0x0005B042
		private void Save()
		{
			Singleton<SaveManager>.Instance.Save();
		}

		// Token: 0x060014F0 RID: 5360 RVA: 0x0005CE4E File Offset: 0x0005B04E
		public void OnSaveStart()
		{
			if (this.onSaveStart != null)
			{
				this.onSaveStart.Invoke();
			}
		}

		// Token: 0x060014F1 RID: 5361 RVA: 0x0005CE63 File Offset: 0x0005B063
		public void OnSaveComplete()
		{
			if (this.onSaveComplete != null)
			{
				this.onSaveComplete.Invoke();
			}
		}

		// Token: 0x0400138A RID: 5002
		public const float SAVE_COOLDOWN = 60f;

		// Token: 0x0400138B RID: 5003
		public InteractableObject IntObj;

		// Token: 0x0400138C RID: 5004
		public UnityEvent onSaveStart;

		// Token: 0x0400138D RID: 5005
		public UnityEvent onSaveComplete;
	}
}
