using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SM64RAM;

namespace SM64ModelImporter
{
    public partial class TextureControl : UserControl
    {
        Font fnt = new Font("", 12, FontStyle.Regular, GraphicsUnit.Pixel);
        StringFormat align = new StringFormat();

        int selectedIndex = 0;
        TextureInfo selectedTexture = null;
        Dictionary<string, TextureInfo> _materialLibrary;
        public Dictionary<string, TextureInfo> materialLibrary
        {
            get { return _materialLibrary; }
            set
            {
                chkCustomAddress.Enabled = value != null && value.Count > 0;
                _materialLibrary = value;
                GetSelectedTexture();
            }
        }

        public TextureControl()
        {
            align.Alignment = StringAlignment.Center;
            InitializeComponent();
            cmbFormat.SelectedIndex = 0;
            cmbAddressX.SelectedIndex = cmbAddressY.SelectedIndex = 0;
            Paint += (object sender, PaintEventArgs e) => panelView.Invalidate();
            panelView.MouseDown += (object sender, MouseEventArgs e) =>
            {
                if (materialLibrary == null) return;
                if (e.X > panelView.Width / 2)
                    selectedIndex = Math.Min(selectedIndex + 1, materialLibrary.Count - 1);
                else
                    selectedIndex = Math.Max(selectedIndex - 1, 0);
                panelView.Invalidate();
                GetSelectedTexture();
                chkCustomAddress.Checked = selectedTexture.isCustom;
            };
        }

        void GetSelectedTexture()
        {
            selectedTexture = null;
            if (materialLibrary == null) return;
            int i = 0;
            foreach (KeyValuePair<string, TextureInfo> texture in materialLibrary)
                if (i++ == selectedIndex)
                {
                    selectedTexture = texture.Value;
                    UpdateControls();
                    break;
                }
        }

        private void panelTextureDisplay_Paint(object sender, PaintEventArgs e)
        {
            if (materialLibrary == null) return;
            e.Graphics.Clear(BackColor);
            int i = 0;
            int centerX = panelView.Width / 2;
            int centerY = panelView.Height / 2;
            int dX = 100;
            foreach (KeyValuePair<string, TextureInfo> texture in materialLibrary)
            {
                int szX = texture.Value.width;
                int szY = texture.Value.height;
                if (i == selectedIndex)
                {
                    szX = (int)(szX * 1.5f);
                    szY = (int)(szY * 1.5f);
                }
                Rectangle rect = new Rectangle(centerX + (i - selectedIndex) * dX - szX / 2, centerY - szY / 2, szX, szY);
                e.Graphics.DrawRectangle(Pens.Black, rect);
                e.Graphics.DrawImage(texture.Value.GetBitmap(), rect);
                i++;
            }
            if (selectedTexture != null && selectedTexture.image != null)
                e.Graphics.DrawString(selectedTexture.image.comment, fnt, Brushes.Black, centerX, 0, align);
        }

        void UpdateControls()
        {
            if (selectedTexture == null) return;
            txtCustomAddress.Text = selectedTexture.customAddress.ToString("X8");
            numericCustomWidth.Value = selectedTexture.customWidth;
            numericCustomHeight.Value = selectedTexture.customHeight;
            chkCustomAddress.Checked = selectedTexture.isCustom;
            cmbAddressX.SelectedIndex = (int)selectedTexture.addressX;
            cmbAddressY.SelectedIndex = (int)selectedTexture.addressY;

            TextureImage.TextureFormat usedFormat = selectedTexture.isCustom ? selectedTexture.customFormat : selectedTexture.image.format;
            TextureImage.BitsPerPixel usedBpp = selectedTexture.isCustom ? selectedTexture.customBpp : selectedTexture.image.bpp;
            if (usedFormat == TextureImage.TextureFormat.G_IM_FMT_RGBA)
            {
                if (usedBpp == TextureImage.BitsPerPixel.G_IM_SIZ_16b)
                    cmbFormat.SelectedIndex = 0;
                else if (usedBpp == TextureImage.BitsPerPixel.G_IM_SIZ_32b)
                    cmbFormat.SelectedIndex = 1;
            }
            else if (usedFormat == TextureImage.TextureFormat.G_IM_FMT_CI)
            {
                if (usedBpp == TextureImage.BitsPerPixel.G_IM_SIZ_4b)
                    cmbFormat.SelectedIndex = 2;
                else if (usedBpp == TextureImage.BitsPerPixel.G_IM_SIZ_8b)
                    cmbFormat.SelectedIndex = 3;
            }
            else if (usedFormat == TextureImage.TextureFormat.G_IM_FMT_IA)
            {
                if (usedBpp == TextureImage.BitsPerPixel.G_IM_SIZ_4b)
                    cmbFormat.SelectedIndex = 4;
                else if (usedBpp == TextureImage.BitsPerPixel.G_IM_SIZ_8b)
                    cmbFormat.SelectedIndex = 5;
                else if (usedBpp == TextureImage.BitsPerPixel.G_IM_SIZ_16b)
                    cmbFormat.SelectedIndex = 6;
            }
            else if (usedFormat == TextureImage.TextureFormat.G_IM_FMT_I)
            {
                if (usedBpp == TextureImage.BitsPerPixel.G_IM_SIZ_4b)
                    cmbFormat.SelectedIndex = 7;
                else if (usedBpp == TextureImage.BitsPerPixel.G_IM_SIZ_8b)
                    cmbFormat.SelectedIndex = 8;
            }

            pointerScrollingTextures.SetRAMPointers(selectedTexture.stRAMPointers);
            pointerScrollingTextures.SetROMPointers(selectedTexture.stROMPointers);
        }

