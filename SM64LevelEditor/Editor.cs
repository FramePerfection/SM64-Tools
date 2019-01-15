using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Windows.Forms;

namespace SM64LevelEditor
{
    public static class Editor
    {
        public static Level currentLevel;
        static int _currentAreaIndex = -1;
        public static int currentAreaIndex
        {
            get { return _currentAreaIndex; }
            set            {                currentLevel.MakeVisible(_currentAreaIndex = value);            }
        }
        public static Area currentArea { get { return currentLevel == null || currentAreaIndex < 0 ? null : currentLevel.areas[currentAreaIndex]; } }
        public static Camera camera = new Camera();

        public static float mouseSensitivity = 3;
        public static bool allowSelect { get { return currentAction == null; } }
        public static ProjectSettings projectSettings;
        private static Stack<Action> undoHistory = new Stack<Action>();
        private static Action currentAction;

        static Dictionary<ActionKeyAttribute, ConstructorInfo> availableActions = new Dictionary<ActionKeyAttribute, ConstructorInfo>();
        static List<ActionKeyAttribute> possibleActions = new List<ActionKeyAttribute>();
        static int currentActionSelect;

        static Editor()
        {
            foreach (Type t in Assembly.GetExecutingAssembly().GetTypes())
                if (typeof(Action).IsAssignableFrom(t))
                {
                    ConstructorInfo ctor = t.GetConstructor(new Type[0]);
                    if (ctor == null) continue;
                    object[] attributes = t.GetCustomAttributes(false);
                    foreach (object att in attributes)
                        if (typeof(ActionKeyAttribute).IsAssignableFrom(att.GetType()))
                            availableActions[(ActionKeyAttribute)att] = ctor;
                }
            ResetActionSelect();
        }

        public static void Update(float time)
        {
            camera.Update(time);

            if (Form.ModifierKeys == Keys.Control)
            {//Select action
                for (int i = 0; i < Main.keyDown.Length; i++)
                    if (Main.keyPress(i) && i != 0x11)
                    {
                        List<ActionKeyAttribute> newPossibilities = new List<ActionKeyAttribute>();
                        foreach (ActionKeyAttribute att in possibleActions)
                            if (currentActionSelect < att.keyOrder.Length && (int)att.keyOrder[currentActionSelect] == i)
                            {
                                if (currentActionSelect == att.keyOrder.Length - 1)
                                {
                                    if (att.instant)
                                    {
                                        Action ack = (Action)availableActions[att].Invoke(new object[0]);
                                        if (att.canUndo)
                                            undoHistory.Push(ack);
                                        goto Break2;
                                    }
                                    else
                                        SetAction((Action)availableActions[att].Invoke(new object[0]));
                                }
                                newPossibilities.Add(att);
                            }
                        possibleActions = newPossibilities;
                        currentActionSelect++;
                    }
            }
            else
                ResetActionSelect();
        Break2:

            if (currentAction != null)
            {
                currentAction.Update();
                if (Main.MouseButtons == MouseButtons.Right || Main.keyPress((int)Keys.Escape))
                    SetAction(null);
                if (Main.MouseButtons == MouseButtons.Left)
                {
                    undoHistory.Push(currentAction);
                    currentAction = null;
                }
            }

            if (currentArea != null)
            {
                currentArea.Update(time);
                if (Form.ActiveForm == Main.instance)
                    ToolBox.instance.objectControl1.UpdateSelectedObjects(currentArea.selectedObjects.ToArray());
            }
        }

        public static void SetAction(Action newAction)
        {
            if (currentAction != null) currentAction.Undo();
            currentAction = newAction;
        }

        public static void Undo()
        {
            if (undoHistory.Count > 0)
                undoHistory.Pop().Undo();
        }

        static void ResetActionSelect()
        {
            currentActionSelect = 0;
            possibleActions.Clear();
            foreach (KeyValuePair<ActionKeyAttribute, ConstructorInfo> ack in availableActions)
                possibleActions.Add(ack.Key);
        }
    }
}
