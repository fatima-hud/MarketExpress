namespace MarketExpress.Core.Results
{
    public enum ResultFailureType
    {
        None = 0,
        NotFound = 1,
        BadRequest = 2,
        Unauthorized = 3,
        Forbidden = 4,
        Conflict = 5,
        InternalError = 6
    }
}
