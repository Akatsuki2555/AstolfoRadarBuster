using System;
using System.Reflection;
using JetBrains.Annotations;
using MSCLoader;
using UnityEngine;

namespace AstolfoRadarBuster
{
    [UsedImplicitly]
    public class AstolfoRadarBuster : Mod
    {
        public override string ID => "AstolfoRadarBuster";

        public override string Version => "0.1";

        public override string Author => "アカツキ";

        public override void ModSetup()
        {
            base.ModSetup();

            SetupFunction(Setup.PostLoad, Mod_Load);
        }

        private void Mod_Load()
        {
            var radarBusterAudio = GameObject.Find("MasterAudio").transform
                .Find("CarFoley/radar_buster").GetComponent<AudioSource>();
            var radarBuster = GameObject.Find("radar buster(Clone)");
            if (radarBuster == null)
            {
                radarBuster = GameObject.Find("SATSUMA(557kg, 248)")
                    .transform.Find("Dashboard/pivot_dashboard/dashboard(Clone)/pivot_radar_buster/radar buster(Clone)")
                    .gameObject;
            }

            byte[] numArray;
            using (var manifestResourceStream = Assembly.GetExecutingAssembly()
                       .GetManifestResourceStream("AstolfoRadarBuster.Resources.astolforadar.unity3d"))
            {
                if (manifestResourceStream == null)
                    throw new Exception("The mod DLL is corrupted, unable to load astolforadar.unity3d. Cannot continue");
                numArray = new byte[manifestResourceStream.Length];
                _ = manifestResourceStream.Read(numArray, 0, numArray.Length);
            }

            var assetBundle = numArray.Length != 0
                ? AssetBundle.CreateFromMemoryImmediate(numArray)
                : throw new Exception("The mod DLL is corrupted, unable to load astolforadar.unity3d. Cannot continue");

            var radarBusterAstolfo = assetBundle.LoadAsset<GameObject>("Astolfo");
            var radarBusterAudioClip = assetBundle.LoadAsset<AudioClip>("yahoo");

            var radarBusterAstolfoInstance = GameObject.Instantiate(radarBusterAstolfo);
            radarBusterAstolfoInstance.transform.parent = radarBuster.transform;
            radarBusterAstolfoInstance.transform.localPosition = new Vector3(0, 0.015f, -0.019f);
            radarBusterAstolfoInstance.transform.localRotation = Quaternion.Euler(300, 285, 225);
            radarBusterAstolfoInstance.transform.localScale = new Vector3(0.025f, 0.025f, 0.025f);
            
            radarBuster.GetComponent<MeshRenderer>().enabled = false;
            
            var radarBusterLogik = radarBuster.transform.Find("Logic");
            radarBusterLogik.localPosition = new Vector3(0.008f, 0.001f, 0.015f);
            
            radarBusterAudio.clip = radarBusterAudioClip;
            radarBusterAudio.Play();

            assetBundle.Unload(false);
        }
    }
}
