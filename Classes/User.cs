﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace RegIN_Filimonova.Classes
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public byte[] Image = new byte[0];
        public string PinCode { get; set; }
        public DateTime DateUpdate { get; set; }
        public DateTime DateCreate { get; set; }
        public CorrectLogin HandlerCorrectLogin;
        public InCorrectLogin HandlerInCorrectLogin;
        public delegate void CorrectLogin();
        public delegate void InCorrectLogin();
        public void GetUserLogin(string Login)
        {
            this.Id = -1;
            this.Login = String.Empty;
            this.Password = String.Empty;
            this.Name = String.Empty;
            this.Image = new byte[0];
            this.PinCode = String.Empty;
            MySqlConnection mySqlConnection = WorkingDB.OpenConnection();
            if (WorkingDB.OpenConnection(mySqlConnection))
            {
                MySqlDataReader userQuery = WorkingDB.Query($"SELECT * FROM `users` WHERE `Login` = '{Login}'", mySqlConnection);
                if (userQuery.HasRows)
                {
                    userQuery.Read();
                    this.Id = userQuery.GetInt32(0);
                    this.Login = userQuery.GetString(1);
                    this.Password = userQuery.GetString(2);
                    this.Name = userQuery.GetString(3);
                    if (!userQuery.IsDBNull(4))
                    {
                        this.Image = new byte[64 * 1024];
                        userQuery.GetBytes(4, 0, Image, 0, Image.Length);
                    }
                    this.PinCode = userQuery.GetString(5);
                    this.DateUpdate = userQuery.GetDateTime(6);
                    this.DateCreate = userQuery.GetDateTime(7);
                    HandlerCorrectLogin.Invoke();
                }
                else
                {
                    HandlerInCorrectLogin.Invoke();
                }
            }
            else
            {
                HandlerInCorrectLogin.Invoke();
                WorkingDB.CloseConnection(mySqlConnection);
            }
        }
        public void SetUser()
        {
            MySqlConnection mySqlConnection = WorkingDB.OpenConnection();
            if (WorkingDB.OpenConnection(mySqlConnection))
            {
                MySqlCommand mySqlCommand = new MySqlCommand("INSERT INTO `users`(`Login`, `Password`, `Name`, `Image`, `PinCode`, `DateUpdate`, `DateCreate`) VALUES" +
                    "(@Login, @Password, @Name, @Image, @PinCode, @DateUpdate, @DateCreate)", mySqlConnection);
                mySqlCommand.Parameters.AddWithValue("@Login", this.Login);
                mySqlCommand.Parameters.AddWithValue("@Password", this.Password);
                mySqlCommand.Parameters.AddWithValue("@Name", this.Name);
                mySqlCommand.Parameters.AddWithValue("@Image", this.Image);
                mySqlCommand.Parameters.AddWithValue("@PinCode", this.PinCode);
                mySqlCommand.Parameters.AddWithValue("@DateUpdate", this.DateUpdate);
                mySqlCommand.Parameters.AddWithValue("@DateCreate", this.DateCreate);
                mySqlCommand.ExecuteNonQuery();
            }
            WorkingDB.CloseConnection(mySqlConnection);
        }
        public void UpdatePinCode()
        {
            using (MySqlConnection mySqlConnection = WorkingDB.OpenConnection())
            {
                if (mySqlConnection != null)
                {
                    MySqlCommand mySqlCommand = new MySqlCommand("UPDATE `users` SET `PinCode` = @PinCode WHERE `Login` = @Login", mySqlConnection);
                    mySqlCommand.Parameters.AddWithValue("@PinCode", this.PinCode);
                    mySqlCommand.Parameters.AddWithValue("@Login", this.Login);
                    mySqlCommand.ExecuteNonQuery();
                }
            }
        }
        public void CreateNewPassword()
        {
            if (Login != String.Empty)
            {
                Password = GeneratePass();
                MySqlConnection mySqlConnection = WorkingDB.OpenConnection();
                if (WorkingDB.OpenConnection(mySqlConnection))
                {
                    WorkingDB.Query($"UPDATE `users` SET `Password` = '{this.Password}' WHERE `Login` = '{this.Login}'", mySqlConnection);
                }
                WorkingDB.CloseConnection(mySqlConnection);
                SendMail.SendMessage($"Your account password has been changed.\nNew password: {this.Password}", this.Login);
            }
        }
        public string GeneratePass()
        {
            List<Char> NewPassword = new List<Char>();
            Random rnd = new Random();
            char[] ArrNumbers = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            char[] ArrSymbols = { '|', '-', '_', '!', '@', '#', '$', '%', '&', '*', '=', '+' };
            char[] ArrUppercase = { 'q', 'w', 'e', 'r', 't', 's', 'y', 'u', 'i', 'o', 'p', 'a', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'z', 'x', 'c', 'v', 'b', 'n', 'm' };
            for (int i = 0; i < 1; i++)
            {
                NewPassword.Add(ArrNumbers[rnd.Next(0, ArrNumbers.Length)]);
            }
            for (int i = 0; i < 1; i++)
            {
                NewPassword.Add(ArrSymbols[rnd.Next(0, ArrSymbols.Length)]);
            }
            for (int i = 0; i < 1; i++)
            {
                NewPassword.Add(char.ToUpper(ArrUppercase[rnd.Next(0, ArrUppercase.Length)]));
            }
            for (int i = 0; i < NewPassword.Count; i++)
            {
                int RandomSymbol = rnd.Next(0, NewPassword.Count);
                char Symbol = NewPassword[RandomSymbol];
                NewPassword[RandomSymbol] = NewPassword[i];
                NewPassword[i] = Symbol;
            }
            string NPassword = "";
            for (int i = 0; i < NewPassword.Count; i++)
            {
                NPassword += NewPassword[i];
            }
            return NPassword;
        }
    }
}
