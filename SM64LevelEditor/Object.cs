using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using System.Windows.Forms;
using System.ComponentModel;

namespace SM64LevelEditor
{
    public class Object
    {
        const float ROTATION_FACTOR = (float)(2 * Math.PI / ushort.MaxValue);
        [Browsable(false)]
        public Level level { get; private set; }
        [Browsable(false)]
        public Area area { get; private set; }
        [Browsable(false)]
        public Vector3 rotation { get { return new Vector3(rotationX, rotationY, rotationZ); } set { rotationX = (short)value.X; rotationY = (short)value.Y; rotationZ = (short)value.Z; } }
        [Browsable(false)]
        public Vector3 position { get { return new Vector3(positionX, positionY, positionZ); } set { positionX = (short)value.X; positionY = (short)value.Y; positionZ = (short)value.Z; } }
        [Browsable(false)]
        public int pickIndex { get; private set; }
        [Browsable(false)]
        public bool selected { get; internal set; }
        [Browsable(false)]
        public GeoLayout geometry { get; private set; }

        [TypeConverter(typeof(HexTo<int>))]
        public int behaviourScript { get; set; }
        [TypeConverter(typeof(HexTo<int>))]
        public int bParam { get; set; }
        [DefaultValue(0x1F), TypeConverter(typeof(HexTo<byte>))]
        public byte acts { get; set; }
        [TypeConverter(typeof(HexTo<byte>))]
        public byte model_ID { get { return m_ID; } set { SetModelID(value); } }

        public short rotationX { get; set; }
        public short rotationY { get; set; }
        public short rotationZ { get; set; }
        public short positionX { get; set; }
        public short positionY { get; set; }
        public short positionZ { get; set; }
        static int maxPickIndex = 1;

        byte m_ID;

        public Object(Level level, Area area)
        {
            this.level = level;
            this.area = area;
            pickIndex = maxPickIndex++;
        }
        public Object(Level level, Area area, Vector3 position, Vector3 rotation, byte modelID, int behaviour, byte acts, int bParam)
            : this(level, area)
        {
            positionX = (short)position.X; positionY = (short)position.Y; positionZ = (short)position.Z;
            rotationX = (short)rotation.X; rotationY = (short)rotation.Y; rotationZ = (short)rotation.Z;
            SetModelID(modelID);
            this.behaviourScript = behaviour;
            this.acts = acts;
            this.bParam = bParam;
        }

        public void SetModelID(byte ID)
        {
            if (ID == this.m_ID) return;
            this.m_ID = ID;
            GeoLayout preset;
            if (geometry != null) geometry.MakeInvisible(Level.renderer);
            if (Level.modelIDs.TryGetValue((byte)ID, out preset))
            {
                geometry = preset.DeepCopy();
                geometry.SetPickIndex(pickIndex);
                if (area.visible)
                    geometry.MakeVisible(Level.renderer);
            }
        }

        public void MakeVisible()
        {
            if (geometry != null)
                geometry.MakeVisible(Level.renderer);
        }
        public void MakeInvisible()
        {
            if (geometry != null)
                geometry.MakeInvisible(Level.renderer);
        }

        public void Update()
        {
            if (geometry != null)
                geometry.SetTransform(GetTransform());
        }

        public byte[] GetParameterBytes()
        {
            byte[] output = new byte[0x16];
            output[0] = acts;
            output[1] = (byte)m_ID;
            SM64RAM.cvt.writeInt16(output, 0x2, positionX);
            SM64RAM.cvt.writeInt16(output, 0x4, positionY);
            SM64RAM.cvt.writeInt16(output, 0x6, positionZ);
            SM64RAM.cvt.writeInt16(output, 0x8, rotationX);
            SM64RAM.cvt.writeInt16(output, 0xA, rotationY);
            SM64RAM.cvt.writeInt16(output, 0xC, rotationZ);
            SM64RAM.cvt.writeInt32(output, 0xE, bParam);
            SM64RAM.cvt.writeInt32(output, 0x12, behaviourScript);
            return output;
        }

        public Vector4 GetBoundingBoxColor() { return selected ? new Vector4(1, 1, 0, 1) : new Vector4(1, 0, 0, 1); }

        public virtual Matrix GetTransform() { return Matrix.RotationYawPitchRoll(rotationY * ROTATION_FACTOR, rotationX * ROTATION_FACTOR, rotationZ * ROTATION_FACTOR) * Matrix.Translation(positionX, positionY, positionZ); }
    }
}
