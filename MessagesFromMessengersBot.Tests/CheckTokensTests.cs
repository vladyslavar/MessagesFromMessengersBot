using Microsoft.VisualStudio.TestTools.UnitTesting;
using MessagesFromMessengersBot.ChekingIncomingMsgs;

namespace MessagesFromMessengersBot.Tests
{
    [TestClass]
    public class CheckTokensTests
    {
        [TestMethod]
        public void TokensReddit_message_key()
        {
            //arrange
            string message = "key_r:sometoken12345";
            string expectedKey = "sometoken12345";
            long randomId = 74123852;

            //act
            CheckTokens checkTokens = new CheckTokens();
            var actualKey = checkTokens.Tokens(message, randomId);

            //assert
            Assert.AreEqual(expectedKey, actualKey);
        }
        /*
        [TestMethod]
        public void TokensTwitter_message_key()
        {
            //arrange
            string message = "key_t:sometoken12345";
            string expectedKey = "sometoken12345";
            long randomId = 74123852;

            //act
            CheckTokens checkTokens = new CheckTokens();
            var actualKey = checkTokens.Tokens(message, randomId);

            //assert
            Assert.AreEqual(expectedKey, actualKey);
        }*/
    }
}
