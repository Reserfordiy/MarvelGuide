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
        private const string rubricsFilePath = "../../../MarvelGuide.Core/Data/Rubrics.json";


        IRepository<Document> _documents;
        IRepository<User> _users;
        IRepository<Rubric> _rubrics;

        bool _documentsLoaded;
        bool _usersLoaded;
        bool _rubricsLoaded;


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

                foreach (var editor in _users.Items.Where(u => u.Editor))
                {
                    foreach (var editorsPublication in editor.EditorsRubrics)
                    {
                        Rubric rubric = Rubrics.Items.FirstOrDefault(rubr => rubr.Id == editorsPublication.RubricClassID);

                        editorsPublication.RubricClass = rubric;
                    }
                }

                return _users;
            }
        }

        public IRepository<Rubric> Rubrics
        {
            get
            {
                if (_rubricsLoaded)
                {
                    return _rubrics;
                }

                _rubricsLoaded = true;

                _rubrics = new JSONRepository<Rubric>(rubricsFilePath);

                foreach (var rubric in _rubrics.Items)
                {
                    Document document = Documents.Items.FirstOrDefault(doc => doc.Id == rubric.DocumentId);

                    rubric.Document = document;
                }

                return _rubrics;
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

        public void ChangingRubricsModels()
        {
            foreach (var rubric in Rubrics.Items)
            {

            }

            Rubrics.Save();
        }
    }
}
