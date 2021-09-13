using System.Threading.Tasks;
using UI.Observers;

namespace UI.Pages
{
    public partial class Index
    {
        private static ReceivedObserver receivedObserver = new();
        private static ApprovedObserver approvedObserver = new();
        private bool _loginDisplayed;

        public static string UserName { get; set; }

        public async Task GetUserDetailsAsync()
        {
            //if (string.IsNullOrWhiteSpace(UserName))
            if (!_loginDisplayed)
            {
                _loginDisplayed = true;
                var dialog = DialogService.Show<LogonDialog>("Login");
                var result = await dialog.Result;
                var userDetails = result.Data as UserDetails;

                UserName = userDetails.UserName;

                StateHasChanged();
            }
        }

        private async Task StartListenerAsync()
        {
            await receivedObserver.Start();
            await approvedObserver.Start();
        }
    }
}