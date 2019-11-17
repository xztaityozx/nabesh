using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;

namespace nabesh {
    public class Nabe {

        public enum BaseNabe {
            辺 = 0,
            邉 = 1,
            邊 = 2,
            部 = 3,
        }


        private static IEnumerable<string> GetTextElements(string str) {
            for (var item = StringInfo.GetTextElementEnumerator(str); item.MoveNext();)
                yield return item.GetTextElement();
        }

        public struct NabeElement {
            public BaseNabe BaseNabe { get; }
            public BaseNabe FlagNabe { get; set; }
            public byte IvsLastByte { get; }

            public NabeElement(BaseNabe flag,BaseNabe bn, byte idx) => (FlagNabe,BaseNabe, IvsLastByte) = (flag, bn, idx);

            public IEnumerable<byte> GetNabeBytes() => BaseNabe == BaseNabe.部
                ? throw new InvalidEnumArgumentException(nameof(BaseNabe))
                : BaseNabe.ToBytes().Concat(GetIvs(IvsLastByte));

            public static byte[] GetIvs(byte index) => new byte[] {0xdb, 0x40, 0xdd, index};

            private static readonly ReadOnlyDictionary<BaseNabe, ReadOnlyDictionary<BaseNabe, char[]>> decodeTable =
                new ReadOnlyDictionary<BaseNabe, ReadOnlyDictionary<BaseNabe, char[]>>(
                    new Dictionary<BaseNabe, ReadOnlyDictionary<BaseNabe, char[]>> {
                        [BaseNabe.辺] = new ReadOnlyDictionary<BaseNabe, char[]>(new Dictionary<BaseNabe, char[]> {
                            [BaseNabe.辺] = new[] {'a', 'b', 'c'},
                            [BaseNabe.邉] = new[] {
                                'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p',
                                'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
                            }
                        }),
                        [BaseNabe.邉] = new ReadOnlyDictionary<BaseNabe, char[]>(new Dictionary<BaseNabe, char[]> {
                            [BaseNabe.邉] = new[] {
                                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P',
                                'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
                            }
                        }),
                        [BaseNabe.邊] = new ReadOnlyDictionary<BaseNabe, char[]>(new Dictionary<BaseNabe, char[]> {
                            [BaseNabe.邊] = new[] {
                                '!', '"', '#', '$', '%', '&', '\'', '(', ')', '*', '+', ',', '-', '.', '/', '0', '1',
                                '2', '3', '4', '5'
                            },
                            [BaseNabe.辺] = new[] {'6', '7', '8'},
                            [BaseNabe.邊] = new[] {
                                ':', ';', '<', '=', '>',
                                '?', '@', '[', '\\', ']',
                                '^', '_', '`', '{', '|',
                                '}', '~', ' '
                            }
                        })
                    });

            private static ReadOnlyDictionary<string, NabeElement> encodeTable;

            public char Decode() {
                return decodeTable[FlagNabe][BaseNabe][IvsLastByte];
            }
        }

    }

    public static class NabeExt {
        public static byte[] ToBytes(this Nabe.BaseNabe baseNabe) => baseNabe switch {
            Nabe.BaseNabe.辺 => new byte[] {0x8f, 0xba},
            Nabe.BaseNabe.邉 => new byte[] {0x90, 0x89},
            Nabe.BaseNabe.邊 => new byte[] {0x90, 0x8A},
            Nabe.BaseNabe.部 => new byte[] {0x90, 0xE8},
            _ => throw new ArgumentOutOfRangeException(nameof(baseNabe))
            };

        public static Nabe.BaseNabe ToBaseNabe(this byte[] bytes) => bytes switch{
            var x when x.Length != 2 => throw new FormatException("Invalid array size"),
            var x when x[0] == 0x8f && x[1] == 0xba => Nabe.BaseNabe.辺,
            var x when x[0] == 0x90 && x[1] == 0x89 => Nabe.BaseNabe.邉,
            var x when x[0] == 0x90 && x[1] == 0x8A => Nabe.BaseNabe.邊,
            var x when x[0] == 0x90 && x[1] == 0xE8 => Nabe.BaseNabe.部,
            _ => throw new FormatException("array elements are unmatched")
            };
    }
}