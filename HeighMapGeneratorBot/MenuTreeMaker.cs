using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace HeighMapGeneratorBot
{
    class MenuTreeMaker
    {
        private static MenuTree menu;
        private static TgBot tgBot;

        public static MenuTree CreateMenu(TgBot bot)
        {
            tgBot = bot;
            menu = new MenuTree(GetRoot());
            return menu;
        }

        #region RootMenu
        static MenuTreeNode GetRoot()
        {
            var root = new MenuTreeNode(string.Empty, GetMenuRootButtons());
            root.ButtonReaction = (msgArg) =>
            {
                switch (msgArg.Message.Text)
                {
                    case "Создать новую карту":
                        menu.Current = menu.Root.NextNodes.First();
                        break;
                    case "Вывести предыдущую созданную":
                        var map = DataBaseReaderWriter.GetMap(msgArg.Message.Chat.Id);
                        var chatId = msgArg.Message.Chat.Id;
                        tgBot.SendPhoto(chatId, map.ToHeightImage());
                        tgBot.SendPhoto(chatId, map.ColorMap.ToColorImage(map.SizeX, map.SizeY));
                        break;
                    default:
                        root.PrintCurrentMessage(tgBot, msgArg);
                        break;
                }
            };
            return root;
        }

        static ReplyMarkupBase GetMenuRootButtons()
        {
            var rkm = new ReplyKeyboardMarkup();
            rkm.Keyboard = new KeyboardButton[][]
                {
                    new[] { new KeyboardButton("Создать новую карту") },
                    new[] { new KeyboardButton("Вывести предыдущую созданную") },
                };
            return rkm;
        }
        #endregion

        static MenuTreeNode GetCreationMapNode()
        {
            throw new NotImplementedException();
        } 
    }
}
