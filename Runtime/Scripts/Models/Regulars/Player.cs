using GRAMOFON.Misc;

namespace GRAMOFON.Models
{
    public class Player : BaseModel
    {
        public int Currency;

        /// <summary>
        /// This function return related data key.
        /// </summary>
        protected override string GetDataKey => GRAMOFONCommonTypes.PLAYER_DATA_KEY;
    }
}