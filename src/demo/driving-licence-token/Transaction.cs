using EllipticCurve;
using LiteDB;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace driving_licence_token
{
    public class Transaction
    {
        public string Hash { get; set; }
        public long TimeStamp { get; set; }
        public string Sender { set; get; }
        public string Recipient { set; get; }
        public int AmountLicenceToken { set; get; }
        public string Data { set; get; }

        public static void AddToPool(Transaction transaction)
        {
            var trxPool = GetPool();
            trxPool.Insert(transaction);
        }

        public static void Add(Transaction transaction)
        {
            var transactions = GetAll();
            transactions.Insert(transaction);
        }

        public static ILiteCollection<Transaction> GetPool()
        {
            var coll = Database.DB.GetCollection<Transaction>(Database.TB_TRANSACTION_POOL);
            return coll;
        }

        public static ILiteCollection<Transaction> GetAll()
        {
            var coll = Database.DB.GetCollection<Transaction>(Database.TB_TRANSACTIONS);
            return coll;
        }

        public static IEnumerable<Transaction> GetTransactions(string address)
        {
            var coll = Database.DB.GetCollection<Transaction>(Database.TB_TRANSACTIONS);
            coll.EnsureIndex(x => x.TimeStamp);
            var transactions = coll.Find(x => x.Sender == address || x.Recipient == address);
            return transactions;
        }

        public static void CreateIcoTransction()
        {
            List<Card> card = new();
            var timeStamp = Utils.GetTime();
            var newTrx = new Transaction()
            {
                TimeStamp = timeStamp,
                Sender = "IcoTransction",
                Recipient = "IcoTransction",
                AmountLicenceToken = 0,
                Data = JsonConvert.SerializeObject(card),
            };
            newTrx.Build();
            AddToPool(newTrx);
        }

        public static void CreateGenesisTransction()
        {
            List<Card> card = new();
            var timeStamp = Utils.GetTime();
            var newTrx = new Transaction()
            {
                TimeStamp = timeStamp,
                Sender = "GenesisTransction",
                Recipient = "GenesisTransction",
                AmountLicenceToken = 0,
                Data = JsonConvert.SerializeObject(card),
            };
            newTrx.Build();
            AddToPool(newTrx);
        }

        public static bool VerifySignature(string publicKeyHex, string message, string signature)
        {
            var byt = Utils.HexToBytes(publicKeyHex);
            var publicKey = PublicKey.fromString(byt);
            return Ecdsa.verify(message, Signature.fromBase64(signature), publicKey);
        }

        public void Build()
        {
            Hash = GetHash();
        }

        public string GetHash()
        {
            var mergeData = $"{TimeStamp}{Sender}{AmountLicenceToken}{Data}{Recipient}";
            return Utils.GenHash(Utils.GenHash(mergeData));
        }
    }
}
