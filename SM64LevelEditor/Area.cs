using System;
using System.Collections.Generic;
using System.Text;
using SM64Renderer;
using SM64RAM;

namespace SM64LevelEditor
{
    public class Area : IEnumerable<Object>
    {
        public Level level;
        public GeoLayout geometry;
        List<Object> objects = new List<Object>();
        public bool visible { get; private set; }
        public List<Object> selectedObjects { get; private set; }
        public int collisionPointer { get; private set; }
        public int musicSequence;

        public List<Warp> warps = new List<Warp>();

        public Area(Level level, int geoLayoutPointer)
        {
            this.level = level;
            selectedObjects = new List<Object>();
            geometry = GeoLayout.LoadSegmented(geoLayoutPointer);
        }

        public void MakeVisible()
        {
            visible = true;
            geometry.MakeVisible(Level.renderer);
            foreach (Object obj in objects)
                obj.MakeVisible();
            ToolBox.instance.warpControl.SetWarps(warps.ToArray());
        }
        public void MakeInvisible()
        {
            if (!visible) return;
            geometry.MakeInvisible(Level.renderer);
            foreach (Object obj in objects)
                obj.MakeInvisible();
            visible = false;
        }

        public void SetCollision(int pointer)
        {
            this.collisionPointer = pointer;
        }

        public void AddObject(Object obj)
        {
            objects.Add(obj);
            if (visible)
                obj.MakeVisible();
        }

        public void RemoveObject(Object obj)
        {
            obj.MakeInvisible();
            objects.Remove(obj);
            obj.selected = false;
        }

        public void Update(float time)
        {
            foreach (Object obj in objects)
                obj.Update();
        }

        public void Select(int pickedValue)
        {
            foreach (Object obj in objects)
                if (obj.pickIndex == pickedValue)
                {
                    if (obj.selected)
                    {
                        Deselect(obj);
                        return;
                    }
                    selectedObjects.Add(obj);
                    obj.selected = true;
                }
        }
        public void Deselect(Object obj)
        {
            obj.selected = false;
            selectedObjects.Remove(obj);
        }
        public void ClearSelection()
        {
            foreach (Object obj in selectedObjects)
                obj.selected = false;
            selectedObjects.Clear();
        }
        public void SelectAll()
        {
            foreach (Object obj in objects)
            {
                if (!selectedObjects.Contains(obj))
                    selectedObjects.Add(obj);
                obj.selected = true;
            }
        }
        public void SetSelection(Object[] newSelection)
        {
            ClearSelection();
            foreach (Object obj in newSelection)
            {
                selectedObjects.Add(obj);
                obj.selected = true;
            }
        }

        public IEnumerator<Object> GetEnumerator() { return objects.GetEnumerator(); }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return objects.GetEnumerator(); }
    }
}
