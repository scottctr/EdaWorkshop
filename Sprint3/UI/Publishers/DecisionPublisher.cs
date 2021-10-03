using BusinessLogic;
using System.Text.Json;
using System.Threading.Tasks;

namespace UI.Publishers
{
    public class DecisionPublisher: Publisher
    {
        public DecisionPublisher(string hubSenderConnectionString)
            : base(hubSenderConnectionString)
        { }

        public async Task SendDecidedRequestAsync(RequestForService rfs)
        {
            var commandJson = JsonSerializer.Serialize(rfs);
            await SendAsync(commandJson);
        }
    }
}
