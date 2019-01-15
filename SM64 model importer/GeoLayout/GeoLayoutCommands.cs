using System;
using System.Collections.Generic;
using System.Text;
using SM64RAM;

namespace SM64ModelImporter.GeoLayoutCommands
{
    public class Command
    {
        public byte command;
        public int length = 4;
        public static Dictionary<byte, Type> commands = new Dictionary<byte, Type>();
        public static Dictionary<Type, Type> commandControls = new Dictionary<Type, Type>();
        byte[] originalSequence;

        static Command()
        {
            commands.Add(2, typeof(Jump));
            commands.Add(8, typeof(ViewPort));
            commands.Add(9, typeof(Render));
            commands.Add(0xA, typeof(Projection));
            commands.Add(0xC, typeof(ZWriteEnable));
            commands.Add(0xD, typeof(Unknown));
            commands.Add(0xE, typeof(Unknown));
            commands.Add(0xF, typeof(Camera));
            commands.Add(0x13, typeof(TransformedDisplayList));
            commands.Add(0x14, typeof(BillBoard));
            commands.Add(0x15, typeof(DisplayList));
            commands.Add(0x16, typeof(Shadow));
            commands.Add(0x18, typeof(EnvironmentEffect));
            commands.Add(0x19, typeof(Background));
            commands.Add(0x1D, typeof(ScalePolygons));
            commands.Add(0x20, typeof(DrawingDistance));

        }
        public static Command FromStream(byte[] stream, ref int cursor)
        {
            byte c = stream[cursor];
            Type type;
            if (!commands.TryGetValue(c, out type))
                type = typeof(Command);
            Command output = (Command)type.GetConstructor(new Type[] { }).Invoke(new object[] { });
            output.command = c;
            output.SetValues(stream, cursor);
            cursor += output.length;
            return output;
        }

        public Command() { }
        public Command(byte command) { this.command = command; }

        protected virtual void SetValues(byte[] source, int cursor) { }

        public virtual void write(byte[] stream, ref int cursor)
        {
            stream[cursor] = command;
            cursor += length;
        }

        public void GetByteSource(StringBuilder appendTo)
        {
            if (originalSequence == null)
                appendTo.Append("<No Sequence>");
            else
                for (int i = 0; i < originalSequence.Length; i++)
                    appendTo.Append(originalSequence[i].ToString("X") + " ");
        }
        public override string ToString()
        {
            switch (command)
            {
                case 4: return "Child";
                case 0x17: return "Mario";
                default: return GetType().Name + " 0x" + command.ToString("X");
            }
        }
    }

    public class Unknown : Command
    {
        public Unknown() { length = 8; }
        protected override void SetValues(byte[] source, int cursor)
        {
            if (command == 0x10) length = 0x10;
            if (command == 0x1C) length = 0xC;
        }
    }

    public class Jump : Command
    {
        public uint value;
        public Jump() { length = 8; }
        protected override void SetValues(byte[] source, int cursor)
        {
            value = cvt.uint32(source, cursor + 4);
        }
    }

    public class ViewPort : Command
    {
        public short centerX, centerY, halfWidth, halfHeight;
        public short weirdValue = 0x64;
        public ViewPort() { length = 0xC; }
        public ViewPort(short centerX, short centerY, short halfWidth, short halfHeight)
        {
            this.centerX = centerX; this.centerY = centerY; this.halfWidth = halfWidth; this.halfHeight = halfHeight;
        }
        protected override void SetValues(byte[] source, int cursor)
        {
            weirdValue = cvt.int16(source, cursor + 2);
            centerX = cvt.int16(source, cursor + 4);
            centerY = cvt.int16(source, cursor + 6);
            halfWidth = cvt.int16(source, cursor + 8);
            halfHeight = cvt.int16(source, cursor + 10);
        }

        public override void write(byte[] stream, ref int cursor)
        {
            cvt.writeInt16(stream, cursor + 2, weirdValue);
            cvt.writeInt16(stream, cursor + 4, centerX);
            cvt.writeInt16(stream, cursor + 6, centerY);
            cvt.writeInt16(stream, cursor + 8, halfWidth);
            cvt.writeInt16(stream, cursor + 10, halfHeight);
            base.write(stream, ref cursor);
        }
    }

