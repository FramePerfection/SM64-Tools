using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using System.Windows.Forms;

namespace SM64LevelEditor
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ActionKeyAttribute : Attribute
    {
        public bool instant { get; private set; }
        public Keys[] keyOrder { get; private set; }
        public bool canUndo { get; private set; }
        public ActionKeyAttribute(bool instant, params Keys[] keyOrder)
        {
            this.instant = instant;
            this.keyOrder = keyOrder;
            canUndo = true;
        }
        public ActionKeyAttribute(bool instant, bool canUndo, params Keys[] keyOrder)
        {
            this.instant = instant;
            this.keyOrder = keyOrder;
            this.canUndo = canUndo;
        }
    }

    public abstract class Action
    {
        protected Object[] objects;
        public Action()
        {
            objects = new Object[Editor.currentArea.selectedObjects.Count];
            int i = 0;
            foreach (Object obj in Editor.currentArea.selectedObjects)
                objects[i++] = obj;
        }
        public abstract void Undo();
        public abstract void Update();
    }

    [ActionKey(true, Keys.Delete), ActionKey(true, Keys.Q, Keys.W)]
    public class DeleteAction : Action
    {
        public DeleteAction()
        {
            foreach (Object obj in objects)
                Editor.currentArea.RemoveObject(obj);
        }
        public override void Undo()
        {
            foreach (Object obj in objects)
                obj.area.AddObject(obj);
        }
        public override void Update() { }
    }

    [ActionKey(true, Keys.O)]
    public class CreateObjectAction : Action
    {

        public CreateObjectAction()
        {
            Object newObj = new Object(Editor.currentLevel, Editor.currentArea, Editor.camera.cursor, Vector3.Empty, 0, 0, 0x1F, 0);
            Editor.currentArea.AddObject(newObj);
            newObj.Update();
            objects = new Object[] { newObj };
        }
        public override void Undo()
        {
            objects[0].area.RemoveObject(objects[0]);
        }

        public override void Update() { }
    }
    [ActionKey(true, Keys.A)]
    public class SelectAllAction : Action
    {
        public SelectAllAction()        {            Editor.currentArea.SelectAll();      }
        public override void Undo() { }
        public override void Update() { }
    }
}
