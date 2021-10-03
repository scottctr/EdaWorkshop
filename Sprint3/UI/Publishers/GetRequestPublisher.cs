using BusinessLogic;
using System.Text.Json;
using System.Threading.Tasks;

namespace UI.Publishers
{
    public class GetRequestPublisher: Publisher
    {
        public GetRequestPublisher(string hubSenderConnectionString)
            : base(hubSenderConnectionString)
        { }

        public async Task RequestRequestAsync(string userName)
        {
            var command = new GetRequestForServiceCommand(userName);
            var commandJson = JsonSerializer.Serialize(command);

            await SendAsync(commandJson);
        }
    }
}
