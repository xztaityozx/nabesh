using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using nabesh;
using Xunit;
using Assert = Xunit.Assert;

namespace UnitTest {
    public class NabeIvsTest {
        [Fact]
        public void GetIvsTest() {
            for(byte index =0x0; index<= 32; index++)
                CollectionAssert.AreEquivalent(new byte[] {0xdb, 0x40, 0xdd, index}, Nabe.NabeElement.GetIvs(index));
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
                    nb.Concat(Nabe.NabeElement.GetIvs(b)).ToArray(),
                    new Nabe.NabeElement(Nabe.BaseNabe.•Ó,(Nabe.BaseNabe) e, b).GetNabeBytes().ToArray()
                );
            }
        }

        [Theory]
        [InlineData(0, new byte[] {0x8f, 0xba})]
        [InlineData(1, new byte[] {0x90, 0x89})]
        [InlineData(2, new byte[] {0x90, 0x8A})]
        [InlineData(3, new byte[] {0x90, 0xE8})]
        public void BaseNabeToBytesTest(int e, byte[] bs) {
            CollectionAssert.AreEquivalent(((Nabe.BaseNabe) e).ToBytes(), bs);
        }

        [Theory]
        [InlineData(0, new byte[] {0x8f, 0xba}, false)]
        [InlineData(1, new byte[] {0x90, 0x89}, false)]
        [InlineData(2, new byte[] {0x90, 0x8A}, false)]
        [InlineData(0, new byte[] { }, true)]
        [InlineData(0, new byte[] {0x11, 0x00}, true)]
        public void BaseNabeToBaseNabe(int e, byte[] bs, bool throws) {
            if (throws) {
                Assert.Throws<FormatException>(() => bs.ToBaseNabe());
            }
            else {
                Assert.Equal((Nabe.BaseNabe) e, bs.ToBaseNabe());
            }
        }

        public static IEnumerable<object[]> GenerateDecodeTarget() {
            for (byte i = 0; i < 3; i++) yield return new object[] {Nabe.BaseNabe.•Ó, Nabe.BaseNabe.•Ó, i};
            for (byte i = 0; i < 23; i++) yield return new object[] {Nabe.BaseNabe.•Ó, Nabe.BaseNabe.ç³, i};
            for (byte i = 0; i < 26; i++) yield return new object[] {Nabe.BaseNabe.ç³, Nabe.BaseNabe.ç³, i};
            for (byte i = 0; i < 21; i++) yield return new object[] {Nabe.BaseNabe.ç², Nabe.BaseNabe.ç², i};
            for (byte i = 0; i < 3; i++) yield return new object[] {Nabe.BaseNabe.ç³, Nabe.BaseNabe.•Ó, i};
            for (byte i = 0; i < 18; i++) yield return new object[] {Nabe.BaseNabe.ç³, Nabe.BaseNabe.ç³, i};
        }

        [Theory, MemberData(nameof(GenerateDecodeTarget))]
        public void DecodeTest(Nabe.BaseNabe f, Nabe.BaseNabe bn, byte idx) {

        }
    }
}
