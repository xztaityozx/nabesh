using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nabesh {
    /// <summary>
    /// ASCIIに含まれる文字だけからなるNabeブロック
    /// </summary>
    internal class NabeBlock : Nabelizer {
        public override IEnumerable<byte> Nabelize() =>
            FlagNabe.ToBytes().Concat(elements.SelectMany(s => s.Nabelize()));


        public override string Restore() => OriginalString;

        private readonly List<NabeElement> elements;
        public void Add(NabeElement element) => elements.Add(element);
        public int Size => elements.Count;

        public NabeBlock(string str) : base(str) {
            var list = str.GetTextElements().ToList();
            if(!list.All(s=>NabeTable.Default.IsNabelizable(s))) throw new Exception("ブロック内にナベ化できないものがあります🍲");
            elements = list.Select(s => NabeTable.Default[s]).ToList();
            if(elements.Any(s => s.Flag != FlagNabe)) throw new Exception("ブロック内の無いナベが統一されていません🍲");
        }
    }
}