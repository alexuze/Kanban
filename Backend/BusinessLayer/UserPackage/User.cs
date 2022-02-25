using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.BusinessLayer.BoardPackage;
using System.Globalization;
using System.Text.RegularExpressions;
namespace IntroSE.Kanban.Backend.BusinessLayer.UserPackage
{
    internal class User
    {
        private const int minValue = 5;
        private const int maxValue = 25;
        private DataAccessLayer.User dalUser;
        private bool _isLogged;
        private BoardPackage.Board _board;
        private string email;
        private string password;
        private string nickname;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public User(string email, string password, string nickname, BoardPackage.Board board)
        {
            _isLogged = false;
            if (board == null)
                _board = new BoardPackage.Board(email);
            else
                _board = board;
            this.email = email;
            this.password = password;
            this.nickname = nickname;
            this.dalUser = new DataAccessLayer.User(0, email, password, nickname, _board.Email, true);
        }
        public User(DataAccessLayer.User dalUser, BoardPackage.Board busBoard)
        {
            _isLogged = false;
            _board = busBoard;
            this.email = dalUser.Email;
            this.password = dalUser.Password;
            this.nickname = dalUser.Nickname;
            this.dalUser = dalUser;
        }

        public User()
        {
        }

        public bool IsLogged { get => _isLogged; set => _isLogged = value; }
        public BoardPackage.Board Board { get => _board; set => _board = value; }
        public string Email
        {
            get => email;
            set
            {
                email = value;
                dalUser.Email = value;
            }
        }
        public string Password
        {
            get => password;
            set
            {
                password = value;
                dalUser.Password = value;
            }
        }
        public string Nickname
        {
            get => nickname;
            set
            {
                nickname = value;
                dalUser.Nickname = value;
            }
        }


        public bool validPass(string password)
        {
            bool upper = false, lower = false, num = false, flag = true;
            foreach (char a in password)
            {
                if (System.Char.IsUpper(a))
                    upper = true;
                if (System.Char.IsLower(a))
                    lower = true;
                if (System.Char.IsNumber(a))
                    num = true;
            }
            if (!(upper && lower && num))
                flag = false;
            if (password.Length < minValue || password.Length > maxValue)
                flag = false;
            if (string.IsNullOrWhiteSpace(password))
                flag = false;
            return flag;
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    var domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        //gets detailes about new user to register and check if its valid.
        public User Register(string email, string password, string nickname, BoardPackage.Board board)
        {
            if (!IsValidEmail(email))
            {
                log.Warn("Register failed: somone tried to register with invalid email");
                throw new Exception("invalid email");
            }
            if (!validPass(password))
            {
                log.Warn("Register failed: invalid password");
                throw new Exception("invalid password");
            }
            if (string.IsNullOrWhiteSpace(nickname))
            {
                log.Warn("Register failed: invalid nickname");
                throw new Exception("invalid nickname");
            }
            User newUser = new User(email, password, nickname, board);
            return newUser;
        }
    }

}
