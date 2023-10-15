using System;
using NUnit.Framework;
using dSPACE.Programming.Task;

namespace dSPACE.PDU.Test
{
    [TestFixture]
    internal class PDUDataTest : IDisposable
    {
        private static SignalDecoder signalData = null;
        [SetUp]
        public void Setup()
        {
            Console.WriteLine("Test for creating PDU Payload Data:");
            signalData = new SignalDecoder();
        }

        [Test]
        public void CreatePduPayloadData_ReturnsPDU()
        {
            string pduTest1 = "0201AAFA";
            string pduTest2 = "0201";
            string pduTest3 = "031";
            string pduTest4 = "01";
            string pduTest5 = "0";

            var getPdu1 = signalData.CreatePduPayloadData(pduTest1);
            Assert.IsNotNull(getPdu1, "Check string pduTest1");

            var getPdu2 = signalData.CreatePduPayloadData(pduTest2);
            Assert.IsNotNull(getPdu2, "Check string pduTest2");

            var getPdu3 = signalData.CreatePduPayloadData(pduTest3);
            Assert.IsNotNull(getPdu3, "Check string pduTest3");

            var getPdu4 = signalData.CreatePduPayloadData(pduTest4);
            Assert.IsNotNull(getPdu4, "Check string pduTest4");

            var getPdu5 = signalData.CreatePduPayloadData(pduTest5);
            Assert.IsNull(getPdu5, "Check string pduTest5");
        }


        [Test]
        public void CreateSignalList_ReturnsListOfSignal()
        {
            string signalTest1 = "11DA";
            string signalTest2 = "01";
            string signalTest3 = "1";
            string signalTest4 = "";

            var getSignals1 = signalData.CreateSignalList(signalTest1);
            Assert.IsNotNull(getSignals1, "Check string signalTest1");

            var getSignals2 = signalData.CreateSignalList(signalTest2);
            Assert.IsNotNull(getSignals2, "Check string signalTest2");

            var getSignals3 = signalData.CreateSignalList(signalTest3);
            Assert.IsNotNull(getSignals3, "Check string signalTest3");

            var getSignals4 = signalData.CreateSignalList(signalTest4);
            Assert.IsNull(getSignals4, "Check string signalTest4");
        }

        [Test]
        public void GetSignal_ReturnsResults()
        {
            //signals : "02020101", "02012AFA", "010210" 
            
            string signal1 = "02012AFA";
            int expectedSignalDecimal = 42;
            string expectedSignalHex = "0x2A";
            int row1 = 1;


            var pduData = signalData.CreatePduPayloadData(signal1);
            Program.GetSignal(pduData, row1, out int actualSignalDecimal, out string actualSignalHex);

            Assert.That(actualSignalDecimal, Is.EqualTo(expectedSignalDecimal));
            Assert.That(actualSignalHex, Is.EqualTo(expectedSignalHex));
        }

        public void Dispose()
        {
            signalData.Dispose();
        }
    }
}
