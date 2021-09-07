using System.Threading.Tasks;
using ThreeAmigosHealthServer.Observers;

namespace ThreeAmigosHealthServer.Pages
{
    public partial class Index
    {
        private static ReceivedObserver receivedObserver = new();
        private static ApprovedObserver approvedObserver = new();
        public static string UserName { get; set; }
        public static MedicalDiscipline Discipline { get; set; }

        public async Task GetUserDetailsAsync()
        {
            if (string.IsNullOrWhiteSpace(UserName))
            {
                var dialog = DialogService.Show<LogonDialog>("Login");
                var result = await dialog.Result;
                var userDetails = result.Data as UserDetails;

                UserName = userDetails.UserName;
                Discipline = userDetails.Discipline;

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