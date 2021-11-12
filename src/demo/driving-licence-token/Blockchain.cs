using LiteDB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace driving_licence_token
{
    public class Blockchain
    {
        public Blockchain()
        {
            Database.Initialize();
            Initialize();
            Validator.Initialize();
            Console.WriteLine("driving licence token processing ...");
        }

        private static void Initialize()
        {
            var blocks = GetBlocks();
            if (blocks.Count() < 1)
            {
                Transaction.CreateGenesisTransction();
                Transaction.CreateIcoTransction();
                var trxPool = Transaction.GetPool();
                var transactions = trxPool.FindAll().ToList();
                var block = Block.GenesisBlock(transactions);
                AddBlock(block);
                foreach (Transaction trx in transactions)
                {
                    Transaction.Add(trx);
                }
                trxPool.DeleteAll();
            }
        }

        public static List<Block> GetBlocks(int pageNumber, int resultPerPage)
        {
            var coll = Database.DB.GetCollection<Block>(Database.TB_BLOCKS);
            coll.EnsureIndex(x => x.Height);
            var query = coll.Query()
                .OrderByDescending(x => x.Height)
                .Offset((pageNumber - 1) * resultPerPage)
                .Limit(resultPerPage).ToList();
            return query;
        }

        public static ILiteCollection<Block> GetBlocks()
        {
            var coll = Database.DB.GetCollection<Block>(Database.TB_BLOCKS);
            coll.EnsureIndex(x => x.Height);
            return coll;
        }

        public static Block GetGenesisBlock()
        {
            var block = GetBlocks().FindAll().FirstOrDefault();
            return block;
        }

        public static Block GetLastBlock()
        {
            var blockchain = GetBlocks();
            var block = blockchain.FindOne(Query.All(Query.Descending));
            return block;
        }

        public static long GetHeight()
        {
            var lastBlock = GetLastBlock();
            return lastBlock.Height;
        }

        public static void AddBlock(Block block)
        {
            var blocks = GetBlocks();
            blocks.Insert(block);
        }

        public static void BuildNewBlock()
        {
            List<Card> card = new();
            var trxPool = Transaction.GetPool();
            var lastBlock = GetLastBlock();
            var height = lastBlock.Height + 1;
            var timestamp = Utils.GetTime();
            var prevHash = lastBlock.Hash;
            var validator = Validator.GetValidator();
            var transactions = new List<Transaction>();

            var conbaseTrx = new Transaction
            {
                AmountLicenceToken = 0,
                Recipient = "GenesisTransction",
                Data = JsonConvert.SerializeObject(card),
                TimeStamp = timestamp,
                Sender = "GenesisTransction"
            };
            if (trxPool.Count() > 0)
            {
                // get all tx from pool
                transactions.AddRange(trxPool.FindAll());
                trxPool.DeleteAll();
            }
            else
            {
                conbaseTrx.Build();
                transactions.Add(conbaseTrx);
            }
            var block = new Block
            {
                Height = height,
                TimeStamp = timestamp,
                PrevHash = prevHash,
                Transactions = transactions,
                Validator = validator
            };
            block.Build();
            AddBlock(block);
            PrintBlock(block);
            // move all record in trx pool to transactions table
            foreach (var trx in transactions)
            {
                Transaction.Add(trx);
            }
        }

        private static void PrintBlock(Block block)
        {
            Console.WriteLine("########## New block created ##########");
            Console.WriteLine("Height      : {0}", block.Height);
            Console.WriteLine("Version     : {0}", block.Version);
            Console.WriteLine("Prev Hash   : {0}", block.PrevHash);
            Console.WriteLine("Merkle Hash : {0}", block.MerkleRoot);
            Console.WriteLine("Timestamp   : {0}", Utils.ToDateTime(block.TimeStamp));
            Console.WriteLine("Difficulty  : {0}", block.Difficulty);
            Console.WriteLine("Validator   : {0}", block.Validator);
            Console.WriteLine("Number Of Tx: {0}", block.NumOfTx);
        }
    }
}
