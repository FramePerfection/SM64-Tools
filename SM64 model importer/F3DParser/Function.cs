using System;
using System.Collections.Generic;
using System.Text;

namespace SM64ModelImporter.F3DParser
{
    class Function : PointerReference
    {
        public int pointer { get; set; }
        public bool resolved { get; set; }
        List<DisplayList.Command> commands = new List<DisplayList.Command>();
        string text;
        int cursor = 0;
        F3DParser parser;
        public Function(F3DParser parser, string block)
        {
            block = block.Trim();
            this.text = block;
            this.parser = parser;
        }

        public void Resolve(ref int offset, byte[] ROM)
        {
            pointer = offset;
            while (cursor < text.Length)
                commands.Add(ParseCall(ref offset, ROM));
            commands.Add(new DisplayList.Command(0xB8));
            foreach (DisplayList.Command cmd in commands)
            {
                if (ROM != null)
                    Array.Copy(cmd.values, 0, ROM, offset, 8);
                offset += 8;
            }
            resolved = true;
        }

        DisplayList.Command ParseCall(ref int offset, byte[] ROM)
        {
            StringBuilder identifierBuilder = new StringBuilder(35);
            char charAtCursor;
            while ((charAtCursor = text[cursor++]) != '(')
            {
                if (Array.IndexOf(F3DParser.specialCharacters, charAtCursor) > -1)
                    throw new F3DParserException("Unexpected token '" + charAtCursor + "', '(' expected instead.");
                else
                    identifierBuilder.Append(charAtCursor);
            }
            string identifier = identifierBuilder.ToString().Trim();
            PointerReference maybeFunction;
            if (parser.knownReferences.TryGetValue("FUNC_" + identifier, out maybeFunction))
            {
                Function func = maybeFunction as Function;
                if (func == null)
                    throw new F3DParserException("Unknown Function " + identifier + ".");
                if (!func.resolved)
                    func.Resolve(ref offset, ROM);
                if ((charAtCursor = text[cursor++]) != ')')
                    throw new F3DParserException("Unexpected token '" + charAtCursor + "', ')' expected instead.");
                if ((charAtCursor = text[cursor++]) != ';')
                    throw new F3DParserException("Unexpected token '" + charAtCursor + "', ';' expected instead.");
                return new DisplayList.Command(0x06, 0, (int)func.pointer);
            }
            CommandDescription desc;
            if (!parser.knownCommands.TryGetValue(identifier, out desc))
                throw new F3DParserException("Unknown Call " + identifier + ".");

            StringBuilder argumentBuilder = new StringBuilder(10);
            List<Argument> arguments = new List<Argument>();
            int arg = 0;
            while ((charAtCursor = text[cursor++]) != ')')
            {
                if (charAtCursor == ',')
                {
                    arguments.Add(new Argument(parser, desc.GetArgumentType(arg++), "<unknown argument>", argumentBuilder.ToString()));
                    argumentBuilder = new StringBuilder(10);
                }
                else
                    argumentBuilder.Append(charAtCursor);
            }
            string argString = argumentBuilder.ToString();
            if ((argString = argString.Trim()).Length > 0)
                arguments.Add(new Argument(parser, desc.GetArgumentType(arg++), "<unknown argument>", argumentBuilder.ToString()));

            while ((charAtCursor = text[cursor++]) != ';')
            {
                if (Array.IndexOf(F3DParser.whiteSpaceCharacters, charAtCursor) == -1)
                    throw new F3DParserException("Unexpected token '" + charAtCursor + "', ';' expected instead.");
            }
            return desc.Parse(arguments.ToArray());
        }
    }
}
