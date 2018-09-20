using System;
using System.Collections.Generic;
using System.Text;

namespace Lands2.ViewModels
{
    
    public class MainViewModel
    {
        #region ViewModels
        public LoginViewModel Login { get; set; }
        #endregion

        #region Constructors
        public MainViewModel()
        {
            this.Login = new LoginViewModel();
        }
        #endregion
    }
}
    
    
