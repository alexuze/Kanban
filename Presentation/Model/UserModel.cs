using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Model
{
    public class UserModel : NotifiableModelObject
    {
        public UserModel(BackendController controller, string email) : base(controller)
        {
            this.Email = email;
        }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                this._email = value;
                RaisePropertyChanged("Email");
            }
        }

        internal BoardModel GetBoard()
        {
            return new BoardModel(Controller, this);
        }
    }
}
