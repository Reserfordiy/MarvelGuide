using System;
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

        public bool Male { get; set; }

        public string Login { get; set; }
        public string Password { get; set; }

        public bool WorkingNow { get; set; }

        public DateTime GotAJob { get; set; }
        public DateTime LostTheJob { get; set; }

        public bool SuperDeveloper { get; set; }
        public bool HighDeveloper { get; set; }
        public bool MediumDeveloper { get; set; }
        public bool LightDeveloperCreator { get; set; }
        public bool LightDeveloperSuperAdmin { get; set; }
        public bool LightDeveloperAdminManager { get; set; }
        public bool LightDeveloperAdminEditor { get; set; }
        public bool LightDeveloperAdminAgent { get; set; }
        public bool LightDeveloperManager { get; set; }
        public bool LightDeveloperAgent { get; set; }
        public bool LightDeveloperEditor { get; set; }
        public bool LightDeveloperModerator { get; set; }

        public bool Creator { get; set; }
        public bool SuperAdmin { get; set; }
        public bool AdminManager { get; set; }
        public bool AdminEditor { get; set; }
        public bool AdminAgent { get; set; }
        public bool Manager { get; set; }        
        public bool Editor { get; set; }
        public bool Agent { get; set; }
        public bool Moderator { get; set; }
        
        public string ManagersRole { get; set; }

        public List<EditorsPublication> EditorsRubrics { get; set; }

        public int AgentsNumber { get; set; }
        public string AgentsFirstWords { get; set; }
        public string AgentsLastWords { get; set; }

        public virtual Picture Avatar { get; set; }



        public string Job()
        {
            const string splitting = "; ";

            const string creator = "Владелец";
            const string superAdmin = "Главный администратор";
            const string adminManager = "Руководитель аппаратного офиса";
            const string adminEditor = "Руководитель редакции";
            const string adminAgent = "Руководитель поддержки";
            const string manager = "Менеджер";
            const string editor = "Редактор";
            const string agent = "Агент поддержки";
            const string moderator = "Модератор";

            string job = "";

            if (Creator) { job += splitting + creator; }
            if (SuperAdmin) { job += splitting + superAdmin; }
            if (AdminManager) { job += splitting + adminManager; }
            if (AdminEditor) { job += splitting + adminEditor; }
            if (AdminAgent) { job += splitting + adminAgent; }
            if (Manager) { job += splitting + manager; }
            if (Editor) { job += splitting + editor; }
            if (Agent) { job += splitting + agent; }
            if (Moderator) { job += splitting + moderator; }

            if (job.Length >= 1)
            {
                job = job.Remove(0, splitting.Count());
            }

            return job;
        }



        public string GenitiveName()
        {
            if (Name == null || Name.Length <= 2)
            {
                return Name;
            }

            string genitiveName = Name;

            if (Male)
            {
                if (genitiveName[genitiveName.Length - 1] == 'й' || genitiveName[genitiveName.Length - 1] == 'ь')
                {
                    genitiveName = genitiveName.Substring(0, genitiveName.Length - 1) + 'я';
                }
                else if(genitiveName[genitiveName.Length - 1] == 'я')
                {
                    genitiveName = genitiveName.Substring(0, genitiveName.Length - 1) + 'и';
                }
                else if (genitiveName[genitiveName.Length - 1] == 'и')
                {
                    
                }
                else
                {
                    genitiveName += 'а';
                }
            }
            else
            {
                if (genitiveName[genitiveName.Length - 1] == 'а')
                {
                    if (genitiveName[genitiveName.Length - 2] == 'г')
                    {
                        genitiveName = genitiveName.Substring(0, genitiveName.Length - 1).ToString() + 'и';
                    }
                    else
                    {
                        genitiveName = genitiveName.Substring(0, genitiveName.Length - 1).ToString() + 'ы';
                    }
                }
                else
                {
                    genitiveName = genitiveName.Substring(0, genitiveName.Length - 1).ToString() + 'и';
                }
            }

            return genitiveName;
        }



        public bool IsDeveloper()
        {
            return SuperDeveloper || HighDeveloper || MediumDeveloper || LightDeveloperCreator || LightDeveloperSuperAdmin || LightDeveloperAdminManager ||
                LightDeveloperAdminEditor || LightDeveloperAdminAgent || LightDeveloperManager || LightDeveloperEditor || LightDeveloperAgent || LightDeveloperModerator;
        }



        public void NullingRubrics()
        {
            List<EditorsPublication> publicationsForRemoving = new List<EditorsPublication>();

            foreach (var publication in EditorsRubrics)
            {
                if (publication.Rubric == null || publication.Frequency == 0)
                {
                    publicationsForRemoving.Add(publication);
                }
            }

            foreach (var publication in publicationsForRemoving)
            {
                EditorsRubrics.Remove(publication);
            }
        }
    }
}
