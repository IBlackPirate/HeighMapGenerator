using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace HeighMapGeneratorBot
{
    class MenuTreeMaker
    {
        private MenuTree menu;
        private TgBot bot;
        private MapCreator mapCreator;

        public static ReplyMarkupBase EmptyMurkup => new ReplyKeyboardRemove();
        public static ReplyKeyboardMarkup YesNoMarkup
        {
            get
            {
                var rkm = new ReplyKeyboardMarkup();
                rkm.Keyboard = new KeyboardButton[][]
                {
                    new[] { new KeyboardButton("Да") },
                    new[] { new KeyboardButton("Нет") }
                };
                return rkm;
            }
        }

        /// <summary>
        /// Создает меню для телеграм-бота
        /// </summary>
        /// <param name="tgBot">Бот, через которого можно происходит отправка сообщений</param>
        /// <returns></returns>
        public MenuTree CreateMenu(TgBot tgBot)
        {
            bot = tgBot;
            menu = new MenuTree(GetRoot());
            mapCreator = new MapCreator();
            InitializeNodes();
            return menu;
        }

        // Инициализация пунктов меню
        private void InitializeNodes()
        {
            var buildNode = GetBuildNode();
            var borderNode = GetBorderValueNode();
            var angleHeightNode = GetAngleHeightNode();
            menu.AddNode(GetSizeNode());
            menu.AddNode(GetSeedNode());
            menu.AddNode(GetRoughnessNode());
            menu.AddNode(GetBiomeNode());
            menu.AddNode(GetAskSmoothNode());
            menu.Tail.NextNodes.Add(borderNode);
            menu.AddNode(GetSmoothNode());
            menu.AddNode(borderNode);
            menu.Tail.NextNodes.Add(angleHeightNode);
            menu.AddNode(GetConstBorderNode());
            menu.AddNode(angleHeightNode);
            menu.Tail.NextNodes.Add(buildNode);
            menu.AddNode(GetLeftTopAngleHeightNode());
            menu.AddNode(GetLeftBottomAngleHeightNode());
            menu.AddNode(GetRightTopAngleHeightNode());
            menu.AddNode(GetRightBottomAngleHeightNode());
            menu.AddNode(buildNode);
            menu.AddNode(menu.Root);
        }

        #region RootNode
        private MenuTreeNode GetRoot()
        {
            var root = new MenuTreeNode("Приветствую тебя. Ты можешь:", GetMenuRootButtons());
            root.ButtonReaction = (msgArg) =>
            {
                switch (msgArg.Message.Text)
                {
                    case "Создать новую карту":

                        menu.Current = menu.Root.NextNodes.First();
                        menu.Current.PrintCurrentMessage(bot, msgArg);
                        break;
                    case "Вывести предыдущую созданную":
                        var chatId = msgArg.Message.Chat.Id;
                        DataBaseReaderWriter.TryGetMap(chatId, out Map map );
                        if (map == null)
                        {
                            bot.SendMessage(chatId, "Возникла ошибка чтения данных из базы. " +
                                "Возможно вы еще не создавали карту либо возникла ошибка доступа к серверу", EmptyMurkup);
                            break;
                        }
                        
                        bot.SendPhoto(chatId, map.ToHeightImage());
                        bot.SendPhoto(chatId, map.ColorMap.ToColorImage(map.SizeX, map.SizeY));

                        break;
                    default:
                        root.PrintCurrentMessage(bot, msgArg);
                        break;
                }
            };
            return root;
        }

        private ReplyMarkupBase GetMenuRootButtons()
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
        #region SizeNode
        private MenuTreeNode GetSizeNode()
        {
            var info = "Укажите размер карты, в пикселях. Размер должен соответствовать формуле 2^n + 1";
            var node = new MenuTreeNode(info, EmptyMurkup);
            node.ButtonReaction = (msgArg) =>
            {
                int size;
                if (TryGetSquareMapSize(out size, msgArg))
                {
                    mapCreator.MapSizeX = mapCreator.MapSizeY = size;
                    menu.Current = menu.Current.NextNodes.First();
                }
                menu.Current.PrintCurrentMessage(bot, msgArg);
            };
            return node;
        }

        private bool TryGetSquareMapSize(out int size, MessageEventArgs msgArg)
        {
            //Определяем, соответствует ли пришедшее число 2^n + 1
            if (int.TryParse(msgArg.Message.Text, out size))
                if (size > 0 && ((size - 1) & (size - 2)) == 0)
                    return true;
            return false;
        }
        #endregion
        #region BiomeNode
        private MenuTreeNode GetBiomeNode()
        {
            var info = "Укажите тип биома, в котором располагается местность";
            var node = new MenuTreeNode(info, GetBiomesButtons());
            node.ButtonReaction = (msgArg) =>
            {
                switch (msgArg.Message.Text)
                {
                    case "Лес":
                        mapCreator.Biome = BiomeType.Лес;
                        menu.Current = menu.Current.NextNodes.First();
                        break;
                    case "Пустыня":
                        mapCreator.Biome = BiomeType.Пустыня;
                        menu.Current = menu.Current.NextNodes.First();
                        break;
                }
                menu.Current.PrintCurrentMessage(bot, msgArg);
            };
            return node;
        }

        private ReplyMarkupBase GetBiomesButtons()
        {
            var rkm = new ReplyKeyboardMarkup();
            rkm.Keyboard = new KeyboardButton[][]
                {
                    new[] { new KeyboardButton(BiomeType.Пустыня.ToString()) },
                    new[] { new KeyboardButton(BiomeType.Лес.ToString()) },
                };
            return rkm;
        }
        #endregion
        #region SmoothNode
        private MenuTreeNode GetAskSmoothNode()
        {
            var node = new MenuTreeNode("Необходимо ли сглаживание?", YesNoMarkup);
            node.ButtonReaction = (msgArg) =>
            {
                switch (msgArg.Message.Text)
                {
                    case "Да":
                        menu.Current = menu.Current.NextNodes[1];
                        break;
                    case "Нет":
                        menu.Current = menu.Current.NextNodes[0];
                        break;
                }
                menu.Current.PrintCurrentMessage(bot, msgArg);
            };
            return node;
        }

        private MenuTreeNode GetSmoothNode()
        {
            var node = new MenuTreeNode("Введите интенсивность сглаживания - целое число от 0 до 5", EmptyMurkup);
            node.ButtonReaction = (msgArg) =>
            {
                int smoothness;
                if (int.TryParse(msgArg.Message.Text, out smoothness))
                {
                    mapCreator.Smoothness = Math.Abs(smoothness % 5);
                    menu.Current = menu.Current.NextNodes.First();
                }
                menu.Current.PrintCurrentMessage(bot, msgArg);
            };
            return node;
        }
        #endregion
        #region BorderNode
        private MenuTreeNode GetBorderValueNode()
        {
            var node = new MenuTreeNode("Как будет определяться высота точек по границе карты?", GetBorderMarkup());
            node.ButtonReaction = (msgArg) =>
            {
                switch (msgArg.Message.Text)
                {
                    case "Ввести константу":
                        mapCreator.IsRandomBorder = false;
                        menu.Current = menu.Current.NextNodes[1];
                        break;
                    case "Рандомно":
                        mapCreator.IsRandomBorder = true;
                        menu.Current = menu.Current.NextNodes[0];
                        break;
                }
                menu.Current.PrintCurrentMessage(bot, msgArg);
            };
            return node;
        }

        private ReplyKeyboardMarkup GetBorderMarkup()
        {
            var rkm = new ReplyKeyboardMarkup();
            rkm.Keyboard = new KeyboardButton[][]
                {
                    new[] { new KeyboardButton("Ввести константу") },
                    new[] { new KeyboardButton("Рандомно") },
                };
            return rkm;
        }

        private MenuTreeNode GetConstBorderNode()
        {
            var node = new MenuTreeNode("Введите значение - целое число от 0 до 255", EmptyMurkup);
            node.ButtonReaction = (msgArg) =>
            {
                byte borderValue;
                if (byte.TryParse(msgArg.Message.Text, out borderValue))
                {
                    mapCreator.DefaultBorderValue = borderValue;
                    menu.Current = menu.Current.NextNodes.First();
                }
                menu.Current.PrintCurrentMessage(bot, msgArg);
            };
            return node;
        }
        #endregion
        #region AngleNode
        private MenuTreeNode GetAngleHeightNode()
        {
            var node = new MenuTreeNode("Желаете указать высоты краев?", YesNoMarkup);
            node.ButtonReaction = (msgArg) =>
            {
                switch (msgArg.Message.Text)
                {
                    case "Да":
                        menu.Current = menu.Current.NextNodes[1];
                        break;
                    case "Нет":
                        menu.Current = menu.Current.NextNodes[0];
                        break;
                }
                menu.Current.PrintCurrentMessage(bot, msgArg);
            };
            return node;
        }

        private MenuTreeNode GetLeftTopAngleHeightNode()
        {
            var node = new MenuTreeNode("Введите высоту левого верхнего угла - целое число от 0 до 255", EmptyMurkup);
            node.ButtonReaction = (msgArg) =>
            {
                byte angleValue;
                if (byte.TryParse(msgArg.Message.Text, out angleValue))
                {
                    mapCreator.LeftTop = angleValue;
                    menu.Current = menu.Current.NextNodes.First();
                }
                menu.Current.PrintCurrentMessage(bot, msgArg);
            };
            return node;
        }

        private MenuTreeNode GetLeftBottomAngleHeightNode()
        {
            var node = new MenuTreeNode("Введите высоту левого нижнего угла - целое число от 0 до 255", EmptyMurkup);
            node.ButtonReaction = (msgArg) =>
            {
                byte angleValue;
                if (byte.TryParse(msgArg.Message.Text, out angleValue))
                {
                    mapCreator.LeftBottom = angleValue;
                    menu.Current = menu.Current.NextNodes.First();
                }
                menu.Current.PrintCurrentMessage(bot, msgArg);
            };
            return node;
        }


        private MenuTreeNode GetRightTopAngleHeightNode()
        {
            var node = new MenuTreeNode("Введите высоту правого верхнего угла - целое число от 0 до 255", EmptyMurkup);
            node.ButtonReaction = (msgArg) =>
            {
                byte angleValue;
                if (byte.TryParse(msgArg.Message.Text, out angleValue))
                {
                    mapCreator.RightTop = angleValue;
                    menu.Current = menu.Current.NextNodes.First();
                }
                menu.Current.PrintCurrentMessage(bot, msgArg);
            };
            return node;
        }

        private MenuTreeNode GetRightBottomAngleHeightNode()
        {
            var node = new MenuTreeNode("Введите высоту правого нижнего угла - целое число от 0 до 255", EmptyMurkup);
            node.ButtonReaction = (msgArg) =>
            {
                byte angleValue;
                if (byte.TryParse(msgArg.Message.Text, out angleValue))
                {
                    mapCreator.RightBottom = angleValue;
                    menu.Current = menu.Current.NextNodes.First();
                }
                menu.Current.PrintCurrentMessage(bot, msgArg);
            };
            return node;
        }
        #endregion
        #region BuidNode
        private MenuTreeNode GetBuildNode()
        {
            var node = new MenuTreeNode("Все готово", GetBuildMarkup());
            node.ButtonReaction = (msgArg) =>
            {
                if (msgArg.Message.Text == "Создать карту")
                {
                    var chatId = msgArg.Message.Chat.Id;
                    bot.SendMessage(chatId, "Начался процесс создания. Он может идти длительное время", EmptyMurkup);

                    var generator = mapCreator.CreateGenerator();
                    var map = generator.GenerateMap();
                    var heightMap = map.ToHeightImage();
                    var colorMap = map.GenerateColor(mapCreator.Biome, mapCreator.Smoothness);

                    bot.SendPhoto(chatId, heightMap);
                    bot.SendPhoto(chatId, colorMap);
                    Thread.Sleep(2000);
                    if(!DataBaseReaderWriter.TryAddMap(map, chatId))
                    {
                        bot.SendMessage(chatId, "Сервер временно недоступен, ваша карта не будет сохранена. " +
                            "Позже вы сможете сгенерировать ее заново используя эти же параметры", EmptyMurkup);
                    }
                    menu.Current = menu.Current.NextNodes.First();
                }
                menu.Current.PrintCurrentMessage(bot, msgArg);
            };
            return node;
        }

        private ReplyKeyboardMarkup GetBuildMarkup()
        {
            var rkm = new ReplyKeyboardMarkup();
            rkm.Keyboard = new KeyboardButton[][] { new[] { new KeyboardButton("Создать карту") } };
            return rkm;
        }
        #endregion

        private MenuTreeNode GetSeedNode()
        {
            var info = "Укажите сид - целое число, влияющее на рандомизированные значения";
            var node = new MenuTreeNode(info, EmptyMurkup);
            node.ButtonReaction = (msgArg) =>
            {
                int seed;
                if (int.TryParse(msgArg.Message.Text, out seed))
                {
                    mapCreator.Seed = seed;
                    menu.Current = menu.Current.NextNodes.First();
                }
                menu.Current.PrintCurrentMessage(bot, msgArg);
            };
            return node;
        }

        private MenuTreeNode GetRoughnessNode()
        {
            var info = "Введите шерховатость карты - дробное значение. Например: 1,6. На больших картах низкое значение шерховатости " +
                "приведет к преобладания равнинной местности, а высокое значение на маленьких - к слишком резким перепадам высоты";
            var node = new MenuTreeNode(info, EmptyMurkup);
            node.ButtonReaction = (msgArg) =>
            {
                float roughness;
                if (float.TryParse(msgArg.Message.Text, out roughness))
                {
                    mapCreator.Roughness = roughness;
                    menu.Current = menu.Current.NextNodes.First();
                }
                menu.Current.PrintCurrentMessage(bot, msgArg);
            };
            return node;
        }
    }
}
