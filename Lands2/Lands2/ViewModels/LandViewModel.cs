namespace Lands2.ViewModels
{
    using Lands2.Models;

    public class LandViewModel
    {
        #region Properties
        public Land Land { get; set; }
        #endregion

        #region Contructors
        public LandViewModel(Land land)
        {
            this.Land = land;
        }
        #endregion
    }
}
