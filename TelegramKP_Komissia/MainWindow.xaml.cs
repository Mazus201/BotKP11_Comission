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
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramKP_Komissia.AppData;

namespace TelegramKP_Komissia
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Скелет бота
        ObservableCollection<TelegUser> Users; //подключаем встроенную в впф базу даднных
        TelegramBotClient bot; //имя для обращения к боту

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                Users = new ObservableCollection<TelegUser>(); //создаем коллекцию юзеров

                LBUsers.ItemsSource = Users; //выводим коллекцию юзеров в соответстующее окно

                string token = "1227072027:AAGGhpJp7UvgEzrVuFq4Msof92rFHHaOTk4"; //токен

                bot = new TelegramBotClient(token); //присвиваем боту токен

                bot.OnMessage += botOnMessageAsync;
                bot.OnCallbackQuery += BotOnCallbackQueryRecived;                 //подключили обработку действий с кнопок

                bot.StartReceiving(); //запуск сервиса
            }
            catch
            {
                MessageBox.Show("Отсутствует подключение к сети.");
            }
        }

        private async void BotOnCallbackQueryRecived(object sender, CallbackQueryEventArgs e)
        {
            string textMessage = e.CallbackQuery.Data;                                          //получем данные из названия кнопки
            string buttonText = textMessage.Trim();                                             //если есть пробелы спереди или сзади - убераем
            string name = $"{e.CallbackQuery.From.FirstName} {e.CallbackQuery.From.LastName}";  //достаем имя пользователя
            Console.WriteLine($"{name} нажал {buttonText}");                                    //отображаем в консоли что выбрал юхер

            string dateToday = Convert.ToString(DateTime.Now);
            dateToday = dateToday.Substring(0, dateToday.Length - 16);
            dateToday = dateToday.Replace(".", "");
            string linkMenu = "https://collegetsaritsyno.mskobr.ru/attach_files/" + $"{dateToday}.jpeg";

            var dateTomorrow = DateTime.Now.AddDays(1);
            string dateTomorrowSTR = Convert.ToString(dateTomorrow);
            dateTomorrowSTR = dateTomorrowSTR.Substring(0, dateTomorrowSTR.Length - 16);
            dateTomorrowSTR = dateTomorrowSTR.Replace(".", "");
            string linkMenuTomorrow = "https://collegetsaritsyno.mskobr.ru/attach_files/" + $"{dateTomorrow}.jpeg";

            switch (buttonText)                                                                  //обрабатываем кнопки
            {
                case "Где столовая?":
                    await bot.SendTextMessageAsync(e.CallbackQuery.From.Id,
                        "https://www.youtube.com/watch?v=V_rj-uCfSAA&ab_channel=%D0%9A%D0%B0%D0%BD%D0%B0%D0%BBCielinG%27a");
                    break;

                case "Где актовый зал?":
                    await bot.SendTextMessageAsync(e.CallbackQuery.From.Id,
                        "https://www.youtube.com/watch?v=83zh8MgvPPM&ab_channel=%D0%9A%D0%B0%D0%BD%D0%B0%D0%BBCielinG%27a ");
                    break;

                case "Расписание":
                    await bot.SendTextMessageAsync(e.CallbackQuery.From.Id,
                        @"Можешь прислать, из какой ты группы и я сразу пришлю твое расписание на сегодня 📋 
А пока, можешь посмотреть общее расписание:
https://vk.com/doc223749979_567148293?hash=a4bfcdd0bf8227c143&dl=46dc3541f255fc0796");
                    break;

                case "Меню на сегодня":
                    await bot.SendPhotoAsync(e.CallbackQuery.From.Id, $"{linkMenu}");
                    break;

                case "Меню на завтра":
                    await bot.SendPhotoAsync(e.CallbackQuery.From.Id, $"{linkMenuTomorrow}");
                    break;
            }

            try
            {
                await bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id, $"Вы выбрали {buttonText}"); //отображаем юзеру, что он выбрал
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);                                                            //или не отображаем))
            }
        }
        #endregion

        #region Обработка текста приходящего от юзера

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Statistic stat = new Statistic();
            stat.Show();
        }

        private async void botOnMessageAsync(object sender, MessageEventArgs e)
        {
            string message = e.Message.Text;

            string msg = $"{DateTime.Now} : {e.Message.Chat.FirstName} {e.Message.Chat.Id} {message}"; //переменная с информацией о сообщении

            System.IO.File.AppendAllText("data.log", $"{msg}\n"); //создаем лог с информацией

            Debug.WriteLine(msg); //в консоль выводим входящеие данные

            switch (message)
            {
                #region start
                case "/start":
                    Global.countStart++;
                    string text =                                                    //переменная для вывода текста пользователю
$@"Привет, {e.Message.From.FirstName}! 

Теперь ты часть нашего лампового сообщества, рад, что ты присоединился 😄
Кстати, ты можешь ознакмиться со списком комманд, которые я умею выполнять, написав '/help'";
                    await bot.SendTextMessageAsync(e.Message.From.Id, text);     //вывод самого сообщения
                    break;
                #endregion

                #region links
                case "/links":
                    Global.countLinks++;
                    var menuInternet = new InlineKeyboardMarkup(new[]                //создание меню с кнопками
                    {
                        new[]                                                        //делаем двумерный массив и получаем два ряда и два столбца кнопок. Это первый ряд
                        {
                            InlineKeyboardButton.WithUrl("VK", "https://vk.com/gapoukp11"),
                            InlineKeyboardButton.WithUrl("Instagram", "http://instagram.com/vseokp11"),
                            InlineKeyboardButton.WithUrl("WhatsUp", "https://api.whatsapp.com/send?phone=79672199390")
                        },
                        new[] //это второй ряд
                        {
                            InlineKeyboardButton.WithUrl("Сайт", "https://www.kp11.ru/"),
                            InlineKeyboardButton.WithUrl("YouTube", "https://www.youtube.com/user/kp11ru/feed?filter=2"),
                            InlineKeyboardButton.WithUrl("FaceBook", "https://www.facebook.com/gapoukp11")
                        }
                    }
                    );

                    await bot.SendTextMessageAsync(e.Message.From.Id, "Мы есть в интернете! Можешь найти по следующим ссылкам :)", replyMarkup: menuInternet); //вывод кнопок на экран
                    break;
                #endregion

                #region keyboard
                case "/keyboard":
                    Global.countKeyboard++;
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
                    await bot.SendTextMessageAsync(e.Message.From.Id, "Выбери, что ты хочешь узнать:", replyMarkup: replaceKeyboard);
                    //обрабатывем действие юхера и выводим клавиатуру
                    break;
                #endregion

                #region menu
                case "/menu":                                                            //меню действий для студента
                    Global.countMenu++;
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

                    await bot.SendTextMessageAsync(e.Message.From.Id, "Что ты хотел узнать?", replyMarkup: menuDo); //выводим эти кнопки

                    break; 
                #endregion

                #region help
                case "/help":                                                           //основыне команды для пользователя
                    Global.countHelp++;
                    string textHelp =                                                                       //описываем действие на ХЕЛП. Такой вид - это для корректного отображения сообщения в телеге
                    $@"Я постараюсь тебе помочь!

Для навигации по боту ты можешь использовать следующие команды:
/start - для вызова первого сообщения от меня;
/menu - для вызова моих основных функций;
/links - это ссылки на нас в интернете;
/keyboard - для удобного доступа к командам;
/support - служба поддержки КП11;
/help - для вызова этого меню;";
                    await bot.SendTextMessageAsync(e.Message.From.Id, textHelp);

                    break;
                #endregion

                #region default
                default:                                                                //это остальное. Тут должна быть обработка сообщений - не команд (обычных сообщений боту), которая осуществляется через апи DialogFlow
                    try                                                                 //но будет тут просто обработка данных введенных юзером без слешей
                    {
                        var messageType = e.Message.Type;
                        string answer = null;
                        string nameOfUser = e.Message.From.FirstName;

                        if (e.Message == null)
                        {
                            Debug.WriteLine(msg); //в консоль выводим входящеие данные
                            await bot.SendTextMessageAsync(e.Message.From.Id, "Я пока маленький бот, который не умеет работать с этим типом данных :(");
                        }

                        else if (e.Message.Type != MessageType.Text)
                        {
                            Debug.WriteLine(msg); //в консоль выводим входящеие данные
                            await bot.SendTextMessageAsync(e.Message.From.Id, "Я пока маленький бот, который не умеет работать с этим типом данных :(");
                        }

                        else
                        {
                            var responce = message;
                            responce = responce.ToLower();
                            Debug.WriteLine(msg); //в консоль выводим входящеие данные

                            if (responce != null)
                            {
                                if (responce == "привет") answer = "Здравствуй!";

                                #region support
                                else if (responce == "/support")
                                {
                                    if (responce != null)
                                    {
                                        this.Dispatcher.Invoke(() => //обрабатываем данные UI 
                                        {
                                            Global.countSupport++;
                                            var person = new TelegUser(e.Message.Chat.FirstName, e.Message.Chat.Id); //создаем нового пользователя в коллекцию 
                                            if (!Users.Contains(person)) Users.Add(person); //добавляем его, если раньше в коллекции его не было
                                            Users[Users.IndexOf(person)].AddMessage($"{person.Nick}: {e.Message.Text}"); //полученное ранее сообщение помещаем в специальную коллекцию и присвиваем определенному пользователю
                                        });
                                    }
                                }
                                #endregion

                                else if (responce == "меню")
                                {
                                    var dateMenu = new InlineKeyboardMarkup(new[]                         //делаем кнопки под сообщением. Опять многомерный массив
                                    {
                                    new[]
                                    {
                                        InlineKeyboardButton.WithCallbackData("Меню на сегодня")
                                    },
                                    new[]
                                    {
                                        InlineKeyboardButton.WithCallbackData("Меню на завтра")
                                    }
                                }
                                    );

                                    await bot.SendTextMessageAsync(e.Message.From.Id, "На когда тебе интересно меню?", replyMarkup: dateMenu); //выводим эти кнопки

                                }

                                else if (responce == "где столовая?" || responce == "где столовая")
                                    answer = "https://youtu.be/crnClMC1wec";
                                else if (responce == "с-12")
                                    await bot.SendTextMessageAsync(e.Message.From.Id, "https://www.kp11.ru/rasp/%D0%A1-12.html");
                                else if (responce == "исип-13")
                                    await bot.SendTextMessageAsync(e.Message.From.Id, "https://www.kp11.ru/rasp/%D0%98%D0%A1%D0%B8%D0%9F-13.html");
                                else if (responce == "исип-15")
                                    await bot.SendTextMessageAsync(e.Message.From.Id, "https://www.kp11.ru/rasp/%D0%98%D0%A1%D0%B8%D0%9F-15.html");
                                else if (responce == "ксик-14")
                                    await bot.SendTextMessageAsync(e.Message.From.Id, "https://www.kp11.ru/rasp/%D0%9A%D0%A1%D0%B8%D0%9A-14.html");
                                else if (responce == "ксик-11")
                                    await bot.SendTextMessageAsync(e.Message.From.Id, "https://www.kp11.ru/rasp/%D0%9A%D0%A1%D0%B8%D0%9A-11.html"); //
                                else if (responce == "с-13")
                                    await bot.SendTextMessageAsync(e.Message.From.Id, "https://www.kp11.ru/rasp/%D0%A1-13.html");
                                else if (responce == "исип-12")
                                    await bot.SendTextMessageAsync(e.Message.From.Id, "https://www.kp11.ru/rasp/%D0%98%D0%A1%D0%B8%D0%9F-12.html");
                                else if (responce == "ксик-13")
                                    await bot.SendTextMessageAsync(e.Message.From.Id, "https://www.kp11.ru/rasp/%D0%9A%D0%A1%D0%B8%D0%9A-13.html");//
                                else if (responce == "с-32")
                                    await bot.SendTextMessageAsync(e.Message.From.Id, "https://www.kp11.ru/rasp/%D0%A1-32.html");
                                else if (responce == "кс-41")
                                    await bot.SendTextMessageAsync(e.Message.From.Id, "https://www.kp11.ru/rasp/%D0%98%D0%A1%D0%B8%D0%9F-41.html");
                                else if (responce == "вкс-42")
                                    await bot.SendTextMessageAsync(e.Message.From.Id, "https://www.kp11.ru/rasp/%D0%9A%D0%A1-41:%D0%92%D0%9A%D0%A1-42.html");
                                else if (responce == "с-21")
                                    await bot.SendTextMessageAsync(e.Message.From.Id, "https://www.kp11.ru/rasp/%D0%A1-21.html");
                                else if (responce == "с-41")
                                    await bot.SendTextMessageAsync(e.Message.From.Id, "https://www.kp11.ru/rasp/%D0%A1-41.html");
                                else if (responce == "ксик-31")
                                    await bot.SendTextMessageAsync(e.Message.From.Id, "https://www.kp11.ru/rasp/%D0%9A%D0%A1%D0%B8%D0%9A-31.html");
                                else if (responce == "исип-24")
                                    await bot.SendTextMessageAsync(e.Message.From.Id, "https://www.kp11.ru/rasp/%D0%98%D0%A1%D0%B8%D0%9F-24.html");
                                else if (responce == "ксик-22")
                                    await bot.SendTextMessageAsync(e.Message.From.Id, "https://www.kp11.ru/rasp/%D0%98%D0%A1%D0%B8%D0%9F-24.html");
                                else if (responce == "исип-21")
                                    await bot.SendTextMessageAsync(e.Message.From.Id, "https://www.kp11.ru/rasp/%D0%98%D0%A1%D0%B8%D0%9F-21.html");
                                else if (responce == "исип-32")
                                    await bot.SendTextMessageAsync(e.Message.From.Id, "https://www.kp11.ru/rasp/%D0%98%D0%A1%D0%B8%D0%9F-32.html");
                                else if (responce == "исип-33")
                                    await bot.SendTextMessageAsync(e.Message.From.Id, "https://www.kp11.ru/rasp/%D0%98%D0%A1%D0%B8%D0%9F-33.html");
                                else if (responce == "исип-41")
                                    await bot.SendTextMessageAsync(e.Message.From.Id, "https://www.kp11.ru/rasp/%D0%98%D0%A1%D0%B8%D0%9F-41.html");

                                else
                                    answer =
    @"Упс! Ошибочка...

Я еще чоень молодой бот и только учусь новым командам 👶 
Если у тебя есть пожелания или предложения по работе бота, то напиши мне в ЛС @Mazus_nikita 📩
Пока ты можешь воспользоваться командой '/help', чтобы узнать, что я умею.";
                                await bot.SendTextMessageAsync(e.Message.From.Id, answer);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    break;
                    #endregion
            }
        }
        #endregion

        #region кнопка для отправки сообщения
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
        #endregion

        #region перенос строки
        private void TxbMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            { 
                SendMsg(); 
            }
        }
        #endregion

        #region Функция отправки сообщения пользователю
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
        #endregion
    }
}
