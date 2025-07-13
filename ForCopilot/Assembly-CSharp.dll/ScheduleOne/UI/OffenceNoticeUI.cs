using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Police;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A69 RID: 2665
	public class OffenceNoticeUI : Singleton<OffenceNoticeUI>
	{
		// Token: 0x060047AF RID: 18351 RVA: 0x0012D2FC File Offset: 0x0012B4FC
		public void ShowOffenceNotice(Offense offence)
		{
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			for (int i = 0; i < this.charges.Count; i++)
			{
				if (i < offence.charges.Count)
				{
					string str = "- ";
					if (offence.charges[i].quantity > 1)
					{
						str = "- " + offence.charges[i].quantity.ToString() + "x ";
					}
					this.charges[i].text = str + offence.charges[i].chargeName;
					this.charges[i].enabled = true;
				}
				else
				{
					this.charges[i].enabled = false;
				}
			}
			for (int j = 0; j < this.penalties.Count; j++)
			{
				if (j < offence.penalties.Count)
				{
					string str2 = "- ";
					this.penalties[j].text = str2 + offence.penalties[j];
					this.penalties[j].enabled = true;
				}
				else
				{
					this.penalties[j].enabled = false;
				}
			}
			this.container.gameObject.SetActive(true);
		}

		// Token: 0x060047B0 RID: 18352 RVA: 0x0012D458 File Offset: 0x0012B658
		protected void Update()
		{
			if (this.container.activeSelf && GameInput.GetButtonDown(GameInput.ButtonCode.Escape))
			{
				PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
				this.container.gameObject.SetActive(false);
				PlayerSingleton<PlayerMovement>.Instance.canMove = true;
				PlayerSingleton<PlayerCamera>.Instance.SetCanLook(true);
				PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
				Singleton<HUD>.Instance.SetCrosshairVisible(true);
				PlayerSingleton<PlayerCamera>.Instance.SetDoFActive(false, 0.2f);
			}
		}

		// Token: 0x0400347B RID: 13435
		[Header("References")]
		[SerializeField]
		protected GameObject container;

		// Token: 0x0400347C RID: 13436
		[SerializeField]
		protected List<Text> charges = new List<Text>();

		// Token: 0x0400347D RID: 13437
		[SerializeField]
		protected List<Text> penalties = new List<Text>();
	}
}
