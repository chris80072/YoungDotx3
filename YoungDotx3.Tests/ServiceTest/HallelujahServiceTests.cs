using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YoungDotx3.App_Start;
using YoungDotx3.DAO.MySQL;
using YoungDotx3.Domain;
using YoungDotx3.Domain.Hallelujah;
using YoungDotx3.Service;

namespace YoungDotx3.Tests.ServiceTest
{
    public class HallelujahServiceTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            DBConfig.Register();
            //DefaultValueConfig.RegisterElasticSearchConfig();
        }

        [TestMethod]
        public void TestCreateMessage()
        {
            using (TransactionScopeEx.CreateTransactionScope())
            {
                Message message = new Message
                {
                    Nickname = "tester",
                    Content = "test content"
                };
                HallelujahService service = new HallelujahService();
                service.CreateMessage(message);
                Assert.AreNotEqual(string.Empty, message.Id);

                var result = service.GetMessage(message.Id);
                Assert.AreNotEqual(null, result);
                Assert.AreEqual(message.Id, result.Id);
                Assert.AreEqual(message.Nickname, result.Nickname);
                Assert.AreEqual(message.Content, result.Content);
                Assert.AreNotEqual(null, result.CreateDate);
                Assert.AreEqual(false, result.IsDelete);
            }
        }
    }
}
