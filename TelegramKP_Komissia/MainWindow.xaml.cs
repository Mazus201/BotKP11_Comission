using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramKP_Komissia.AppData;

namespace TelegramKP_Komissia
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        ObservableCollection<TelegUser> Users; //подключаем встроенную в впф базу даднных
        Telegrambot bot; //имя для обращения к боту
        public MainWindow()
        {
            InitializeComponent();
            Users = new ObservableCollection<TelegUser>(); //создаем коллекцию юзеров

            LBUsers.ItemsSource = Users; //выводим коллекцию юзеров в соответстующее окно

            string token = "1227072027:AAGGhpJp7UvgEzrVuFq4Msof92rFHHaOTk4"; //токен

            bot = new Telegrambot(token); //присвиваем боту токен

            bot.OnMessage += Bot_OnMessage;                                   //подписались на получение сообщения
            //bot.OnCallbackQuery += BotOnCallbackQueryRecived;                 //подключили обработку действий с кнопок

            //bot.OnMessage += delegate (object sender, MessageEventArgs e) //подписываем бота на действие
            //{
            //    string msg = $"{DateTime.Now} : {e.Message.Chat.FirstName} {e.Message.Chat.Id} {e.Message.Text}"; //переменная с информацией о сообщении

            //    System.IO.File.AppendAllText("data.log", $"{msg}\n"); //создаем лог с информацией

            //    Debug.WriteLine(msg); //в консоль выводим входящеие данные

            //    if (msg == "/support")
            //    {
            //        do
            //        {
            //            this.Dispatcher.Invoke(() => //обрабатываем данные UI 
            //            {
            //                var person = new TelegUser(e.Message.Chat.FirstName, e.Message.Chat.Id); //создаем нового пользователя в коллекцию 
            //                if (!Users.Contains(person)) Users.Add(person); //добавляем его, если раньше в коллекции его не было
            //                Users[Users.IndexOf(person)].AddMessage($"{person.Nick}: {e.Message.Text}"); //полученное ранее сообщение помещаем в специальную коллекцию и присвиваем определенному пользователю
            //            });
            //        }
            //        while (TxbMessage.Text != "EndSupport");
            //    }
            //};

            bot.StartReceiving(); //запуск сервиса

        }

        private static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            bool isSupportable = false;

            string message = e.Message.Text;

            string msg = $"{DateTime.Now} : {e.Message.Chat.FirstName} {e.Message.Chat.Id} {e.Message.Text}"; //переменная с информацией о сообщении

            System.IO.File.AppendAllText("data.log", $"{msg}\n"); //создаем лог с информацией

            Debug.WriteLine(msg); //в консоль выводим входящеие данные

            switch (e.Message.Text)
            {     //обработка команд введеных пользователями
                case "/support":
                    while (isSupportable != true)
                    {
                        this.Dispatcher.Invoke(() => //обрабатываем данные UI 
                        {
                            var person = new TelegUser(e.Message.Chat.FirstName, e.Message.Chat.Id); //создаем нового пользователя в коллекцию 
                            if (!Users.Contains(person)) Users.Add(person); //добавляем его, если раньше в коллекции его не было
                            Users[Users.IndexOf(person)].AddMessage($"{person.Nick}: {e.Message.Text}"); //полученное ранее сообщение помещаем в специальную коллекцию и присвиваем определенному пользователю
                        });
                    }
                    break;
                case "/start":
                    string text =                                                    //переменная для вывода текста пользователю
$@"Привет, {e.Message.Chat.FirstName}! 

Теперь ты часть нашего лампового сообщества, рад, что ты присоединился 😄
Кстати, ты можешь ознакмиться со списком комманд, которые я умею выполнять, написав '/help'";
                    await bot.SendTextMessageAsync(message.From.Id, text);     //вывод самого сообщения
                    break;

                case "/ourLinks":
                    var menuInternet = new InlineKeyboardMarkup(new[]                //создание меню с кнопками
                    {
                        new[]                                                        //делаем двумерный массив и получаем два ряда и два столбца кнопок. Это первый ряд
                        {
                            InlineKeyboardButton.WithUrl("VK", "https://vk.com/gapoukp11"),
                            InlineKeyboardButton.WithUrl("Instagram", "http://instagram.com/vseokp11"),
                            InlineKeyboardButton.WithUrl("WhatsUp", "https://api.whatsapp.com/send?phone=796721993902")
                        },
                        new[] //это второй ряд
                        {
                            InlineKeyboardButton.WithUrl("Сайт", "https://www.kp11.ru/"),
                            InlineKeyboardButton.WithUrl("YouTube", "https://www.youtube.com/user/kp11ru/feed?filter=2"),
                            InlineKeyboardButton.WithUrl("FaceBook", "https://www.facebook.com/gapoukp11")
                        }
                    }
                    );

                    await bot.SendTextMessageAsync(message.From.Id, "Мы есть в интернете! Можешь найти по следующим ссылкам :)", replyMarkup: menuInternet); //вывод кнопок на экран
                    break;

                case "/keyboard":
                    var replaceKeyboard = new ReplyKeyboardMarkup(new[]                 //создаем кнопки для "клавиатуры". Это двумерный массив
                    {
                        new[]                                                           //первый ряд кнопок клавиатуры
                        {
                            new KeyboardButton("Поделиться геолокацией") {RequestLocation = true}
                        },
                        new[]                                                           //второй ряд клавиатуры
                        {
                            new KeyboardButton("Поделиться контактом") {RequestContact = true}
                        }
                    });
                    replaceKeyboard.ResizeKeyboard = true;
                    await bot.SendTextMessageAsync(message.From.Id, "Выбери, что ты хочешь узнать:", replyMarkup: replaceKeyboard);
                    //обрабатывем действие юхера и выводим клавиатуру
                    break;

                case "/menu":                                                           //меню действий для студента
                    var menuDo = new InlineKeyboardMarkup(new[]                         //делаем кнопки под сообщением. Опять многомерный массив
                    {
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Расписание")
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Меню на сегодня"),
                            InlineKeyboardButton.WithCallbackData("Меню на завтра")
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Где актовый зал?"),
                            InlineKeyboardButton.WithCallbackData("Где столовая?")
                        }
                    }
                    );

                    await bot.SendTextMessageAsync(message.From.Id, "Что ты хотел узнать?", replyMarkup: menuDo); //выводим эти кнопки

                    break;

                case "/help":                                                           //основыне команды для пользователя
                    string textHelp =                                                                       //описываем действие на ХЕЛП. Такой вид - это для корректного отображения сообщения в телеге
                    $@"Я постараюсь тебе помочь!

