using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace SM64_model_importer
{
    public class RenderStateControl : PropertyControl<RenderStates>
    {
        public RenderStateControl()
            : base(6, 5)
        {}
    }
    public class CombinerStateControl : PropertyControl<CombinerStates>
    {
        public CombinerStateControl() : base(8, 4)
        {}
    }

    public partial class PropertyControl<T> : UserControl
    {
        public event EventHandler StateChanged;

        Dictionary<PropertyInfo, ComboBox> cmbBoxes = new Dictionary<PropertyInfo, ComboBox>();
        Dictionary<PropertyInfo, CheckBox> chkBoxes = new Dictionary<PropertyInfo, CheckBox>();

        static Type targetType = typeof(T);
        bool haltEvents = true;
        public PropertyControl(int numColumns, int numRows)
        {
            InitializeComponent();
            flowLayoutPanel.ColumnCount = numColumns;
            flowLayoutPanel.RowCount = numColumns;
            groupBox.Text = targetType.Name;
            flowLayoutPanel.AutoScroll = true;
            foreach (PropertyInfo property in targetType.GetProperties())
                if (property.PropertyType.IsEnum)
                {
                    ComboBox box = new ComboBox();
                    box.Enabled = false;
                    box.DataSource = Enum.GetValues(property.PropertyType);
                    flowLayoutPanel.Controls.Add(box);
                    cmbBoxes[property] = box;
                    box.SelectedIndexChanged += OnChange;

                    Label nam = new Label();
                    nam.Text = property.Name;
                    flowLayoutPanel.Controls.Add(nam);
                }
            foreach (PropertyInfo property in targetType.GetProperties())
                if (property.PropertyType == typeof(bool))
                {
                    CheckBox chk = new CheckBox();
                    chk.Enabled = false;
                    chk.Text = property.Name;
                    chk.Width = 110;
                    flowLayoutPanel.Controls.Add(chk);
                    chkBoxes[property] = chk;
                    chk.CheckedChanged += OnChange;
                }
            flowLayoutPanel.PerformLayout();
            haltEvents = false;
        }

        T states;

        public void Bind(T states)
        {
            this.states = states;
            foreach (KeyValuePair<PropertyInfo, CheckBox> chk in chkBoxes)
                chk.Value.Enabled = true;
            foreach (KeyValuePair<PropertyInfo, ComboBox> cmb in cmbBoxes)
                cmb.Value.Enabled = true;
            SetValues();
        }

        void SetValues()
        {
            if (haltEvents) return;
            haltEvents = true;
            foreach (PropertyInfo property in targetType.GetProperties())
            {
                ComboBox cmb;
                if (cmbBoxes.TryGetValue(property, out cmb))
                    cmb.SelectedItem = property.GetValue(states, null);
                CheckBox chk;
                if (chkBoxes.TryGetValue(property, out chk))
                    chk.Checked = (bool)property.GetValue(states, null);
            }
            haltEvents = false;
        }

        void OnChange(object sender, EventArgs e)
        {
            if (haltEvents || states == null) return;
            haltEvents = true;
            foreach (KeyValuePair<PropertyInfo, CheckBox> chk in chkBoxes)
                chk.Key.SetValue(states, chk.Value.Checked, null);
            foreach (KeyValuePair<PropertyInfo, ComboBox> cmb in cmbBoxes)
                cmb.Key.SetValue(states, cmb.Value.SelectedItem, null);
            if (StateChanged != null)
                StateChanged(this, e);
            haltEvents = false;
        }
    }
}
