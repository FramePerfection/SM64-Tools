using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SM64ModelImporter.F3DParser
{
    public interface PointerReference
    {
        int pointer { get; set; }
        bool resolved { get; set; }
    }

    public class F3DParserException : Exception
    {
        public string error;
        public F3DParserException(string error)
        {
            this.error = error;
        }
        public override string ToString()
        {
            return base.ToString() + "\nError: " + error.ToString();
        }
    }

    public class F3DParser
    {
        public Dictionary<string, PointerReference> knownReferences = new Dictionary<string, PointerReference>();
        public Dictionary<string, CommandDescription> knownCommands = new Dictionary<string, CommandDescription>(F3DCommandDescriptors.descriptions);

        public static readonly char[] specialCharacters = new char[] { '(', ')', ',', ';', '[', ']' };
        public static readonly char[] whiteSpaceCharacters = new char[] { ' ', '\t', '\n', '\r' };
        string text;
        Function main;
        int cursor;
        string error;
        enum ParserState
        {
            None,
            Parsing,
            Error,
            Done
        }
        ParserState state = ParserState.None;
        public F3DParser(string fileName, int offset, byte[] ROM)
        {
            text = File.ReadAllText(fileName);
            state = ParserState.Parsing;
            try
            {
                while (cursor < text.Length && state != ParserState.Error)
                {
                    string hook = text.Substring(cursor, 5);
                    while (Array.IndexOf(whiteSpaceCharacters, text[cursor]) != -1) cursor++;
                    if (text.Substring(cursor, 5) != "func ")
                        throw new F3DParserException("'func' expected.");
                    cursor += 5;
                    char charAtCursor;
                    StringBuilder identifierBuilder = new StringBuilder(35);
                    bool mustTerminate = false;
                    while ((charAtCursor = text[cursor++]) != '{')
                    {
                        if (Array.IndexOf(specialCharacters, charAtCursor) > -1)
                            throw new F3DParserException("Unexpected token '" + charAtCursor + "'.");
                        else if (Array.IndexOf(whiteSpaceCharacters, charAtCursor) > -1)
                            mustTerminate = true;
                        else
                            identifierBuilder.Append(charAtCursor);
                    }
                    StringBuilder blockBuilder = new StringBuilder(256);
                    while ((charAtCursor = text[cursor++]) != '}') blockBuilder.Append(charAtCursor);
                    knownReferences["FUNC_" + identifierBuilder.ToString().Trim()] = new Function(this, blockBuilder.ToString());
                }
                PointerReference mainRef;
                if (!knownReferences.TryGetValue("FUNC_main", out mainRef))
                    throw new F3DParserException("No func main declared.");
                main = mainRef as Function;
                main.Resolve(ref offset, ROM);
                state = ParserState.Done;
            }
            catch (InvalidOperationException ex)
            {
                Error(ex.ToString());
            }
            catch (IndexOutOfRangeException)
            {
                Error("Unexpected end of File");
            }
        }

        void Error(string error)
        {
            state = ParserState.Error;
            this.error = error;
        }
    }
}
