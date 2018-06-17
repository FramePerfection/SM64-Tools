using System;
using System.Collections.Generic;
using System.Text;

namespace SM64ModelImporter.F3DParser
{
    public class CommandDescription
    {
        public delegate DisplayList.Command CommandParser(byte opCode, Argument[] arguments);
        byte opCode;
        ArgumentType[] expectedArguments;
        CommandParser parser;

        public CommandDescription(byte opCode)
        {
            this.opCode = opCode;
            expectedArguments = new ArgumentType[0];
            parser = (op, arguments) => new DisplayList.Command(op);
        }

        public CommandDescription(byte opCode, ArgumentType[] expectedArguments, CommandParser parser)
        {
            this.opCode = opCode;
            this.expectedArguments = expectedArguments;
            this.parser = parser;
        }
        public ArgumentType GetArgumentType(int index)
        {
            return expectedArguments[index];
        }
        public DisplayList.Command Parse(params Argument[] arguments)
        {
            return parser(opCode, arguments);
        }
    }
}
