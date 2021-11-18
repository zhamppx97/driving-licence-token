using EllipticCurve;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace driving_licence_token
{
    public class Validators
    {
        public string Address { set; get; }
        public byte[] PrivKey { set; get; }
        public byte[] PubKey { set; get; }
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
                var pKey1 = new PrivateKey();
                Add(new Validators
                {
                    Address = "ZX_jVL7M99B+Gq8MzRoUtKrYsRDCJeVo/n+DN9ldpV09Oc=",
                    PrivKey = pKey1.toString(),
                    PubKey = pKey1.publicKey().toString(),
                });
                var pKey2 = new PrivateKey();
                Add(new Validators
                {
                    Address = "ZX_ximh6/aA0n3WEyruyad8IG7HgnSg3S8qzJhlAtF/cqw=",
                    PrivKey = pKey2.toString(),
                    PubKey = pKey2.publicKey().toString(),
                });
                var pKey3 = new PrivateKey();
                Add(new Validators
                {
                    Address = "ZX_PuKJ3XcUYnCVW0aTR+n+WQMuLTe0jF7iMUj8n3MdIRw=",
                    PrivKey = pKey3.toString(),
                    PubKey = pKey3.publicKey().toString(),
                });
                var pKey4 = new PrivateKey();
                Add(new Validators
                {
                    Address = "ZX_mOS1iARv0zTnOtqo2IzpHKeWarI2jGZKCyUrIBw8HqQ=",
                    PrivKey = pKey4.toString(),
                    PubKey = pKey4.publicKey().toString(),
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

        public static string GetPubKeyHex(byte[] publicKey)
        {
            return Convert.ToHexString(publicKey).ToLower();
        }

        public static string CreateSignature(string message, byte[] privateKeyStr)
        {
            Signature signature = Ecdsa.sign(message, PrivateKey.fromString(privateKeyStr));
            return signature.toBase64();
        }
    }
}
