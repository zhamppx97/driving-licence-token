using EllipticCurve;
using System;
using System.Numerics;
using System.Security.Cryptography;

namespace driving_licence_token
{
    public class Account
    {
        public BigInteger SecretNumber { set; get; }
        public PrivateKey PrivKey { set; get; }
        public PublicKey PubKey { set; get; }

        public Account(string secret = "")
        {
            if (secret != "")
            {
                PrivKey = new PrivateKey("secp256k1", BigInteger.Parse(secret));
            }
            else
            {
                PrivKey = new PrivateKey();
            }
            SecretNumber = PrivKey.secret;
            PubKey = PrivKey.publicKey();
        }

        public string GetPubKeyHex()
        {
            return Convert.ToHexString(PubKey.toString()).ToLower();
        }

        public string GetAddress()
        {
            byte[] hash = SHA256.Create().ComputeHash(PubKey.toString());
            return "ZX_" + Convert.ToBase64String(hash);
        }

        public string CreateSignature(string message)
        {
            Signature signature = Ecdsa.sign(message, PrivKey);
            return signature.toBase64();
        }
    }
}
