using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedPeopleSystem
{
	// Token: 0x02000222 RID: 546
	[CreateAssetMenu(fileName = "NewCharacterSettings", menuName = "Advanced People Pack/Settings", order = 1)]
	public class CharacterSettings : ScriptableObject
	{
		// Token: 0x04000CA5 RID: 3237
		public GameObject OriginalMesh;

		// Token: 0x04000CA6 RID: 3238
		public Material bodyMaterial;

		// Token: 0x04000CA7 RID: 3239
		[Space(20f)]
		public List<CharacterAnimationPreset> characterAnimationPresets = new List<CharacterAnimationPreset>();

		// Token: 0x04000CA8 RID: 3240
		[Space(20f)]
		public List<CharacterBlendshapeData> characterBlendshapeDatas = new List<CharacterBlendshapeData>();

		// Token: 0x04000CA9 RID: 3241
		[Space(20f)]
		public List<CharacterElementsPreset> hairPresets = new List<CharacterElementsPreset>();

		// Token: 0x04000CAA RID: 3242
		public List<CharacterElementsPreset> beardPresets = new List<CharacterElementsPreset>();

		// Token: 0x04000CAB RID: 3243
		public List<CharacterElementsPreset> hatsPresets = new List<CharacterElementsPreset>();

		// Token: 0x04000CAC RID: 3244
		public List<CharacterElementsPreset> accessoryPresets = new List<CharacterElementsPreset>();

		// Token: 0x04000CAD RID: 3245
		public List<CharacterElementsPreset> shirtsPresets = new List<CharacterElementsPreset>();

		// Token: 0x04000CAE RID: 3246
		public List<CharacterElementsPreset> pantsPresets = new List<CharacterElementsPreset>();

		// Token: 0x04000CAF RID: 3247
		public List<CharacterElementsPreset> shoesPresets = new List<CharacterElementsPreset>();

		// Token: 0x04000CB0 RID: 3248
		public List<CharacterElementsPreset> item1Presets = new List<CharacterElementsPreset>();

		// Token: 0x04000CB1 RID: 3249
		[Space(20f)]
		public List<CharacterSettingsSelector> settingsSelectors = new List<CharacterSettingsSelector>();

		// Token: 0x04000CB2 RID: 3250
		[Space(20f)]
		public RuntimeAnimatorController Animator;

		// Token: 0x04000CB3 RID: 3251
		public Avatar Avatar;

		// Token: 0x04000CB4 RID: 3252
		[Space(20f)]
		public CharacterGeneratorSettings generator;

		// Token: 0x04000CB5 RID: 3253
		[Space(20f)]
		public CharacterSelectedElements DefaultSelectedElements = new CharacterSelectedElements();

		// Token: 0x04000CB6 RID: 3254
		[Space(20f)]
		public bool DisableBlendshapeModifier;
	}
}
