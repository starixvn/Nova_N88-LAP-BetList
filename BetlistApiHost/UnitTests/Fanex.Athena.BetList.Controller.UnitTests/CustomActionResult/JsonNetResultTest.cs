namespace Fanex.Athena.BetList.Controller.UnitTests.CustomActionResult
{
    using System;
    using System.Text;
    using Controller.CustomActionResult;
    using Newtonsoft.Json;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    public class JsonNetResultTest
    {
        [Test]
        public void JsonNetResult_ContextIsNull_ThrowArgumentException()
        {
            // Arrange
            var jsonResult = new JsonNetResult
            {
                Data = FakeHelper.Create<string>()
            };

            // Action
            TestDelegate testDelegate = () => jsonResult.ExecuteResult(null);

            // Assert
            Assert.That(testDelegate, Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void JsonNetResult_ContentTypeIsEmpty_UpdateContentTypeToJson()
        {
            // Arrange
            var context = MvcHelper.CreateControllerContext();
            context.HttpContext.Response.ContentType = string.Empty;
            var jsonResult = new JsonNetResult
            {
                Data = FakeHelper.Create<string>()
            };

            // Action
            jsonResult.ExecuteResult(context);

            // Assert
            Assert.That(context.HttpContext.Response.ContentType, Is.EqualTo("application/json"));
        }

        [Test]
        public void JsonNetResult_ContentEncodingIsNotNull_UpdateResponseContentEncoding()
        {
            // Arrange
            var context = MvcHelper.CreateControllerContext();
            var jsonResult = new JsonNetResult
            {
                Data = FakeHelper.Create<string>(),
                ContentEncoding = Encoding.ASCII
            };

            // Action
            jsonResult.ExecuteResult(context);

            // Assert
            Assert.That(context.HttpContext.Response.ContentEncoding, Is.EqualTo(jsonResult.ContentEncoding));
        }

        [Test]
        public void JsonNetResult_WhenCall_WriteSerializeObjectToOuputStream()
        {
            // Arrange
            var context = MvcHelper.CreateControllerContext();
            var jsonResult = new JsonNetResult
            {
                Data = FakeHelper.CreateMany<int>(2),
                ContentType = "application/json"
            };

            // Action
            jsonResult.ExecuteResult(context);

            // Assert
            context.HttpContext.Response.Received(1).Write(JsonConvert.SerializeObject(jsonResult.Data, Formatting.Indented));
        }
    }
}