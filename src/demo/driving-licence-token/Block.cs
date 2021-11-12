using System.Collections.Generic;
using System.Linq;

namespace driving_licence_token
{
    public class Block
    {
        public int Version { get; set; }
        public long Height { get; set; }
        public long TimeStamp { get; set; }
        public string PrevHash { get; set; }
        public string Hash { get; set; }
        public string MerkleRoot { get; set; }
        public IList<Transaction> Transactions { get; set; }
        public string Validator { get; set; }
        public int NumOfTx { get; set; }
        public int TotalAmountLicenceToken { get; set; }
        public int Difficulty { get; set; }

        public void Build()
        {
            Version = 1;
            NumOfTx = Transactions.Count;
            MerkleRoot = GetMerkleRoot();
            Hash = GetBlockHash();
            Difficulty = 1;
        }

        public static Block GenesisBlock(IList<Transaction> transactions)
        {
            var ts = 1636625397; // Nov 11, 2021
            var validator = Genesis.GetAll().FirstOrDefault();
            var block = new Block
            {
                Height = 0,
                TimeStamp = ts,
                PrevHash = "-",
                Transactions = transactions,
                Validator = validator.Address
            };
            block.Build();
            return block;
        }

        public string GetBlockHash()
        {
            var strSum = Version + PrevHash + MerkleRoot + TimeStamp + Difficulty + Validator;
            var hash = Utils.GenHash(strSum);
            return hash;
        }

        private string GetMerkleRoot()
        {
            var txsHash = new List<string>();
            foreach (var tx in Transactions)
            {
                txsHash.Add(tx.Hash);
            }
            var hashRoot = Utils.CreateMerkleRoot(txsHash.ToArray());
            return hashRoot;
        }
    }
}
