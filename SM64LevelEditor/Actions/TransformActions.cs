using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using System.Windows.Forms;

namespace SM64LevelEditor
{
    public abstract class TransformAction : Action
    {
        public byte actionAxis = 0xFF;
        public float speedMultiplier = 1;

        public override void Update()
        {
            bool not = (Main.keyDown[(int)Keys.ShiftKey]);
            if (Main.keyDown[(int)Keys.X]) actionAxis = (byte)((not ? 0xFF : 0) ^ 0x1);
            if (Main.keyDown[(int)Keys.Y]) actionAxis = (byte)((not ? 0xFF : 0) ^ 0x2);
            if (Main.keyDown[(int)Keys.Z]) actionAxis = (byte)((not ? 0xFF : 0) ^ 0x4);
            speedMultiplier = (Main.keyDown[(int)Keys.ShiftKey]) ? 0.1f : 1;
        }
    }

    [ActionKey(false, Keys.G)]
    public class TranslateAction : TransformAction
    {
        public Vector3 initialPosition { get; private set; }
        private Vector3[] objectOffsets;
        public Vector3 displacement;

        public TranslateAction(Vector3 displacement)
            : this()
        {
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].position = initialPosition + objectOffsets[i] + displacement;
                objects[i].Update();
            }
        }


        public TranslateAction()
        {
            objectOffsets = new Vector3[objects.Length];
            initialPosition = Vector3.Empty;
            for (int i = 0; i < objects.Length; i++)
                initialPosition += objects[i].position;
            initialPosition = initialPosition * (1f / objects.Length);
            for (int i = 0; i < objects.Length; i++)
                objectOffsets[i] = objects[i].position - initialPosition;
        }
        public override void Undo()
        {
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].position = initialPosition + objectOffsets[i];
                objects[i].Update();
            }
        }

        public override void Update()
        {
            base.Update();
            displacement += (Editor.camera.localX * (-Main.mouseDelta.X * Editor.mouseSensitivity) + Editor.camera.localY * (Main.mouseDelta.Y * Editor.mouseSensitivity)) * speedMultiplier;
            Vector3 limitedDisplacement = displacement;
            if ((actionAxis & 1) == 0) limitedDisplacement.X = 0;
            if ((actionAxis & 2) == 0) limitedDisplacement.Y = 0;
            if ((actionAxis & 4) == 0) limitedDisplacement.Z = 0;
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].position = initialPosition + objectOffsets[i] + limitedDisplacement;
                objects[i].Update();
            }
        }
    }

    [ActionKey(false, Keys.G, Keys.G)]
    public class CarryAction : TransformAction
    {
        public Vector3 initialPosition { get; private set; }
        private Vector3[] objectOffsets;
        public Vector3 displacement;

        public CarryAction()
        {
            objectOffsets = new Vector3[objects.Length];
            initialPosition = Vector3.Empty;
            for (int i = 0; i < objects.Length; i++)
                initialPosition += objects[i].position;
            initialPosition = initialPosition * (1f / objects.Length);
            for (int i = 0; i < objects.Length; i++)
                objectOffsets[i] = objects[i].position - initialPosition;
        }
        public override void Undo()
        {
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].position = initialPosition + objectOffsets[i];
                objects[i].Update();
            }
        }

        public override void Update()
        {
            base.Update();
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].position = Editor.camera.cursor + objectOffsets[i];
                objects[i].Update();
            }
        }
    }

    [ActionKey(false, Keys.R)]
    public class RotateAction : TransformAction
    {
        private Vector3[] objectOffsets;
        public Vector3 displacement;


        public RotateAction(Vector3 displacement)
            : this()
        {
            actionAxis = 0xFF;
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].rotation = objectOffsets[i] + displacement;
                objects[i].Update();
            }
        }

        public RotateAction()
        {
            objectOffsets = new Vector3[objects.Length];
            for (int i = 0; i < objects.Length; i++)
                objectOffsets[i] = objects[i].rotation;
            actionAxis = 2; //Rotate Y by default
        }
        public override void Undo()
        {
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].rotation = objectOffsets[i];
                objects[i].Update();
            }
        }

        public override void Update()
        {
            base.Update();
            displacement += new Vector3(1, 1, 1) * Main.mouseDelta.X * -10 * Editor.mouseSensitivity * speedMultiplier;
            Vector3 limitedDisplacement = displacement;
            if ((actionAxis & 1) == 0) limitedDisplacement.X = 0;
            if ((actionAxis & 2) == 0) limitedDisplacement.Y = 0;
            if ((actionAxis & 4) == 0) limitedDisplacement.Z = 0;
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].rotation = objectOffsets[i] + limitedDisplacement;
                objects[i].Update();
            }
        }
    }
}
