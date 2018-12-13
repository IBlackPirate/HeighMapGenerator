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
            //await botClient.SendPhotoAsync();
        }

        //void InitializeButtons()
        //{
        //    MapMessages = new List<Tuple<string, Func<ReplyMarkupBase>>>();
        //    MapMessages[0] = Tuple.Create<string, Func<ReplyMarkupBase>>("Укажите размер карты. Размер должен соответствовать формуле 2^n + 1", GetEmptyMarkup);
        //    MapMessages[1] = Tuple.Create<string, Func<ReplyMarkupBase>>("Укажите сид - число, влияющее на рандомизированные значения", GetEmptyMarkup);
        //    MapMessages["Укажите тип биома, в котором располагается местность"] = GetBiomes;
        //    MapMessages["Необходимо ли сглаживание?"] = GetYesNoButtons;
        //    MapMessages["Желаете ли указать высоты краев карты?"] = GetYesNoButtons;
        //    MapMessages["Введите высоту левого верхнего угла"] = () => new ReplyKeyboardRemove();
        //    MapMessages["Введите высоту левого нижнего угла"] = () => new ReplyKeyboardRemove();
        //    MapMessages["Введите высоту правого верхнего угла"] = () => new ReplyKeyboardRemove();
        //    MapMessages["Введите высоту правого нижнего угла"] = () => new ReplyKeyboardRemove();
        //}

        bool TryGetSquareMapSize(out int size)
        {
            //Определяем, соответствует ли пришедшее число 2^n + 1
            if (int.TryParse(message.Message.Text, out size))
                if (size > 0 && ((size - 1) & (size - 2)) == 0)
                    return true;
            return false;
        }

        ReplyMarkupBase CreateMap(int size)
        {
            var rkm = new ReplyKeyboardRemove();
            return rkm;
        }

        ReplyMarkupBase GetBiomes()
        {
            throw new NotImplementedException();
        }

        ReplyMarkupBase GetYesNoButtons()
        {
            var rkm = new ReplyKeyboardMarkup();
            rkm.Keyboard = new KeyboardButton[][]
                {
                    new[] { new KeyboardButton("Да") },
                    new[] { new KeyboardButton("Нет") },
                };
            return rkm;
        }

        private ReplyMarkupBase GetEmptyMarkup() => new ReplyKeyboardRemove();
    }
}