using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace XD
{
    public class PlayerSpawnPoints : MonoBehaviour
    {
        [SerializeField] public Transform[] spawns = new Transform[Constants.maxPlayers];

#if UNITY_EDITOR
        public void OnDrawGizmos()
        {
            for (int i = 0; i < spawns.Length; i++)
            {
                Vector3 pos = spawns[i].position;
                Gizmos.color = GetGizmoColor(i);
                DebugExtension.DrawCapsule(pos, pos + Vector3.up * 1.0f, GetGizmoColor(i), 0.2f);
            }
        }

        public Color GetGizmoColor(int index)
        {
            switch (index)
            {
                case 0:
                    return Color.white;
                case 1:
                    return Color.red;
                case 2:
                    return Color.green;
                case 3:
                    return Color.blue;
            }

            return Color.white;
        }
#endif
    }   
}
