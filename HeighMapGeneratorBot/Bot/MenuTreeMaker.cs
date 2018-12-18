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
        private static MenuTree menu;
        private static TgBot bot;

        public static MenuTree CreateMenu(TgBot tgBot)
        {
            bot = tgBot;
            menu = new MenuTree(GetRoot());
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
            return menu;
        }

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

        #region RootNode
        static MenuTreeNode GetRoot()
        {
            var root = new MenuTreeNode("Приветствую тебя. Что бы ты желал?", GetMenuRootButtons());
            root.ButtonReaction = (msgArg) =>
            {
                switch (msgArg.Message.Text)
                {
                    case "Создать новую карту":
                        menu.Current = menu.Root.NextNodes.First();
                        menu.Current.PrintCurrentMessage(bot, msgArg);
                        break;
                    case "Вывести предыдущую созданную":
                        if (DataBaseReaderWriter.TryGetMap(msgArg.Message.Chat.Id, out Map map))
                        {
                            var chatId = msgArg.Message.Chat.Id;
                            bot.SendPhoto(chatId, map.ToHeightImage());
                            bot.SendPhoto(chatId, map.ColorMap.ToColorImage(map.SizeX, map.SizeY));
                        }
                        break;    
                    default:
                        root.PrintCurrentMessage(bot, msgArg);
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
        #region SizeNode
        static MenuTreeNode GetSizeNode()
        {
            var node = new MenuTreeNode("Укажите размер карты. Размер должен соответствовать формуле 2^n + 1", EmptyMurkup);
            node.ButtonReaction = (msgArg) =>
            {
                int size;
                if (TryGetSquareMapSize(out size, msgArg))
                {
                    MapCreator.MapSizeX = MapCreator.MapSizeY = size;
                    menu.Current = menu.Current.NextNodes.First();
                }
                menu.Current.PrintCurrentMessage(bot, msgArg);
            };
            return node;
        }

        static bool TryGetSquareMapSize(out int size, MessageEventArgs msgArg)
        {
            //Определяем, соответствует ли пришедшее число 2^n + 1
            if (int.TryParse(msgArg.Message.Text, out size))
                if (size > 0 && ((size - 1) & (size - 2)) == 0)
                    return true;
            return false;
        }
        #endregion

        private static MenuTreeNode GetSeedNode()
        {
            var node = new MenuTreeNode("Укажите сид - число, влияющее на рандомизированные значения", EmptyMurkup);
            node.ButtonReaction = (msgArg) =>
            {
                int seed;
                if (int.TryParse(msgArg.Message.Text, out seed))
                {
                    MapCreator.Seed = seed;
                    menu.Current = menu.Current.NextNodes.First();
                }
                menu.Current.PrintCurrentMessage(bot, msgArg);
            };
            return node;
        }

        private static MenuTreeNode GetRoughnessNode()
        {
            var node = new MenuTreeNode("Введите шерховатость карты - дробное значение", EmptyMurkup);
            node.ButtonReaction = (msgArg) =>
            {
                float roughness;
                if (float.TryParse(msgArg.Message.Text, out roughness))
                {
                    MapCreator.Roughness = roughness;
                    menu.Current = menu.Current.NextNodes.First();
                }
                menu.Current.PrintCurrentMessage(bot, msgArg);
            };
            return node;
        }

        #region BiomeNode
        private static MenuTreeNode GetBiomeNode()
        {
            var node = new MenuTreeNode("Укажите тип биома, в котором располагается местность", GetBiomesButtons());
            node.ButtonReaction = (msgArg) =>
            {
                switch (msgArg.Message.Text)
                {
                    case "Лес":
                        MapCreator.Biome = BiomeType.Лес;
                        menu.Current = menu.Current.NextNodes.First();
                        break;
                    case "Пустыня":
                        MapCreator.Biome = BiomeType.Пустыня;
                        menu.Current = menu.Current.NextNodes.First();
                        break;
                }
                menu.Current.PrintCurrentMessage(bot, msgArg);
            };
            return node;
        }

        private static ReplyMarkupBase GetBiomesButtons()
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
        private static MenuTreeNode GetAskSmoothNode()
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

        private static MenuTreeNode GetSmoothNode()
        {
            var node = new MenuTreeNode("Введите интенсивность сглаживания - целое число от 0 до 5", EmptyMurkup);
            node.ButtonReaction = (msgArg) =>
            {
                int smoothness;
                if (int.TryParse(msgArg.Message.Text, out smoothness))
                {
                    MapCreator.Smoothness = Math.Abs(smoothness % 5);
                    menu.Current = menu.Current.NextNodes.First();
                }
                menu.Current.PrintCurrentMessage(bot, msgArg);
            };
            return node;
        }
        #endregion
        #region BorderNode
        private static MenuTreeNode GetBorderValueNode()
        {
            var node = new MenuTreeNode("Как будет определяться высота точек по границе карты?", GetBorderMarkup());
            node.ButtonReaction = (msgArg) =>
            {
                switch (msgArg.Message.Text)
                {
                    case "Ввести константу":
                        MapCreator.IsRandomBorder = false;
                        menu.Current = menu.Current.NextNodes[1];
                        break;
                    case "Рандомно":
                        MapCreator.IsRandomBorder = true;
                        menu.Current = menu.Current.NextNodes[0];
                        break;
                }
                menu.Current.PrintCurrentMessage(bot, msgArg);
            };
            return node;
        }

        private static ReplyKeyboardMarkup GetBorderMarkup()
        {
            var rkm = new ReplyKeyboardMarkup();
            rkm.Keyboard = new KeyboardButton[][]
                {
                    new[] { new KeyboardButton("Ввести константу") },
                    new[] { new KeyboardButton("Рандомно") },
                };
            return rkm;
        }

        private static MenuTreeNode GetConstBorderNode()
        {
            var node = new MenuTreeNode("Введите значение - целое число от 0 до 255", EmptyMurkup);
            node.ButtonReaction = (msgArg) =>
            {
                byte borderValue;
                if (byte.TryParse(msgArg.Message.Text, out borderValue))
                {
                    MapCreator.DefaultBorderValue = borderValue;
                    menu.Current = menu.Current.NextNodes.First();
                }
                menu.Current.PrintCurrentMessage(bot, msgArg);
            };
            return node;
        }
        #endregion
        #region AngleNode
        private static MenuTreeNode GetAngleHeightNode()
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

        private static MenuTreeNode GetLeftTopAngleHeightNode()
        {
            var node = new MenuTreeNode("Введите высоту левого верхнего угла - целое число от 0 до 255", EmptyMurkup);
            node.ButtonReaction = (msgArg) =>
            {
                byte angleValue;
                if (byte.TryParse(msgArg.Message.Text, out angleValue))
                {
                    MapCreator.LeftTop = angleValue;
                    menu.Current = menu.Current.NextNodes.First();
                }
                menu.Current.PrintCurrentMessage(bot, msgArg);
            };
            return node;
        }

        private static MenuTreeNode GetLeftBottomAngleHeightNode()
        {
            var node = new MenuTreeNode("Введите высоту левого нижнего угла - целое число от 0 до 255", EmptyMurkup);
            node.ButtonReaction = (msgArg) =>
            {
                byte angleValue;
                if (byte.TryParse(msgArg.Message.Text, out angleValue))
                {
                    MapCreator.LeftBottom = angleValue;
                    menu.Current = menu.Current.NextNodes.First();
                }
                menu.Current.PrintCurrentMessage(bot, msgArg);
            };
            return node;
        }


        private static MenuTreeNode GetRightTopAngleHeightNode()
        {
            var node = new MenuTreeNode("Введите высоту правого верхнего угла - целое число от 0 до 255", EmptyMurkup);
            node.ButtonReaction = (msgArg) =>
            {
                byte angleValue;
                if (byte.TryParse(msgArg.Message.Text, out angleValue))
                {
                    MapCreator.RightTop = angleValue;
                    menu.Current = menu.Current.NextNodes.First();
                }
                menu.Current.PrintCurrentMessage(bot, msgArg);
            };
            return node;
        }

        private static MenuTreeNode GetRightBottomAngleHeightNode()
        {
            var node = new MenuTreeNode("Введите высоту правого нижнего угла - целое число от 0 до 255", EmptyMurkup);
            node.ButtonReaction = (msgArg) =>
            {
                byte angleValue;
                if (byte.TryParse(msgArg.Message.Text, out angleValue))
                {
                    MapCreator.RightBottom = angleValue;
                    menu.Current = menu.Current.NextNodes.First();
                }
                menu.Current.PrintCurrentMessage(bot, msgArg);
            };
            return node;
        }
        #endregion

        private static MenuTreeNode GetBuildNode()
        {
            var node = new MenuTreeNode("Все готово", GetBuildMarkup());
            node.ButtonReaction = (msgArg) =>
            {
                if(msgArg.Message.Text == "Создать карту")
                {
                    var chatId = msgArg.Message.Chat.Id;
                    bot.SendMessage(chatId, "Начался процесс создания. Он может идти длительное время", EmptyMurkup);

                    var generator = MapCreator.CreateGenerator();
                    var map = generator.GenerateMap();
                    var heightMap = map.ToHeightImage();
                    var colorMap = map.GenerateColor(MapCreator.Biome, MapCreator.Smoothness);

                    bot.SendPhoto(chatId, heightMap);
                    bot.SendPhoto(chatId, colorMap);
                    DataBaseReaderWriter.AddMap(map, chatId);
                    menu.Current = menu.Current.NextNodes.First();
                }
                menu.Current.PrintCurrentMessage(bot, msgArg);
            };
            return node;
        }

        private static ReplyKeyboardMarkup GetBuildMarkup()
        {
            var rkm = new ReplyKeyboardMarkup();
            rkm.Keyboard = new KeyboardButton[][] { new[] { new KeyboardButton("Создать карту") } };
            return rkm;
        }
    }
}
