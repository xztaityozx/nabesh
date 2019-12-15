using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nabesh {
    public class NabeBlock : INabelizer {
        public string Nabelize() {
            return new UnicodeEncoding(true, false).GetString(
                Flag.ToBytes().Concat(elements.SelectMany(s => s.Nabelize())).ToArray()
            );
        }

        public string Asciilize() {
            return string.Join("", elements.Select(s => s.Ascii));
        }

        private readonly List<NabeElement> elements;
        public void Add(NabeElement element) => elements.Add(element);
        public int Size => elements.Count;
        public void RemoveAt(int index) => elements.RemoveAt(index);
        public void RemoveLast() => RemoveAt(Size - 1);
        public BaseNabe Flag { get; }

        public NabeBlock(string str) {
            var list = str.GetTextElements().ToList();
            if(!list.All(s=>NabeTable.Default.IsNabelizable(s))) throw new Exception("ブロック内にナベ化できないものがあります🍲");
            elements = list.Select(s => NabeTable.Default[s]).ToList();
            Flag = elements[0].Flag;
            if(elements.Any(s => s.Flag != Flag)) throw new Exception("ブロック内の無いナベが統一されていません🍲");
        }
    }
}