using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using System.Threading;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace HeighMapGeneratorBot
{
    class TgBot
    {
        private ITelegramBotClient botClient;
        private MessageEventArgs message;

        static CountdownEvent countdown;
        static int upCount = 0;
        static object lockObj = new object();
        const bool resolveNames = true;

        private Dictionary<long, MenuTree> menuToUser;

        public void StartBot()
        {
            botClient = new TelegramBotClient("666935188:AAH68Z3CWZ9gGsiH37CAlxSjzteDW3QwTL8");
            var me = botClient.GetMeAsync().Result;
            botClient.StartReceiving();
            menuToUser = new Dictionary<long, MenuTree>();

            botClient.OnMessage += Bot_OnMessage;
            Thread.Sleep(int.MaxValue);
        }

        async void Bot_OnMessage(object sender, MessageEventArgs msgArg)
        {
            var id = msgArg.Message.Chat.Id;
            if (!menuToUser.ContainsKey(id))
                menuToUser[id] = MenuTreeMaker.CreateMenu(this);
            menuToUser[id].Current.ButtonReaction(msgArg);
        }

        public async void SendMessage(long chatId, string text, ReplyMarkupBase buttons)
        {
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: text,
                replyMarkup: buttons);
        }

        public async void SendPhoto(long chatId, Bitmap image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png);
                ms.Position = 0;
                await botClient.SendPhotoAsync(chatId, new Telegram.Bot.Types.InputFiles.InputOnlineFile(ms));
            }
        }
    }
}