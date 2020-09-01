using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpex.Extenders
{
    public static class Extenders
    {
        public static T ToEnum<T>(this string text, bool ignoreCase = true) where T : struct
        {
            return Enum.TryParse(text, ignoreCase, out T parsed) ? parsed : default;
        }
    }
}
