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
    // Каждый пукт меню представлен нодой со своей функциональностью
    class MenuTreeNode
    {
        // Сообщение, которое отправляется пользователю
        public string CurrentMessage;
        // Кнопки, которые видит пользователь
        public ReplyMarkupBase CurrentButtons;

        // После одного пункта меню может следовать сразу несколько,
        // в зависимости от выбора пользователя
        public List<MenuTreeNode> NextNodes;
        // Реакция на выбор/ввод пользователя
        public Action<MessageEventArgs> ButtonReaction;

        /// <summary>
        /// Вывод текущего сообщения пользователю, своебразное представление
        /// </summary>
        /// <param name="bot">через что отправляем сообщение</param>
        /// <param name="msgArg">откуда сообщение пришло</param>
        public void PrintCurrentMessage(TgBot bot, MessageEventArgs msgArg)
        {
            bot.SendMessage(msgArg.Message.Chat.Id, CurrentMessage, CurrentButtons);
        }

        /// <summary>
        /// Инициализация ноды
        /// </summary>
        /// <param name="message">сообщение, которое увидит пользователь</param>
        /// <param name="buttons">кнопки, которые отправятся пользователю</param>
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

    // Меню представляет из себя дерево, состоящее из узлов - пунктов меню
    class MenuTree
    {
        // Начальный пункт меню
        public readonly MenuTreeNode Root;
        // Текущий пункт меню
        public MenuTreeNode Current;
        // Последний пункт меню
        public MenuTreeNode Tail;

        public MenuTree(MenuTreeNode root)
        {
            Root = Current = Tail = root;
        }

        /// <summary>
        /// Добавляет узел в самый конец меню
        /// </summary>
        /// <param name="node">новый пункт меню</param>
        public void AddNode(MenuTreeNode node)
        {
            Tail.NextNodes.Add(node);
            Tail = node;
        }
    }
}
