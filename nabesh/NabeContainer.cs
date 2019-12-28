using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nabesh {
    public class NabeContainer {
        private List<Nabelizer> nabelizers;

        public void Parse(string str) {
            nabelizers = Split(str);
        }

        /// <summary>
        /// ナベ化した文字列を返す
        /// </summary>
        /// <returns></returns>
        public string Nabelize() {
            var sb=new StringBuilder();
            var encoder = new UnicodeEncoding(true, false);

            foreach (var nabelizer in nabelizers) {
                sb.Append(encoder.GetString(nabelizer.Nabelize().ToArray()));
            }

            return sb.ToString();
        }

        /// <summary>
        /// ナベ化したバイト列を返す
        /// </summary>
        /// <returns></returns>
        public IEnumerable<byte> GetNabeBytes() {
            return nabelizers.SelectMany(nz => nz.Nabelize());
        }

        public string Restore() => string.Join("", nabelizers.Select(nz => nz.Restore()));

        private static List<Nabelizer> Split(string str) {
            var box = new List<Nabelizer>();
            var textElements = str.GetTextElements().ToArray();
            if (textElements.Length == 0) return new List<Nabelizer>();
            var bn = NabeTable.Default.GetBaseNabe(textElements[0]);
            var item = "";
            foreach (var textElement in textElements) {
                var next = NabeTable.Default.GetBaseNabe(textElement);
                if (bn == next) item += textElement;
                else {
                    // NabeBlock化
                    if (bn == BaseNabe.部) box.Add(new Base64NabeBlock(item));
                    else box.Add(new NabeBlock(item));

                    item = textElement;
                    bn = next;
                }
            }

            if (item.Length <= 0) return box;
            if (bn == BaseNabe.部) box.Add(new Base64NabeBlock(item));
            else box.Add(new NabeBlock(item));

            return box;
        } 
    }
}