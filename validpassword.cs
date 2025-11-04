using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WinFormsApp4
{
    public class validpassword
    {
        public bool Passwortd(string password)
        {
            try
            {
                if (password.Length <= 8)
                {
                    MessageBox.Show("Пароль должен содержать от 8 символов");
                    return false;
                }
                    string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$";
                    //MessageBox.Show($"=== ДИАГНОСТИКА ПАРОЛЯ ===");
                    //MessageBox.Show($"Введенный пароль: '{password}'");
                    //MessageBox.Show($"Длина пароля: {password.Length} символов");
                if (!Regex.IsMatch(password, pattern))
                {
                    MessageBox.Show("Пароль должен содержать минимум одну заглавную букву, одну строчную букву, одну цифру и один специальный символ");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка проверки пароля" + ex.Message);
                return false;
            }
        }
        

    }
}
