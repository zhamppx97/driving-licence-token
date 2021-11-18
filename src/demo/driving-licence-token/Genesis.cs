using System.Collections.Generic;

namespace driving_licence_token
{
    public class GenesisAccount
    {
        public string Address { set; get; }
        public string PublicKey { set; get; }
        public double BalanceLicenceToken { set; get; }
    }

    public static class Genesis
    {
        public static List<GenesisAccount> GetAll()
        {
            var list = new List<GenesisAccount>
            {
                new GenesisAccount
                {
                    // secret nunber
                    // 9037598758175796877469642835103372668090558850907464818137939150342753595290
                    Address = "ZX_l5+DlJiQherZ+sbdAXCPZEuq8FQX8+GPuENm03Sc6lI=",
                    PublicKey = "09924545798ea5d1918504445ab7cde99590f69b3afaa8310d618c40649248c195a02bf20d0d41103e5d1d47a8d26902d4309e6c1fdf8ccac253c4e8d84f52f6",
                    BalanceLicenceToken = 2
                },
                new GenesisAccount
                {
                    // secret number
                    // 35135936049611014436353059650510881094530231327628798498591367487079272826819
                    Address = "ZX_BIzcoHrPfH4SSLXUrWOW2jOAwI14ptlCiP/3C2ZFEOQ=",
                    PublicKey = "f9446e96caf7b344cddad68c566c83c2c78160baf33d1bc80b1c3ba7eec3f5971ad4ebf0b54ab0ed1347a4fc8d5786ef392dbbad64fed40f1d45adbff56f9400",
                    BalanceLicenceToken = 2
                }
            };
            return list;
        }
    }
}