    public class Render : Command
    {
        public short weirdValue = 0x64;
        public byte rendererIndex = 0x4;
        public Render() { length = 8; }
        protected override void SetValues(byte[] source, int cursor)
        {
            weirdValue = cvt.int16(source, cursor + 2);
            rendererIndex = source[cursor + 4];
        }

        public override void write(byte[] stream, ref int cursor)
        {
            cvt.writeInt16(stream, cursor + 2, weirdValue);
            stream[cursor + 4] = rendererIndex;
            base.write(stream, ref cursor);
        }
    }

    public class Projection : Command
    {
        public short nearClip = 0x64, farClip = 0x7530;
        public short weirdValue = 0x2D;
        public uint updateFunction = 0;
        public Projection()
        {
            length = 8;
            this.updateFunction = 0;
        }
        protected override void SetValues(byte[] source, int cursor)
        {
            weirdValue = cvt.int16(source, cursor + 2);
            nearClip = cvt.int16(source, cursor + 4);
            farClip = cvt.int16(source, cursor + 6);
            if (source[cursor + 1] > 0)
            {
                length = 0xC;
                updateFunction = cvt.uint32(source, cursor + 8);
            }
        }

        public override void write(byte[] stream, ref int cursor)
        {
            stream[cursor + 1] = (byte)(updateFunction != 0 ? 1 : 0);
            cvt.writeInt16(stream, cursor + 2, weirdValue);
            cvt.writeInt16(stream, cursor + 4, nearClip);
            cvt.writeInt16(stream, cursor + 6, farClip);
            if (updateFunction != 0) cvt.writeInt32(stream, cursor + 8, (int)updateFunction);
            base.write(stream, ref cursor);
        }
    }

    public class ZWriteEnable : Command
    {
        public bool value;
        protected override void SetValues(byte[] source, int cursor)
        {
            value = source[cursor + 1] != 0;
        }
        public override void write(byte[] stream, ref int cursor)
        {
            stream[cursor + 1] = (byte)(value ? 1 : 0);
            base.write(stream, ref cursor);
        }
    }

    public class Camera : Command
    {
        public byte preset = 0x1;
        public short someX, someY, someZ;
        public short focusX, focusY, focusZ;
        public uint updateFunction = 0;
        public Camera() { length = 0x14; }
        protected override void SetValues(byte[] source, int cursor)
        {
            preset = source[cursor + 3];
            someX = cvt.int16(source, cursor + 4);
            someY = cvt.int16(source, cursor + 6);
            someZ = cvt.int16(source, cursor + 8);
            focusX = cvt.int16(source, cursor + 10);
            focusY = cvt.int16(source, cursor + 12);
            focusZ = cvt.int16(source, cursor + 14);
            updateFunction = cvt.uint32(source, cursor + 16);
        }

        public override void write(byte[] stream, ref int cursor)
        {
            stream[cursor + 3] = preset;
            cvt.writeInt16(stream, cursor + 4, someX);
            cvt.writeInt16(stream, cursor + 6, someY);
            cvt.writeInt16(stream, cursor + 8, someZ);
            cvt.writeInt16(stream, cursor + 10, focusX);
            cvt.writeInt16(stream, cursor + 12, focusY);
            cvt.writeInt16(stream, cursor + 14, focusZ);
            cvt.writeInt32(stream, cursor + 16, (int)updateFunction);
            base.write(stream, ref cursor);
        }
    }

    public class TransformedDisplayList : DisplayList
    {
        public short posX, posY, posZ;
        public TransformedDisplayList() { length = 0xC; }
        protected override void SetValues(byte[] source, int cursor)
        {
            layer = source[cursor + 1];
            posX = cvt.int16(source, cursor + 2);
            posY = cvt.int16(source, cursor + 4);
            posZ = cvt.int16(source, cursor + 6);
            displayList = cvt.int32(source, cursor + 8);
        }

        public override void write(byte[] stream, ref int cursor)
        {
            stream[cursor + 1] = layer;
            cvt.writeInt16(stream, cursor + 2, posX);
            cvt.writeInt16(stream, cursor + 4, posY);
            cvt.writeInt16(stream, cursor + 6, posZ);
            cvt.writeInt32(stream, cursor + 8, (int)displayList);
            base.write(stream, ref cursor);
        }
    }

