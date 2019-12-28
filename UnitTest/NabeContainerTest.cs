using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using nabesh;
using Xunit;
using Assert = Xunit.Assert;

namespace UnitTest {
    public class NabeContainerTest {
        [Fact]
        public void ParseTest() {
            var cnr=new NabeContainer();

            cnr.Parse("abcdefghijklmnopqrstuvwxyz");
            var expect = new byte[] {
                0x8f, 0xba, 0x8f, 0xba, 0xdb, 0x40, 0xdd, 0x00, 0x8f, 0xba, 0xdb, 0x40, 0xdd, 0x01, 0x8f, 0xba,
                0xdb,
                0x40, 0xdd, 0x02, 0x90, 0x89, 0xdb, 0x40, 0xdd, 0x00, 0x90, 0x89, 0xdb, 0x40, 0xdd, 0x01, 0x90,
                0x89,
                0xdb, 0x40, 0xdd, 0x02, 0x90, 0x89, 0xdb, 0x40, 0xdd, 0x03, 0x90, 0x89, 0xdb, 0x40, 0xdd, 0x04,
                0x90,
                0x89, 0xdb, 0x40, 0xdd, 0x05, 0x90, 0x89, 0xdb, 0x40, 0xdd, 0x06, 0x90, 0x89, 0xdb, 0x40, 0xdd,
                0x07,
                0x90, 0x89, 0xdb, 0x40, 0xdd, 0x08, 0x90, 0x89, 0xdb, 0x40, 0xdd, 0x09, 0x90, 0x89, 0xdb, 0x40,
                0xdd,
                0x0a, 0x90, 0x89, 0xdb, 0x40, 0xdd, 0x0b, 0x90, 0x89, 0xdb, 0x40, 0xdd, 0x0c, 0x90, 0x89, 0xdb,
                0x40,
                0xdd, 0x0d, 0x90, 0x89, 0xdb, 0x40, 0xdd, 0x0e, 0x90, 0x89, 0xdb, 0x40, 0xdd, 0x0f, 0x90, 0x89,
                0xdb,
                0x40, 0xdd, 0x10, 0x90, 0x89, 0xdb, 0x40, 0xdd, 0x11, 0x90, 0x89, 0xdb, 0x40, 0xdd, 0x12, 0x90,
                0x89,
                0xdb, 0x40, 0xdd, 0x13, 0x90, 0x89, 0xdb, 0x40, 0xdd, 0x14, 0x90, 0x89, 0xdb, 0x40, 0xdd, 0x15,
                0x90,
                0x89, 0xdb, 0x40, 0xdd, 0x16
            };
            var actual = cnr.GetNabeBytes().ToArray();
            CollectionAssert.AreEqual(expect, actual);

            Assert.Equal(new UnicodeEncoding(true, false).GetString(expect), cnr.Nabelize());
        }
    }
}
