using System;
using System.Collections.Generic;
using NUnit.Framework;
using dSPACE.Programming.Task;

namespace dSPACE.PDU.Test
{
    [TestFixture]
    internal class PDUDataTest
    {
        [SetUp]
        public void Setup()
        {
            Console.WriteLine("Create PDU Payload Data test.");
        }

        [Test]
        public void CreatePduPayloadData_ReturnsPDU()
        {
            string ptest1 = "0201AAFA";
            string ptest2 = "0201";
            string ptest3 = "031";
            string ptest4 = "01";
            string ptest5 = "0";

            SignalDecoder pduDecoder = new SignalDecoder();

            var getPdu1 = pduDecoder.CreatePduPayloadData(ptest1);
            Assert.IsNotNull(getPdu1);

            var getPdu2 = pduDecoder.CreatePduPayloadData(ptest2);
            Assert.IsNotNull(getPdu2);

            var getPdu3 = pduDecoder.CreatePduPayloadData(ptest3);
            Assert.IsNotNull(getPdu3);

            var getPdu4 = pduDecoder.CreatePduPayloadData(ptest4);
            Assert.IsNotNull(getPdu4);

            var getPdu5 = pduDecoder.CreatePduPayloadData(ptest5);
            Assert.IsNull(getPdu5);
        }
    }
}
