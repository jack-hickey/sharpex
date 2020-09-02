using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpex.Extenders.Internal
{
    internal class Constants
    {
        public struct RegexFormats
        {
            public const string WhiteSpace = @"\s+";

            public const string NonNumerics = "[^0-9]";

            public const string Numerics = "[0-9]";

            public const string NonAlphaNumerics = "[^a-zA-Z0-9 -]";
        }

        public const string HashByteFormat = "x2";
    }
}
