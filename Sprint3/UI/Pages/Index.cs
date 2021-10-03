using BusinessLogic;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using UI.Observers;
using UI.Publishers;
using UI.Shared;

namespace UI.Pages
{
    public partial class Index
    {
        private readonly ReceivedObserver2 _receivedObserver;
        private readonly DecidedObserver _decidedObserver;
        private readonly GetRequestPublisher _getRequestPublisher = new(Variables.GetRequestHubSenderConnectionString);
        private readonly AssignedObserver _assignedObserver;
        private bool _loginDisplayed;
        private RequestView _rfsview;

        public static string UserName { get; set; }
        public static RequestForService _request { get; set; } = null;

        public Index()
        {
            try
            {
                _assignedObserver = new AssignedObserver(Variables.AssignedHubConsumerGroupStorageContainerName
                    , Variables.StorageAccountConnectionString
                    , Variables.AssignedHubName
                    , Variables.AssignedHubListenerConnectionString
                    , Variables.AssignedHubConsumerGroupName
                    , async (rfs) =>
                    {
                        if (rfs.AssignedToUser == UserName)
                        {
                            _request = rfs;
                            await UpdateRequestValue();
                            await InvokeAsync(StateHasChanged);
                        }
                    });

                _decidedObserver = new DecidedObserver(Variables.DecidedHubConsumerGroupStorageContainerName
                    , Variables.StorageAccountConnectionString
                    , Variables.DecidedHubName
                    , Variables.DecidedHubListenerConnectionString
                    , Variables.DecidedHubConsumerGroupName
                    , (rfs) =>
                    {
                        RequestForServiceMetrics.Add(rfs);
                    });

                _receivedObserver = new ReceivedObserver2(Variables.ReceivedHubConsumerGroupStorageContainerName
                    , Variables.StorageAccountConnectionString
                    , Variables.ReceivedHubName
                    , Variables.ReceivedHubListenerConnectionString
                    , Variables.ReceivedHubConsumerGroupName
                    , (rfs) =>
                    {
                        RequestForServiceMetrics.Add(rfs);
                    });

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task OnGetNextClick()
        {
            await _getRequestPublisher.RequestRequestAsync(UserName);
        }

        [Parameter]
        public EventCallback<RequestForService> RequestValueChanged { get; set; }

        public async Task GetRequestAsync()
        {
            await _getRequestPublisher.RequestRequestAsync(UserName);
        }

        public async Task GetUserDetailsAsync()
        {
            if (!_loginDisplayed)
            {
                _loginDisplayed = true;
                var dialog = DialogService.Show<LogonDialog>("Login");
                var result = await dialog.Result;
                var userDetails = result.Data as UserDetails;

                if (userDetails != null)
                {
                    UserName = userDetails.UserName;
                    await GetRequestAsync();
                    StateHasChanged();
                }
            }
        }

        private async Task StartListenersAsync()
        {
            await _assignedObserver.StartAsync();

            if (Variables.UseDashboard)
            {
                await _receivedObserver.StartAsync(default);
                await _decidedObserver.StartAsync(default);
            }
        }

        private async Task UpdateRequestValue()
        {
            await RequestValueChanged.InvokeAsync(_request);
        }
    }
}