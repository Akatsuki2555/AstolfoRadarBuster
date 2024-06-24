using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSCLoader;
using UnityEngine;

namespace AstolfoBeanPlushie
{
    public class AstolfoBeanPlushie : Mod
    {
        public override string ID => "AstolfoBeanPlushie";

        public override string Version => "0.1";

        public override string Author => "アカツキ";

        public override void ModSetup()
        {
            base.ModSetup();

            SetupFunction(Setup.OnLoad, Mod_Load);
        }

        private void Mod_Load()
        {
            var assetBundle = AssetBundle.CreateFromMemoryImmediate(Resource1.astolfoplushie);

            var beanPlushie = assetBundle.LoadAsset<GameObject>("Astolfo");
            var instance = GameObject.Instantiate(beanPlushie);
            instance.AddComponent<Rigidbody>();
            instance.MakePickable();
            instance.name = "Astolfo Plushie(Clone)";
            var player = GameObject.Find("PLAYER");
            instance.transform.position = player.transform.position;

            assetBundle.Unload(false);
        }
    }
}
