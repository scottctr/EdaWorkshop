using AutoApprovalService.AutoApprovalService;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoApprovalService
{
    public static class Function1
    {
        private static Random _randomizer = new Random();

        [FunctionName("Function1")]
        public static async Task Run([EventHubTrigger("requestreceived", Connection = "EventHubListenerConnString", ConsumerGroup = "autoapprovalreceivedrequestconsumer")] EventData[] events, ILogger log)
        {
            var exceptions = new List<Exception>();

            foreach (EventData eventData in events)
            {
                try
                {
                    string messageBody = Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);

                    if (_randomizer.Next(0, 2) == 1)
                    {
                        //Mark it as auto approved
                        messageBody = messageBody.Replace(",\"Status\":0}", ",\"Status\":4}");

                        await Publisher.SendApprovalAsync(messageBody);
                        log.LogInformation($"Message approved: {messageBody}");
                    }
                    else
                    {
                        await Publisher.SendNonApprovalAsync(messageBody);
                        log.LogInformation($"Message not approved: {messageBody}");
                    }
                    log.LogInformation($"C# Event Hub trigger function processed a message: {messageBody}");
                    await Task.Yield();
                }
                catch (Exception e)
                {
                    // We need to keep processing the rest of the batch - capture this exception and continue.
                    // Also, consider capturing details of the message that failed processing so it can be processed again later.
                    exceptions.Add(e);
                }
            }

            // Once processing of the batch is complete, if any messages in the batch failed processing throw an exception so that there is a record of the failure.

            if (exceptions.Count > 1)
                throw new AggregateException(exceptions);

            if (exceptions.Count == 1)
                throw exceptions.Single();
        }
    }
}
