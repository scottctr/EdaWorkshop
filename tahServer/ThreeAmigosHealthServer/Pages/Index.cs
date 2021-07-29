using MudBlazor;
using System.Threading.Tasks;

namespace ThreeAmigosHealthServer.Pages
{
    public partial class Index
    {
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
    }
}