using Coravel.Invocable;
using System.Threading.Tasks;

namespace driving_licence_token
{
    public class BlockJob : IInvocable
    {
        public Task Invoke()
        {
            Blockchain.BuildNewBlock();
            return Task.CompletedTask;
        }
    }
}