Для навигации по боту ты можешь использовать следующие команды:
/start - для вызова первого сообщения от меня;
/menu - для вызова моих основных функций;
/ourLinks - это ссылки на нас в интернете;
/keyboard - для удобного доступа к командам;
/help - для вызова этого меню;";
                    await bot.SendTextMessageAsync(message.From.Id, textHelp);

                    break;

                default:                                                                //это остальное. Тут должна быть обработка сообщений - не команд (обычных сообщений боту), которая осуществляется через апи DialogFlow
                    try                                                                 //но будет тут просто обработка данных введенных юзером без слешей
                    {
                        var responce = message;
                        string answer = null;
                        responce = responce.ToLower();

                        if (responce != null)
                        {
                            if (responce == "привет")
                                answer = "Здравствуй!";

                            else if (responce == "меню")
                            {
                                var dateMenu = new InlineKeyboardMarkup(new[]                         //делаем кнопки под сообщением. Опять многомерный массив
                                {
                                    new[]
                                    {
                                        InlineKeyboardButton.WithCallbackData("меню на сегодня")
                                    },
                                    new[]
                                    {
                                        InlineKeyboardButton.WithCallbackData("меню на завтра")
                                    }
                                }
                                );

                                await bot.SendTextMessageAsync(message.From.Id, "На когда тебе интересно меню?", replyMarkup: dateMenu); //выводим эти кнопки

                            }

                            else if (responce == "где столовая?" || responce == "где столовая")
                                answer = "https://youtu.be/crnClMC1wec";
                            else if (responce == "с-12")
                                await bot.SendTextMessageAsync(message.From.Id, "https://www.kp11.ru/rasp/%D0%A1-12.html");
                            else if (responce == "исип-13")
                                await bot.SendTextMessageAsync(message.From.Id, "https://www.kp11.ru/rasp/%D0%98%D0%A1%D0%B8%D0%9F-13.html");
                            else if (responce == "исип-15")
                                await bot.SendTextMessageAsync(message.From.Id, "https://www.kp11.ru/rasp/%D0%98%D0%A1%D0%B8%D0%9F-15.html");
                            else if (responce == "ксик-14")
                                await bot.SendTextMessageAsync(message.From.Id, "https://www.kp11.ru/rasp/%D0%9A%D0%A1%D0%B8%D0%9A-14.html");
                            else if (responce == "ксик-11")
                                await bot.SendTextMessageAsync(message.From.Id, "https://www.kp11.ru/rasp/%D0%9A%D0%A1%D0%B8%D0%9A-11.html"); //
                            else if (responce == "с-13")
                                await bot.SendTextMessageAsync(message.From.Id, "https://www.kp11.ru/rasp/%D0%A1-13.html");
                            else if (responce == "исип-12")
                                await bot.SendTextMessageAsync(message.From.Id, "https://www.kp11.ru/rasp/%D0%98%D0%A1%D0%B8%D0%9F-12.html");
                            else if (responce == "ксик-13")
                                await bot.SendTextMessageAsync(message.From.Id, "https://www.kp11.ru/rasp/%D0%9A%D0%A1%D0%B8%D0%9A-13.html");//
                            else if (responce == "с-32")
                                await bot.SendTextMessageAsync(message.From.Id, "https://www.kp11.ru/rasp/%D0%A1-32.html");
                            else if (responce == "кс-41")
                                await bot.SendTextMessageAsync(message.From.Id, "https://www.kp11.ru/rasp/%D0%98%D0%A1%D0%B8%D0%9F-41.html");
                            else if (responce == "вкс-42")
                                await bot.SendTextMessageAsync(message.From.Id, "https://www.kp11.ru/rasp/%D0%9A%D0%A1-41:%D0%92%D0%9A%D0%A1-42.html");
                            else if (responce == "с-21")
                                await bot.SendTextMessageAsync(message.From.Id, "https://www.kp11.ru/rasp/%D0%A1-21.html");
                            else if (responce == "с-41")
                                await bot.SendTextMessageAsync(message.From.Id, "https://www.kp11.ru/rasp/%D0%A1-41.html");
                            else if (responce == "ксик-31")
                                await bot.SendTextMessageAsync(message.From.Id, "https://www.kp11.ru/rasp/%D0%9A%D0%A1%D0%B8%D0%9A-31.html");
                            else if (responce == "исип-24")
                                await bot.SendTextMessageAsync(message.From.Id, "https://www.kp11.ru/rasp/%D0%98%D0%A1%D0%B8%D0%9F-24.html");
                            else if (responce == "ксик-22")
                                await bot.SendTextMessageAsync(message.From.Id, "https://www.kp11.ru/rasp/%D0%98%D0%A1%D0%B8%D0%9F-24.html");
                            else if (responce == "исип-21")
                                await bot.SendTextMessageAsync(message.From.Id, "https://www.kp11.ru/rasp/%D0%98%D0%A1%D0%B8%D0%9F-21.html");
                            else if (responce == "исип-32")
                                await bot.SendTextMessageAsync(message.From.Id, "https://www.kp11.ru/rasp/%D0%98%D0%A1%D0%B8%D0%9F-32.html");
                            else if (responce == "исип-33")
                                await bot.SendTextMessageAsync(message.From.Id, "https://www.kp11.ru/rasp/%D0%98%D0%A1%D0%B8%D0%9F-33.html");
                            else if (responce == "исип-41")
                                await bot.SendTextMessageAsync(message.From.Id, "https://www.kp11.ru/rasp/%D0%98%D0%A1%D0%B8%D0%9F-41.html");

                            else
                                answer =
@"Упс! Ошибочка...

Я еще чоень молодой бот и только учусь новым командам 👶 
Если у тебя есть пожелания или предложения по работе бота, то напиши мне в ЛС @Mazus_nikita 📩
Пока ты можешь воспользоваться командой '/help', чтобы узнать, что я умею.";
                            await bot.SendTextMessageAsync(message.From.Id, answer);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    break;
            }
        }


        private void BotOnCallbackQueryRecived(object sender, CallbackQueryEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void SendMsg() //фунцкия для отправи сообщения пользователя
        {
            try
            {
                var concreteUser = Users[Users.IndexOf(LBUsers.SelectedItem as TelegUser)]; //выделенного в LB пользователя запоминаем
                string responseMsg = $"Support: {TxbMessage.Text}"; //формироем сообщение от поддержки их Txb
                concreteUser.Messages.Add(responseMsg); //сообщение добавляем в коллекцию

                bot.SendTextMessageAsync(concreteUser.Id, TxbMessage.Text); //отправляем сообщение выбранному ранее пользователю
                string logText = $"{DateTime.Now}: >> {concreteUser.Id} {concreteUser.Nick} {responseMsg}\n"; 
                System.IO.File.AppendAllText("data.log", logText); //добавляем данные в лог файл

                TxbMessage.Text = String.Empty; //очищаем поле ввода
            }
            catch
            {
                MessageBox.Show("Выберите пользователя, котоорму надо отправить сообщение!", "Ошибка");
            }
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SendMsg();
            }
            catch
            {

            }
        }

        private void TxbMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            { 
                SendMsg(); 
            }
        }
    }

}