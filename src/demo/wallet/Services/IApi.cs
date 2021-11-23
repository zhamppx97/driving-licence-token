using System.Threading.Tasks;

namespace wallet.Services
{
    interface IApi
    {
        Task<string> GetBalanceToken(string address);
    }
}
