#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace XD
{
    public class LevelExport : MonoBehaviour
    {
        public GameObject originalTree;
        public GameObject originalMine;

        public LevelData level = new LevelData();
        public string exportPath;

        [System.Serializable]
        public class RotationData
        {
            public RotationData(){}
            public RotationData(Quaternion quat)
            {
                x = quat.x;
                y = quat.y;
                z = quat.z;
                w = quat.w;
            }
            public float x, y, z, w;
        }

        [System.Serializable]
        public class TreeData
        {
            public Vector3 position;
            public RotationData rotation;
            public Vector3 scale;
            public int index;
        }

        [System.Serializable]
        public class MineData
        {
            public Vector3 position;
            public RotationData rotation;
            public Vector3 scale;
            public int index;
        }

        [System.Serializable]
        public class CamData
        {
            public Vector3 position;
            public RotationData rotation;
            public float fov;
        }

        [System.Serializable]
        public class LevelData
        {
            public CamData cam;
            public TreeData[] trees;
            public MineData[] mines;
        }

        [ContextMenu("Check Vectors")]
        public void CheckVectors()
        {
            Debug.Log(transform.forward);
            Debug.Log(transform.up);
            Debug.Log(transform.rotation);
            Debug.Log(transform.position);
            Matrix4x4 mat = Matrix4x4.TRS(Vector3.zero, transform.rotation, Vector3.one);
        }

        [ContextMenu("Export")]
        public void Export()
        {
            CamData cam = new CamData
            {
                position = Camera.main.transform.position,
                rotation = new RotationData(Camera.main.transform.rotation),
                fov = Camera.main.fieldOfView
            };

            int mineCount = 0;
            int treeCount = 0;
            GathereableResource[] resources = FindObjectsOfType<GathereableResource>();
            for (int i = 0; i < resources.Length; i++)
            {
                if (resources[i].type == RESOURCE_TYPE.WOOD) treeCount++;
                if (resources[i].type == RESOURCE_TYPE.STONE) mineCount++;
            }

            TreeData[] trees = new TreeData[treeCount];
            MineData[] mines = new MineData[mineCount];
            int nextTreeIndex = 0;
            int nextMineIndex = 0;
            for (int i = 0; i < resources.Length; i++)
            {
                if (resources[i] == null) continue;
                if (resources[i].type == RESOURCE_TYPE.WOOD)
                {
                    trees[nextTreeIndex] = new TreeData
                    {
                        position = resources[i].transform.position,
                        rotation = new RotationData(resources[i].transform.rotation),
                        scale = resources[i].transform.localScale,
                        index = (resources[i].name.Contains("variant")) ? 1 : 0,
                    };
                    nextTreeIndex++;
                }
                if (resources[i].type == RESOURCE_TYPE.STONE)
                {
                    mines[nextMineIndex] = new MineData
                    {
                        position = resources[i].transform.position,
                        rotation = new RotationData(resources[i].transform.rotation),
                        scale = resources[i].transform.localScale,
                        index = (resources[i].name.Contains("variant")) ? 1 : 0,
                    };
                    nextMineIndex++;
                }
            }

            level.cam = cam;
            level.mines = mines;
            level.trees = trees;

            XmlSerializer serializer = new XmlSerializer(typeof(LevelData));
            if(File.Exists(exportPath)) File.Delete(exportPath);
            FileStream stream = new FileStream(exportPath, FileMode.CreateNew);
            serializer.Serialize(stream, level);
        }
    }   
}
#endif
