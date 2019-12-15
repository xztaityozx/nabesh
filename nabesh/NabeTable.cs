using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.Json;

namespace nabesh {

    public class NabeTable {
        private readonly ReadOnlyDictionary<BaseNabe, ReadOnlyDictionary<BaseNabe, char[]>> decodeTable =
                new ReadOnlyDictionary<BaseNabe, ReadOnlyDictionary<BaseNabe, char[]>>(
                    new Dictionary<BaseNabe, ReadOnlyDictionary<BaseNabe, char[]>>
                    {
                        [BaseNabe.辺] = new ReadOnlyDictionary<BaseNabe, char[]>(new Dictionary<BaseNabe, char[]>
                        {
                            [BaseNabe.辺] = new[] { 'a', 'b', 'c' },
                            [BaseNabe.邉] = new[] {
                                'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p',
                                'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
                            }
                        }),
                        [BaseNabe.邉] = new ReadOnlyDictionary<BaseNabe, char[]>(new Dictionary<BaseNabe, char[]>
                        {
                            [BaseNabe.邉] = new[] {
                                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P',
                                'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
                            }
                        }),
                        [BaseNabe.邊] = new ReadOnlyDictionary<BaseNabe, char[]>(new Dictionary<BaseNabe, char[]>
                        {
                            [BaseNabe.邊] = new[] {
                                '!', '"', '#', '$', '%', '&', '\'', '(', ')', '*', '+', ',', '-', '.', '/', '0', '1',
                                '2', '3', '4', '5'
                            },
                            [BaseNabe.辺] = new[] { '6', '7', '8' },
                            [BaseNabe.邊] = new[] {
                                ':', ';', '<', '=', '>',
                                '?', '@', '[', '\\', ']',
                                '^', '_', '`', '{', '|',
                                '}', '~', ' '
                            }
                        })
                    });

        private readonly Dictionary<string, NabeElement> encodeTable;

        public NabeTable() {
            encodeTable = new Dictionary<string, NabeElement>();
            foreach (var (f, table) in decodeTable) {
                foreach (var (b, values) in table) {
                    foreach (var (ascii, i) in values.Select((c, i) => Tuple.Create(c,i))) {
                        encodeTable.Add($"{ascii}", new NabeElement {
                            Flag = f, Base = b, IvsIndex = (byte) i, Ascii = $"{ascii}"
                        });
                    }
                }
            }
        }

        public char this[BaseNabe f, BaseNabe b, byte ivs] => decodeTable[f][b][ivs];

        public NabeElement this[string index] => encodeTable[index];
        public bool IsNabelizable(string str) => encodeTable.ContainsKey(str);

        private static NabeTable instance = null;

        public static NabeTable Default => instance ??= new NabeTable();
    }
}