using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SM64LevelEditor.Controls
{
    public partial class ObjectControl : UserControl
    {
        public ObjectControl()
        {
            InitializeComponent();
            propertyCommand.PropertyValueChanged += UpdateControls;
            propertyCommand.SelectedObjectsChanged += UpdateControls;
        }

        public void UpdateSelectedObjects(Object[] objects)
        {
            propertyCommand.SelectedObjects = objects;
        }

        public void UpdateAlias()
        {
            cmbBehaviourAlias.DataSource = null;
            cmbBehaviourAlias.DataSource = Editor.currentLevel.behaviourAlias;
            cmbModelIDAlias.DataSource = null;
            cmbModelIDAlias.DataSource = Editor.currentLevel.modelIDAlias;
        }

        private void cmbBehaviourAlias_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBehaviourAlias.SelectedItem == null) return;
            int newBehaviour = ((Alias<int>)cmbBehaviourAlias.SelectedItem).value;
            foreach (object a in propertyCommand.SelectedObjects)
            {
                Object obj = a as Object;
                if (obj == null) continue;
                obj.behaviourScript = newBehaviour;
            }
        }

        private void cmbModelAlias_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbModelIDAlias.SelectedItem == null) return;
            byte newModelID = ((Alias<byte>)cmbModelIDAlias.SelectedItem).value;
            foreach (object a in propertyCommand.SelectedObjects)
            {
                Object obj = a as Object;
                if (obj == null) continue;
                obj.model_ID = newModelID;
            }
        }

        private void UpdateControls(object sender, EventArgs e)
        {
            if (propertyCommand.SelectedObject == null) return;
            int behaviour = ((Object)propertyCommand.SelectedObject).behaviourScript;
            foreach (Alias<int> behaviourAlias in cmbBehaviourAlias.Items)
                if (behaviourAlias.value == behaviour)
                    cmbBehaviourAlias.SelectedItem = behaviourAlias;
            int modelID = ((Object)propertyCommand.SelectedObject).model_ID;
            foreach (Alias<byte> modelIDAlias in cmbModelIDAlias.Items)
                if (modelIDAlias.value == modelID)
                    cmbModelIDAlias.SelectedItem = modelIDAlias;
        }
    }
}
