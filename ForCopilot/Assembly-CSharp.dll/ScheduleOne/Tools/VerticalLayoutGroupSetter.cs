using System;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.Tools
{
	// Token: 0x020008B8 RID: 2232
	public class VerticalLayoutGroupSetter : MonoBehaviour
	{
		// Token: 0x06003C5C RID: 15452 RVA: 0x000FE6B6 File Offset: 0x000FC8B6
		private void Awake()
		{
			this.layoutGroup = base.GetComponent<VerticalLayoutGroup>();
		}

		// Token: 0x06003C5D RID: 15453 RVA: 0x000FE6C4 File Offset: 0x000FC8C4
		public void Update()
		{
			if (this.layoutGroup.padding.left != (int)this.LeftSpacing)
			{
				this.layoutGroup.padding.left = (int)this.LeftSpacing;
				LayoutRebuilder.ForceRebuildLayoutImmediate(this.layoutGroup.GetComponent<RectTransform>());
			}
		}

		// Token: 0x04002B23 RID: 11043
		public float LeftSpacing;

		// Token: 0x04002B24 RID: 11044
		private VerticalLayoutGroup layoutGroup;
	}
}
