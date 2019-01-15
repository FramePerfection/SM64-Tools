using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace SM64LevelEditor.Actions
{
    [ActionKey(true, false, Keys.D)]
    public class DuplicateAction : Action
    {
        Object[] newObjects;
        public DuplicateAction()
        {
            newObjects = new Object[objects.Length];
            int i = 0;
            foreach (Object obj in objects)
            {
                obj.area.AddObject(newObjects[i] = new Object(obj));
                newObjects[i++].Update();
            }
            Editor.currentArea.SetSelection(newObjects);
            Editor.SetAction(new DuplicateInternal(objects, newObjects));
        }
        public override void Update() { throw new NotSupportedException(); }

        public override void Undo() { throw new NotSupportedException(); }

        class DuplicateInternal : TranslateAction
        {
            Object[] oldObjects;
            public DuplicateInternal(Object[] oldObjects, Object[] newObjects)
            {
                this.objects = newObjects;
                this.oldObjects = oldObjects;
            }
            public override void Undo()
            {
                foreach (Object obj in objects)
                    obj.area.RemoveObject(obj);
                Editor.currentArea.SetSelection(oldObjects);
            }
        }
    }
}
