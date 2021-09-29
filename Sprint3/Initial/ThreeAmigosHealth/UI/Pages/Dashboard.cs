using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace UI.Pages
{
    public partial class Dashboard
    {
        private Timer _timer;
        private int Index = -1; //default value cannot be 0 -> first selectedindex is 0.
        int dataSize = 2;
        string[] labels = { "Undecided", "Auto Approved" };
        private List<ChartSeries> _series;
        private string[] _xAxisLabels;

        public Dashboard()
        {
            UpdateChartData();

            _timer = new(4000);
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            UpdateChartData();
            InvokeAsync(StateHasChanged);
        }

        public void UpdateChartData()
        {
            _series = new()
            {
                new ChartSeries { Name = "Total Received", Data = RequestForServiceMetrics.GetAllCountsByMinute().OrderBy(e => e.Hour).Select(e => Convert.ToDouble(e.Count)).ToArray() },
                new ChartSeries { Name = "Auto Approvals", Data = RequestForServiceMetrics.GetAutoApprovalCountsByMinute().OrderBy(e => e.Hour).Select(e => Convert.ToDouble(e.Count)).ToArray() },
            };

            _xAxisLabels = RequestForServiceMetrics.GetAllCountsByMinute().OrderBy(e => e.Hour).Select(e => $":{e.Hour % 100}").ToArray();
        }
    }
}
