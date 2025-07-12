using System;
using System.Collections.Generic;
using UnityEngine;

namespace LiquidVolumeFX
{
	// Token: 0x02000189 RID: 393
	[ExecuteInEditMode]
	[HelpURL("https://kronnect.com/support")]
	[AddComponentMenu("Effects/Liquid Volume")]
	[DisallowMultipleComponent]
	public class LiquidVolume : MonoBehaviour
	{
		// Token: 0x14000005 RID: 5
		// (add) Token: 0x0600075F RID: 1887 RVA: 0x00021988 File Offset: 0x0001FB88
		// (remove) Token: 0x06000760 RID: 1888 RVA: 0x000219C0 File Offset: 0x0001FBC0
		public event PropertiesChangedEvent onPropertiesChanged;

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000761 RID: 1889 RVA: 0x000219F5 File Offset: 0x0001FBF5
		// (set) Token: 0x06000762 RID: 1890 RVA: 0x000219FD File Offset: 0x0001FBFD
		public TOPOLOGY topology
		{
			get
			{
				return this._topology;
			}
			set
			{
				if (this._topology != value)
				{
					this._topology = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000763 RID: 1891 RVA: 0x00021A15 File Offset: 0x0001FC15
		// (set) Token: 0x06000764 RID: 1892 RVA: 0x00021A1D File Offset: 0x0001FC1D
		public DETAIL detail
		{
			get
			{
				return this._detail;
			}
			set
			{
				if (this._detail != value)
				{
					this._detail = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000765 RID: 1893 RVA: 0x00021A35 File Offset: 0x0001FC35
		// (set) Token: 0x06000766 RID: 1894 RVA: 0x00021A3D File Offset: 0x0001FC3D
		public float level
		{
			get
			{
				return this._level;
			}
			set
			{
				if (this._level != value)
				{
					this._level = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000767 RID: 1895 RVA: 0x00021A55 File Offset: 0x0001FC55
		// (set) Token: 0x06000768 RID: 1896 RVA: 0x00021A5D File Offset: 0x0001FC5D
		public float levelMultiplier
		{
			get
			{
				return this._levelMultiplier;
			}
			set
			{
				if (this._levelMultiplier != value)
				{
					this._levelMultiplier = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000769 RID: 1897 RVA: 0x00021A75 File Offset: 0x0001FC75
		// (set) Token: 0x0600076A RID: 1898 RVA: 0x00021A7D File Offset: 0x0001FC7D
		public bool useLightColor
		{
			get
			{
				return this._useLightColor;
			}
			set
			{
				if (this._useLightColor != value)
				{
					this._useLightColor = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x0600076B RID: 1899 RVA: 0x00021A95 File Offset: 0x0001FC95
		// (set) Token: 0x0600076C RID: 1900 RVA: 0x00021A9D File Offset: 0x0001FC9D
		public bool useLightDirection
		{
			get
			{
				return this._useLightDirection;
			}
			set
			{
				if (this._useLightDirection != value)
				{
					this._useLightDirection = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x0600076D RID: 1901 RVA: 0x00021AB5 File Offset: 0x0001FCB5
		// (set) Token: 0x0600076E RID: 1902 RVA: 0x00021ABD File Offset: 0x0001FCBD
		public Light directionalLight
		{
			get
			{
				return this._directionalLight;
			}
			set
			{
				if (this._directionalLight != value)
				{
					this._directionalLight = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x0600076F RID: 1903 RVA: 0x00021ADA File Offset: 0x0001FCDA
		// (set) Token: 0x06000770 RID: 1904 RVA: 0x00021AE2 File Offset: 0x0001FCE2
		public Color liquidColor1
		{
			get
			{
				return this._liquidColor1;
			}
			set
			{
				if (this._liquidColor1 != value)
				{
					this._liquidColor1 = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000771 RID: 1905 RVA: 0x00021AFF File Offset: 0x0001FCFF
		// (set) Token: 0x06000772 RID: 1906 RVA: 0x00021B07 File Offset: 0x0001FD07
		public float liquidScale1
		{
			get
			{
				return this._liquidScale1;
			}
			set
			{
				if (this._liquidScale1 != value)
				{
					this._liquidScale1 = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000773 RID: 1907 RVA: 0x00021B1F File Offset: 0x0001FD1F
		// (set) Token: 0x06000774 RID: 1908 RVA: 0x00021B27 File Offset: 0x0001FD27
		public Color liquidColor2
		{
			get
			{
				return this._liquidColor2;
			}
			set
			{
				if (this._liquidColor2 != value)
				{
					this._liquidColor2 = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000775 RID: 1909 RVA: 0x00021B44 File Offset: 0x0001FD44
		// (set) Token: 0x06000776 RID: 1910 RVA: 0x00021B4C File Offset: 0x0001FD4C
		public float liquidScale2
		{
			get
			{
				return this._liquidScale2;
			}
			set
			{
				if (this._liquidScale2 != value)
				{
					this._liquidScale2 = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000777 RID: 1911 RVA: 0x00021B64 File Offset: 0x0001FD64
		// (set) Token: 0x06000778 RID: 1912 RVA: 0x00021B6C File Offset: 0x0001FD6C
		public float alpha
		{
			get
			{
				return this._alpha;
			}
			set
			{
				if (this._alpha != Mathf.Clamp01(value))
				{
					this._alpha = Mathf.Clamp01(value);
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000779 RID: 1913 RVA: 0x00021B8E File Offset: 0x0001FD8E
		// (set) Token: 0x0600077A RID: 1914 RVA: 0x00021B96 File Offset: 0x0001FD96
		public Color emissionColor
		{
			get
			{
				return this._emissionColor;
			}
			set
			{
				if (this._emissionColor != value)
				{
					this._emissionColor = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x0600077B RID: 1915 RVA: 0x00021BB3 File Offset: 0x0001FDB3
		// (set) Token: 0x0600077C RID: 1916 RVA: 0x00021BBB File Offset: 0x0001FDBB
		public bool ditherShadows
		{
			get
			{
				return this._ditherShadows;
			}
			set
			{
				if (this._ditherShadows != value)
				{
					this._ditherShadows = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x0600077D RID: 1917 RVA: 0x00021BD3 File Offset: 0x0001FDD3
		// (set) Token: 0x0600077E RID: 1918 RVA: 0x00021BDB File Offset: 0x0001FDDB
		public float murkiness
		{
			get
			{
				return this._murkiness;
			}
			set
			{
				if (this._murkiness != value)
				{
					this._murkiness = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x0600077F RID: 1919 RVA: 0x00021BF3 File Offset: 0x0001FDF3
		// (set) Token: 0x06000780 RID: 1920 RVA: 0x00021BFB File Offset: 0x0001FDFB
		public float turbulence1
		{
			get
			{
				return this._turbulence1;
			}
			set
			{
				if (this._turbulence1 != value)
				{
					this._turbulence1 = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000781 RID: 1921 RVA: 0x00021C13 File Offset: 0x0001FE13
		// (set) Token: 0x06000782 RID: 1922 RVA: 0x00021C1B File Offset: 0x0001FE1B
		public float turbulence2
		{
			get
			{
				return this._turbulence2;
			}
			set
			{
				if (this._turbulence2 != value)
				{
					this._turbulence2 = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000783 RID: 1923 RVA: 0x00021C33 File Offset: 0x0001FE33
		// (set) Token: 0x06000784 RID: 1924 RVA: 0x00021C3B File Offset: 0x0001FE3B
		public float frecuency
		{
			get
			{
				return this._frecuency;
			}
			set
			{
				if (this._frecuency != value)
				{
					this._frecuency = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000785 RID: 1925 RVA: 0x00021C53 File Offset: 0x0001FE53
		// (set) Token: 0x06000786 RID: 1926 RVA: 0x00021C5B File Offset: 0x0001FE5B
		public float speed
		{
			get
			{
				return this._speed;
			}
			set
			{
				if (this._speed != value)
				{
					this._speed = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000787 RID: 1927 RVA: 0x00021C73 File Offset: 0x0001FE73
		// (set) Token: 0x06000788 RID: 1928 RVA: 0x00021C7B File Offset: 0x0001FE7B
		public float sparklingIntensity
		{
			get
			{
				return this._sparklingIntensity;
			}
			set
			{
				if (this._sparklingIntensity != value)
				{
					this._sparklingIntensity = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000789 RID: 1929 RVA: 0x00021C93 File Offset: 0x0001FE93
		// (set) Token: 0x0600078A RID: 1930 RVA: 0x00021C9B File Offset: 0x0001FE9B
		public float sparklingAmount
		{
			get
			{
				return this._sparklingAmount;
			}
			set
			{
				if (this._sparklingAmount != value)
				{
					this._sparklingAmount = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x0600078B RID: 1931 RVA: 0x00021CB3 File Offset: 0x0001FEB3
		// (set) Token: 0x0600078C RID: 1932 RVA: 0x00021CBB File Offset: 0x0001FEBB
		public float deepObscurance
		{
			get
			{
				return this._deepObscurance;
			}
			set
			{
				if (this._deepObscurance != value)
				{
					this._deepObscurance = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x0600078D RID: 1933 RVA: 0x00021CD3 File Offset: 0x0001FED3
		// (set) Token: 0x0600078E RID: 1934 RVA: 0x00021CDB File Offset: 0x0001FEDB
		public Color foamColor
		{
			get
			{
				return this._foamColor;
			}
			set
			{
				if (this._foamColor != value)
				{
					this._foamColor = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x0600078F RID: 1935 RVA: 0x00021CF8 File Offset: 0x0001FEF8
		// (set) Token: 0x06000790 RID: 1936 RVA: 0x00021D00 File Offset: 0x0001FF00
		public float foamScale
		{
			get
			{
				return this._foamScale;
			}
			set
			{
				if (this._foamScale != value)
				{
					this._foamScale = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000791 RID: 1937 RVA: 0x00021D18 File Offset: 0x0001FF18
		// (set) Token: 0x06000792 RID: 1938 RVA: 0x00021D20 File Offset: 0x0001FF20
		public float foamThickness
		{
			get
			{
				return this._foamThickness;
			}
			set
			{
				if (this._foamThickness != value)
				{
					this._foamThickness = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000793 RID: 1939 RVA: 0x00021D38 File Offset: 0x0001FF38
		// (set) Token: 0x06000794 RID: 1940 RVA: 0x00021D40 File Offset: 0x0001FF40
		public float foamDensity
		{
			get
			{
				return this._foamDensity;
			}
			set
			{
				if (this._foamDensity != value)
				{
					this._foamDensity = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000795 RID: 1941 RVA: 0x00021D58 File Offset: 0x0001FF58
		// (set) Token: 0x06000796 RID: 1942 RVA: 0x00021D60 File Offset: 0x0001FF60
		public float foamWeight
		{
			get
			{
				return this._foamWeight;
			}
			set
			{
				if (this._foamWeight != value)
				{
					this._foamWeight = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000797 RID: 1943 RVA: 0x00021D78 File Offset: 0x0001FF78
		// (set) Token: 0x06000798 RID: 1944 RVA: 0x00021D80 File Offset: 0x0001FF80
		public float foamTurbulence
		{
			get
			{
				return this._foamTurbulence;
			}
			set
			{
				if (this._foamTurbulence != value)
				{
					this._foamTurbulence = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000799 RID: 1945 RVA: 0x00021D98 File Offset: 0x0001FF98
		// (set) Token: 0x0600079A RID: 1946 RVA: 0x00021DA0 File Offset: 0x0001FFA0
		public bool foamVisibleFromBottom
		{
			get
			{
				return this._foamVisibleFromBottom;
			}
			set
			{
				if (this._foamVisibleFromBottom != value)
				{
					this._foamVisibleFromBottom = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x0600079B RID: 1947 RVA: 0x00021DB8 File Offset: 0x0001FFB8
		// (set) Token: 0x0600079C RID: 1948 RVA: 0x00021DC0 File Offset: 0x0001FFC0
		public bool smokeEnabled
		{
			get
			{
				return this._smokeEnabled;
			}
			set
			{
				if (this._smokeEnabled != value)
				{
					this._smokeEnabled = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x0600079D RID: 1949 RVA: 0x00021DD8 File Offset: 0x0001FFD8
		// (set) Token: 0x0600079E RID: 1950 RVA: 0x00021DE0 File Offset: 0x0001FFE0
		public Color smokeColor
		{
			get
			{
				return this._smokeColor;
			}
			set
			{
				if (this._smokeColor != value)
				{
					this._smokeColor = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x0600079F RID: 1951 RVA: 0x00021DFD File Offset: 0x0001FFFD
		// (set) Token: 0x060007A0 RID: 1952 RVA: 0x00021E05 File Offset: 0x00020005
		public float smokeScale
		{
			get
			{
				return this._smokeScale;
			}
			set
			{
				if (this._smokeScale != value)
				{
					this._smokeScale = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x060007A1 RID: 1953 RVA: 0x00021E1D File Offset: 0x0002001D
		// (set) Token: 0x060007A2 RID: 1954 RVA: 0x00021E25 File Offset: 0x00020025
		public float smokeBaseObscurance
		{
			get
			{
				return this._smokeBaseObscurance;
			}
			set
			{
				if (this._smokeBaseObscurance != value)
				{
					this._smokeBaseObscurance = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x060007A3 RID: 1955 RVA: 0x00021E3D File Offset: 0x0002003D
		// (set) Token: 0x060007A4 RID: 1956 RVA: 0x00021E45 File Offset: 0x00020045
		public float smokeHeightAtten
		{
			get
			{
				return this._smokeHeightAtten;
			}
			set
			{
				if (this._smokeHeightAtten != value)
				{
					this._smokeHeightAtten = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x060007A5 RID: 1957 RVA: 0x00021E5D File Offset: 0x0002005D
		// (set) Token: 0x060007A6 RID: 1958 RVA: 0x00021E65 File Offset: 0x00020065
		public float smokeSpeed
		{
			get
			{
				return this._smokeSpeed;
			}
			set
			{
				if (this._smokeSpeed != value)
				{
					this._smokeSpeed = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x060007A7 RID: 1959 RVA: 0x00021E7D File Offset: 0x0002007D
		// (set) Token: 0x060007A8 RID: 1960 RVA: 0x00021E85 File Offset: 0x00020085
		public bool fixMesh
		{
			get
			{
				return this._fixMesh;
			}
			set
			{
				if (this._fixMesh != value)
				{
					this._fixMesh = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x060007A9 RID: 1961 RVA: 0x00021E9D File Offset: 0x0002009D
		// (set) Token: 0x060007AA RID: 1962 RVA: 0x00021EA5 File Offset: 0x000200A5
		public Vector3 pivotOffset
		{
			get
			{
				return this._pivotOffset;
			}
			set
			{
				if (this._pivotOffset != value)
				{
					this._pivotOffset = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x060007AB RID: 1963 RVA: 0x00021EC2 File Offset: 0x000200C2
		// (set) Token: 0x060007AC RID: 1964 RVA: 0x00021ECA File Offset: 0x000200CA
		public bool limitVerticalRange
		{
			get
			{
				return this._limitVerticalRange;
			}
			set
			{
				if (this._limitVerticalRange != value)
				{
					this._limitVerticalRange = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x060007AD RID: 1965 RVA: 0x00021EE2 File Offset: 0x000200E2
		// (set) Token: 0x060007AE RID: 1966 RVA: 0x00021EEA File Offset: 0x000200EA
		public float upperLimit
		{
			get
			{
				return this._upperLimit;
			}
			set
			{
				if (this._upperLimit != value)
				{
					this._upperLimit = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x060007AF RID: 1967 RVA: 0x00021F02 File Offset: 0x00020102
		// (set) Token: 0x060007B0 RID: 1968 RVA: 0x00021F0A File Offset: 0x0002010A
		public float lowerLimit
		{
			get
			{
				return this._lowerLimit;
			}
			set
			{
				if (this._lowerLimit != value)
				{
					this._lowerLimit = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x060007B1 RID: 1969 RVA: 0x00021F22 File Offset: 0x00020122
		// (set) Token: 0x060007B2 RID: 1970 RVA: 0x00021F2A File Offset: 0x0002012A
		public int subMeshIndex
		{
			get
			{
				return this._subMeshIndex;
			}
			set
			{
				if (this._subMeshIndex != value)
				{
					this._subMeshIndex = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x060007B3 RID: 1971 RVA: 0x00021F42 File Offset: 0x00020142
		// (set) Token: 0x060007B4 RID: 1972 RVA: 0x00021F4A File Offset: 0x0002014A
		public Material flaskMaterial
		{
			get
			{
				return this._flaskMaterial;
			}
			set
			{
				if (this._flaskMaterial != value)
				{
					this._flaskMaterial = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x060007B5 RID: 1973 RVA: 0x00021F67 File Offset: 0x00020167
		// (set) Token: 0x060007B6 RID: 1974 RVA: 0x00021F6F File Offset: 0x0002016F
		public float flaskThickness
		{
			get
			{
				return this._flaskThickness;
			}
			set
			{
				if (this._flaskThickness != value)
				{
					this._flaskThickness = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x060007B7 RID: 1975 RVA: 0x00021F87 File Offset: 0x00020187
		// (set) Token: 0x060007B8 RID: 1976 RVA: 0x00021F8F File Offset: 0x0002018F
		public float glossinessInternal
		{
			get
			{
				return this._glossinessInternal;
			}
			set
			{
				if (this._glossinessInternal != value)
				{
					this._glossinessInternal = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x060007B9 RID: 1977 RVA: 0x00021FA7 File Offset: 0x000201A7
		// (set) Token: 0x060007BA RID: 1978 RVA: 0x00021FAF File Offset: 0x000201AF
		public bool scatteringEnabled
		{
			get
			{
				return this._scatteringEnabled;
			}
			set
			{
				if (this._scatteringEnabled != value)
				{
					this._scatteringEnabled = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x060007BB RID: 1979 RVA: 0x00021FC7 File Offset: 0x000201C7
		// (set) Token: 0x060007BC RID: 1980 RVA: 0x00021FCF File Offset: 0x000201CF
		public int scatteringPower
		{
			get
			{
				return this._scatteringPower;
			}
			set
			{
				if (this._scatteringPower != value)
				{
					this._scatteringPower = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x060007BD RID: 1981 RVA: 0x00021FE7 File Offset: 0x000201E7
		// (set) Token: 0x060007BE RID: 1982 RVA: 0x00021FEF File Offset: 0x000201EF
		public float scatteringAmount
		{
			get
			{
				return this._scatteringAmount;
			}
			set
			{
				if (this._scatteringAmount != value)
				{
					this._scatteringAmount = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x060007BF RID: 1983 RVA: 0x00022007 File Offset: 0x00020207
		// (set) Token: 0x060007C0 RID: 1984 RVA: 0x0002200F File Offset: 0x0002020F
		public bool refractionBlur
		{
			get
			{
				return this._refractionBlur;
			}
			set
			{
				if (this._refractionBlur != value)
				{
					this._refractionBlur = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x060007C1 RID: 1985 RVA: 0x00022027 File Offset: 0x00020227
		// (set) Token: 0x060007C2 RID: 1986 RVA: 0x0002202F File Offset: 0x0002022F
		public float blurIntensity
		{
			get
			{
				return this._blurIntensity;
			}
			set
			{
				if (this._blurIntensity != Mathf.Clamp01(value))
				{
					this._blurIntensity = Mathf.Clamp01(value);
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x060007C3 RID: 1987 RVA: 0x00022051 File Offset: 0x00020251
		// (set) Token: 0x060007C4 RID: 1988 RVA: 0x00022059 File Offset: 0x00020259
		public int liquidRaySteps
		{
			get
			{
				return this._liquidRaySteps;
			}
			set
			{
				if (this._liquidRaySteps != value)
				{
					this._liquidRaySteps = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x060007C5 RID: 1989 RVA: 0x00022071 File Offset: 0x00020271
		// (set) Token: 0x060007C6 RID: 1990 RVA: 0x00022079 File Offset: 0x00020279
		public int foamRaySteps
		{
			get
			{
				return this._foamRaySteps;
			}
			set
			{
				if (this._foamRaySteps != value)
				{
					this._foamRaySteps = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x060007C7 RID: 1991 RVA: 0x00022091 File Offset: 0x00020291
		// (set) Token: 0x060007C8 RID: 1992 RVA: 0x00022099 File Offset: 0x00020299
		public int smokeRaySteps
		{
			get
			{
				return this._smokeRaySteps;
			}
			set
			{
				if (this._smokeRaySteps != value)
				{
					this._smokeRaySteps = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x060007C9 RID: 1993 RVA: 0x000220B1 File Offset: 0x000202B1
		// (set) Token: 0x060007CA RID: 1994 RVA: 0x000220B9 File Offset: 0x000202B9
		public Texture2D bumpMap
		{
			get
			{
				return this._bumpMap;
			}
			set
			{
				if (this._bumpMap != value)
				{
					this._bumpMap = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x060007CB RID: 1995 RVA: 0x000220D6 File Offset: 0x000202D6
		// (set) Token: 0x060007CC RID: 1996 RVA: 0x000220DE File Offset: 0x000202DE
		public float bumpStrength
		{
			get
			{
				return this._bumpStrength;
			}
			set
			{
				if (this._bumpStrength != value)
				{
					this._bumpStrength = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x060007CD RID: 1997 RVA: 0x000220F6 File Offset: 0x000202F6
		// (set) Token: 0x060007CE RID: 1998 RVA: 0x000220FE File Offset: 0x000202FE
		public float bumpDistortionScale
		{
			get
			{
				return this._bumpDistortionScale;
			}
			set
			{
				if (this._bumpDistortionScale != value)
				{
					this._bumpDistortionScale = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x060007CF RID: 1999 RVA: 0x00022116 File Offset: 0x00020316
		// (set) Token: 0x060007D0 RID: 2000 RVA: 0x0002211E File Offset: 0x0002031E
		public Vector2 bumpDistortionOffset
		{
			get
			{
				return this._bumpDistortionOffset;
			}
			set
			{
				if (this._bumpDistortionOffset != value)
				{
					this._bumpDistortionOffset = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x060007D1 RID: 2001 RVA: 0x0002213B File Offset: 0x0002033B
		// (set) Token: 0x060007D2 RID: 2002 RVA: 0x00022143 File Offset: 0x00020343
		public Texture2D distortionMap
		{
			get
			{
				return this._distortionMap;
			}
			set
			{
				if (this._distortionMap != value)
				{
					this._distortionMap = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x060007D3 RID: 2003 RVA: 0x00022160 File Offset: 0x00020360
		// (set) Token: 0x060007D4 RID: 2004 RVA: 0x00022168 File Offset: 0x00020368
		public Texture2D texture
		{
			get
			{
				return this._texture;
			}
			set
			{
				if (this._texture != value)
				{
					this._texture = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x060007D5 RID: 2005 RVA: 0x00022185 File Offset: 0x00020385
		// (set) Token: 0x060007D6 RID: 2006 RVA: 0x0002218D File Offset: 0x0002038D
		public Vector2 textureScale
		{
			get
			{
				return this._textureScale;
			}
			set
			{
				if (this._textureScale != value)
				{
					this._textureScale = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x060007D7 RID: 2007 RVA: 0x000221AA File Offset: 0x000203AA
		// (set) Token: 0x060007D8 RID: 2008 RVA: 0x000221B2 File Offset: 0x000203B2
		public Vector2 textureOffset
		{
			get
			{
				return this._textureOffset;
			}
			set
			{
				if (this._textureOffset != value)
				{
					this._textureOffset = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x060007D9 RID: 2009 RVA: 0x000221CF File Offset: 0x000203CF
		// (set) Token: 0x060007DA RID: 2010 RVA: 0x000221D7 File Offset: 0x000203D7
		public float distortionAmount
		{
			get
			{
				return this._distortionAmount;
			}
			set
			{
				if (this._distortionAmount != value)
				{
					this._distortionAmount = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x060007DB RID: 2011 RVA: 0x000221EF File Offset: 0x000203EF
		// (set) Token: 0x060007DC RID: 2012 RVA: 0x000221F7 File Offset: 0x000203F7
		public bool depthAware
		{
			get
			{
				return this._depthAware;
			}
			set
			{
				if (this._depthAware != value)
				{
					this._depthAware = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x060007DD RID: 2013 RVA: 0x0002220F File Offset: 0x0002040F
		// (set) Token: 0x060007DE RID: 2014 RVA: 0x00022217 File Offset: 0x00020417
		public float depthAwareOffset
		{
			get
			{
				return this._depthAwareOffset;
			}
			set
			{
				if (this._depthAwareOffset != value)
				{
					this._depthAwareOffset = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x060007DF RID: 2015 RVA: 0x0002222F File Offset: 0x0002042F
		// (set) Token: 0x060007E0 RID: 2016 RVA: 0x00022237 File Offset: 0x00020437
		public bool irregularDepthDebug
		{
			get
			{
				return this._irregularDepthDebug;
			}
			set
			{
				if (this._irregularDepthDebug != value)
				{
					this._irregularDepthDebug = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x060007E1 RID: 2017 RVA: 0x0002224F File Offset: 0x0002044F
		// (set) Token: 0x060007E2 RID: 2018 RVA: 0x00022257 File Offset: 0x00020457
		public bool depthAwareCustomPass
		{
			get
			{
				return this._depthAwareCustomPass;
			}
			set
			{
				if (this._depthAwareCustomPass != value)
				{
					this._depthAwareCustomPass = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x060007E3 RID: 2019 RVA: 0x0002226F File Offset: 0x0002046F
		// (set) Token: 0x060007E4 RID: 2020 RVA: 0x00022277 File Offset: 0x00020477
		public bool depthAwareCustomPassDebug
		{
			get
			{
				return this._depthAwareCustomPassDebug;
			}
			set
			{
				if (this._depthAwareCustomPassDebug != value)
				{
					this._depthAwareCustomPassDebug = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x060007E5 RID: 2021 RVA: 0x0002228F File Offset: 0x0002048F
		// (set) Token: 0x060007E6 RID: 2022 RVA: 0x00022297 File Offset: 0x00020497
		public float doubleSidedBias
		{
			get
			{
				return this._doubleSidedBias;
			}
			set
			{
				if (this._doubleSidedBias != value)
				{
					this._doubleSidedBias = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x060007E7 RID: 2023 RVA: 0x000222AF File Offset: 0x000204AF
		// (set) Token: 0x060007E8 RID: 2024 RVA: 0x000222B7 File Offset: 0x000204B7
		public float backDepthBias
		{
			get
			{
				return this._backDepthBias;
			}
			set
			{
				if (value < 0f)
				{
					value = 0f;
				}
				if (this._backDepthBias != value)
				{
					this._backDepthBias = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x060007E9 RID: 2025 RVA: 0x000222DE File Offset: 0x000204DE
		// (set) Token: 0x060007EA RID: 2026 RVA: 0x000222E6 File Offset: 0x000204E6
		public LEVEL_COMPENSATION rotationLevelCompensation
		{
			get
			{
				return this._rotationLevelCompensation;
			}
			set
			{
				if (this._rotationLevelCompensation != value)
				{
					this._rotationLevelCompensation = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x060007EB RID: 2027 RVA: 0x000222FE File Offset: 0x000204FE
		// (set) Token: 0x060007EC RID: 2028 RVA: 0x00022306 File Offset: 0x00020506
		public bool ignoreGravity
		{
			get
			{
				return this._ignoreGravity;
			}
			set
			{
				if (this._ignoreGravity != value)
				{
					this._ignoreGravity = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x060007ED RID: 2029 RVA: 0x0002231E File Offset: 0x0002051E
		// (set) Token: 0x060007EE RID: 2030 RVA: 0x00022326 File Offset: 0x00020526
		public bool reactToForces
		{
			get
			{
				return this._reactToForces;
			}
			set
			{
				if (this._reactToForces != value)
				{
					this._reactToForces = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x060007EF RID: 2031 RVA: 0x0002233E File Offset: 0x0002053E
		// (set) Token: 0x060007F0 RID: 2032 RVA: 0x00022346 File Offset: 0x00020546
		public Vector3 extentsScale
		{
			get
			{
				return this._extentsScale;
			}
			set
			{
				if (this._extentsScale != value)
				{
					this._extentsScale = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x060007F1 RID: 2033 RVA: 0x00022363 File Offset: 0x00020563
		// (set) Token: 0x060007F2 RID: 2034 RVA: 0x0002236B File Offset: 0x0002056B
		public int noiseVariation
		{
			get
			{
				return this._noiseVariation;
			}
			set
			{
				if (this._noiseVariation != value)
				{
					this._noiseVariation = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x060007F3 RID: 2035 RVA: 0x00022383 File Offset: 0x00020583
		// (set) Token: 0x060007F4 RID: 2036 RVA: 0x0002238B File Offset: 0x0002058B
		public bool allowViewFromInside
		{
			get
			{
				return this._allowViewFromInside;
			}
			set
			{
				if (this._allowViewFromInside != value)
				{
					this._allowViewFromInside = value;
					this.lastDistanceToCam = -1f;
					this.CheckInsideOut();
				}
			}
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x060007F5 RID: 2037 RVA: 0x000223AE File Offset: 0x000205AE
		// (set) Token: 0x060007F6 RID: 2038 RVA: 0x000223B6 File Offset: 0x000205B6
		public bool debugSpillPoint
		{
			get
			{
				return this._debugSpillPoint;
			}
			set
			{
				if (this._debugSpillPoint != value)
				{
					this._debugSpillPoint = value;
				}
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x060007F7 RID: 2039 RVA: 0x000223C8 File Offset: 0x000205C8
		// (set) Token: 0x060007F8 RID: 2040 RVA: 0x000223D0 File Offset: 0x000205D0
		public int renderQueue
		{
			get
			{
				return this._renderQueue;
			}
			set
			{
				if (this._renderQueue != value)
				{
					this._renderQueue = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x060007F9 RID: 2041 RVA: 0x000223E8 File Offset: 0x000205E8
		// (set) Token: 0x060007FA RID: 2042 RVA: 0x000223F0 File Offset: 0x000205F0
		public Cubemap reflectionTexture
		{
			get
			{
				return this._reflectionTexture;
			}
			set
			{
				if (this._reflectionTexture != value)
				{
					this._reflectionTexture = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x060007FB RID: 2043 RVA: 0x0002240D File Offset: 0x0002060D
		// (set) Token: 0x060007FC RID: 2044 RVA: 0x00022415 File Offset: 0x00020615
		public float physicsMass
		{
			get
			{
				return this._physicsMass;
			}
			set
			{
				if (this._physicsMass != value)
				{
					this._physicsMass = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x060007FD RID: 2045 RVA: 0x0002242D File Offset: 0x0002062D
		// (set) Token: 0x060007FE RID: 2046 RVA: 0x00022435 File Offset: 0x00020635
		public float physicsAngularDamp
		{
			get
			{
				return this._physicsAngularDamp;
			}
			set
			{
				if (this._physicsAngularDamp != value)
				{
					this._physicsAngularDamp = value;
					this.UpdateMaterialProperties();
				}
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x060007FF RID: 2047 RVA: 0x000022C9 File Offset: 0x000004C9
		public static bool useFPRenderTextures
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x00022450 File Offset: 0x00020650
		private void OnEnable()
		{
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			this.levelMultipled = this._level * this._levelMultiplier;
			this.turb.z = 1f;
			this.turbulenceDueForces = 0f;
			this.turbulenceSpeed = 1f;
			this.liquidRot = base.transform.rotation;
			this.currentDetail = this._detail;
			this.currentNoiseVariation = -1;
			this.lastPosition = base.transform.position;
			this.lastRotation = base.transform.rotation;
			this.lastScale = base.transform.localScale;
			this.prevThickness = this._flaskThickness;
			if (this._depthAwareCustomPass && base.transform.parent == null)
			{
				this._depthAwareCustomPass = false;
			}
			this.UpdateMaterialPropertiesNow();
			if (!Application.isPlaying)
			{
				this.shouldUpdateMaterialProperties = true;
			}
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x00022540 File Offset: 0x00020740
		private void Reset()
		{
			if (this.mesh == null)
			{
				return;
			}
			if (this.mesh.vertexCount == 24)
			{
				this.topology = TOPOLOGY.Cube;
				return;
			}
			Renderer component = base.GetComponent<Renderer>();
			if (component == null)
			{
				if (this.mesh.bounds.extents.y > this.mesh.bounds.extents.x)
				{
					this.topology = TOPOLOGY.Cylinder;
					return;
				}
			}
			else if (component.bounds.extents.y > component.bounds.extents.x)
			{
				this.topology = TOPOLOGY.Cylinder;
				if (!Application.isPlaying && base.transform.rotation.eulerAngles != Vector3.zero && (this.mesh.bounds.extents.y <= this.mesh.bounds.extents.x || this.mesh.bounds.extents.y <= this.mesh.bounds.extents.z))
				{
					Debug.LogWarning("Intrinsic model rotation detected. Consider using the Bake Transform and/or Center Pivot options in Advanced section.");
				}
			}
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x0002268C File Offset: 0x0002088C
		private void OnDestroy()
		{
			this.RestoreOriginalMesh();
			this.liqMat = null;
			if (this.liqMatSimple != null)
			{
				UnityEngine.Object.DestroyImmediate(this.liqMatSimple);
				this.liqMatSimple = null;
			}
			if (this.liqMatDefaultNoFlask != null)
			{
				UnityEngine.Object.DestroyImmediate(this.liqMatDefaultNoFlask);
				this.liqMatDefaultNoFlask = null;
			}
			if (this.noise3DTex != null)
			{
				for (int i = 0; i < this.noise3DTex.Length; i++)
				{
					Texture3D texture3D = this.noise3DTex[i];
					if (texture3D != null && texture3D.name.Contains("Clone"))
					{
						UnityEngine.Object.DestroyImmediate(texture3D);
						this.noise3DTex[i] = null;
					}
				}
			}
			LiquidVolumeDepthPrePassRenderFeature.RemoveLiquidFromBackRenderers(this);
			LiquidVolumeDepthPrePassRenderFeature.RemoveLiquidFromFrontRenderers(this);
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x00022740 File Offset: 0x00020940
		private void RenderObject()
		{
			bool flag = base.gameObject.activeInHierarchy && base.enabled;
			if (this.shouldUpdateMaterialProperties || !Application.isPlaying)
			{
				this.shouldUpdateMaterialProperties = false;
				this.UpdateMaterialPropertiesNow();
			}
			if (flag && this._allowViewFromInside)
			{
				this.CheckInsideOut();
			}
			this.UpdateAnimations();
			if (!flag || this._topology != TOPOLOGY.Irregular)
			{
				LiquidVolumeDepthPrePassRenderFeature.RemoveLiquidFromBackRenderers(this);
			}
			else if (this._topology == TOPOLOGY.Irregular)
			{
				LiquidVolumeDepthPrePassRenderFeature.AddLiquidToBackRenderers(this);
			}
			if (base.transform.parent != null)
			{
				base.GetComponentInParent<Renderer>();
				if (!flag || !this._depthAwareCustomPass)
				{
					LiquidVolumeDepthPrePassRenderFeature.RemoveLiquidFromFrontRenderers(this);
				}
				else if (this._depthAwareCustomPass)
				{
					LiquidVolumeDepthPrePassRenderFeature.AddLiquidToFrontRenderers(this);
				}
			}
			if (this._debugSpillPoint)
			{
				this.UpdateSpillPointGizmo();
			}
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x00022805 File Offset: 0x00020A05
		public void OnWillRenderObject()
		{
			this.RenderObject();
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x00022810 File Offset: 0x00020A10
		private void FixedUpdate()
		{
			this.turbulenceSpeed += Time.deltaTime * 3f * this._speed;
			this.liqMat.SetFloat(LiquidVolume.ShaderParams.TurbulenceSpeed, this.turbulenceSpeed * 4f);
			this.murkinessSpeed += Time.deltaTime * 0.05f * (this.shaderTurb.x + this.shaderTurb.y);
			this.liqMat.SetFloat(LiquidVolume.ShaderParams.MurkinessSpeed, this.murkinessSpeed);
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x0002289E File Offset: 0x00020A9E
		private void OnDidApplyAnimationProperties()
		{
			this.shouldUpdateMaterialProperties = true;
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x000228A7 File Offset: 0x00020AA7
		public void ClearMeshCache()
		{
			LiquidVolume.meshCache.Clear();
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x000228B4 File Offset: 0x00020AB4
		private void ReadVertices()
		{
			if (this.mesh == null)
			{
				return;
			}
			LiquidVolume.MeshCache meshCache;
			if (!LiquidVolume.meshCache.TryGetValue(this.mesh, out meshCache))
			{
				if (!this.mesh.isReadable)
				{
					Debug.LogError("Mesh " + this.mesh.name + " is not readable. Please select your mesh and enable the Read/Write Enabled option.");
				}
				this.verticesUnsorted = this.mesh.vertices;
				this.verticesIndices = this.mesh.triangles;
				int num = this.verticesUnsorted.Length;
				if (this.verticesSorted == null || this.verticesSorted.Length != num)
				{
					this.verticesSorted = new Vector3[num];
				}
				Array.Copy(this.verticesUnsorted, this.verticesSorted, num);
				Array.Sort<Vector3>(this.verticesSorted, new Comparison<Vector3>(this.vertexComparer));
				meshCache.verticesUnsorted = this.verticesUnsorted;
				meshCache.indices = this.verticesIndices;
				meshCache.verticesSorted = this.verticesSorted;
				if (LiquidVolume.meshCache.Count > 64)
				{
					this.ClearMeshCache();
				}
				LiquidVolume.meshCache[this.mesh] = meshCache;
				return;
			}
			this.verticesUnsorted = meshCache.verticesUnsorted;
			this.verticesIndices = meshCache.indices;
			this.verticesSorted = meshCache.verticesSorted;
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x000229F7 File Offset: 0x00020BF7
		private int vertexComparer(Vector3 v0, Vector3 v1)
		{
			if (v1.y < v0.y)
			{
				return -1;
			}
			if (v1.y > v0.y)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x00022A1C File Offset: 0x00020C1C
		private void UpdateAnimations()
		{
			TOPOLOGY topology = this.topology;
			if (topology != TOPOLOGY.Sphere)
			{
				if (topology == TOPOLOGY.Cylinder)
				{
					if (base.transform.localScale.z != base.transform.localScale.x)
					{
						base.transform.localScale = new Vector3(base.transform.localScale.x, base.transform.localScale.y, base.transform.localScale.x);
					}
				}
			}
			else if (base.transform.localScale.y != base.transform.localScale.x || base.transform.localScale.z != base.transform.localScale.x)
			{
				base.transform.localScale = new Vector3(base.transform.localScale.x, base.transform.localScale.x, base.transform.localScale.x);
			}
			if (this.liqMat != null)
			{
				Vector3 vector = Vector3.right;
				Quaternion rotation = base.transform.rotation;
				if (this._reactToForces)
				{
					Quaternion b = base.transform.rotation;
					float deltaTime = Time.deltaTime;
					if (Application.isPlaying && deltaTime > 0f)
					{
						Vector3 vector2 = (base.transform.position - this.lastPosition) / deltaTime;
						Vector3 vector3 = vector2 - this.lastAvgVelocity;
						this.lastAvgVelocity = vector2;
						this.inertia += vector2;
						float num = Mathf.Max(vector3.magnitude / this._physicsMass - this._physicsAngularDamp * 150f * deltaTime, 0f);
						this.angularInertia += num;
						this.angularVelocity += this.angularInertia;
						if (this.angularVelocity > 0f)
						{
							this.angularInertia -= Mathf.Abs(this.angularVelocity) * deltaTime * this._physicsMass;
						}
						else if (this.angularVelocity < 0f)
						{
							this.angularInertia += Mathf.Abs(this.angularVelocity) * deltaTime * this._physicsMass;
						}
						float num2 = 1f - this._physicsAngularDamp;
						this.angularInertia *= num2;
						this.inertia *= num2;
						float angle = Mathf.Clamp(this.angularVelocity, -90f, 90f);
						float magnitude = this.inertia.magnitude;
						if (magnitude > 0f)
						{
							vector = this.inertia / magnitude;
						}
						Vector3 axis = Vector3.Cross(vector, Vector3.down);
						b = Quaternion.AngleAxis(angle, axis);
						float num3 = Mathf.Abs(this.angularInertia) + Mathf.Abs(this.angularVelocity);
						this.turbulenceDueForces = Mathf.Min(0.5f / this._physicsMass, this.turbulenceDueForces + num3 / 1000f);
						this.turbulenceDueForces *= num2;
					}
					else
					{
						this.turbulenceDueForces = 0f;
					}
					if (this._topology == TOPOLOGY.Sphere)
					{
						this.liquidRot = Quaternion.Lerp(this.liquidRot, b, 0.1f);
						rotation = this.liquidRot;
					}
				}
				else if (this.turbulenceDueForces > 0f)
				{
					this.turbulenceDueForces *= 0.1f;
				}
				Matrix4x4 matrix4x = Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);
				this.liqMat.SetMatrix(LiquidVolume.ShaderParams.RotationMatrix, matrix4x.inverse);
				if (this._topology != TOPOLOGY.Sphere)
				{
					float x = vector.x;
					vector.x += (vector.z - vector.x) * 0.25f;
					vector.z += (x - vector.z) * 0.25f;
				}
				this.turb.z = vector.x;
				this.turb.w = vector.z;
			}
			bool flag = base.transform.rotation != this.lastRotation;
			if (this._reactToForces || flag || base.transform.position != this.lastPosition || base.transform.localScale != this.lastScale)
			{
				this.UpdateLevels(flag);
			}
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x00022E84 File Offset: 0x00021084
		public void UpdateMaterialProperties()
		{
			if (Application.isPlaying)
			{
				this.shouldUpdateMaterialProperties = true;
				return;
			}
			this.UpdateMaterialPropertiesNow();
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x00022E9C File Offset: 0x0002109C
		private void UpdateMaterialPropertiesNow()
		{
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			DETAIL detail = this._detail;
			if (detail <= DETAIL.SimpleNoFlask)
			{
				if (this.liqMatSimple == null)
				{
					this.liqMatSimple = UnityEngine.Object.Instantiate<Material>(Resources.Load<Material>("Materials/LiquidVolumeSimple"));
				}
				this.liqMat = this.liqMatSimple;
			}
			else
			{
				if (this.liqMatDefaultNoFlask == null)
				{
					this.liqMatDefaultNoFlask = UnityEngine.Object.Instantiate<Material>(Resources.Load<Material>("Materials/LiquidVolumeDefaultNoFlask"));
				}
				this.liqMat = this.liqMatDefaultNoFlask;
			}
			if (this._flaskMaterial == null)
			{
				this._flaskMaterial = UnityEngine.Object.Instantiate<Material>(Resources.Load<Material>("Materials/Flask"));
			}
			if (this.liqMat == null)
			{
				return;
			}
			this.CheckMeshDisplacement();
			if (this.currentDetail != this._detail)
			{
				this.currentDetail = this._detail;
			}
			this.UpdateLevels(true);
			if (this.mr == null)
			{
				return;
			}
			this.mr.GetSharedMaterials(LiquidVolume.mrSharedMaterials);
			int count = LiquidVolume.mrSharedMaterials.Count;
			if (this._subMeshIndex < 0)
			{
				int num = 0;
				while (num < LiquidVolume.defaultContainerNames.Length && this._subMeshIndex < 0)
				{
					for (int i = 0; i < count; i++)
					{
						if (LiquidVolume.mrSharedMaterials[i] != null && LiquidVolume.mrSharedMaterials[i] != this._flaskMaterial && LiquidVolume.mrSharedMaterials[i].name.ToUpper().Contains(LiquidVolume.defaultContainerNames[num]))
						{
							this._subMeshIndex = i;
							break;
						}
					}
					num++;
				}
			}
			if (this._subMeshIndex < 0)
			{
				this._subMeshIndex = 0;
			}
			if (count > 1 && this._subMeshIndex >= 0 && this._subMeshIndex < count)
			{
				LiquidVolume.mrSharedMaterials[this._subMeshIndex] = this.liqMat;
			}
			else
			{
				LiquidVolume.mrSharedMaterials.Clear();
				LiquidVolume.mrSharedMaterials.Add(this.liqMat);
			}
			if (this._flaskMaterial != null)
			{
				bool flag = this._detail.usesFlask();
				if (flag && !LiquidVolume.mrSharedMaterials.Contains(this._flaskMaterial))
				{
					for (int j = 0; j < LiquidVolume.mrSharedMaterials.Count; j++)
					{
						if (LiquidVolume.mrSharedMaterials[j] == null)
						{
							LiquidVolume.mrSharedMaterials[j] = this._flaskMaterial;
							flag = false;
						}
					}
					if (flag)
					{
						LiquidVolume.mrSharedMaterials.Add(this._flaskMaterial);
					}
				}
				else if (!flag && LiquidVolume.mrSharedMaterials.Contains(this._flaskMaterial))
				{
					LiquidVolume.mrSharedMaterials.Remove(this._flaskMaterial);
				}
				this._flaskMaterial.SetFloat(LiquidVolume.ShaderParams.QueueOffset, (float)(this._renderQueue - 3000));
				this._flaskMaterial.SetFloat(LiquidVolume.ShaderParams.PreserveSpecular, 0f);
			}
			this.mr.sharedMaterials = LiquidVolume.mrSharedMaterials.ToArray();
			this.liqMat.SetColor(LiquidVolume.ShaderParams.Color1, this.ApplyGlobalAlpha(this._liquidColor1));
			this.liqMat.SetColor(LiquidVolume.ShaderParams.Color2, this.ApplyGlobalAlpha(this._liquidColor2));
			this.liqMat.SetColor(LiquidVolume.ShaderParams.EmissionColor, this._emissionColor);
			if (this._useLightColor && this._directionalLight != null)
			{
				Color color = this._directionalLight.color;
				this.liqMat.SetColor(LiquidVolume.ShaderParams.LightColor, color);
			}
			else
			{
				this.liqMat.SetColor(LiquidVolume.ShaderParams.LightColor, Color.white);
			}
			if (this._useLightDirection && this._directionalLight != null)
			{
				this.liqMat.SetVector(LiquidVolume.ShaderParams.LightDir, -this._directionalLight.transform.forward);
			}
			else
			{
				this.liqMat.SetVector(LiquidVolume.ShaderParams.LightDir, Vector3.up);
			}
			int num2 = this._scatteringPower;
			float z = this._scatteringAmount;
			if (!this._scatteringEnabled)
			{
				num2 = 0;
				z = 0f;
			}
			this.liqMat.SetVector(LiquidVolume.ShaderParams.GlossinessInt, new Vector4((1f - this._glossinessInternal) * 96f + 1f, Mathf.Pow(2f, (float)num2), z, this._glossinessInternal));
			this.liqMat.SetFloat(LiquidVolume.ShaderParams.DoubleSidedBias, this._doubleSidedBias);
			this.liqMat.SetFloat(LiquidVolume.ShaderParams.BackDepthBias, -this._backDepthBias);
			this.liqMat.SetFloat(LiquidVolume.ShaderParams.Muddy, this._murkiness);
			this.liqMat.SetFloat(LiquidVolume.ShaderParams.Alpha, this._alpha);
			float num3 = this._alpha * Mathf.Clamp01((this._liquidColor1.a + this._liquidColor2.a) * 4f);
			if (this._ditherShadows)
			{
				this.liqMat.SetFloat(LiquidVolume.ShaderParams.AlphaCombined, num3);
			}
			else
			{
				this.liqMat.SetFloat(LiquidVolume.ShaderParams.AlphaCombined, (num3 > 0f) ? 1000f : 0f);
			}
			this.liqMat.SetFloat(LiquidVolume.ShaderParams.SparklingIntensity, this._sparklingIntensity * 250f);
			this.liqMat.SetFloat(LiquidVolume.ShaderParams.SparklingThreshold, 1f - this._sparklingAmount);
			this.liqMat.SetFloat(LiquidVolume.ShaderParams.DepthAtten, this._deepObscurance);
			Color value = this.ApplyGlobalAlpha(this._smokeColor);
			int num4 = this._smokeRaySteps;
			if (!this._smokeEnabled)
			{
				value.a = 0f;
				num4 = 1;
			}
			this.liqMat.SetColor(LiquidVolume.ShaderParams.SmokeColor, value);
			this.liqMat.SetFloat(LiquidVolume.ShaderParams.SmokeAtten, this._smokeBaseObscurance);
			this.liqMat.SetFloat(LiquidVolume.ShaderParams.SmokeHeightAtten, this._smokeHeightAtten);
			this.liqMat.SetFloat(LiquidVolume.ShaderParams.SmokeSpeed, this._smokeSpeed);
			this.liqMat.SetFloat(LiquidVolume.ShaderParams.SmokeRaySteps, (float)num4);
			this.liqMat.SetFloat(LiquidVolume.ShaderParams.LiquidRaySteps, (float)this._liquidRaySteps);
			this.liqMat.SetColor(LiquidVolume.ShaderParams.FoamColor, this.ApplyGlobalAlpha(this._foamColor));
			this.liqMat.SetFloat(LiquidVolume.ShaderParams.FoamRaySteps, (float)((this._foamThickness > 0f) ? this._foamRaySteps : 1));
			this.liqMat.SetFloat(LiquidVolume.ShaderParams.FoamDensity, (this._foamThickness > 0f) ? this._foamDensity : -1f);
			this.liqMat.SetFloat(LiquidVolume.ShaderParams.FoamWeight, this._foamWeight);
			this.liqMat.SetFloat(LiquidVolume.ShaderParams.FoamBottom, this._foamVisibleFromBottom ? 1f : 0f);
			this.liqMat.SetFloat(LiquidVolume.ShaderParams.FoamTurbulence, this._foamTurbulence);
			if (this._noiseVariation != this.currentNoiseVariation)
			{
				this.currentNoiseVariation = this._noiseVariation;
				if (this.noise3DTex == null || this.noise3DTex.Length != 4)
				{
					this.noise3DTex = new Texture3D[4];
				}
				if (this.noise3DTex[this.currentNoiseVariation] == null)
				{
					this.noise3DTex[this.currentNoiseVariation] = Resources.Load<Texture3D>("Textures/Noise3D" + this.currentNoiseVariation.ToString());
				}
				Texture3D texture3D = this.noise3DTex[this.currentNoiseVariation];
				if (texture3D != null)
				{
					this.liqMat.SetTexture(LiquidVolume.ShaderParams.NoiseTex, texture3D);
				}
			}
			this.liqMat.renderQueue = this._renderQueue;
			this.UpdateInsideOut();
			if (this._topology == TOPOLOGY.Irregular && this.prevThickness != this._flaskThickness)
			{
				this.prevThickness = this._flaskThickness;
			}
			PropertiesChangedEvent propertiesChangedEvent = this.onPropertiesChanged;
			if (propertiesChangedEvent == null)
			{
				return;
			}
			propertiesChangedEvent(this);
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x00023651 File Offset: 0x00021851
		private Color ApplyGlobalAlpha(Color originalColor)
		{
			return new Color(originalColor.r, originalColor.g, originalColor.b, originalColor.a * this._alpha);
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x00023678 File Offset: 0x00021878
		private void GetRenderer()
		{
			MeshFilter component = base.GetComponent<MeshFilter>();
			if (component != null)
			{
				this.mesh = component.sharedMesh;
				this.mr = base.GetComponent<MeshRenderer>();
				return;
			}
			SkinnedMeshRenderer component2 = base.GetComponent<SkinnedMeshRenderer>();
			if (component2 != null)
			{
				this.mesh = component2.sharedMesh;
				this.mr = component2;
			}
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x000236D4 File Offset: 0x000218D4
		private void UpdateLevels(bool updateShaderKeywords = true)
		{
			this._level = Mathf.Clamp01(this._level);
			this.levelMultipled = this._level * this._levelMultiplier;
			if (this.liqMat == null)
			{
				return;
			}
			if (this.mesh == null)
			{
				this.GetRenderer();
				this.ReadVertices();
			}
			else if (this.mr == null)
			{
				this.GetRenderer();
			}
			if (this.mesh == null || this.mr == null)
			{
				return;
			}
			Vector4 vector = new Vector4(this.mesh.bounds.extents.x * 2f * base.transform.lossyScale.x, this.mesh.bounds.extents.y * 2f * base.transform.lossyScale.y, this.mesh.bounds.extents.z * 2f * base.transform.lossyScale.z, 0f);
			vector.x *= this._extentsScale.x;
			vector.y *= this._extentsScale.y;
			vector.z *= this._extentsScale.z;
			float num = Mathf.Max(vector.x, vector.z);
			Vector3 vector2 = this._ignoreGravity ? new Vector3(vector.x * 0.5f, vector.y * 0.5f, vector.z * 0.5f) : this.mr.bounds.extents;
			vector2 *= 1f - this._flaskThickness;
			vector2.x *= this._extentsScale.x;
			vector2.y *= this._extentsScale.y;
			vector2.z *= this._extentsScale.z;
			float num2;
			if (this._upperLimit < 1f && !this._ignoreGravity)
			{
				float y = base.transform.TransformPoint(Vector3.up * vector2.y).y;
				num2 = Mathf.Max(base.transform.TransformPoint(Vector3.up * (vector2.y * this._upperLimit)).y - y, 0f);
			}
			else
			{
				num2 = 0f;
			}
			float num3 = this.levelMultipled;
			if (this._rotationLevelCompensation != LEVEL_COMPENSATION.None && !this._ignoreGravity && num3 > 0f)
			{
				LiquidVolume.MeshVolumeCalcFunction meshVolumeCalcFunction;
				int num4;
				if (this._rotationLevelCompensation == LEVEL_COMPENSATION.Fast)
				{
					meshVolumeCalcFunction = new LiquidVolume.MeshVolumeCalcFunction(this.GetMeshVolumeUnderLevelFast);
					num4 = 8;
				}
				else
				{
					meshVolumeCalcFunction = new LiquidVolume.MeshVolumeCalcFunction(this.GetMeshVolumeUnderLevel);
					num4 = 10;
				}
				if (this.lastLevelVolumeRef != num3)
				{
					this.lastLevelVolumeRef = num3;
					if (this._topology == TOPOLOGY.Cylinder)
					{
						float num5 = vector.x * 0.5f;
						float num6 = vector.y * num3;
						this.volumeRef = 3.1415927f * num5 * num5 * num6;
					}
					else
					{
						Quaternion rotation = base.transform.rotation;
						base.transform.rotation = Quaternion.identity;
						float num7 = this._ignoreGravity ? (vector.y * 0.5f) : this.mr.bounds.extents.y;
						num7 *= 1f - this._flaskThickness;
						num7 *= this._extentsScale.y;
						this.RotateVertices();
						this.volumeRef = meshVolumeCalcFunction(num3, num7);
						base.transform.rotation = rotation;
					}
				}
				this.RotateVertices();
				float num8 = num3;
				float num9 = float.MaxValue;
				float num10 = Mathf.Clamp01(num3 + 0.5f);
				float num11 = Mathf.Clamp01(num3 - 0.5f);
				for (int i = 0; i < 12; i++)
				{
					num3 = (num11 + num10) * 0.5f;
					float num12 = meshVolumeCalcFunction(num3, vector2.y);
					float num13 = Mathf.Abs(this.volumeRef - num12);
					if (num13 < num9)
					{
						num9 = num13;
						num8 = num3;
					}
					if (num12 < this.volumeRef)
					{
						num11 = num3;
					}
					else
					{
						if (i >= num4)
						{
							break;
						}
						num10 = num3;
					}
				}
				num3 = num8 * this._levelMultiplier;
			}
			else if (this.levelMultipled <= 0f)
			{
				num3 = -0.001f;
			}
			this.liquidLevelPos = this.mr.bounds.center.y - vector2.y;
			this.liquidLevelPos += vector2.y * 2f * num3 + num2;
			this.liqMat.SetFloat(LiquidVolume.ShaderParams.LevelPos, this.liquidLevelPos);
			float num14 = this.mesh.bounds.extents.y * this._extentsScale.y * this._upperLimit;
			this.liqMat.SetFloat(LiquidVolume.ShaderParams.UpperLimit, this._limitVerticalRange ? num14 : float.MaxValue);
			float num15 = this.mesh.bounds.extents.y * this._extentsScale.y * this._lowerLimit;
			this.liqMat.SetFloat(LiquidVolume.ShaderParams.LowerLimit, this._limitVerticalRange ? num15 : float.MinValue);
			float num16 = (this.levelMultipled <= 0f || this.levelMultipled >= 1f) ? 0f : 1f;
			this.UpdateTurbulence();
			float value = this.mr.bounds.center.y - vector2.y + (num2 + vector2.y * 2f * (num3 + this._foamThickness)) * num16;
			this.liqMat.SetFloat(LiquidVolume.ShaderParams.FoamMaxPos, value);
			Vector4 vector3 = new Vector4(1f - this._flaskThickness, 1f - this._flaskThickness * num / vector.z, 1f - this._flaskThickness * num / vector.z, 0f);
			this.liqMat.SetVector(LiquidVolume.ShaderParams.FlaskThickness, vector3);
			vector.w = vector.x * 0.5f * vector3.x;
			vector.x = Vector3.Distance(this.mr.bounds.max, this.mr.bounds.min);
			this.liqMat.SetVector(LiquidVolume.ShaderParams.Size, vector);
			float num17 = vector.y * 0.5f * (1f - this._flaskThickness * num / vector.y);
			this.liqMat.SetVector(LiquidVolume.ShaderParams.Scale, new Vector4(this._smokeScale / num17, this._foamScale / num17, this._liquidScale1 / num17, this._liquidScale2 / num17));
			this.liqMat.SetVector(LiquidVolume.ShaderParams.Center, base.transform.position);
			if (this.shaderKeywords == null || this.shaderKeywords.Length != 6)
			{
				this.shaderKeywords = new string[6];
			}
			for (int j = 0; j < this.shaderKeywords.Length; j++)
			{
				this.shaderKeywords[j] = null;
			}
			if (this._depthAware)
			{
				this.shaderKeywords[0] = "LIQUID_VOLUME_DEPTH_AWARE";
				this.liqMat.SetFloat(LiquidVolume.ShaderParams.DepthAwareOffset, this._depthAwareOffset);
			}
			if (this._depthAwareCustomPass)
			{
				this.shaderKeywords[1] = "LIQUID_VOLUME_DEPTH_AWARE_PASS";
			}
			if (this._reactToForces && this._topology == TOPOLOGY.Sphere)
			{
				this.shaderKeywords[2] = "LIQUID_VOLUME_IGNORE_GRAVITY";
			}
			else if (this._ignoreGravity)
			{
				this.shaderKeywords[2] = "LIQUID_VOLUME_IGNORE_GRAVITY";
			}
			else if (base.transform.rotation.eulerAngles != Vector3.zero)
			{
				this.shaderKeywords[3] = "LIQUID_VOLUME_NON_AABB";
			}
			switch (this._topology)
			{
			case TOPOLOGY.Sphere:
				this.shaderKeywords[4] = "LIQUID_VOLUME_SPHERE";
				break;
			case TOPOLOGY.Cylinder:
				this.shaderKeywords[4] = "LIQUID_VOLUME_CYLINDER";
				break;
			case TOPOLOGY.Cube:
				this.shaderKeywords[4] = "LIQUID_VOLUME_CUBE";
				break;
			default:
				this.shaderKeywords[4] = "LIQUID_VOLUME_IRREGULAR";
				break;
			}
			if (this._refractionBlur && this._detail.allowsRefraction())
			{
				this.liqMat.SetFloat(LiquidVolume.ShaderParams.FlaskBlurIntensity, this._blurIntensity * (this._refractionBlur ? 1f : 0f));
				this.shaderKeywords[5] = "LIQUID_VOLUME_USE_REFRACTION";
			}
			if (updateShaderKeywords)
			{
				this.liqMat.shaderKeywords = this.shaderKeywords;
			}
			this.lastPosition = base.transform.position;
			this.lastScale = base.transform.localScale;
			this.lastRotation = base.transform.rotation;
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x00023FCC File Offset: 0x000221CC
		private void RotateVertices()
		{
			int num = this.verticesUnsorted.Length;
			if (LiquidVolume.rotatedVertices == null || LiquidVolume.rotatedVertices.Length != num)
			{
				LiquidVolume.rotatedVertices = new Vector3[num];
			}
			for (int i = 0; i < num; i++)
			{
				LiquidVolume.rotatedVertices[i] = base.transform.TransformPoint(this.verticesUnsorted[i]);
			}
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x0002402C File Offset: 0x0002222C
		private float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 zeroPoint)
		{
			p1.x -= zeroPoint.x;
			p1.y -= zeroPoint.y;
			p1.z -= zeroPoint.z;
			p2.x -= zeroPoint.x;
			p2.y -= zeroPoint.y;
			p2.z -= zeroPoint.z;
			p3.x -= zeroPoint.x;
			p3.y -= zeroPoint.y;
			p3.z -= zeroPoint.z;
			float num = p3.x * p2.y * p1.z;
			float num2 = p2.x * p3.y * p1.z;
			float num3 = p3.x * p1.y * p2.z;
			float num4 = p1.x * p3.y * p2.z;
			float num5 = p2.x * p1.y * p3.z;
			float num6 = p1.x * p2.y * p3.z;
			return 0.16666667f * (-num + num2 + num3 - num4 - num5 + num6);
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x00024170 File Offset: 0x00022370
		public float GetMeshVolumeUnderLevelFast(float level01, float yExtent)
		{
			float num = this.mr.bounds.center.y - yExtent;
			num += yExtent * 2f * level01;
			return this.GetMeshVolumeUnderLevelWSFast(num);
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x000241AB File Offset: 0x000223AB
		public float GetMeshVolumeWSFast()
		{
			return this.GetMeshVolumeUnderLevelWSFast(float.MaxValue);
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x000241B8 File Offset: 0x000223B8
		public float GetMeshVolumeUnderLevelWSFast(float level)
		{
			Vector3 center = this.mr.bounds.center;
			float num = 0f;
			for (int i = 0; i < this.verticesIndices.Length; i += 3)
			{
				Vector3 vector = LiquidVolume.rotatedVertices[this.verticesIndices[i]];
				Vector3 vector2 = LiquidVolume.rotatedVertices[this.verticesIndices[i + 1]];
				Vector3 vector3 = LiquidVolume.rotatedVertices[this.verticesIndices[i + 2]];
				if (vector.y > level)
				{
					vector.y = level;
				}
				if (vector2.y > level)
				{
					vector2.y = level;
				}
				if (vector3.y > level)
				{
					vector3.y = level;
				}
				num += this.SignedVolumeOfTriangle(vector, vector2, vector3, center);
			}
			return Mathf.Abs(num);
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x00024284 File Offset: 0x00022484
		private Vector3 ClampVertexToSlicePlane(Vector3 p, Vector3 q, float level)
		{
			Vector3 normalized = (q - p).normalized;
			float d = p.y - level;
			return p + normalized * d / -normalized.y;
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x000242C4 File Offset: 0x000224C4
		public float GetMeshVolumeUnderLevel(float level01, float yExtent)
		{
			float num = this.mr.bounds.center.y - yExtent;
			num += yExtent * 2f * level01;
			return this.GetMeshVolumeUnderLevelWS(num);
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x000242FF File Offset: 0x000224FF
		public float GetMeshVolumeWS()
		{
			return this.GetMeshVolumeUnderLevelWS(float.MaxValue);
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x0002430C File Offset: 0x0002250C
		public float GetMeshVolumeUnderLevelWS(float level)
		{
			Vector3 center = this.mr.bounds.center;
			this.cutPlaneCenter = Vector3.zero;
			this.cutPoints.Clear();
			this.verts.Clear();
			int num = this.verticesIndices.Length;
			for (int i = 0; i < num; i += 3)
			{
				Vector3 vector = LiquidVolume.rotatedVertices[this.verticesIndices[i]];
				Vector3 vector2 = LiquidVolume.rotatedVertices[this.verticesIndices[i + 1]];
				Vector3 vector3 = LiquidVolume.rotatedVertices[this.verticesIndices[i + 2]];
				if (vector.y <= level || vector2.y <= level || vector3.y <= level)
				{
					if (vector.y < level && vector2.y > level && vector3.y > level)
					{
						vector2 = this.ClampVertexToSlicePlane(vector2, vector, level);
						vector3 = this.ClampVertexToSlicePlane(vector3, vector, level);
						this.cutPoints.Add(vector2);
						this.cutPoints.Add(vector3);
						this.cutPlaneCenter += vector2;
						this.cutPlaneCenter += vector3;
					}
					else if (vector2.y < level && vector.y > level && vector3.y > level)
					{
						vector = this.ClampVertexToSlicePlane(vector, vector2, level);
						vector3 = this.ClampVertexToSlicePlane(vector3, vector2, level);
						this.cutPoints.Add(vector);
						this.cutPoints.Add(vector3);
						this.cutPlaneCenter += vector;
						this.cutPlaneCenter += vector3;
					}
					else if (vector3.y < level && vector.y > level && vector2.y > level)
					{
						vector = this.ClampVertexToSlicePlane(vector, vector3, level);
						vector2 = this.ClampVertexToSlicePlane(vector2, vector3, level);
						this.cutPoints.Add(vector);
						this.cutPoints.Add(vector2);
						this.cutPlaneCenter += vector;
						this.cutPlaneCenter += vector2;
					}
					else
					{
						if (vector.y > level && vector2.y < level && vector3.y < level)
						{
							Vector3 vector4 = this.ClampVertexToSlicePlane(vector, vector2, level);
							Vector3 vector5 = this.ClampVertexToSlicePlane(vector, vector3, level);
							this.verts.Add(vector4);
							this.verts.Add(vector2);
							this.verts.Add(vector3);
							this.verts.Add(vector5);
							this.verts.Add(vector4);
							this.verts.Add(vector3);
							this.cutPoints.Add(vector4);
							this.cutPoints.Add(vector5);
							this.cutPlaneCenter += vector4;
							this.cutPlaneCenter += vector5;
							goto IL_4C2;
						}
						if (vector2.y > level && vector.y < level && vector3.y < level)
						{
							Vector3 vector6 = this.ClampVertexToSlicePlane(vector2, vector, level);
							Vector3 vector7 = this.ClampVertexToSlicePlane(vector2, vector3, level);
							this.verts.Add(vector);
							this.verts.Add(vector6);
							this.verts.Add(vector3);
							this.verts.Add(vector6);
							this.verts.Add(vector7);
							this.verts.Add(vector3);
							this.cutPoints.Add(vector6);
							this.cutPoints.Add(vector7);
							this.cutPlaneCenter += vector6;
							this.cutPlaneCenter += vector7;
							goto IL_4C2;
						}
						if (vector3.y > level && vector.y < level && vector2.y < level)
						{
							Vector3 vector8 = this.ClampVertexToSlicePlane(vector3, vector, level);
							Vector3 vector9 = this.ClampVertexToSlicePlane(vector3, vector2, level);
							this.verts.Add(vector8);
							this.verts.Add(vector);
							this.verts.Add(vector2);
							this.verts.Add(vector9);
							this.verts.Add(vector8);
							this.verts.Add(vector2);
							this.cutPoints.Add(vector8);
							this.cutPoints.Add(vector9);
							this.cutPlaneCenter += vector8;
							this.cutPlaneCenter += vector9;
							goto IL_4C2;
						}
					}
					this.verts.Add(vector);
					this.verts.Add(vector2);
					this.verts.Add(vector3);
				}
				IL_4C2:;
			}
			int count = this.cutPoints.Count;
			if (this.cutPoints.Count >= 3)
			{
				this.cutPlaneCenter /= (float)count;
				this.cutPoints.Sort(new Comparison<Vector3>(this.PolygonSortOnPlane));
				for (int j = 0; j < count; j++)
				{
					Vector3 item = this.cutPoints[j];
					Vector3 item2;
					if (j == count - 1)
					{
						item2 = this.cutPoints[0];
					}
					else
					{
						item2 = this.cutPoints[j + 1];
					}
					this.verts.Add(this.cutPlaneCenter);
					this.verts.Add(item);
					this.verts.Add(item2);
				}
			}
			int count2 = this.verts.Count;
			float num2 = 0f;
			for (int k = 0; k < count2; k += 3)
			{
				num2 += this.SignedVolumeOfTriangle(this.verts[k], this.verts[k + 1], this.verts[k + 2], center);
			}
			return Mathf.Abs(num2);
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x00024904 File Offset: 0x00022B04
		private int PolygonSortOnPlane(Vector3 p1, Vector3 p2)
		{
			float num = Mathf.Atan2(p1.x - this.cutPlaneCenter.x, p1.z - this.cutPlaneCenter.z);
			float num2 = Mathf.Atan2(p2.x - this.cutPlaneCenter.x, p2.z - this.cutPlaneCenter.z);
			if (num < num2)
			{
				return -1;
			}
			if (num > num2)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x00024974 File Offset: 0x00022B74
		private void UpdateTurbulence()
		{
			if (this.liqMat == null)
			{
				return;
			}
			float num = (this.levelMultipled > 0f) ? 1f : 0f;
			float num2 = (this.camInside && this._allowViewFromInside) ? 0f : 1f;
			this.turb.x = this._turbulence1 * num * num2;
			this.turb.y = Mathf.Max(this._turbulence2, this.turbulenceDueForces) * num * num2;
			this.shaderTurb = this.turb;
			this.shaderTurb.z = this.shaderTurb.z * (3.1415927f * this._frecuency * 4f);
			this.shaderTurb.w = this.shaderTurb.w * (3.1415927f * this._frecuency * 4f);
			this.liqMat.SetVector(LiquidVolume.ShaderParams.Turbulence, this.shaderTurb);
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x00024A60 File Offset: 0x00022C60
		private void CheckInsideOut()
		{
			Camera current = Camera.current;
			if (current == null || this.mr == null)
			{
				if (!this._allowViewFromInside)
				{
					this.UpdateInsideOut();
				}
				return;
			}
			Vector3 vector = current.transform.position + current.transform.forward * current.nearClipPlane;
			float sqrMagnitude = (vector - base.transform.position).sqrMagnitude;
			if (sqrMagnitude == this.lastDistanceToCam)
			{
				return;
			}
			this.lastDistanceToCam = sqrMagnitude;
			TOPOLOGY topology = this._topology;
			bool flag;
			if (topology != TOPOLOGY.Cylinder)
			{
				if (topology == TOPOLOGY.Cube)
				{
					flag = this.PointInAABB(vector);
				}
				else
				{
					float num = this.mesh.bounds.extents.x * 2f;
					flag = ((vector - base.transform.position).sqrMagnitude < num * num);
				}
			}
			else
			{
				flag = this.PointInCylinder(vector);
			}
			if (flag != this.camInside)
			{
				this.camInside = flag;
				this.UpdateInsideOut();
			}
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x00024B70 File Offset: 0x00022D70
		private bool PointInAABB(Vector3 point)
		{
			point = base.transform.InverseTransformPoint(point);
			Vector3 extents = this.mesh.bounds.extents;
			return point.x < extents.x && point.x > -extents.x && point.y < extents.y && point.y > -extents.y && point.z < extents.z && point.z > -extents.z;
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x00024BFC File Offset: 0x00022DFC
		private bool PointInCylinder(Vector3 point)
		{
			point = base.transform.InverseTransformPoint(point);
			Vector3 extents = this.mesh.bounds.extents;
			if (point.x < extents.x && point.x > -extents.x && point.y < extents.y && point.y > -extents.y && point.z < extents.z && point.z > -extents.z)
			{
				point.y = 0f;
				Vector3 position = base.transform.position;
				position.y = 0f;
				return (point - position).sqrMagnitude < extents.x * extents.x;
			}
			return false;
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x00024CCC File Offset: 0x00022ECC
		private void UpdateInsideOut()
		{
			if (this.liqMat == null)
			{
				return;
			}
			if (this._allowViewFromInside && this.camInside)
			{
				this.liqMat.SetInt(LiquidVolume.ShaderParams.CullMode, 1);
				this.liqMat.SetInt(LiquidVolume.ShaderParams.ZTestMode, 8);
				if (this._flaskMaterial != null)
				{
					this._flaskMaterial.SetInt(LiquidVolume.ShaderParams.CullMode, 1);
					this._flaskMaterial.SetInt(LiquidVolume.ShaderParams.ZTestMode, 8);
				}
			}
			else
			{
				this.liqMat.SetInt(LiquidVolume.ShaderParams.CullMode, 2);
				this.liqMat.SetInt(LiquidVolume.ShaderParams.ZTestMode, 4);
				if (this._flaskMaterial != null)
				{
					this._flaskMaterial.SetInt(LiquidVolume.ShaderParams.CullMode, 2);
					this._flaskMaterial.SetInt(LiquidVolume.ShaderParams.ZTestMode, 4);
				}
			}
			this.UpdateTurbulence();
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x0600081F RID: 2079 RVA: 0x00024DA4 File Offset: 0x00022FA4
		public float liquidSurfaceYPosition
		{
			get
			{
				return this.liquidLevelPos;
			}
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x00024DAC File Offset: 0x00022FAC
		public bool GetSpillPoint(out Vector3 spillPosition, float apertureStart = 1f)
		{
			float num;
			return this.GetSpillPoint(out spillPosition, out num, apertureStart, LEVEL_COMPENSATION.None);
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x00024DC4 File Offset: 0x00022FC4
		public bool GetSpillPoint(out Vector3 spillPosition, out float spillAmount, float apertureStart = 1f, LEVEL_COMPENSATION rotationCompensation = LEVEL_COMPENSATION.None)
		{
			spillPosition = Vector3.zero;
			spillAmount = 0f;
			if (this.mesh == null || this.verticesSorted == null || this.levelMultipled <= 0f)
			{
				return false;
			}
			float num = float.MinValue;
			for (int i = 0; i < this.verticesSorted.Length; i++)
			{
				Vector3 vector = this.verticesSorted[i];
				if (vector.y > num)
				{
					num = vector.y;
				}
			}
			float num2 = num * apertureStart * 0.99f;
			Vector3 vector2 = base.transform.position;
			bool flag = false;
			float num3 = float.MaxValue;
			for (int j = 0; j < this.verticesSorted.Length; j++)
			{
				Vector3 vector3 = this.verticesSorted[j];
				if (vector3.y < num2)
				{
					break;
				}
				vector3 = base.transform.TransformPoint(vector3);
				if (vector3.y < this.liquidLevelPos && vector3.y < num3)
				{
					num3 = vector3.y;
					vector2 = vector3;
					flag = true;
				}
			}
			if (!flag)
			{
				return false;
			}
			spillPosition = vector2;
			if (rotationCompensation != LEVEL_COMPENSATION.Fast)
			{
				if (rotationCompensation == LEVEL_COMPENSATION.Accurate)
				{
					spillAmount = this.GetMeshVolumeUnderLevelWS(this.liquidLevelPos) - this.GetMeshVolumeUnderLevelWS(vector2.y);
				}
				else
				{
					spillAmount = (this.liquidLevelPos - vector2.y) / (this.mr.bounds.extents.y * 2f);
				}
			}
			else
			{
				spillAmount = this.GetMeshVolumeUnderLevelWSFast(this.liquidLevelPos) - this.GetMeshVolumeUnderLevelWSFast(vector2.y);
			}
			return true;
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x00024F50 File Offset: 0x00023150
		private void UpdateSpillPointGizmo()
		{
			if (!this._debugSpillPoint)
			{
				if (this.spillPointGizmo != null)
				{
					UnityEngine.Object.DestroyImmediate(this.spillPointGizmo.gameObject);
					this.spillPointGizmo = null;
				}
				return;
			}
			if (this.spillPointGizmo == null)
			{
				Transform transform = base.transform.Find("SpillPointGizmo");
				if (transform != null)
				{
					UnityEngine.Object.DestroyImmediate(transform.gameObject);
				}
				this.spillPointGizmo = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				this.spillPointGizmo.name = "SpillPointGizmo";
				this.spillPointGizmo.transform.SetParent(base.transform, true);
				Collider component = this.spillPointGizmo.GetComponent<Collider>();
				if (component != null)
				{
					UnityEngine.Object.DestroyImmediate(component);
				}
				MeshRenderer component2 = this.spillPointGizmo.GetComponent<MeshRenderer>();
				if (component2 != null)
				{
					component2.sharedMaterial = UnityEngine.Object.Instantiate<Material>(component2.sharedMaterial);
					component2.sharedMaterial.hideFlags = HideFlags.DontSave;
					component2.sharedMaterial.color = Color.yellow;
				}
			}
			Vector3 position;
			if (this.GetSpillPoint(out position, 1f))
			{
				this.spillPointGizmo.transform.position = position;
				if (this.mesh != null)
				{
					Vector3 vector = this.mesh.bounds.extents * 0.2f;
					float num = (vector.x > vector.y) ? vector.x : vector.z;
					num = ((num > vector.z) ? num : vector.z);
					this.spillPointGizmo.transform.localScale = new Vector3(num, num, num);
				}
				else
				{
					this.spillPointGizmo.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
				}
				this.spillPointGizmo.SetActive(true);
				return;
			}
			this.spillPointGizmo.SetActive(false);
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x00025138 File Offset: 0x00023338
		public void BakeRotation()
		{
			if (base.transform.localRotation == base.transform.rotation)
			{
				return;
			}
			MeshFilter component = base.GetComponent<MeshFilter>();
			Mesh mesh = component.sharedMesh;
			if (mesh == null)
			{
				return;
			}
			mesh = UnityEngine.Object.Instantiate<Mesh>(mesh);
			Vector3[] vertices = mesh.vertices;
			Vector3 localScale = base.transform.localScale;
			Vector3 localPosition = base.transform.localPosition;
			base.transform.localScale = Vector3.one;
			Transform parent = base.transform.parent;
			if (parent != null)
			{
				base.transform.SetParent(null, false);
			}
			for (int i = 0; i < vertices.Length; i++)
			{
				vertices[i] = base.transform.TransformVector(vertices[i]);
			}
			mesh.vertices = vertices;
			mesh.RecalculateBounds();
			mesh.RecalculateNormals();
			component.sharedMesh = mesh;
			if (parent != null)
			{
				base.transform.SetParent(parent, false);
				base.transform.localPosition = localPosition;
			}
			base.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
			base.transform.localScale = localScale;
			this.RefreshMeshAndCollider();
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x00025273 File Offset: 0x00023473
		public void CenterPivot()
		{
			this.CenterPivot(Vector3.zero);
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x00025280 File Offset: 0x00023480
		public void CenterPivot(Vector3 offset)
		{
			MeshFilter component = base.GetComponent<MeshFilter>();
			Mesh mesh = component.sharedMesh;
			if (mesh == null)
			{
				return;
			}
			mesh = UnityEngine.Object.Instantiate<Mesh>(mesh);
			mesh.name = component.sharedMesh.name;
			Vector3[] vertices = mesh.vertices;
			Vector3 vector = Vector3.zero;
			for (int i = 0; i < vertices.Length; i++)
			{
				vector += vertices[i];
			}
			vector /= (float)vertices.Length;
			vector += offset;
			for (int j = 0; j < vertices.Length; j++)
			{
				vertices[j] -= vector;
			}
			mesh.vertices = vertices;
			mesh.RecalculateBounds();
			component.sharedMesh = mesh;
			this.fixedMesh = mesh;
			Vector3 localScale = base.transform.localScale;
			vector.x *= localScale.x;
			vector.y *= localScale.y;
			vector.z *= localScale.z;
			base.transform.localPosition += vector;
			this.RefreshMeshAndCollider();
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x000253A4 File Offset: 0x000235A4
		public void RefreshMeshAndCollider()
		{
			this.ClearMeshCache();
			MeshCollider component = base.GetComponent<MeshCollider>();
			if (component != null)
			{
				Mesh sharedMesh = component.sharedMesh;
				component.sharedMesh = null;
				component.sharedMesh = sharedMesh;
			}
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x000253DC File Offset: 0x000235DC
		public void Redraw()
		{
			this.UpdateMaterialProperties();
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x000253E4 File Offset: 0x000235E4
		private void CheckMeshDisplacement()
		{
			MeshFilter component = base.GetComponent<MeshFilter>();
			if (component == null)
			{
				this.originalMesh = null;
				return;
			}
			Mesh sharedMesh = component.sharedMesh;
			if (sharedMesh == null)
			{
				if (!this._fixMesh)
				{
					this.originalMesh = null;
					return;
				}
				if (this.fixedMesh != null)
				{
					component.sharedMesh = this.fixedMesh;
					return;
				}
				if (this.originalMesh != null)
				{
					component.sharedMesh = this.originalMesh;
				}
				sharedMesh = component.sharedMesh;
			}
			if (!this._fixMesh)
			{
				this.RestoreOriginalMesh();
				this.originalMesh = null;
				return;
			}
			if (this.originalMesh == null || !this.originalMesh.name.Equals(sharedMesh.name))
			{
				this.originalMesh = component.sharedMesh;
			}
			if (sharedMesh != this.originalMesh)
			{
				this.RestoreOriginalMesh();
			}
			Vector3 localPosition = base.transform.localPosition;
			this.CenterPivot(this._pivotOffset);
			this.originalPivotOffset = base.transform.localPosition - localPosition;
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x000254F4 File Offset: 0x000236F4
		private void RestoreOriginalMesh()
		{
			this.fixedMesh = null;
			if (this.originalMesh == null)
			{
				return;
			}
			MeshFilter component = base.GetComponent<MeshFilter>();
			if (component == null)
			{
				return;
			}
			component.sharedMesh = this.originalMesh;
			base.transform.localPosition -= this.originalPivotOffset;
			this.RefreshMeshAndCollider();
		}

		// Token: 0x0600082A RID: 2090 RVA: 0x00025558 File Offset: 0x00023758
		public void CopyFrom(LiquidVolume lv)
		{
			if (lv == null)
			{
				return;
			}
			this._allowViewFromInside = lv._allowViewFromInside;
			this._alpha = lv._alpha;
			this._backDepthBias = lv._backDepthBias;
			this._blurIntensity = lv._blurIntensity;
			this._bumpDistortionOffset = lv._bumpDistortionOffset;
			this._bumpDistortionScale = lv._bumpDistortionScale;
			this._bumpMap = lv._bumpMap;
			this._bumpStrength = lv._bumpStrength;
			this._debugSpillPoint = lv._debugSpillPoint;
			this._deepObscurance = lv._deepObscurance;
			this._depthAware = lv._depthAware;
			this._depthAwareCustomPass = lv._depthAwareCustomPass;
			this._depthAwareCustomPassDebug = lv._depthAwareCustomPassDebug;
			this._depthAwareOffset = lv._depthAwareOffset;
			this._detail = lv._detail;
			this._distortionAmount = lv._distortionAmount;
			this._distortionMap = lv._distortionMap;
			this._ditherShadows = lv._ditherShadows;
			this._doubleSidedBias = lv._doubleSidedBias;
			this._emissionColor = lv._emissionColor;
			this._extentsScale = lv._extentsScale;
			this._fixMesh = lv._fixMesh;
			this._flaskThickness = lv._flaskThickness;
			this._foamColor = lv._foamColor;
			this._foamDensity = lv._foamDensity;
			this._foamRaySteps = lv._foamRaySteps;
			this._foamScale = lv._foamScale;
			this._foamThickness = lv._foamThickness;
			this._foamTurbulence = lv._foamTurbulence;
			this._foamVisibleFromBottom = lv._foamVisibleFromBottom;
			this._foamWeight = lv._foamWeight;
			this._frecuency = lv._frecuency;
			this._ignoreGravity = lv._ignoreGravity;
			this._irregularDepthDebug = lv._irregularDepthDebug;
			this._level = lv._level;
			this._levelMultiplier = lv._levelMultiplier;
			this._liquidColor1 = lv._liquidColor1;
			this._liquidColor2 = lv._liquidColor2;
			this._liquidRaySteps = lv._liquidRaySteps;
			this._liquidScale1 = lv._liquidScale1;
			this._liquidScale2 = lv._liquidScale2;
			this._lowerLimit = lv._lowerLimit;
			this._murkiness = lv._murkiness;
			this._noiseVariation = lv._noiseVariation;
			this._physicsAngularDamp = lv._physicsAngularDamp;
			this._physicsMass = lv._physicsMass;
			this._pivotOffset = lv._pivotOffset;
			this._reactToForces = lv._reactToForces;
			this._reflectionTexture = lv._reflectionTexture;
			this._refractionBlur = lv._refractionBlur;
			this._renderQueue = lv._renderQueue;
			this._scatteringAmount = lv._scatteringAmount;
			this._scatteringEnabled = lv._scatteringEnabled;
			this._scatteringPower = lv._scatteringPower;
			this._smokeBaseObscurance = lv._smokeBaseObscurance;
			this._smokeColor = lv._smokeColor;
			this._smokeEnabled = lv._smokeEnabled;
			this._smokeHeightAtten = lv._smokeHeightAtten;
			this._smokeRaySteps = lv._smokeRaySteps;
			this._smokeScale = lv._smokeScale;
			this._smokeSpeed = lv._smokeSpeed;
			this._sparklingAmount = lv._sparklingAmount;
			this._sparklingIntensity = lv._sparklingIntensity;
			this._speed = lv._speed;
			this._subMeshIndex = lv._subMeshIndex;
			this._texture = lv._texture;
			this._textureOffset = lv._textureOffset;
			this._textureScale = lv._textureScale;
			this._topology = lv._topology;
			this._turbulence1 = lv._turbulence1;
			this._turbulence2 = lv._turbulence2;
			this._upperLimit = lv._upperLimit;
			this.shouldUpdateMaterialProperties = true;
		}

		// Token: 0x04000848 RID: 2120
		public static bool FORCE_GLES_COMPATIBILITY = false;

		// Token: 0x0400084A RID: 2122
		[SerializeField]
		private TOPOLOGY _topology;

		// Token: 0x0400084B RID: 2123
		[SerializeField]
		private DETAIL _detail = DETAIL.Default;

		// Token: 0x0400084C RID: 2124
		[SerializeField]
		[Range(0f, 1f)]
		private float _level = 0.5f;

		// Token: 0x0400084D RID: 2125
		[SerializeField]
		[Range(0f, 1f)]
		private float _levelMultiplier = 1f;

		// Token: 0x0400084E RID: 2126
		[SerializeField]
		[Tooltip("Uses directional light color")]
		private bool _useLightColor;

		// Token: 0x0400084F RID: 2127
		[SerializeField]
		[Tooltip("Uses directional light direction for day/night cycle")]
		private bool _useLightDirection;

		// Token: 0x04000850 RID: 2128
		[SerializeField]
		private Light _directionalLight;

		// Token: 0x04000851 RID: 2129
		[SerializeField]
		[ColorUsage(true)]
		private Color _liquidColor1 = new Color(0f, 1f, 0f, 0.1f);

		// Token: 0x04000852 RID: 2130
		[SerializeField]
		[Range(0.1f, 4.85f)]
		private float _liquidScale1 = 1f;

		// Token: 0x04000853 RID: 2131
		[SerializeField]
		[ColorUsage(true)]
		private Color _liquidColor2 = new Color(1f, 0f, 0f, 0.3f);

		// Token: 0x04000854 RID: 2132
		[SerializeField]
		[Range(2f, 4.85f)]
		private float _liquidScale2 = 5f;

		// Token: 0x04000855 RID: 2133
		[SerializeField]
		[Range(0f, 1f)]
		private float _alpha = 1f;

		// Token: 0x04000856 RID: 2134
		[SerializeField]
		[ColorUsage(true)]
		private Color _emissionColor = new Color(0f, 0f, 0f);

		// Token: 0x04000857 RID: 2135
		[SerializeField]
		private bool _ditherShadows = true;

		// Token: 0x04000858 RID: 2136
		[SerializeField]
		[Range(0f, 1f)]
		private float _murkiness = 1f;

		// Token: 0x04000859 RID: 2137
		[SerializeField]
		[Range(0f, 1f)]
		private float _turbulence1 = 0.5f;

		// Token: 0x0400085A RID: 2138
		[SerializeField]
		[Range(0f, 1f)]
		private float _turbulence2 = 0.2f;

		// Token: 0x0400085B RID: 2139
		[SerializeField]
		private float _frecuency = 1f;

		// Token: 0x0400085C RID: 2140
		[SerializeField]
		[Range(0f, 2f)]
		private float _speed = 1f;

		// Token: 0x0400085D RID: 2141
		[SerializeField]
		[Range(0f, 5f)]
		private float _sparklingIntensity = 0.1f;

		// Token: 0x0400085E RID: 2142
		[SerializeField]
		[Range(0f, 1f)]
		private float _sparklingAmount = 0.2f;

		// Token: 0x0400085F RID: 2143
		[SerializeField]
		[Range(0f, 10f)]
		private float _deepObscurance = 2f;

		// Token: 0x04000860 RID: 2144
		[SerializeField]
		[ColorUsage(true)]
		private Color _foamColor = new Color(1f, 1f, 1f, 0.65f);

		// Token: 0x04000861 RID: 2145
		[SerializeField]
		[Range(0.01f, 1f)]
		private float _foamScale = 0.2f;

		// Token: 0x04000862 RID: 2146
		[SerializeField]
		[Range(0f, 0.1f)]
		private float _foamThickness = 0.04f;

		// Token: 0x04000863 RID: 2147
		[SerializeField]
		[Range(-1f, 1f)]
		private float _foamDensity = 0.5f;

		// Token: 0x04000864 RID: 2148
		[SerializeField]
		[Range(4f, 100f)]
		private float _foamWeight = 10f;

		// Token: 0x04000865 RID: 2149
		[SerializeField]
		[Range(0f, 1f)]
		private float _foamTurbulence = 1f;

		// Token: 0x04000866 RID: 2150
		[SerializeField]
		private bool _foamVisibleFromBottom = true;

		// Token: 0x04000867 RID: 2151
		[SerializeField]
		private bool _smokeEnabled = true;

		// Token: 0x04000868 RID: 2152
		[ColorUsage(true)]
		[SerializeField]
		private Color _smokeColor = new Color(0.7f, 0.7f, 0.7f, 0.25f);

		// Token: 0x04000869 RID: 2153
		[SerializeField]
		[Range(0.01f, 1f)]
		private float _smokeScale = 0.25f;

		// Token: 0x0400086A RID: 2154
		[SerializeField]
		[Range(0f, 10f)]
		private float _smokeBaseObscurance = 2f;

		// Token: 0x0400086B RID: 2155
		[SerializeField]
		[Range(0f, 10f)]
		private float _smokeHeightAtten;

		// Token: 0x0400086C RID: 2156
		[SerializeField]
		[Range(0f, 20f)]
		private float _smokeSpeed = 5f;

		// Token: 0x0400086D RID: 2157
		[SerializeField]
		private bool _fixMesh;

		// Token: 0x0400086E RID: 2158
		public Mesh originalMesh;

		// Token: 0x0400086F RID: 2159
		public Vector3 originalPivotOffset;

		// Token: 0x04000870 RID: 2160
		[SerializeField]
		private Vector3 _pivotOffset;

		// Token: 0x04000871 RID: 2161
		[SerializeField]
		private bool _limitVerticalRange;

		// Token: 0x04000872 RID: 2162
		[SerializeField]
		[Range(0f, 1.5f)]
		private float _upperLimit = 1.5f;

		// Token: 0x04000873 RID: 2163
		[SerializeField]
		[Range(-1.5f, 1.5f)]
		private float _lowerLimit = -1.5f;

		// Token: 0x04000874 RID: 2164
		[SerializeField]
		private int _subMeshIndex = -1;

		// Token: 0x04000875 RID: 2165
		[SerializeField]
		private Material _flaskMaterial;

		// Token: 0x04000876 RID: 2166
		[SerializeField]
		[Range(0f, 1f)]
		private float _flaskThickness = 0.03f;

		// Token: 0x04000877 RID: 2167
		[SerializeField]
		[Range(0f, 1f)]
		private float _glossinessInternal = 0.3f;

		// Token: 0x04000878 RID: 2168
		[SerializeField]
		private bool _scatteringEnabled;

		// Token: 0x04000879 RID: 2169
		[SerializeField]
		[Range(1f, 16f)]
		private int _scatteringPower = 5;

		// Token: 0x0400087A RID: 2170
		[SerializeField]
		[Range(0f, 10f)]
		private float _scatteringAmount = 0.3f;

		// Token: 0x0400087B RID: 2171
		[SerializeField]
		private bool _refractionBlur = true;

		// Token: 0x0400087C RID: 2172
		[SerializeField]
		[Range(0f, 1f)]
		private float _blurIntensity = 0.75f;

		// Token: 0x0400087D RID: 2173
		[SerializeField]
		private int _liquidRaySteps = 10;

		// Token: 0x0400087E RID: 2174
		[SerializeField]
		private int _foamRaySteps = 7;

		// Token: 0x0400087F RID: 2175
		[SerializeField]
		private int _smokeRaySteps = 5;

		// Token: 0x04000880 RID: 2176
		[SerializeField]
		private Texture2D _bumpMap;

		// Token: 0x04000881 RID: 2177
		[SerializeField]
		[Range(0f, 1f)]
		private float _bumpStrength = 1f;

		// Token: 0x04000882 RID: 2178
		[SerializeField]
		[Range(0f, 10f)]
		private float _bumpDistortionScale = 1f;

		// Token: 0x04000883 RID: 2179
		[SerializeField]
		private Vector2 _bumpDistortionOffset;

		// Token: 0x04000884 RID: 2180
		[SerializeField]
		private Texture2D _distortionMap;

		// Token: 0x04000885 RID: 2181
		[SerializeField]
		private Texture2D _texture;

		// Token: 0x04000886 RID: 2182
		[SerializeField]
		private Vector2 _textureScale = Vector2.one;

		// Token: 0x04000887 RID: 2183
		[SerializeField]
		private Vector2 _textureOffset;

		// Token: 0x04000888 RID: 2184
		[SerializeField]
		[Range(0f, 10f)]
		private float _distortionAmount = 1f;

		// Token: 0x04000889 RID: 2185
		[SerializeField]
		private bool _depthAware;

		// Token: 0x0400088A RID: 2186
		[SerializeField]
		private float _depthAwareOffset;

		// Token: 0x0400088B RID: 2187
		[SerializeField]
		private bool _irregularDepthDebug;

		// Token: 0x0400088C RID: 2188
		[SerializeField]
		private bool _depthAwareCustomPass;

		// Token: 0x0400088D RID: 2189
		[SerializeField]
		private bool _depthAwareCustomPassDebug;

		// Token: 0x0400088E RID: 2190
		[SerializeField]
		[Range(0f, 5f)]
		private float _doubleSidedBias;

		// Token: 0x0400088F RID: 2191
		[SerializeField]
		private float _backDepthBias;

		// Token: 0x04000890 RID: 2192
		[SerializeField]
		private LEVEL_COMPENSATION _rotationLevelCompensation;

		// Token: 0x04000891 RID: 2193
		[SerializeField]
		private bool _ignoreGravity;

		// Token: 0x04000892 RID: 2194
		[SerializeField]
		private bool _reactToForces;

		// Token: 0x04000893 RID: 2195
		[SerializeField]
		private Vector3 _extentsScale = Vector3.one;

		// Token: 0x04000894 RID: 2196
		[SerializeField]
		[Range(1f, 3f)]
		private int _noiseVariation = 1;

		// Token: 0x04000895 RID: 2197
		[SerializeField]
		private bool _allowViewFromInside;

		// Token: 0x04000896 RID: 2198
		[SerializeField]
		private bool _debugSpillPoint;

		// Token: 0x04000897 RID: 2199
		[SerializeField]
		private int _renderQueue = 3001;

		// Token: 0x04000898 RID: 2200
		[SerializeField]
		private Cubemap _reflectionTexture;

		// Token: 0x04000899 RID: 2201
		[SerializeField]
		[Range(0.1f, 5f)]
		private float _physicsMass = 1f;

		// Token: 0x0400089A RID: 2202
		[SerializeField]
		[Range(0f, 0.2f)]
		private float _physicsAngularDamp = 0.02f;

		// Token: 0x0400089B RID: 2203
		private const int SHADER_KEYWORD_DEPTH_AWARE_INDEX = 0;

		// Token: 0x0400089C RID: 2204
		private const int SHADER_KEYWORD_DEPTH_AWARE_CUSTOM_PASS_INDEX = 1;

		// Token: 0x0400089D RID: 2205
		private const int SHADER_KEYWORD_IGNORE_GRAVITY_INDEX = 2;

		// Token: 0x0400089E RID: 2206
		private const int SHADER_KEYWORD_NON_AABB_INDEX = 3;

		// Token: 0x0400089F RID: 2207
		private const int SHADER_KEYWORD_TOPOLOGY_INDEX = 4;

		// Token: 0x040008A0 RID: 2208
		private const int SHADER_KEYWORD_REFRACTION_INDEX = 5;

		// Token: 0x040008A1 RID: 2209
		private const string SHADER_KEYWORD_DEPTH_AWARE = "LIQUID_VOLUME_DEPTH_AWARE";

		// Token: 0x040008A2 RID: 2210
		private const string SHADER_KEYWORD_DEPTH_AWARE_CUSTOM_PASS = "LIQUID_VOLUME_DEPTH_AWARE_PASS";

		// Token: 0x040008A3 RID: 2211
		private const string SHADER_KEYWORD_NON_AABB = "LIQUID_VOLUME_NON_AABB";

		// Token: 0x040008A4 RID: 2212
		private const string SHADER_KEYWORD_IGNORE_GRAVITY = "LIQUID_VOLUME_IGNORE_GRAVITY";

		// Token: 0x040008A5 RID: 2213
		private const string SHADER_KEYWORD_SPHERE = "LIQUID_VOLUME_SPHERE";

		// Token: 0x040008A6 RID: 2214
		private const string SHADER_KEYWORD_CUBE = "LIQUID_VOLUME_CUBE";

		// Token: 0x040008A7 RID: 2215
		private const string SHADER_KEYWORD_CYLINDER = "LIQUID_VOLUME_CYLINDER";

		// Token: 0x040008A8 RID: 2216
		private const string SHADER_KEYWORD_IRREGULAR = "LIQUID_VOLUME_IRREGULAR";

		// Token: 0x040008A9 RID: 2217
		private const string SHADER_KEYWORD_FP_RENDER_TEXTURE = "LIQUID_VOLUME_FP_RENDER_TEXTURES";

		// Token: 0x040008AA RID: 2218
		private const string SHADER_KEYWORD_USE_REFRACTION = "LIQUID_VOLUME_USE_REFRACTION";

		// Token: 0x040008AB RID: 2219
		private const string SPILL_POINT_GIZMO = "SpillPointGizmo";

		// Token: 0x040008AC RID: 2220
		[NonSerialized]
		public Material liqMat;

		// Token: 0x040008AD RID: 2221
		private Material liqMatSimple;

		// Token: 0x040008AE RID: 2222
		private Material liqMatDefaultNoFlask;

		// Token: 0x040008AF RID: 2223
		private Mesh mesh;

		// Token: 0x040008B0 RID: 2224
		[NonSerialized]
		public Renderer mr;

		// Token: 0x040008B1 RID: 2225
		private static readonly List<Material> mrSharedMaterials = new List<Material>();

		// Token: 0x040008B2 RID: 2226
		private Vector3 lastPosition;

		// Token: 0x040008B3 RID: 2227
		private Vector3 lastScale;

		// Token: 0x040008B4 RID: 2228
		private Quaternion lastRotation;

		// Token: 0x040008B5 RID: 2229
		private string[] shaderKeywords;

		// Token: 0x040008B6 RID: 2230
		private bool camInside;

		// Token: 0x040008B7 RID: 2231
		private float lastDistanceToCam;

		// Token: 0x040008B8 RID: 2232
		private DETAIL currentDetail;

		// Token: 0x040008B9 RID: 2233
		private Vector4 turb;

		// Token: 0x040008BA RID: 2234
		private Vector4 shaderTurb;

		// Token: 0x040008BB RID: 2235
		private float turbulenceSpeed;

		// Token: 0x040008BC RID: 2236
		private float murkinessSpeed;

		// Token: 0x040008BD RID: 2237
		private float liquidLevelPos;

		// Token: 0x040008BE RID: 2238
		private bool shouldUpdateMaterialProperties;

		// Token: 0x040008BF RID: 2239
		private int currentNoiseVariation;

		// Token: 0x040008C0 RID: 2240
		private float levelMultipled;

		// Token: 0x040008C1 RID: 2241
		private Texture2D noise3DUnwrapped;

		// Token: 0x040008C2 RID: 2242
		private Texture3D[] noise3DTex;

		// Token: 0x040008C3 RID: 2243
		private Color[][] colors3D;

		// Token: 0x040008C4 RID: 2244
		private Vector3[] verticesUnsorted;

		// Token: 0x040008C5 RID: 2245
		private Vector3[] verticesSorted;

		// Token: 0x040008C6 RID: 2246
		private static Vector3[] rotatedVertices;

		// Token: 0x040008C7 RID: 2247
		private int[] verticesIndices;

		// Token: 0x040008C8 RID: 2248
		private float volumeRef;

		// Token: 0x040008C9 RID: 2249
		private float lastLevelVolumeRef;

		// Token: 0x040008CA RID: 2250
		private Vector3 inertia;

		// Token: 0x040008CB RID: 2251
		private Vector3 lastAvgVelocity;

		// Token: 0x040008CC RID: 2252
		private float angularVelocity;

		// Token: 0x040008CD RID: 2253
		private float angularInertia;

		// Token: 0x040008CE RID: 2254
		private float turbulenceDueForces;

		// Token: 0x040008CF RID: 2255
		private Quaternion liquidRot;

		// Token: 0x040008D0 RID: 2256
		private float prevThickness;

		// Token: 0x040008D1 RID: 2257
		private GameObject spillPointGizmo;

		// Token: 0x040008D2 RID: 2258
		private static string[] defaultContainerNames = new string[]
		{
			"GLASS",
			"CONTAINER",
			"BOTTLE",
			"POTION",
			"FLASK",
			"LIQUID"
		};

		// Token: 0x040008D3 RID: 2259
		private Color[] pointLightColorBuffer;

		// Token: 0x040008D4 RID: 2260
		private Vector4[] pointLightPositionBuffer;

		// Token: 0x040008D5 RID: 2261
		private int lastPointLightCount;

		// Token: 0x040008D6 RID: 2262
		private static readonly Dictionary<Mesh, LiquidVolume.MeshCache> meshCache = new Dictionary<Mesh, LiquidVolume.MeshCache>();

		// Token: 0x040008D7 RID: 2263
		private readonly List<Vector3> verts = new List<Vector3>();

		// Token: 0x040008D8 RID: 2264
		private readonly List<Vector3> cutPoints = new List<Vector3>();

		// Token: 0x040008D9 RID: 2265
		private Vector3 cutPlaneCenter;

		// Token: 0x040008DA RID: 2266
		[SerializeField]
		private Mesh fixedMesh;

		// Token: 0x0200018A RID: 394
		private struct MeshCache
		{
			// Token: 0x040008DB RID: 2267
			public Vector3[] verticesSorted;

			// Token: 0x040008DC RID: 2268
			public Vector3[] verticesUnsorted;

			// Token: 0x040008DD RID: 2269
			public int[] indices;
		}

		// Token: 0x0200018B RID: 395
		// (Invoke) Token: 0x0600082E RID: 2094
		private delegate float MeshVolumeCalcFunction(float level01, float yExtent);

		// Token: 0x0200018C RID: 396
		private static class ShaderParams
		{
			// Token: 0x040008DE RID: 2270
			public static int PointLightInsideAtten = Shader.PropertyToID("_PointLightInsideAtten");

			// Token: 0x040008DF RID: 2271
			public static int PointLightColorArray = Shader.PropertyToID("_PointLightColor");

			// Token: 0x040008E0 RID: 2272
			public static int PointLightPositionArray = Shader.PropertyToID("_PointLightPosition");

			// Token: 0x040008E1 RID: 2273
			public static int PointLightCount = Shader.PropertyToID("_PointLightCount");

			// Token: 0x040008E2 RID: 2274
			public static int GlossinessInt = Shader.PropertyToID("_GlossinessInternal");

			// Token: 0x040008E3 RID: 2275
			public static int DoubleSidedBias = Shader.PropertyToID("_DoubleSidedBias");

			// Token: 0x040008E4 RID: 2276
			public static int BackDepthBias = Shader.PropertyToID("_BackDepthBias");

			// Token: 0x040008E5 RID: 2277
			public static int Muddy = Shader.PropertyToID("_Muddy");

			// Token: 0x040008E6 RID: 2278
			public static int Alpha = Shader.PropertyToID("_Alpha");

			// Token: 0x040008E7 RID: 2279
			public static int AlphaCombined = Shader.PropertyToID("_AlphaCombined");

			// Token: 0x040008E8 RID: 2280
			public static int SparklingIntensity = Shader.PropertyToID("_SparklingIntensity");

			// Token: 0x040008E9 RID: 2281
			public static int SparklingThreshold = Shader.PropertyToID("_SparklingThreshold");

			// Token: 0x040008EA RID: 2282
			public static int DepthAtten = Shader.PropertyToID("_DeepAtten");

			// Token: 0x040008EB RID: 2283
			public static int SmokeColor = Shader.PropertyToID("_SmokeColor");

			// Token: 0x040008EC RID: 2284
			public static int SmokeAtten = Shader.PropertyToID("_SmokeAtten");

			// Token: 0x040008ED RID: 2285
			public static int SmokeSpeed = Shader.PropertyToID("_SmokeSpeed");

			// Token: 0x040008EE RID: 2286
			public static int SmokeHeightAtten = Shader.PropertyToID("_SmokeHeightAtten");

			// Token: 0x040008EF RID: 2287
			public static int SmokeRaySteps = Shader.PropertyToID("_SmokeRaySteps");

			// Token: 0x040008F0 RID: 2288
			public static int LiquidRaySteps = Shader.PropertyToID("_LiquidRaySteps");

			// Token: 0x040008F1 RID: 2289
			public static int FlaskBlurIntensity = Shader.PropertyToID("_FlaskBlurIntensity");

			// Token: 0x040008F2 RID: 2290
			public static int FoamColor = Shader.PropertyToID("_FoamColor");

			// Token: 0x040008F3 RID: 2291
			public static int FoamRaySteps = Shader.PropertyToID("_FoamRaySteps");

			// Token: 0x040008F4 RID: 2292
			public static int FoamDensity = Shader.PropertyToID("_FoamDensity");

			// Token: 0x040008F5 RID: 2293
			public static int FoamWeight = Shader.PropertyToID("_FoamWeight");

			// Token: 0x040008F6 RID: 2294
			public static int FoamBottom = Shader.PropertyToID("_FoamBottom");

			// Token: 0x040008F7 RID: 2295
			public static int FoamTurbulence = Shader.PropertyToID("_FoamTurbulence");

			// Token: 0x040008F8 RID: 2296
			public static int RefractTex = Shader.PropertyToID("_RefractTex");

			// Token: 0x040008F9 RID: 2297
			public static int FlaskThickness = Shader.PropertyToID("_FlaskThickness");

			// Token: 0x040008FA RID: 2298
			public static int Size = Shader.PropertyToID("_Size");

			// Token: 0x040008FB RID: 2299
			public static int Scale = Shader.PropertyToID("_Scale");

			// Token: 0x040008FC RID: 2300
			public static int Center = Shader.PropertyToID("_Center");

			// Token: 0x040008FD RID: 2301
			public static int SizeWorld = Shader.PropertyToID("_SizeWorld");

			// Token: 0x040008FE RID: 2302
			public static int DepthAwareOffset = Shader.PropertyToID("_DepthAwareOffset");

			// Token: 0x040008FF RID: 2303
			public static int Turbulence = Shader.PropertyToID("_Turbulence");

			// Token: 0x04000900 RID: 2304
			public static int TurbulenceSpeed = Shader.PropertyToID("_TurbulenceSpeed");

			// Token: 0x04000901 RID: 2305
			public static int MurkinessSpeed = Shader.PropertyToID("_MurkinessSpeed");

			// Token: 0x04000902 RID: 2306
			public static int Color1 = Shader.PropertyToID("_Color1");

			// Token: 0x04000903 RID: 2307
			public static int Color2 = Shader.PropertyToID("_Color2");

			// Token: 0x04000904 RID: 2308
			public static int EmissionColor = Shader.PropertyToID("_EmissionColor");

			// Token: 0x04000905 RID: 2309
			public static int LightColor = Shader.PropertyToID("_LightColor");

			// Token: 0x04000906 RID: 2310
			public static int LightDir = Shader.PropertyToID("_LightDir");

			// Token: 0x04000907 RID: 2311
			public static int LevelPos = Shader.PropertyToID("_LevelPos");

			// Token: 0x04000908 RID: 2312
			public static int UpperLimit = Shader.PropertyToID("_UpperLimit");

			// Token: 0x04000909 RID: 2313
			public static int LowerLimit = Shader.PropertyToID("_LowerLimit");

			// Token: 0x0400090A RID: 2314
			public static int FoamMaxPos = Shader.PropertyToID("_FoamMaxPos");

			// Token: 0x0400090B RID: 2315
			public static int CullMode = Shader.PropertyToID("_CullMode");

			// Token: 0x0400090C RID: 2316
			public static int ZTestMode = Shader.PropertyToID("_ZTestMode");

			// Token: 0x0400090D RID: 2317
			public static int NoiseTex = Shader.PropertyToID("_NoiseTex");

			// Token: 0x0400090E RID: 2318
			public static int NoiseTexUnwrapped = Shader.PropertyToID("_NoiseTexUnwrapped");

			// Token: 0x0400090F RID: 2319
			public static int GlobalRefractionTexture = Shader.PropertyToID("_VLGrabBlurTexture");

			// Token: 0x04000910 RID: 2320
			public static int RotationMatrix = Shader.PropertyToID("_Rot");

			// Token: 0x04000911 RID: 2321
			public static int QueueOffset = Shader.PropertyToID("_QueueOffset");

			// Token: 0x04000912 RID: 2322
			public static int PreserveSpecular = Shader.PropertyToID("_BlendModePreserveSpecular");
		}
	}
}
