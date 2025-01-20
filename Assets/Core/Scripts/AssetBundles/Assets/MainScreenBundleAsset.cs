using UnityEngine;

namespace Core.Scripts.AssetBundles
{
    [CreateAssetMenu(menuName = "SO/BundleAssets/MainScreenAsset")]
    public class MainScreenBundleAsset : ScriptableObject
    {
        [field: SerializeField] public Sprite IncreaseCounterButton { get; private set; }
    }
}