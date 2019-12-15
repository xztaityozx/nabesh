using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using nabesh;
using Xunit;
using Assert = Xunit.Assert;

namespace UnitTest {
    public class NabeBlockTest {
        [Fact]
        public void NewTest() {
            var instance = new NabeBlock("abc");
            Assert.Equal(BaseNabe.•Ó, instance.Flag);
            Assert.Equal(3, instance.Size);
            Assert.Equal("abc", instance.Asciilize());
            CollectionAssert.AreEqual(
                new byte[] {
                    0x8f, 0xba, 0x8f, 0xba, 0xdb, 0x40, 0xdd, 0x00, 0x8f, 0xba, 0xdb, 0x40, 0xdd, 0x01, 0x8f,
                    0xba, 0xdb, 0x40, 0xdd, 0x02
                },
                new UnicodeEncoding(true, false).GetBytes(instance.Nabelize())
            );
        }
    }
}
