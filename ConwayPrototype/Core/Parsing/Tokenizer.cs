using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino;

namespace ConwayPrototype.Core.Parsing
{
    public static class Tokenizer
    {
        public static string PossibleTokens = "kadbejlmnotzq";
        public static string NameDelimiter = "-";

        private static bool IsToken(char c)
        {
            return PossibleTokens.IndexOf(c) >= 0;
        }

        private static bool IsNumeric(char c)
        {
            return 48 <= c && c <= 57;
        }

        private static Operation ToOperation(char c)
        {
            switch (c)
            {
                case 'k':
                    return Operation.kis;
                case 'a':
                    return Operation.ambo;
                case 'd':
                    return Operation.dual;
                case 'b':
                    return Operation.bevel;
                case 'e':
                    return Operation.expand;
                case 'j':
                    return Operation.join;
                case 'l':
                    return Operation.loft;
                case 'm':
                    return Operation.meta;
                case 'n':
                    return Operation.needle;
                case 'o':
                    return Operation.ortho;
                case 't':
                    return Operation.truncate;
                case 'z':
                    return Operation.zip;
                case 'q':
                    return Operation.quinto;
                default:
                    return Operation.none;
            }
        }

        public static IEnumerable<Token> Tokenize(string txt)
        {
            int ptr = 0;
            List<Token> token = new List<Token>();
            while (ptr < txt.Length)
            {
                char op = txt[ptr];
                if (!IsToken(op))
                {
                    RhinoApp.WriteLine("Unexpected token: " + op + " in input " + txt + " at " + ptr);
                }
                int start_n = ++ptr;
                while (ptr < txt.Length && IsNumeric(txt[ptr]))
                {
                    ++ptr;
                }
                int n = 0;
                if (ptr - start_n > 0)
                {
                    n = int.Parse(txt.Substring(start_n, ptr - start_n));
                }

                token.Add(new Token{Operation = ToOperation(op), Numeric = n});
            }

            return token;
        }

        public static string ToName(this IEnumerable<Token> token)
        {
            string name = "";
            foreach (var tok in token)
            {
                name += tok.Operation.ToString();
                name += NameDelimiter;
            }

            if(name != "") name = name.Remove(name.Length - 1);

            return name;
        }
    }
}
