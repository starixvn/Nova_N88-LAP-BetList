namespace Fanex.Athena.BetList.Model.MetaModel
{
    public interface ICommonValueMetaModel
    {
        string BaseCurrency { get; set; }
        int PageIndex { get; set; }
        int PageSize { get; set; }
        int CustId { get; set; }
    }
}
