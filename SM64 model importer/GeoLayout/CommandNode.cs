using System;
using System.Collections.Generic;
using System.Text;

namespace SM64ModelImporter
{
    public class GeoLayoutNode : GeoLayoutCommands.Command
    {
        public List<GeoLayoutCommands.Command> commands = new List<GeoLayoutCommands.Command>();
        public GeoLayoutNode()
        {
        }
    }
}
