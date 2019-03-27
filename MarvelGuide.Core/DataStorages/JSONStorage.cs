using MarvelGuide.Core.Intefraces;
using MarvelGuide.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarvelGuide.Core.DataStorages
{
    internal class JSONStorage : IStorage
    {
        private const string documentsFilePath = "../../../MarvelGuide.Core/Data/Documents.json";
        private const string usersFilePath = "../../../MarvelGuide.Core/Data/Users.json";


        IRepository<Document> _documents;
        IRepository<User> _users;

        bool _documentsLoaded;
        bool _usersLoaded;


        public IRepository<Document> Documents
        {
            get
            {
                if (_documentsLoaded)
                {
                    return _documents;
                }

                _documentsLoaded = true;

                _documents = new JSONRepository<Document>(documentsFilePath);

                return _documents;
            }
        }

        public IRepository<User> Users
        {
            get
            {
                if (_usersLoaded)
                {
                    return _users;
                }

                _usersLoaded = true;

                _users = new JSONRepository<User>(usersFilePath);

                return _users;
            }
        }



        public void ChangingUsersModels()
        {
            foreach (var user in Users.Items)
            {
                user.UpdatingUser();
            }

            Users.Save();
        }

        public void ChangingDocumentsModels()
        {
            foreach (var document in Documents.Items)
            {
                document.UpdatingDocument();
            }

            Documents.Save();
        }
    }
}
