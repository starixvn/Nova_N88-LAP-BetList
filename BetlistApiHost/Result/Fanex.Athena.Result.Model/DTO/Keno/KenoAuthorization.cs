namespace Fanex.Athena.Result.Model.DTO.Keno
{
    public class KenoAuthorization
    {
        public int ErrorCode { get; set; }

        public string ErrorMsg { get; set; }

        public KenoAuthorizedData Data { get; set; }
    }
}
