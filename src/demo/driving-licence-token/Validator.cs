using LiteDB;
using System;
using System.Collections.Generic;

namespace driving_licence_token
{
    public class Validators
    {
        public string Address { set; get; }
    }

    public class Validator
    {
        public static List<Validators> ValidatorList { get; set; }

        public static void Add(Validators v)
        {
            var validators = GetAll();
            validators.Insert(v);
            ValidatorList.Add(v);
        }

        public static ILiteCollection<Validators> GetAll()
        {
            var coll = Database.DB.GetCollection<Validators>(Database.TB_VALIDATORS);
            return coll;
        }

        internal static void Initialize()
        {
            ValidatorList = new List<Validators>();
            var validator = GetAll();
            if (validator.Count() < 1)
            {
                Add(new Validators
                {
                    Address = "ZX_JavaPsOANbgT5anGjTg0Ih6qdC4mHgbmpF5ptjAJb0g=",
                });
                Add(new Validators
                {
                    Address = "ZX_mGyJe2kD3cNs4c8d/KHVe4+DSt9mwrLLqlDejXUgdzA=",
                });
                Add(new Validators
                {
                    Address = "ZX_ZOm+XeyKAEbIb/L41TPEzRRxwMOsZW6HE2WjdxeCFFI=",
                });
                Add(new Validators
                {
                    Address = "ZX_rMOHTqvkDCLtaoqbkgF3GmM2lewE3R2ZFYDGfq0A/fI=",
                });
                ValidatorList.AddRange(GetAll().FindAll());
            }
            else
            {
                ValidatorList.AddRange(GetAll().FindAll());
            }
        }

        public static string GetValidator()
        {
            var numOfValidators = ValidatorList.Count;
            var random = new Random();
            int choosed = random.Next(0, numOfValidators);
            var validatorAddr = ValidatorList[choosed].Address;
            return validatorAddr;
        }
    }
}
