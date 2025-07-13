using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using UnityEngine;

namespace ScheduleOne.Property
{
	// Token: 0x0200084B RID: 2123
	public class BusinessManager : Singleton<BusinessManager>, IBaseSaveable, ISaveable
	{
		// Token: 0x17000810 RID: 2064
		// (get) Token: 0x06003967 RID: 14695 RVA: 0x000F3E9F File Offset: 0x000F209F
		public string SaveFolderName
		{
			get
			{
				return "Businesses";
			}
		}

		// Token: 0x17000811 RID: 2065
		// (get) Token: 0x06003968 RID: 14696 RVA: 0x000F3E9F File Offset: 0x000F209F
		public string SaveFileName
		{
			get
			{
				return "Businesses";
			}
		}

		// Token: 0x17000812 RID: 2066
		// (get) Token: 0x06003969 RID: 14697 RVA: 0x000F3EA6 File Offset: 0x000F20A6
		public Loader Loader
		{
			get
			{
				return this.loader;
			}
		}

		// Token: 0x17000813 RID: 2067
		// (get) Token: 0x0600396A RID: 14698 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000814 RID: 2068
		// (get) Token: 0x0600396B RID: 14699 RVA: 0x000F3EAE File Offset: 0x000F20AE
		// (set) Token: 0x0600396C RID: 14700 RVA: 0x000F3EB6 File Offset: 0x000F20B6
		public List<string> LocalExtraFiles { get; set; } = new List<string>();

		// Token: 0x17000815 RID: 2069
		// (get) Token: 0x0600396D RID: 14701 RVA: 0x000F3EBF File Offset: 0x000F20BF
		// (set) Token: 0x0600396E RID: 14702 RVA: 0x000F3EC7 File Offset: 0x000F20C7
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x17000816 RID: 2070
		// (get) Token: 0x0600396F RID: 14703 RVA: 0x000F3ED0 File Offset: 0x000F20D0
		// (set) Token: 0x06003970 RID: 14704 RVA: 0x000F3ED8 File Offset: 0x000F20D8
		public bool HasChanged { get; set; } = true;

		// Token: 0x06003971 RID: 14705 RVA: 0x000F3EE1 File Offset: 0x000F20E1
		protected override void Awake()
		{
			base.Awake();
			this.InitializeSaveable();
		}

		// Token: 0x06003972 RID: 14706 RVA: 0x0003DA73 File Offset: 0x0003BC73
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x06003973 RID: 14707 RVA: 0x00092542 File Offset: 0x00090742
		public virtual string GetSaveString()
		{
			return string.Empty;
		}

		// Token: 0x06003974 RID: 14708 RVA: 0x000F3EF0 File Offset: 0x000F20F0
		public virtual List<string> WriteData(string parentFolderPath)
		{
			List<string> list = new List<string>();
			string containerFolder = ((ISaveable)this).GetContainerFolder(parentFolderPath);
			for (int i = 0; i < Business.UnownedBusinesses.Count; i++)
			{
				new SaveRequest(Business.UnownedBusinesses[i], containerFolder);
				list.Add(Business.UnownedBusinesses[i].SaveFolderName);
			}
			for (int j = 0; j < Business.OwnedBusinesses.Count; j++)
			{
				new SaveRequest(Business.OwnedBusinesses[j], containerFolder);
				list.Add(Business.OwnedBusinesses[j].SaveFolderName);
			}
			return list;
		}

		// Token: 0x06003975 RID: 14709 RVA: 0x000F3F88 File Offset: 0x000F2188
		public virtual void DeleteUnapprovedFiles(string parentFolderPath)
		{
			string[] directories = Directory.GetDirectories(((ISaveable)this).GetContainerFolder(parentFolderPath));
			for (int i = 0; i < directories.Length; i++)
			{
				new DirectoryInfo(directories[i]);
				Directory.Delete(directories[i], true);
			}
		}

		// Token: 0x06003976 RID: 14710 RVA: 0x000F3FC4 File Offset: 0x000F21C4
		public void LoadBusiness(BusinessData businessData)
		{
			Business business = Business.Businesses.FirstOrDefault((Business p) => p.PropertyCode == businessData.PropertyCode);
			if (business == null)
			{
				business = Business.Businesses.FirstOrDefault((Business p) => p.PropertyCode == businessData.PropertyCode);
			}
			if (business == null)
			{
				Debug.LogWarning("Business not found: " + businessData.PropertyCode);
				return;
			}
			business.Load(businessData);
		}

		// Token: 0x0400297C RID: 10620
		private BusinessesLoader loader = new BusinessesLoader();
	}
}
