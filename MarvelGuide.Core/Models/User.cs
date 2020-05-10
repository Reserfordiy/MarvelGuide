using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
        public bool LightDeveloperGeneralDirector { get; set; }
        public bool LightDeveloperDirector { get; set; }
        public bool LightDeveloperDeputyGeneralDirector { get; set; }
        public bool LightDeveloperHeadOfManagers { get; set; }
        public bool LightDeveloperHeadOfExperts { get; set; }
        public bool LightDeveloperHeadOfMarketers { get; set; }
        public bool LightDeveloperHeadOfEditors { get; set; }
        public bool LightDeveloperHeadOfSpecials { get; set; }
        public bool LightDeveloperHeadOfAgents { get; set; }
        public bool LightDeveloperHeadOfModerators { get; set; }
        public bool LightDeveloperHeadOfTechnicians { get; set; }
        public bool LightDeveloperManager { get; set; }
        public bool LightDeveloperExpert { get; set; }
        public bool LightDeveloperMarketer { get; set; }
        public bool LightDeveloperEditor { get; set; }
        public bool LightDeveloperSpecial { get; set; }
        public bool LightDeveloperAgent { get; set; }
        public bool LightDeveloperModerator { get; set; }

        public bool LightDeveloperTechnician { get; set; }

        public bool GeneralDirector { get; set; }
        public bool Director { get; set; }
        public bool DeputyGeneralDirector { get; set; }
        public bool HeadOfManagers { get; set; }
        public bool HeadOfExperts { get; set; }
        public bool HeadOfMarketers { get; set; }
        public bool HeadOfEditors { get; set; }
        public bool HeadOfSpecials { get; set; }
        public bool HeadOfAgents { get; set; }
        public bool HeadOfModerators { get; set; }
        public bool HeadOfTechnicians { get; set; }
        public bool Manager { get; set; }
        public bool Expert { get; set; }
        public bool Marketer { get; set; }
        public bool Editor { get; set; }
        public bool Special { get; set; }
        public bool Agent { get; set; }
        public bool Moderator { get; set; }
        public bool Technician { get; set; }

        public string DirectorsPosition { get; set; }

        public string ManagersPosition { get; set; }

        public List<EditorsPublication> EditorsRubrics { get; set; }

        public List<EditorsPublication> SpecialsProjects { get; set; }

        public int AgentsNumber { get; set; }
        public string AgentsFirstWords { get; set; }
        public string AgentsLastWords { get; set; }

        public virtual Picture Avatar { get; set; }


        private readonly Func<User, bool>[] _checksIfUserCanSeeUserDetails = new Func<User, bool>[]
        {
            u => u.Editor,
            u => u.Special,
            u => u.Agent
        };

        private readonly Func<User, bool>[] _checksIfUserWhoWatchCanSeeUsersDetails = new Func<User, bool>[]
        {
            uWW => uWW.LightDeveloperEditor || uWW.IsMoreThanLightDeveloper,
            uWW => uWW.LightDeveloperSpecial || uWW.IsMoreThanLightDeveloper,
            uWW => uWW.LightDeveloperAgent || uWW.IsMoreThanLightDeveloper
        };



        public string Job()
        {
            const string splitting = "; ";

            const string generalDirector = "Генеральный директор";
            const string director = "Директор";
            const string deputyGeneralDirector = "Заместитель генерального директора";
            const string headOfManagers = "Руководитель аппаратного офиса";
            const string headOfExperts = "Руководитель отдела контроля";
            const string headOfMarketers = "Руководитель отдела маркетинга";
            const string headOfEditors = "Руководитель отдела редакции";
            const string headOfSpecials = "Руководитель отдела спецпроектов";
            const string headOfAgents = "Руководитель отдела поддержки";
            const string headOfModerators = "Руководитель отдела модерации";
            const string headOfTechnicians = "Руководитель технического отдела";
            const string manager = "Менеджер";
            const string expert = "Эксперт";
            const string marketer = "Маркетолог";
            const string editor = "Редактор";
            const string special = "Спецредактор";
            const string agent = "Агент поддержки";
            const string moderator = "Модератор";
            const string technician = "Техник";

            string job = "";
            
            if (GeneralDirector) { job += splitting + generalDirector; }
            if (Director) { job += splitting + director; }
            if (DeputyGeneralDirector) { job += splitting + deputyGeneralDirector; }
            if (HeadOfManagers) { job += splitting + headOfManagers; }
            if (HeadOfExperts) { job += splitting + headOfExperts; }
            if (HeadOfMarketers) { job += splitting + headOfMarketers; }
            if (HeadOfEditors) { job += splitting + headOfEditors; }
            if (HeadOfSpecials) { job += splitting + headOfSpecials; }
            if (HeadOfAgents) { job += splitting + headOfAgents; }
            if (HeadOfModerators) { job += splitting + headOfModerators; }
            if (HeadOfTechnicians) { job += splitting + headOfTechnicians; }
            if (Manager) { job += splitting + manager; }
            if (Expert) { job += splitting + expert; }
            if (Marketer) { job += splitting + marketer; }
            if (Editor) { job += splitting + editor; }
            if (Special) { job += splitting + special; }
            if (Agent) { job += splitting + agent; }
            if (Moderator) { job += splitting + moderator; }
            if (Technician) { job += splitting + technician; }

            if (job.Length >= 1)
            {
                job = job.Remove(0, splitting.Count());
            }

            return job;
        }



        public bool CheckingWhichJobsDetailsCanBeDisplayedInUserDetailsWindow(User _userWhoWantsToWatch, out bool[] matches)
        {
            matches = new bool[_checksIfUserCanSeeUserDetails.Length];

            for (int i = 0; i < _checksIfUserCanSeeUserDetails.Length; i++)
            {
                var funcForUser = _checksIfUserCanSeeUserDetails[i];
                var funcForWatcher = _checksIfUserWhoWatchCanSeeUsersDetails[i];
                if (_userWhoWantsToWatch == null) { funcForWatcher = u => true; }

                if (funcForUser(this) && funcForWatcher(_userWhoWantsToWatch))
                {
                    matches[i] = true;
                }
                else
                {
                    matches[i] = false;
                }
            }

            return matches.Any(b => b);
        }



        public string GenitiveName()
        {
            if (Name == null || Name.Length <= 1)
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
                else if (genitiveName[genitiveName.Length - 1] == 'я')
                {
                    genitiveName = genitiveName.Substring(0, genitiveName.Length - 1) + 'и';
                }
                else if (genitiveName[genitiveName.Length - 1] == 'а')
                {
                    genitiveName = genitiveName.Substring(0, genitiveName.Length - 1) + 'ы';
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


        [JsonIgnore]
        public bool IsLightDeveloper
        {
            get
            {
                return LightDeveloperGeneralDirector ||
                    LightDeveloperDirector ||
                    LightDeveloperDeputyGeneralDirector ||
                    LightDeveloperHeadOfManagers ||
                    LightDeveloperHeadOfExperts ||
                    LightDeveloperHeadOfMarketers ||
                    LightDeveloperHeadOfEditors ||
                    LightDeveloperHeadOfSpecials ||
                    LightDeveloperHeadOfAgents ||
                    LightDeveloperHeadOfModerators ||
                    LightDeveloperHeadOfTechnicians ||
                    LightDeveloperManager ||
                    LightDeveloperExpert ||
                    LightDeveloperMarketer ||
                    LightDeveloperEditor ||
                    LightDeveloperSpecial ||
                    LightDeveloperAgent ||
                    LightDeveloperModerator ||
                    LightDeveloperTechnician;
            }
        }

        [JsonIgnore]
        public bool IsDeveloper
        {
            get
            {
                return SuperDeveloper ||
                    HighDeveloper ||
                    MediumDeveloper ||
                    IsLightDeveloper;
            }
        }

        [JsonIgnore]
        public bool IsMoreThanLightDeveloper
        {
            get
            {
                return IsDeveloper && !IsLightDeveloper;
            }
        }

        [JsonIgnore]
        public bool IsHead
        {
            get
            {
                return HeadOfManagers ||
                    HeadOfExperts ||
                    HeadOfMarketers ||                    
                    HeadOfEditors ||
                    HeadOfSpecials ||
                    HeadOfAgents ||
                    HeadOfModerators ||
                    HeadOfTechnicians;
            } 
        }



        public void NotADeveloper()
        {
            SuperDeveloper = false;
            HighDeveloper = false;
            MediumDeveloper = false;
            LightDeveloperGeneralDirector = false;
            LightDeveloperDirector = false;
            LightDeveloperDeputyGeneralDirector = false;
            LightDeveloperHeadOfManagers = false;
            LightDeveloperHeadOfExperts = false;
            LightDeveloperHeadOfMarketers = false;
            LightDeveloperHeadOfEditors = false;
            LightDeveloperHeadOfSpecials = false;
            LightDeveloperHeadOfAgents = false;
            LightDeveloperHeadOfModerators = false;
            LightDeveloperHeadOfTechnicians = false;
            LightDeveloperManager = false;
            LightDeveloperExpert = false;
            LightDeveloperMarketer = false;
            LightDeveloperEditor = false;
            LightDeveloperSpecial = false;
            LightDeveloperAgent = false;
            LightDeveloperModerator = false;
            LightDeveloperTechnician = false;
        }



        public void UpdatingUser()
        {

        }



        public static string GetHash(string password)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(
            password));
            return Convert.ToBase64String(hash);
        }
    }
}
