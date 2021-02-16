using System;
using System.Collections.Generic;
using System.Text;
using Mr.LINEbot.Configurations;
using LineDC.Messaging;
using LineDC.Messaging.Webhooks;
using LineDC.Messaging.Webhooks.Events;
using LineDC.Messaging.Webhooks.Messages;
using Microsoft.Azure.WebJobs.Logging;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Mr.LINEbot
{
    class LineBotApp : WebhookApplication
    {
        private ILogger Logger { get; }

        public LineBotApp(ILineMessagingClient lineMessagingClient, LineBotSettings settings, ILoggerFactory loggerFactory)
            : base(lineMessagingClient, settings.ChannelSecret)
        {
            Logger = loggerFactory.CreateLogger(LogCategories.CreateFunctionUserCategory(nameof(WebhookEndpoint)));
        }

        protected override async Task OnMessageAsync(MessageEvent ev)
        {
            Logger?.LogTrace($"OnMessageAsync => Type: {ev.Source.Type}, Id: {ev.Source.Id}");
            switch (ev.Message)
            {
                case TextEventMessage textMessage:
                    await Client.ReplyMessageAsync(ev.ReplyToken, textMessage.Text);
                    break;
                case MediaEventMessage mediaMessage:
                    await Client.ReplyMessageAsync(ev.ReplyToken, $"contentProvider: {mediaMessage.ContentProvider}");
                    break;
                case FileEventMessage fileMessage:
                    await Client.ReplyMessageAsync(ev.ReplyToken, $"filename: {fileMessage.FileName}");
                    break;
                case LocationEventMessage locationMessage:
                    await Client.ReplyMessageAsync(ev.ReplyToken, $"{locationMessage.Title}({locationMessage.Latitude}, {locationMessage.Longitude})");
                    break;
                case StickerEventMessage stickerMessage:
                    await Client.ReplyMessageAsync(ev.ReplyToken, $"sticker id: {stickerMessage.PackageId}-{stickerMessage.StickerId}");
                    break;
            }
        }
    }
}
