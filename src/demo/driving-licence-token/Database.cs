using LiteDB;

namespace driving_licence_token
{
    public class Database
    {
        public static LiteDatabase DB { set; get; }
        public const string DB_NAME = @"driving-licence-token.db";
        public const string TB_BLOCKS = "tb_blocks";
        public const string TB_TRANSACTION_POOL = "tb_transaction_pool";
        public const string TB_TRANSACTIONS = "tb_transactions";
        public const string TB_VALIDATORS = "tb_validators";

        public static void Initialize()
        {
            DB = new LiteDatabase(DB_NAME);
        }

        public static void ClearDB()
        {
            var coll = DB.GetCollection<Block>(TB_BLOCKS);
            coll.DeleteAll();
            var coll2 = DB.GetCollection<Transaction>(TB_TRANSACTION_POOL);
            coll2.DeleteAll();
            var coll3 = DB.GetCollection<Transaction>(TB_TRANSACTIONS);
            coll3.DeleteAll();
            var coll4 = DB.GetCollection<Transaction>(TB_VALIDATORS);
            coll4.DeleteAll();
        }

        public static void CloseDB()
        {
            DB.Dispose();
        }
    }
}