    public class BillBoard : Command
    {
        public short offsetX, offsetY, offsetZ;
        public byte weirdByte = 0;
        public static bool SetBillBoard;
        public BillBoard() { length = 8; }
        protected override void SetValues(byte[] source, int cursor)
        {
            weirdByte = source[cursor + 1];
            offsetX = cvt.int16(source, cursor + 2);
            offsetY = cvt.int16(source, cursor + 4);
            offsetZ = cvt.int16(source, cursor + 6);
        }
        public override void write(byte[] stream, ref int cursor)
        {
            stream[cursor + 1] = weirdByte;
            cvt.writeInt16(stream, cursor + 2, offsetX);
            cvt.writeInt16(stream, cursor + 4, offsetY);
            cvt.writeInt16(stream, cursor + 6, offsetZ);
            base.write(stream, ref cursor);
        }
    }

    public class DisplayList : Command
    {
        public byte layer = 1;
        public int displayList;
        public DisplayList() { length = 0x8; }
        protected override void SetValues(byte[] source, int cursor)
        {
            layer = source[cursor + 1];
            displayList = cvt.int32(source, cursor + 4);
        }

        public override void write(byte[] stream, ref int cursor)
        {
            stream[cursor + 1] = layer;
            if (displayList != null)
                cvt.writeInt32(stream, cursor + 4, displayList);
            else
                cvt.writeInt32(stream, cursor + 4, 0);
            base.write(stream, ref cursor);
        }
    }

    public class Shadow : Command
    {
        public byte shape, transparency;
        public short size;
        public Shadow() { length = 8; }
        protected override void SetValues(byte[] source, int cursor)
        {
            shape = source[cursor + 3];
            transparency = source[cursor + 5];
            size = cvt.int16(source, cursor + 6);
        }
        public override void write(byte[] stream, ref int cursor)
        {
            stream[cursor + 3] = shape;
            stream[cursor + 5] = transparency;
            cvt.writeInt16(stream, cursor + 6, size);
            base.write(stream, ref cursor);
        }
    }

    public class EnvironmentEffect : Command
    {
        public byte effectID;
        public uint renderFunction; //?
        public EnvironmentEffect() { length = 8; }
        protected override void SetValues(byte[] source, int cursor)
        {
            effectID = source[cursor + 3];
            renderFunction = cvt.uint32(source, cursor + 4);
        }
        public override void write(byte[] stream, ref int cursor)
        {
            stream[cursor + 3] = effectID;
            cvt.writeInt32(stream, cursor + 4, (int)renderFunction);
            base.write(stream, ref cursor);
        }
    }

    public class Background : Command
    {
        public short destroy = 0;
        public uint ClearFunction = 0x802763D4;
        public Background() { length = 8; }
        protected override void SetValues(byte[] source, int cursor)
        {
            destroy = cvt.int16(source, cursor + 2);
            ClearFunction = cvt.uint32(source, cursor + 4);
        }
        public override void write(byte[] stream, ref int cursor)
        {
            cvt.writeInt16(stream, cursor + 2, destroy);
            cvt.writeInt32(stream, cursor + 4, (int)ClearFunction);
            base.write(stream, ref cursor);
        }
    }

    public class ScalePolygons : Command
    {
        public int scale = 0x10000;
        public ScalePolygons() { length = 8; }
        protected override void SetValues(byte[] source, int cursor)
        {
            scale = cvt.int32(source, cursor + 4);
        }
        public override void write(byte[] stream, ref int cursor)
        {
            cvt.writeInt32(stream, cursor + 4, scale);
            base.write(stream, ref cursor);
        }
    }

    public class DrawingDistance : Command
    {
        public short value = 4000;
        protected override void SetValues(byte[] source, int cursor)
        {
            value = cvt.int16(source, cursor + 2);
        }
        public override void write(byte[] stream, ref int cursor)
        {
            cvt.writeInt16(stream, cursor + 2, value);
            base.write(stream, ref cursor);
        }
    }
}
