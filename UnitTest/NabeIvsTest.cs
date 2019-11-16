using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using nabesh;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace UnitTest
{
    public class NabeIvsTest {
        [Fact]
        public void GetIvsTest() {
            for(byte index =0x0; index<= 32; index++)
                CollectionAssert.AreEquivalent(new byte[] {0xdb, 0x40, 0xdd, index}, NabeIvs.GetIvs(index));
        }

        [Theory]
        [InlineData(0, 3)]
        [InlineData(1, 32)]
        [InlineData(2, 21)]
        public void GetNabeBytes(int e, byte idx) {
            for (byte b = 0; b < idx; b++) {
                var nb = e switch {
                    0 => new byte[] {0x8f, 0xba},
                    1 => new byte[] {0x90, 0x89},
                    2 => new byte[] {0x90, 0x8A},
                    _ => throw new Exception()
                    };
                CollectionAssert.AreEquivalent(
                    nb.Concat(NabeIvs.GetIvs(b)).ToArray(),
                    new NabeIvs.Nabe((NabeIvs.BaseNabe) e, b).GetNabeBytes().ToArray()
                );
            }
        }
    }
}
