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

        private MenuTree menu;

        public void StartBot()
        {
            botClient = new TelegramBotClient("666935188:AAH68Z3CWZ9gGsiH37CAlxSjzteDW3QwTL8");
            menu = MenuTreeMaker.CreateMenu(this);
            var me = botClient.GetMeAsync().Result;
            botClient.StartReceiving();

            botClient.OnMessage += Bot_OnMessage;
            Thread.Sleep(int.MaxValue);
        }

        async void Bot_OnMessage(object sender, MessageEventArgs msg)
        {
            menu.Current.ButtonReaction(msg);
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
            SendMessage(chatId, "Вы получили фото", MenuTreeMaker.EmptyMurkup);
        }
    }
}