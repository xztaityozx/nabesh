using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace nabesh {
    public class NabeIvs {

        public enum BaseNabe {
            辺 = 0,
            邊 = 2,
            邉 = 1
        }

        private static readonly Dictionary<BaseNabe, (string delimitor, int count)> nabeDictionary =
            new Dictionary<BaseNabe, (string delimitor, int ivsCount)> {
                [BaseNabe.辺] = ("辺", 3),
                [BaseNabe.邉] = ("邉", 32),
                [BaseNabe.邊] = ("邊", 21)
            };

        private readonly string[] charTable = {
            "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u",
            "v", "w", "x", "y", "z",
            "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P",
            "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
            "!", "\"", "#", "$", "%", "&", "'", "(", ")", "*", "+", ",", "-", ".", "/", "0", "1", "2", "3", "4", "5",
            "6", "7", "8", "9", ":", ";", "<", "=", ">", "?", "@",
            "[", "\"", "]", "^", "_", "`", "{", "|", "}", "~", " "
        };

        private static IEnumerable<string> GetTextElements(string str) {
            for (var item = StringInfo.GetTextElementEnumerator(str); item.MoveNext();)
                yield return item.GetTextElement();
        }

        public class Nabe {
            public BaseNabe BaseNabe { get; }
            public byte IvsLastByte { get; }
            public bool IsFlagNabe { get; }

            public Nabe(BaseNabe bn, byte idx) => (BaseNabe, IvsLastByte, IsFlagNabe) = (bn, idx, false);


            public IEnumerable<byte> GetNabeBytes() {
                var bn = BaseNabe switch {
                    BaseNabe.辺 => new byte[] {0x8f, 0xba},
                    BaseNabe.邉 => new byte[] {0x90, 0x89},
                    BaseNabe.邊 => new byte[] {0x90, 0x8A},
                    _ => throw new IndexOutOfRangeException()
                    };


                return bn.Concat(GetIvs(IvsLastByte));
            }

        }

        public static byte[] GetIvs(byte index) => new byte[]{0xdb, 0x40, 0xdd, index};
    }

}
