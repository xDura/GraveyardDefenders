using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

namespace XD
{

    [System.Serializable] public class ResourceSpritesDictionary : SerializableDictionaryBase<RESOURCE_TYPE, Sprite>{ };

    [CreateAssetMenu(menuName = "XD/Databases/ResourceSpritesDB")]
    public class ResourceIconsDatabase : ScriptableObject
    {
        public ResourceSpritesDictionary dic;

        public Sprite GetSprite(RESOURCE_TYPE resource)
        {
            if (dic.TryGetValue(resource, out Sprite sp)) return sp;
            else return null;
        }
    }   
}
