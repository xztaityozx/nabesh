using System;
using System.Collections.Generic;
using System.Text;

namespace nabesh {
    internal class Base64NabeBlock : Nabelizer {
        public override IEnumerable<byte> Nabelize() {
            return new UnicodeEncoding(true, false).GetBytes(buffer);
        }

        public override string Restore() {
            return buffer;
        }

        private readonly List<NabeBlock> blocks;

        private string buffer;
        public Base64NabeBlock(string str) : base(str) {
            buffer = str;
        }
    }
}