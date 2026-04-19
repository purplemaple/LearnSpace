using _2._0_MediatorExample.Countries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2._0_MediatorExample.Mediators
{
    //联合国安理会(具体的中介者)
    class UnitedNationsSecurityCouncil : UniteNations
    {
        private USA usa;
        private Iraq iraq;
        public USA Usa
        {
            set => usa = value;
        }
        public Iraq Iraq
        {
            set => iraq = value;
        }

        public override void Declare(string message, Country country)
        {
            if(country == usa)
            {
                iraq.GetMessage(message);
            }
            else
            {
                usa.GetMessage(message);
            }
        }
    }
}
