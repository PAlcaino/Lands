namespace Lands2.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Text;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class LoginViewModel : BaseViewModel
    {
        #region Events

        #endregion

        #region Propierties
        public string Email { get; set; }

        public string Password
        {
            get {return this.password;}
            set {SetValue(ref this.password, value);}
        }

        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }
        public bool IsRemembered { get; set; }
        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { SetValue(ref this.isEnabled, value); }
        }
        #endregion

        #region Atributtes
        private string password;
        private bool isEnabled;
        private bool isRunning;
        #endregion

        #region Constructors
        public LoginViewModel()
        {
            this.IsRemembered = true;
            this.IsEnabled = true;
        }
        #endregion

        #region Commands
        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand(Login);
            }
        }       

        private async void Login()
        {
            if(string.IsNullOrEmpty(Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Debe ingresar un email",
                    "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(Password))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Debe ingresar una contraseña",
                    "Aceptar");
                return;
            }

            this.IsEnabled = false;
            this.IsRunning = true;

            if(this.Email != "palcaino.lleite@gmail.com" || this.Password != "123456")
            {
                this.IsEnabled = true;
                this.IsRunning = false;

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Correo o contraseña incorrectos",
                    "Aceptar");
                this.Password = string.Empty;
                return;
            }

            await Application.Current.MainPage.DisplayAlert(
                    "Ok",
                    "Login Correcto!",
                    "Aceptar");


            return;
        }
        #endregion
    }
}
