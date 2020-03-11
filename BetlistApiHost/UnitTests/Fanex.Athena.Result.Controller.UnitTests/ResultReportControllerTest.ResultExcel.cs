namespace Fanex.Athena.Result.Controller.UnitTests
{
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Text;
    using Fanex.Result.Core.Sportsbook.DTO;
    using Model.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;

    [TestFixture]
    public partial class ResultReportControllerTest
    {
        [Test]
        public void ResultExcel_InvalidModelState_ReturnBadRequestResponse()
        {
            using (var controller = CreateController())
            {
                // Arrange
                controller.ModelState.AddModelError("error", "hihi");
                var response = controller.HttpContext.Response;

                // Action
                controller.ResultExcel(new ResultMetaModel());

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(response.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
                    Assert.That(response.StatusDescription, Is.EqualTo(ViewResources.InvalidParams));
                });
            }
        }

        [Test]
        public void ResultExcel_ValidModelState_ReturnCorrectResponse()
        {
            // Arrange
            const string ExpectedStreamData = "Hello everyone";
            using (var controller = CreateController())
            using (var excelStream = new MemoryStream(Encoding.UTF8.GetBytes(ExpectedStreamData)))
            using (var outputStream = new MemoryStream())
            {
                var response = controller.HttpContext.Response;
                response.OutputStream.Returns(outputStream);
                _resultService.ExcelParser(new List<Sportsbook>(), null).ReturnsForAnyArgs(excelStream);

                // Action
                controller.ResultExcel(new ResultMetaModel());

                // Assert
                Assert.That(Encoding.UTF8.GetString(((MemoryStream)response.OutputStream).ToArray()), Is.EqualTo(ExpectedStreamData));
            }
        }
    }
}