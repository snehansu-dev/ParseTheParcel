using ParseTheParcel.Application.Models;


namespace ParseTheParcel.Application.Interfaces
{
    public interface IParcelRuleProvider
    {
        IReadOnlyList<ParcelRule> GetRules();
    }
}
