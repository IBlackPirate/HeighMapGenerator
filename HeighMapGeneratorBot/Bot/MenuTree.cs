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
    class MenuTreeNode
    {
        public string CurrentMessage;
        public ReplyMarkupBase CurrentButtons;

        public List<MenuTreeNode> NextNodes;
        public Action<MessageEventArgs> ButtonReaction;

        public void PrintCurrentMessage(TgBot bot, MessageEventArgs msg)
        {
            bot.SendMessage(msg.Message.Chat.Id, CurrentMessage, CurrentButtons);
        }

        public MenuTreeNode(string message, ReplyMarkupBase buttons)
        {
            CurrentMessage = message;
            CurrentButtons = buttons;
            NextNodes = new List<MenuTreeNode>();
            ButtonReaction = (x) => Console.WriteLine($"received {x.Message.Text}, action is empty");
        }
    }

    class MenuTree
    {
        public readonly MenuTreeNode Root;
        public MenuTreeNode Current;

        public MenuTree(MenuTreeNode root)
        {
            Root = root;
            Current = Root;
        }
    }
}
