using System;
using System.Collections.Generic;
using ScheduleOne.UI.MainMenu;
using TMPro;
using UnityEngine;

namespace ScheduleOne
{
	// Token: 0x02000281 RID: 641
	public class CommandListScreen : MainMenuScreen
	{
		// Token: 0x06000D71 RID: 3441 RVA: 0x0003B554 File Offset: 0x00039754
		private void Start()
		{
			if (this.commandEntries.Count == 0)
			{
				foreach (Console.ConsoleCommand consoleCommand in Console.Commands)
				{
					RectTransform rectTransform = UnityEngine.Object.Instantiate<RectTransform>(this.CommandEntryPrefab, this.CommandEntryContainer);
					rectTransform.Find("Command").GetComponent<TextMeshProUGUI>().text = consoleCommand.CommandWord;
					rectTransform.Find("Description").GetComponent<TextMeshProUGUI>().text = consoleCommand.CommandDescription;
					rectTransform.Find("Example").GetComponent<TextMeshProUGUI>().text = consoleCommand.ExampleUsage;
					this.commandEntries.Add(rectTransform);
				}
			}
			this.CommandEntryContainer.offsetMin = new Vector2(this.CommandEntryContainer.offsetMin.x, 0f);
			this.CommandEntryContainer.offsetMax = new Vector2(this.CommandEntryContainer.offsetMax.x, 0f);
		}

		// Token: 0x04000DC7 RID: 3527
		public RectTransform CommandEntryContainer;

		// Token: 0x04000DC8 RID: 3528
		public RectTransform CommandEntryPrefab;

		// Token: 0x04000DC9 RID: 3529
		private List<RectTransform> commandEntries = new List<RectTransform>();
	}
}
