using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;
using UnityEngine;
using Il2CppInterop.Runtime.Attributes;

namespace MixerThreholdMod_0_0_1
{
    [RegisterTypeInIl2Cpp]
    public class PreferencesUI : Il2CppUnityEngine.MonoBehaviour
    {
        public PreferencesUI(IntPtr ptr) : base(ptr) { }

        public void Start()
        {
            // This class helps expose your mod's preferences to MelonPreferencesManager
        }
    }
}