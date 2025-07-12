using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A2C RID: 2604
	public class GenericSelectionModule : Singleton<GenericSelectionModule>
	{
		// Token: 0x170009CB RID: 2507
		// (get) Token: 0x06004609 RID: 17929 RVA: 0x001261CB File Offset: 0x001243CB
		// (set) Token: 0x0600460A RID: 17930 RVA: 0x001261D3 File Offset: 0x001243D3
		public bool isOpen { get; protected set; }

		// Token: 0x170009CC RID: 2508
		// (get) Token: 0x0600460B RID: 17931 RVA: 0x001261DC File Offset: 0x001243DC
		// (set) Token: 0x0600460C RID: 17932 RVA: 0x001261E4 File Offset: 0x001243E4
		[HideInInspector]
		public int ChosenOptionIndex { get; protected set; } = -1;

		// Token: 0x0600460D RID: 17933 RVA: 0x001261ED File Offset: 0x001243ED
		protected override void Awake()
		{
			base.Awake();
			this.Close();
		}

		// Token: 0x0600460E RID: 17934 RVA: 0x001261FB File Offset: 0x001243FB
		protected override void Start()
		{
			base.Start();
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 50);
		}

		// Token: 0x0600460F RID: 17935 RVA: 0x00126216 File Offset: 0x00124416
		private void Exit(ExitAction action)
		{
			if (!this.isOpen)
			{
				return;
			}
			if (action.Used)
			{
				return;
			}
			if (action.exitType == ExitType.Escape)
			{
				action.Used = true;
				this.Cancel();
			}
		}

		// Token: 0x06004610 RID: 17936 RVA: 0x00126240 File Offset: 0x00124440
		public void Open(string title, List<string> options)
		{
			this.isOpen = true;
			this.OptionChosen = false;
			this.ChosenOptionIndex = -1;
			this.ClearOptions();
			this.TitleText.text = title;
			for (int i = 0; i < options.Count; i++)
			{
				RectTransform component = UnityEngine.Object.Instantiate<GameObject>(this.ListOptionPrefab, this.OptionContainer).GetComponent<RectTransform>();
				component.Find("Label").GetComponent<TextMeshProUGUI>().text = options[i];
				component.anchoredPosition = new Vector2(0f, -((float)i + 0.5f) * component.sizeDelta.y);
				int index = i;
				component.GetComponent<Button>().onClick.AddListener(delegate()
				{
					this.ListOptionClicked(index);
				});
			}
			this.canvas.enabled = true;
		}

		// Token: 0x06004611 RID: 17937 RVA: 0x0012631F File Offset: 0x0012451F
		public void Close()
		{
			this.isOpen = false;
			this.canvas.enabled = false;
			this.ClearOptions();
		}

		// Token: 0x06004612 RID: 17938 RVA: 0x0012633A File Offset: 0x0012453A
		public void Cancel()
		{
			this.ChosenOptionIndex = -1;
			this.OptionChosen = true;
			this.Close();
		}

		// Token: 0x06004613 RID: 17939 RVA: 0x00126350 File Offset: 0x00124550
		private void ClearOptions()
		{
			int childCount = this.OptionContainer.childCount;
			for (int i = 0; i < childCount; i++)
			{
				UnityEngine.Object.Destroy(this.OptionContainer.GetChild(0).gameObject);
			}
		}

		// Token: 0x06004614 RID: 17940 RVA: 0x0012638B File Offset: 0x0012458B
		private void ListOptionClicked(int index)
		{
			this.ChosenOptionIndex = index;
			this.OptionChosen = true;
			this.Close();
		}

		// Token: 0x040032BE RID: 12990
		[Header("References")]
		public Canvas canvas;

		// Token: 0x040032BF RID: 12991
		public TextMeshProUGUI TitleText;

		// Token: 0x040032C0 RID: 12992
		public RectTransform OptionContainer;

		// Token: 0x040032C1 RID: 12993
		public Button CloseButton;

		// Token: 0x040032C2 RID: 12994
		[Header("Prefabs")]
		public GameObject ListOptionPrefab;

		// Token: 0x040032C3 RID: 12995
		[HideInInspector]
		public bool OptionChosen;
	}
}
