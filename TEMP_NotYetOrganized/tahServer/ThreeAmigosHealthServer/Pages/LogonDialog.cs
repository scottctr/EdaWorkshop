using Microsoft.AspNetCore.Components;
using System;

namespace ThreeAmigosHealthServer.Pages
{
    public partial class LogonDialog
    {
        private string _userName;

        [Parameter]
        public string UserName
        {
            get => _userName;
            set
            {
                if (_userName == value) return;
                _userName = value;
                UserNameChanged.InvokeAsync(value);
                Console.WriteLine("Dialog: UserName -> " + value);
            }
        }

        [Parameter]
        public EventCallback<string> UserNameChanged { get; set; }

        [Parameter]
        public string Discipline { get; set; }
    }
}