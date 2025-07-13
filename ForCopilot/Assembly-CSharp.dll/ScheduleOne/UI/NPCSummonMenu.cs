using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A4F RID: 2639
	public class NPCSummonMenu : Singleton<NPCSummonMenu>
	{
		// Token: 0x170009EA RID: 2538
		// (get) Token: 0x060046E1 RID: 18145 RVA: 0x00129717 File Offset: 0x00127917
		// (set) Token: 0x060046E2 RID: 18146 RVA: 0x0012971F File Offset: 0x0012791F
		public bool IsOpen { get; private set; }

		// Token: 0x060046E3 RID: 18147 RVA: 0x00129728 File Offset: 0x00127928
		protected override void Start()
		{
			base.Start();
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 5);
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
		}

		// Token: 0x060046E4 RID: 18148 RVA: 0x0012975F File Offset: 0x0012795F
		private void Exit(ExitAction exit)
		{
			if (!this.IsOpen)
			{
				return;
			}
			if (exit.Used)
			{
				return;
			}
			if (exit.exitType == ExitType.Escape)
			{
				exit.Used = true;
				this.Close();
			}
		}

		// Token: 0x060046E5 RID: 18149 RVA: 0x0012978C File Offset: 0x0012798C
		public void Open(List<NPC> npcs, Action<NPC> _callback)
		{
			this.IsOpen = true;
			this.Canvas.enabled = true;
			this.Container.gameObject.SetActive(true);
			this.callback = _callback;
			for (int i = 0; i < this.Entries.Length; i++)
			{
				if (npcs.Count > i)
				{
					this.Entries[i].Find("Icon").GetComponent<Image>().sprite = npcs[i].MugshotSprite;
					this.Entries[i].Find("Name").GetComponent<TextMeshProUGUI>().text = npcs[i].fullName;
					this.Entries[i].gameObject.SetActive(true);
					NPC npc = npcs[i];
					this.Entries[i].GetComponent<Button>().onClick.RemoveAllListeners();
					this.Entries[i].GetComponent<Button>().onClick.AddListener(delegate()
					{
						this.NPCSelected(npc);
					});
				}
				else
				{
					this.Entries[i].gameObject.SetActive(false);
				}
			}
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
		}

		// Token: 0x060046E6 RID: 18150 RVA: 0x001298F0 File Offset: 0x00127AF0
		public void Close()
		{
			this.IsOpen = false;
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
			this.callback = null;
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(true);
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
		}

		// Token: 0x060046E7 RID: 18151 RVA: 0x00129963 File Offset: 0x00127B63
		public void NPCSelected(NPC npc)
		{
			this.callback(npc);
			this.Close();
		}

		// Token: 0x040033AA RID: 13226
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x040033AB RID: 13227
		public RectTransform Container;

		// Token: 0x040033AC RID: 13228
		public RectTransform EntryContainer;

		// Token: 0x040033AD RID: 13229
		public RectTransform[] Entries;

		// Token: 0x040033AE RID: 13230
		private Action<NPC> callback;
	}
}
