namespace Lands2.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Lands2.Helpers;
    using Lands2.Services;
    using Lands2.Views;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Text;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class LoginViewModel : BaseViewModel
    {
        #region Services
        private APIServices apiService;
        #endregion

        #region Events

        #endregion

        #region Propierties
        public string Email
        {
            get { return this.email; }
            set { SetValue(ref this.email, value); }
        }

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
        private string email;
        private string password;
        private bool isEnabled;
        private bool isRunning;
        #endregion

        #region Constructors
        public LoginViewModel()
        {
            this.apiService = new APIServices();
            this.IsRemembered = true;
            this.IsEnabled = true;

            this.Email = "palcaino.lleite@gmail.com";
            this.Password = "123456";
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

            if (string.IsNullOrEmpty(Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.EmailValidation,
                    Languages.Accept);
                return;
            }
            if (string.IsNullOrEmpty(Password))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PasswordValidation,
                    Languages.Accept);
                return;
            }

            this.IsEnabled = true;
            this.IsRunning = false;

            var connection = await this.apiService.CheckConnection();

            if(!connection.IsSuccess)
            {
                this.IsEnabled = false;
                this.IsRunning = true;

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Aceptar");
                this.IsEnabled = true;
                this.IsRunning = false;
                return;
            }

            var token = await apiService.GetToken(
                "https://lands2apialpha.azurewebsites.net/",
                this.Email,
                this.Password);

            if(token == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Algo ha ocurrido con su peticion, intente nuevamente",
                    "Aceptar");
                this.IsEnabled = true;
                this.IsRunning = false;
                return;
            }

            if(string.IsNullOrEmpty(token.AccessToken))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    token.ErrorDescription,
                    "Aceptar");
                this.Password = string.Empty;
                this.IsEnabled = true;
                this.IsRunning = false;
                return;
            }

            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Token = token;
            mainViewModel.Lands = new LandsViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new LandsPage());

            this.IsEnabled = true;
            this.IsRunning = false;

            this.Email = string.Empty;
            this.Password = string.Empty;
            return;
        }
        #endregion
    }
}
