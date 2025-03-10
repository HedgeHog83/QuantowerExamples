// Copyright QUANTOWER LLC. © 2017-2023. All rights reserved.

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TradingPlatform.BusinessLayer;

namespace Webhook_Strategy_Example
{
    /// <summary>
    /// An example of blank strategy. Add your code, compile it and run via Strategy Runner panel in the assigned trading terminal.
    /// Information about API you can find here: http://api.quantower.com
    /// Code samples: https://github.com/Quantower/Examples 
    /// </summary>
    
    public class Webhook_Strategy_Example : Strategy
    {
        private CancellationTokenSource cancellationToken;

        private event Action<string> OnWebhookMessageReceived;

        public Webhook_Strategy_Example()
            : base()
        {
            this.Name = "Webhook_Strategy_Example";
            this.Description = "Strategy for demonstrating webhook capabilities";
        }

        protected override void OnRun()
        {         
            this.cancellationToken = new CancellationTokenSource();

            Thread webhookListenerThread = new Thread(async () => await this.StartListening(this.cancellationToken.Token));
            webhookListenerThread.Start();

            OnWebhookMessageReceived += this.ProcessWebhookMessage;
        }

        protected override void OnStop()
        {
            OnWebhookMessageReceived -= this.ProcessWebhookMessage;
            this.cancellationToken.Cancel();
        }

        private void ProcessWebhookMessage(string message)
        {
            Log($"Webhook get: {message}");           
        }

        private async Task StartListening(CancellationToken token)
        {
            string url = "http://localhost:5000/";

            HttpListener webhookListener = new HttpListener();

            try
            {
                webhookListener.Prefixes.Add(url);
                webhookListener.Start();

                Log("Webhook listener started");

                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        var contextTask = webhookListener.GetContextAsync();
                        var completedTask = await Task.WhenAny(contextTask, Task.Delay(Timeout.Infinite, token));

                        if (completedTask == contextTask)
                        {
                            HttpListenerContext context = await contextTask;
                            HttpListenerRequest request = context.Request;

                            using (StreamReader reader = new StreamReader(request.InputStream, Encoding.UTF8))
                            {
                                string message = reader.ReadToEnd();
                                OnWebhookMessageReceived?.Invoke(message);
                            }
                        }
                        else
                            break;

                    }
                    catch (Exception ex)
                    {
                        Log($"Webhook error: {ex.Message}", StrategyLoggingLevel.Error);
                    }
                }
            }
            finally
            {
                if(webhookListener.IsListening)
                {
                    webhookListener.Stop();
                    webhookListener.Close();
                }

                Log("Webhook listener stopped");
            }
        }
    }
}