using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Unicode;

namespace nabesh {
    internal interface INabelizer {
        string Nabelize();
        string Asciilize();
    }

    public enum BaseNabe {
        辺 = 0,
        邉 = 1,
        邊 = 2,
        部 = 3,
    }

    public class NabeElement {
        public BaseNabe Flag { get; set; }
        public BaseNabe Base { get; set; }
        public byte IvsIndex { get; set; }
        public string Ascii { get; set; }

        public byte[] Nabelize() {
            return Base.ToBytes().Concat(GetIvs()).ToArray();
        }

        private byte[] GetIvs() => new byte[] {0xdb, 0x40, 0xdd, IvsIndex};

        public override string ToString() {
            return
                $"Flag: {Encoding.UTF8.GetString(Flag.ToBytes())}\nBase: {Encoding.UTF8.GetString(Base.ToBytes())}\nIvsIndex: {IvsIndex}\nAscii: {Ascii}";
        }
    }

    internal static class NabeExtensions {

        public static IEnumerable<string> GetTextElements(this string str) {
            for (var item = StringInfo.GetTextElementEnumerator(str); item.MoveNext();)
                yield return item.GetTextElement();
        }

        public static byte[] ToBytes(this BaseNabe baseNabe) => baseNabe switch
            {
            BaseNabe.辺 => new byte[] {0x8f, 0xba},
            BaseNabe.邉 => new byte[] {0x90, 0x89},
            BaseNabe.邊 => new byte[] {0x90, 0x8A},
            BaseNabe.部 => new byte[] {0x90, 0xE8},
            _ => throw new ArgumentOutOfRangeException(nameof(baseNabe))
            };

        public static BaseNabe ToBaseNabe(this byte[] bytes) => bytes switch
            {
            var x when x.Length != 2 => throw new FormatException("byte[]のサイズが違うんですけど！"),
            var x when x[0] == 0x8f && x[1] == 0xba => BaseNabe.辺,
            var x when x[0] == 0x90 && x[1] == 0x89 => BaseNabe.邉,
            var x when x[0] == 0x90 && x[1] == 0x8A => BaseNabe.邊,
            var x when x[0] == 0x90 && x[1] == 0xE8 => BaseNabe.部,
            _ => throw new FormatException("ナベじゃないんですけど！")
            };

        public static BaseNabe ToBaseNabe(this string str) => Encoding.UTF8.GetBytes(str).ToBaseNabe();
    }
}