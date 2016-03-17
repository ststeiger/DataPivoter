
namespace DataPivoter
{


    public class JsonPrettyPrinter
    {

        const string Space = " ";
        const int DefaultIndent = 0;
        const string Indent = Space + Space + Space + Space;

        static readonly string NewLine = System.Environment.NewLine;
        private static void BuildIndents(int indents, System.Text.StringBuilder output)
        {
            indents += DefaultIndent;
            while (indents > 0)
            {
                output.Append(Indent);
                indents -= 1;
            }
        }


        private bool inDoubleString = false;
        private bool inSingleString = false;
        private bool inVariableAssignment = false;

        private char prevChar = '\0'; // ControlChars.NullChar
        private enum JsonContextType
        {
            Object,
            Array
        }


        private System.Collections.Generic.Stack<JsonContextType> context = new System.Collections.Generic.Stack<JsonContextType>();
        private bool InString()
        {
            return inDoubleString || inSingleString;
        }

        public void PrettyPrint(System.Text.StringBuilder input, System.Text.StringBuilder output)
        {
            if (input == null)
            {
                throw new System.ArgumentNullException("input");
            }
            if (output == null)
            {
                throw new System.ArgumentNullException("output");
            }

            int inputLength = input.Length;
            char c = '\0';

            for (int i = 0; i <= inputLength - 1; i++)
            {
                c = input[i];

                switch (c)
                {
                    case '{':
                        if (!InString())
                        {
                            if (inVariableAssignment || (context.Count > 0 && context.Peek() != JsonContextType.Array))
                            {
                                output.Append(NewLine);
                                BuildIndents(context.Count, output);
                            }
                            output.Append(c);
                            context.Push(JsonContextType.Object);
                            output.Append(NewLine);
                            BuildIndents(context.Count, output);
                        }
                        else
                        {
                            output.Append(c);
                        }

                        break; // TODO: might not be correct. Was : Exit Select


                        break;
                    case '}':
                        if (!InString())
                        {
                            output.Append(NewLine);
                            context.Pop();
                            BuildIndents(context.Count, output);
                            output.Append(c);
                        }
                        else
                        {
                            output.Append(c);
                        }

                        break; // TODO: might not be correct. Was : Exit Select


                        break;
                    case '[':
                        output.Append(c);

                        if (!InString())
                        {
                            context.Push(JsonContextType.Array);
                        }

                        break; // TODO: might not be correct. Was : Exit Select


                        break;
                    case ']':
                        if (!InString())
                        {
                            output.Append(c);
                            context.Pop();
                        }
                        else
                        {
                            output.Append(c);
                        }

                        break; // TODO: might not be correct. Was : Exit Select


                        break;
                    case '=':
                        output.Append(c);
                        break; // TODO: might not be correct. Was : Exit Select


                        break;
                    case ',':
                        output.Append(c);

                        if (!InString() && context.Peek() != JsonContextType.Array)
                        {
                            BuildIndents(context.Count, output);
                            output.Append(NewLine);
                            BuildIndents(context.Count, output);
                            inVariableAssignment = false;
                        }

                        break; // TODO: might not be correct. Was : Exit Select


                        break;
                    case '\'':
                        if (!inDoubleString && prevChar != '\\')
                        {
                            inSingleString = !inSingleString;
                        }

                        output.Append(c);
                        break; // TODO: might not be correct. Was : Exit Select


                        break;
                    case ':':
                        if (!InString())
                        {
                            inVariableAssignment = true;
                            output.Append(Space);
                            output.Append(c);
                            output.Append(Space);
                        }
                        else
                        {
                            output.Append(c);
                        }

                        break; // TODO: might not be correct. Was : Exit Select


                        break;
                    case '"':
                        if (!inSingleString && prevChar != '\\')
                        {
                            inDoubleString = !inDoubleString;
                        }

                        output.Append(c);
                        break; // TODO: might not be correct. Was : Exit Select

                        break;
                    default:

                        output.Append(c);
                        break; // TODO: might not be correct. Was : Exit Select

                        break;
                }
                prevChar = c;
            }
        }


    }


}
