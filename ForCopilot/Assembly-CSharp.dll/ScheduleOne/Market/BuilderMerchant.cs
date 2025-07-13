using System;
using ScheduleOne.Construction;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Interaction;
using ScheduleOne.Property;
using ScheduleOne.UI;
using UnityEngine;

namespace ScheduleOne.Market
{
	// Token: 0x02000595 RID: 1429
	public class BuilderMerchant : MonoBehaviour
	{
		// Token: 0x06002291 RID: 8849 RVA: 0x0008EC50 File Offset: 0x0008CE50
		public void Hovered()
		{
			if (Singleton<ConstructionManager>.Instance.constructionModeEnabled || this.selector.isOpen)
			{
				this.intObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
				return;
			}
			if (NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(this.openTime, this.closeTime))
			{
				this.intObj.SetMessage("View construction menu");
				this.intObj.SetInteractableState(InteractableObject.EInteractableState.Default);
				return;
			}
			this.intObj.SetInteractableState(InteractableObject.EInteractableState.Invalid);
			this.intObj.SetMessage("Closed");
		}

		// Token: 0x06002292 RID: 8850 RVA: 0x0008ECD4 File Offset: 0x0008CED4
		public void Interacted()
		{
			this.selector.OpenSelector(new PropertySelector.PropertySelected(this.PropertySelected));
		}

		// Token: 0x06002293 RID: 8851 RVA: 0x0008ECED File Offset: 0x0008CEED
		private void PropertySelected(Property p)
		{
			Singleton<ConstructionManager>.Instance.EnterConstructionMode(p);
		}

		// Token: 0x04001A4A RID: 6730
		[Header("Settings")]
		[SerializeField]
		protected int openTime = 600;

		// Token: 0x04001A4B RID: 6731
		[SerializeField]
		protected int closeTime = 1800;

		// Token: 0x04001A4C RID: 6732
		[Header("References")]
		[SerializeField]
		protected InteractableObject intObj;

		// Token: 0x04001A4D RID: 6733
		[SerializeField]
		private PropertySelector selector;
	}
}
