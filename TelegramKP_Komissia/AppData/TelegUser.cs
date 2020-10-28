using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using Telegram.Bot.Types;

namespace TelegramKP_Komissia.AppData
{
    /// <summary>
    /// модель пользователя
    /// </summary>
    class TelegUser : INotifyPropertyChanging, IEquatable<TelegUser> //объявляем работу с интерфейсом
    {
        public TelegUser(string NickName, long ChatId) //передаем в класс данные об имени и ид пользователя
        {
            this.nick = NickName; //присваиваем значения переменных
            this.id = ChatId;
            Messages = new ObservableCollection<string>(); //добавляем сообщение в коллекцию
        }

        private string nick; //создаем пересменную для хранения имени

        public string Nick //создаем свойство для доступа к переменной nick
        {
            get { return this.nick; }
            set
            {
                this.nick = value;
                PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(this.Nick)));
            }
        }

        private long id; //переменная для хранения ид
        public long Id //свойство для доступа к ид
        {
            get { return this.id;  }
            set
            {
                this.id = value;
                PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(this.Id)));
            }
        }

        public event PropertyChangingEventHandler PropertyChanging; //оповещение листбоксов об измненениях данных через интерфейс. Благодаря этому будет происходить обновление данных

        /// <summary>
        /// сравнение двух пользователей
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(TelegUser other) => other.Id == this.Id; //чтобы быть уверенным, что сообещние принадлежит нужному правильному подьзователю

        /// <summary>
        /// хранение всех сообщений от юзеров и поддержки
        /// </summary>
        public ObservableCollection<string> Messages { get; set; }

        /// <summary>
        /// метод для добавления сообщений в коллекцию
        /// </summary>
        /// <param name="Text"></param>
        public void AddMessage(string Text) => Messages.Add(Text);
    }

}