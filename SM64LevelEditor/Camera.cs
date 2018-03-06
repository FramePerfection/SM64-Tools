using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using System.Windows.Forms;

namespace SM64LevelEditor
{
    public enum CameraState
    {
        Free = 0,
        Orbit = 1
    }
    public class Camera
    {
        const float MAX_PITCH = 1.4f;

        public CameraState state;
        public Vector3 position, target = new Vector3(0, 0, 1);
        public Vector3 cursor { get; private set; }
        public float verticalViewAngle = 1;
        public Vector3 localX { get; private set; }
        public Vector3 localY { get; private set; }
        public Vector3 localZ { get; private set; }
        Vector3 up = new Vector3(0, 1, 0);
        float yaw, pitch;
        float cameraSpeed = 500, fastCameraSpeed = 5000;
        float cursorDistance = 500;

        public void Move(Vector3 relativeValues, float time)
        {
            Vector3 delta = (localX * relativeValues.X + up * relativeValues.Y + localZ * relativeValues.Z) * time;
            position += delta;
            if (state == CameraState.Free)
                target += delta;
        }

        public void Rotate(Vector2 values, float time)
        {
            yaw += values.X * time;
            pitch += values.Y * time;
            pitch = Math.Max(-MAX_PITCH, Math.Min(MAX_PITCH, pitch));
            float len = (target - position).Length();
            float ajiwoa = (float)Math.Cos(pitch);
            Vector3 d = new Vector3(ajiwoa * (float)Math.Sin(yaw), (float)Math.Sin(pitch), ajiwoa * (float)Math.Cos(yaw));
            if (state == CameraState.Free)
                target = position + d * len;
            else
                position = target - d * len;
        }

        public Matrix GetView()
        {
            return Matrix.LookAtRH(position, target, up);
        }

        public void Update(float time)
        {
            localZ = Vector3.Normalize(target - position);
            localX = Vector3.Normalize(Vector3.Cross(localZ, up));
            localY = Vector3.Normalize(Vector3.Cross(localX, localZ));

            Vector3 move = new Vector3();
            if (Main.keyDown[(int)Keys.A]) move.X -= 1;
            if (Main.keyDown[(int)Keys.D]) move.X += 1;
            if (Main.keyDown[(int)Keys.Q]) move.Y -= 1;
            if (Main.keyDown[(int)Keys.E]) move.Y += 1;
            if (Main.keyDown[(int)Keys.S]) move.Z -= 1;
            if (Main.keyDown[(int)Keys.W]) move.Z += 1;
            Move(move * (Main.keyDown[(int)Keys.ShiftKey] ? fastCameraSpeed : cameraSpeed), time);
            cursorDistance += Main.mouseDeltaZ  * (Main.keyDown[(int)Keys.ShiftKey] ? fastCameraSpeed : cameraSpeed / 10) / 1000;
            if (Editor.currentLevel != null)
            {
                Level.renderer.view = GetView();
                Level.renderer.projection = Matrix.PerspectiveFovRH(verticalViewAngle, (float)Main.instance.Width / Main.instance.Height, 15, 50000);
            }
            float tan = (float)Math.Tan(verticalViewAngle / 2);
            System.Drawing.Point p = Main.instance.PointToClient(Cursor.Position);
            cursor = position + ((p.X / (float)Main.instance.Width - 0.5f) * tan * (float)Main.instance.Width / Main.instance.Height * 2 * localX - (p.Y / (float)Main.instance.Height - 0.5f) * tan * 2 * localY + localZ) * cursorDistance;
        }
    }
}
