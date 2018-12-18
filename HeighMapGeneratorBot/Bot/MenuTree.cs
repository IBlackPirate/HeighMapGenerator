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

        public void PrintCurrentMessage(TgBot bot, MessageEventArgs msgArg)
        {
            bot.SendMessage(msgArg.Message.Chat.Id, CurrentMessage, CurrentButtons);
        }

        public MenuTreeNode(string message, ReplyMarkupBase buttons)
        {
            CurrentMessage = message;
            CurrentButtons = buttons;
            NextNodes = new List<MenuTreeNode>();
            ButtonReaction = (x) => Console.WriteLine($"received {x.Message.Text}, action is empty");
        }

        public override string ToString()
        {
            return CurrentMessage;
        }
    }

    class MenuTree
    {
        public readonly MenuTreeNode Root;
        public MenuTreeNode Current;
        public MenuTreeNode Tail;

        public MenuTree(MenuTreeNode root)
        {
            Root = Current = Tail = root;
        }

        public void AddNode(MenuTreeNode node)
        {
            Tail.NextNodes.Add(node);
            Tail = node;
        }
    }
}
