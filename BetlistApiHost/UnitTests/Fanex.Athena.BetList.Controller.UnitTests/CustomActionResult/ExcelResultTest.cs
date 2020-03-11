namespace Fanex.Athena.BetList.Controller.UnitTests.CustomActionResult
{
    using Controller.CustomActionResult;
    using NPOI.SS.UserModel;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    public class ExcelResultTest
    {
        [Test]
        public void ExcelResult_WhenCall_WriteWorkBookToOutputStream()
        {
            // Arrange
            var hssfWorkbook = Substitute.For<IWorkbook>();
            var context = MvcHelper.CreateControllerContext();

            // Action
            (new ExcelResult(hssfWorkbook)).ExecuteResult(context);

            // Assert
            hssfWorkbook.Received(1).Write(context.HttpContext.Response.OutputStream);
        }
    }
}