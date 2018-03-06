using System;
using System.Collections.Generic;
using System.Text;
using SM64RAM;

namespace SM64_model_importer
{
    public class Area
    {
        public enum ObjectStorageType
        {
            LevelScript,
            Macro,
            Special
        }

        public void AddObject(Object obj, ObjectStorageType storageType) { }
    }
    public class Level
    {
    }
    public class Object
    {
        Vector3 position, rotation;
        int behaviour, modelID, bParams;
        byte acts;

        public Object(Vector3 pos, Vector3 rot, int behav, int mID, byte acts, int bParams)
        {
            this.position = pos; this.rotation = rot; this.behaviour = behav; this.modelID = mID;
            this.acts = acts; this.bParams = bParams;
        }
    }
    public class WaterBox
    {
        public int ID;
        public Vector3 lo, high;
    }
}
