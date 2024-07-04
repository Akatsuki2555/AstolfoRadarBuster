using System;
using System.Reflection;
using MSCLoader;
using UnityEngine;

namespace AstolfoRadarBuster
{
    public class AstolfoRadarBuster : Mod
    {
        public override string ID => "AstolfoRadarBuster";

        public override string Version => "0.1";

        public override string Author => "アカツキ";

        public override void ModSetup()
        {
            base.ModSetup();

            SetupFunction(Setup.OnLoad, Mod_Load);
            SetupFunction(Setup.OnSave, Mod_OnSave);
        }

        private GameObject _plushie;

        private void Mod_Load()
        {
            var radarBusterAudio = GameObject.Find("MasterAudio").transform
                .Find("CarFoley/radar_buster").gameObject;
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

            var radarBusterModel = assetBundle.LoadAsset<GameObject>("bean2");
            var radarBusterTexture = assetBundle.LoadAsset<Texture2D>("bean");
            var radarBusterAudioClip = assetBundle.LoadAsset<AudioClip>("yahoo");
            
            radarBuster.GetComponent<MeshFilter>().mesh = radarBusterModel.GetComponent<MeshFilter>().sharedMesh;
            radarBuster.GetComponent<MeshRenderer>().material.mainTexture = radarBusterTexture;
            radarBusterAudio.GetComponent<AudioSource>().clip = radarBusterAudioClip;

            assetBundle.Unload(false);
        }

        private void Mod_OnSave()
        {
            SaveLoad.WriteValue(this, "plushiePos", _plushie.transform.position);
            SaveLoad.WriteValue(this, "plushieRot", _plushie.transform.rotation);
        }
    }
}