        private void chkCustomAddress_CheckedChanged(object sender, EventArgs e)
        {
            selectedTexture.isCustom = chkCustomAddress.Checked;
            txtCustomAddress.Enabled = numericCustomWidth.Enabled = numericCustomHeight.Enabled = chkCustomAddress.Checked;
            selectedTexture.UpdateCustom();
        }

        private void txtCustomAddress_TextChanged(object sender, EventArgs e)
        {
            int address;
            if (cvt.ParseIntHex(txtCustomAddress.Text, out address))
                selectedTexture.SetCustomAddress(address);
            panelView.Invalidate();
        }

        private void numericCustomWidth_ValueChanged(object sender, EventArgs e)
        {
            selectedTexture.customWidth = (int)numericCustomWidth.Value;
            if (selectedTexture != null)
                selectedTexture.UpdateCustom();
            panelView.Invalidate();
        }

        private void numericHeight_ValueChanged(object sender, EventArgs e)
        {
            selectedTexture.customHeight = (int)numericCustomHeight.Value;
            if (selectedTexture != null)
                selectedTexture.UpdateCustom();
            panelView.Invalidate();
        }

        private void cmbFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectedTexture == null) return;
            switch (cmbFormat.SelectedIndex)
            {
                case 0: // 16 bit RGBA
                    selectedTexture.SetFormat(TextureImage.TextureFormat.G_IM_FMT_RGBA, TextureImage.BitsPerPixel.G_IM_SIZ_16b);
                    break;
                case 1: // 32 bit RGBA
                    selectedTexture.SetFormat(TextureImage.TextureFormat.G_IM_FMT_RGBA, TextureImage.BitsPerPixel.G_IM_SIZ_32b);
                    break;
                case 2: // 4 bit CI
                    selectedTexture.SetFormat(TextureImage.TextureFormat.G_IM_FMT_CI, TextureImage.BitsPerPixel.G_IM_SIZ_4b);
                    break;
                case 3: // 8 bit CI
                    selectedTexture.SetFormat(TextureImage.TextureFormat.G_IM_FMT_CI, TextureImage.BitsPerPixel.G_IM_SIZ_8b);
                    break;
                case 4: // 4 bit IA
                    selectedTexture.SetFormat(TextureImage.TextureFormat.G_IM_FMT_IA, TextureImage.BitsPerPixel.G_IM_SIZ_4b);
                    break;
                case 5: // 8 bit IA
                    selectedTexture.SetFormat(TextureImage.TextureFormat.G_IM_FMT_IA, TextureImage.BitsPerPixel.G_IM_SIZ_8b);
                    break;
                case 6: // 16 bit IA
                    selectedTexture.SetFormat(TextureImage.TextureFormat.G_IM_FMT_IA, TextureImage.BitsPerPixel.G_IM_SIZ_16b);
                    break;
                case 7: // 4 bit I
                    selectedTexture.SetFormat(TextureImage.TextureFormat.G_IM_FMT_I, TextureImage.BitsPerPixel.G_IM_SIZ_4b);
                    break;
                case 8: // 8 bit I
                    selectedTexture.SetFormat(TextureImage.TextureFormat.G_IM_FMT_I, TextureImage.BitsPerPixel.G_IM_SIZ_8b);
                    break;
            }
        }

        private void cmbAddressX_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectedTexture == null) return;
            selectedTexture.addressX = (TextureInfo.AddressMode)cmbAddressX.SelectedIndex;
        }

        private void cmbAddressY_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectedTexture == null) return;
            selectedTexture.addressY = (TextureInfo.AddressMode)cmbAddressY.SelectedIndex;
        }

        private void pointerScrollingTextures_ValueChanged(object sender, EventArgs e)
        {
            selectedTexture.stRAMPointers = pointerScrollingTextures.GetRAMPointers();
            selectedTexture.stROMPointers = pointerScrollingTextures.GetROMPointers();
        }
    }
}
