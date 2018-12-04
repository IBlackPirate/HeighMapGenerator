using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace HeighMapGeneratorBot
{
    public class TgBot
    {
        static ITelegramBotClient botClient;
        static IEnumerator<Tuple<string, ReplyMarkupBase>> gameKeys;

        static CountdownEvent countdown;
        static int upCount = 0;
        static object lockObj = new object();
        const bool resolveNames = true;
        static MessageEventArgs message;

        static void Bot()
        {
            botClient = new TelegramBotClient("666935188:AAH68Z3CWZ9gGsiH37CAlxSjzteDW3QwTL8");
            var me = botClient.GetMeAsync().Result;

            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();
            Thread.Sleep(int.MaxValue);
        }

        static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            await botClient.SendTextMessageAsync(
                chatId: e.Message.Chat,
                text: "Приветствую тебя. Что бы ты хотел сделать?");
        }


        static ReplyKeyboardMarkup GetMenu()
        {
            var rkm = new ReplyKeyboardMarkup();
            rkm.Keyboard =
                new KeyboardButton[][]
                {
                    new[] { new KeyboardButton("Вывести все имена") },
                    new[] { new KeyboardButton("Начать сначала") },
                    new[] { new KeyboardButton("Удалить запись") },
                    new[] { new KeyboardButton("IP") }
                };
            return rkm;
        }
    }
}