using System;
using ScheduleOne.Management;
using ScheduleOne.ObjectScripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000B56 RID: 2902
	public class PotUIElement : WorldspaceUIElement
	{
		// Token: 0x17000AA2 RID: 2722
		// (get) Token: 0x06004D4A RID: 19786 RVA: 0x00145877 File Offset: 0x00143A77
		// (set) Token: 0x06004D4B RID: 19787 RVA: 0x0014587F File Offset: 0x00143A7F
		public Pot AssignedPot { get; protected set; }

		// Token: 0x06004D4C RID: 19788 RVA: 0x00145888 File Offset: 0x00143A88
		public void Initialize(Pot pot)
		{
			this.AssignedPot = pot;
			this.AssignedPot.Configuration.onChanged.AddListener(new UnityAction(this.RefreshUI));
			this.RefreshUI();
			base.gameObject.SetActive(false);
		}

		// Token: 0x06004D4D RID: 19789 RVA: 0x001458C8 File Offset: 0x00143AC8
		protected virtual void RefreshUI()
		{
			PotConfiguration potConfiguration = this.AssignedPot.Configuration as PotConfiguration;
			this.NoSeed.gameObject.SetActive(potConfiguration.Seed.SelectedItem == null);
			this.SeedIcon.gameObject.SetActive(potConfiguration.Seed.SelectedItem != null);
			if (potConfiguration.Seed.SelectedItem != null)
			{
				this.SeedIcon.sprite = potConfiguration.Seed.SelectedItem.Icon;
			}
			if (potConfiguration.Additive1.SelectedItem != null)
			{
				this.Additive1Icon.sprite = potConfiguration.Additive1.SelectedItem.Icon;
				this.Additive1Icon.gameObject.SetActive(true);
			}
			else
			{
				this.Additive1Icon.gameObject.SetActive(false);
			}
			if (potConfiguration.Additive2.SelectedItem != null)
			{
				this.Additive2Icon.sprite = potConfiguration.Additive2.SelectedItem.Icon;
				this.Additive2Icon.gameObject.SetActive(true);
			}
			else
			{
				this.Additive2Icon.gameObject.SetActive(false);
			}
			if (potConfiguration.Additive3.SelectedItem != null)
			{
				this.Additive3Icon.sprite = potConfiguration.Additive3.SelectedItem.Icon;
				this.Additive3Icon.gameObject.SetActive(true);
			}
			else
			{
				this.Additive3Icon.gameObject.SetActive(false);
			}
			base.SetAssignedNPC(potConfiguration.AssignedBotanist.SelectedNPC);
		}

		// Token: 0x04003996 RID: 14742
		[Header("References")]
		public Image SeedIcon;

		// Token: 0x04003997 RID: 14743
		public GameObject NoSeed;

		// Token: 0x04003998 RID: 14744
		public Image Additive1Icon;

		// Token: 0x04003999 RID: 14745
		public Image Additive2Icon;

		// Token: 0x0400399A RID: 14746
		public Image Additive3Icon;
	}
}
