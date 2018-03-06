using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using System.Windows.Forms;

namespace SM64Renderer
{
    public class DeviceWrapper
    {
        public Device device { get; private set; }

        public Effect LoadEffect(string sourceString)
        {
            string err;
            Effect e = Effect.FromString(device, sourceString, null, null, ShaderFlags.None, null, out err);
            if (err != "") MessageBox.Show(err);
            return e;
        }

        public bool Init(Control c)
        {
            PresentParameters p = new PresentParameters();
            p.DeviceWindow = c;
            p.EnableAutoDepthStencil = true;
            p.SwapEffect = SwapEffect.Discard;
            p.AutoDepthStencilFormat = DepthFormat.D24S8;
            p.Windowed = true;
            try
            {
                device = new Device(0, DeviceType.Hardware, c, CreateFlags.HardwareVertexProcessing, p);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to init D3D Device!\n" + ex.ToString());
                return false;
            }
            return true;
        }

        public static implicit operator Device(DeviceWrapper wrapper) { return wrapper.device; }
    }
}
