#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace XD
{
    public class LevelExport : MonoBehaviour
    {
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
        public class WallData
        {
            public Vector3 position;
            public RotationData rotation;
            public int index;
        }

        [System.Serializable]
        public class BreakableWallData
        {
            public Vector3 position;
            public RotationData rotation;
            public int index;
        }

        [System.Serializable]
        public class DoorData
        {
            public Vector3 position;
            public RotationData rotation;
            public int index;
        }

        [System.Serializable]
        public class LevelData
        {
            public CamData cam;
            public TreeData[] trees;
            public MineData[] mines;
            public WallData[] walls;
            public BreakableWallData[] breakableWalls;
            public DoorData[] doors;
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

            #region TREES_&_MINES
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
            #endregion

            #region WALLS & DOORS
            int numDoors;
            PlayerDoor[] doors = FindObjectsOfType<PlayerDoor>();
            numDoors = doors.Length;
            DoorData[] doorDatas = new DoorData[numDoors];
            for (int i = 0; i < doors.Length; i++)
            {
                doorDatas[i] = new DoorData
                {
                    index = 0,
                    position = doors[i].transform.position,
                    rotation = new RotationData(doors[i].transform.rotation)
                };
            }

            BreakableObject[] breakables = FindObjectsOfType<BreakableObject>();
            List<BreakableWallData> breakableWallDatas = new List<BreakableWallData>();
            for (int i = 0; i < breakables.Length; i++)
            {
                if (breakables[i].name.Contains("Fence"))
                    breakableWallDatas.Add(new BreakableWallData {
                                            position = breakables[i].transform.position,
                                            rotation = new RotationData(breakables[i].transform.rotation),
                                            index = 0,
                                            });
            }


            GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
            WallData[] wallDatas = new WallData[walls.Length];
            for (int i = 0; i < wallDatas.Length; i++)
            {
                wallDatas[i] = new WallData
                {
                    position = walls[i].transform.position,
                    rotation = new RotationData(walls[i].transform.rotation),
                    index = 0
                };
            }

            #endregion

            level.cam = cam;
            level.mines = mines;
            level.trees = trees;
            level.doors = doorDatas;
            level.walls = wallDatas;
            level.breakableWalls = breakableWallDatas.ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(LevelData));
            if(File.Exists(exportPath)) File.Delete(exportPath);
            FileStream stream = new FileStream(exportPath, FileMode.CreateNew);
            serializer.Serialize(stream, level);
        }
    }   
}
#endif
