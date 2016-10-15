using UnityEngine;
using System.Collections;

namespace Amigosi.Data
{
    public class BundleLoader : MonoBehaviour
    {
        private AssetBundle assetBundle;
        public BundleLoadClient klient;

        public void SetAssetBundle(AssetBundle bundle)
        {
            assetBundle = bundle;
        }
        public AssetBundle GetAssetBundle()
        {
            return assetBundle;
        }
        public BundleLoader(string path)
        {
            assetBundle = AssetBundle.CreateFromFile(path);
            if (assetBundle == null)
                throw new System.Exception();
        }
        public string[] AssetsInBundle()
        {
            if (assetBundle == null)
                return null;
            return assetBundle.GetAllAssetNames();
        }
        //Svi iskorisceni objekti nece biti obrisani
        public void UnloadAssets()
        {
            assetBundle.Unload(false);
        }
    }

    //Sadrzi metode za implimentaciju ucitavanja asseta iz bundla

    public interface BundleLoadClient
    {
        GameObject[] LoadAssetFromBandle(AssetBundle bundle);
    }
}


