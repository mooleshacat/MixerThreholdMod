using System;
using System.Collections.Generic;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x02000724 RID: 1828
	public class MetadataManager : Singleton<MetadataManager>, IBaseSaveable, ISaveable
	{
		// Token: 0x17000717 RID: 1815
		// (get) Token: 0x0600316C RID: 12652 RVA: 0x000CE550 File Offset: 0x000CC750
		// (set) Token: 0x0600316D RID: 12653 RVA: 0x000CE558 File Offset: 0x000CC758
		public DateTime CreationDate { get; protected set; }

		// Token: 0x17000718 RID: 1816
		// (get) Token: 0x0600316E RID: 12654 RVA: 0x000CE561 File Offset: 0x000CC761
		// (set) Token: 0x0600316F RID: 12655 RVA: 0x000CE569 File Offset: 0x000CC769
		public string CreationVersion { get; protected set; } = string.Empty;

		// Token: 0x17000719 RID: 1817
		// (get) Token: 0x06003170 RID: 12656 RVA: 0x000CE572 File Offset: 0x000CC772
		public string SaveFolderName
		{
			get
			{
				return "Metadata";
			}
		}

		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x06003171 RID: 12657 RVA: 0x000CE572 File Offset: 0x000CC772
		public string SaveFileName
		{
			get
			{
				return "Metadata";
			}
		}

		// Token: 0x1700071B RID: 1819
		// (get) Token: 0x06003172 RID: 12658 RVA: 0x000CE579 File Offset: 0x000CC779
		public Loader Loader
		{
			get
			{
				return this.loader;
			}
		}

		// Token: 0x1700071C RID: 1820
		// (get) Token: 0x06003173 RID: 12659 RVA: 0x00014B5A File Offset: 0x00012D5A
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700071D RID: 1821
		// (get) Token: 0x06003174 RID: 12660 RVA: 0x000CE581 File Offset: 0x000CC781
		// (set) Token: 0x06003175 RID: 12661 RVA: 0x000CE589 File Offset: 0x000CC789
		public List<string> LocalExtraFiles { get; set; } = new List<string>();

		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x06003176 RID: 12662 RVA: 0x000CE592 File Offset: 0x000CC792
		// (set) Token: 0x06003177 RID: 12663 RVA: 0x000CE59A File Offset: 0x000CC79A
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x1700071F RID: 1823
		// (get) Token: 0x06003178 RID: 12664 RVA: 0x000CE5A3 File Offset: 0x000CC7A3
		// (set) Token: 0x06003179 RID: 12665 RVA: 0x000CE5AB File Offset: 0x000CC7AB
		public bool HasChanged { get; set; }

		// Token: 0x0600317A RID: 12666 RVA: 0x000CE5B4 File Offset: 0x000CC7B4
		protected override void Awake()
		{
			base.Awake();
			this.InitializeSaveable();
			if (this.CreationVersion == string.Empty)
			{
				this.CreationVersion = Application.version;
			}
		}

		// Token: 0x0600317B RID: 12667 RVA: 0x0003DA73 File Offset: 0x0003BC73
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x0600317C RID: 12668 RVA: 0x000CE5E0 File Offset: 0x000CC7E0
		public virtual string GetSaveString()
		{
			DateTime now = DateTime.Now;
			return new MetaData(new DateTimeData(this.CreationDate), new DateTimeData(now), this.CreationVersion, Application.version, false).GetJson(true);
		}

		// Token: 0x0600317D RID: 12669 RVA: 0x000CE61B File Offset: 0x000CC81B
		public void Load(MetaData data)
		{
			this.CreationDate = data.CreationDate.GetDateTime();
			this.CreationVersion = data.CreationVersion;
			this.HasChanged = true;
		}

		// Token: 0x040022D0 RID: 8912
		private MetadataLoader loader = new MetadataLoader();
	}
}
