﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarvelGuide.Core.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }

        public string Login { get; set; }
        public string Password { get; set; }

        public bool Creator { get; set; }
        public bool SuperAdmin { get; set; }
        public bool AdminEditor { get; set; }
        public bool AdminAgent { get; set; }
        public bool Manager { get; set; }        
        public bool Editor { get; set; }
        public bool Agent { get; set; }
        public bool Moderator { get; set; }
        
        public string ManagersRole { get; set; }

        public string EditorsRubric { get; set; }
        public int EditorsFrequency { get; set; }

        public int AgentsNumber { get; set; }
        public string AgentsFirstWords { get; set; }
        public string AgentsLastWords { get; set; }

        public virtual Picture Avatar { get; set; }


        public string Job()
        {
            const string splitting = "; ";

            const string creator = "Создатель";
            const string superAdmin = "Главный администратор";
            const string adminEditor = "Исполнительный администратор";
            const string adminAgent = "Директор Поддержки";
            const string manager = "Менеджер";
            const string editor = "Редактор";
            const string agent = "Агент Поддержки";
            const string moderator = "Модератор";

            string job = "";

            if (Creator) { job += splitting + creator; }
            if (SuperAdmin) { job += splitting + superAdmin; }
            if (AdminEditor) { job += splitting + adminEditor; }
            if (AdminAgent) { job += splitting + adminAgent; }
            if (Manager) { job += splitting + manager; }
            if (Editor) { job += splitting + editor; }
            if (Agent) { job += splitting + agent; }
            if (Moderator) { job += splitting + moderator; }

            job = job.Remove(0, splitting.Count());

            return job;
        }
    }
}
