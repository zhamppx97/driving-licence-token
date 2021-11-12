using driving_licence_token;
using Grpc.Core;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace GrpcService.Services
{
    public class BlockchainService : DrivingLicenceTokenBlockChainService.DrivingLicenceTokenBlockChainServiceBase
    {
        public override Task<BlockResponse> GenesisBlock(EmptyRequest request, ServerCallContext context)
        {
            var block = Blockchain.GetGenesisBlock();
            BlockModel mdl = ConvertBlock(block);
            return Task.FromResult(new BlockResponse
            {
                Block = mdl
            });
        }

        public override Task<BlockResponse> LastBlock(EmptyRequest request, ServerCallContext context)
        {
            var block = Blockchain.GetLastBlock();
            BlockModel mdl = ConvertBlock(block);
            return Task.FromResult(new BlockResponse
            {
                Block = mdl
            });
        }

        public override Task<BlocksResponse> GetBlocks(BlockRequest request, ServerCallContext context)
        {
            var blocks = Blockchain.GetBlocks(request.PageNumber, request.ResultPerPage);
            BlocksResponse response = new();
            foreach (Block block in blocks)
            {
                BlockModel mdl = ConvertBlock(block);
                response.Blocks.Add(mdl);
            }
            return Task.FromResult(response);
        }

        public override Task<TransactionsResponse> GetTransactions(AccountRequest request, ServerCallContext context)
        {
            TransactionsResponse response = new();
            var transactions = Transaction.GetTransactions(request.Address);
            foreach (Transaction trx in transactions)
            {
                TrxModel mdl = ConvertTrxModel(trx);
                response.Transactions.Add(mdl);
            }
            return Task.FromResult(response);
        }

        public override Task<TrxResponse> SendToken(SendRequest request, ServerCallContext context)
        {
            var newTrx = new Transaction()
            {
                Hash = request.TrxId,
                TimeStamp = request.TrxInput.TimeStamp,
                Sender = request.TrxInput.SenderAddress,
                Recipient = request.TrxOutput.RecipientAddress,
                AmountLicenceToken = request.TrxOutput.AmountLicenceToken,
                Data = request.TrxOutput.Data,
            };
            // verify transaction ID
            var trxHash = newTrx.GetHash();
            if (!trxHash.Equals(request.TrxId))
            {
                return Task.FromResult(new TrxResponse
                {
                    Result = "Transaction ID not valid"
                });
            }
            // Verify signature
            var trxValid = Transaction.VerifySignature(request.PublicKey, request.TrxId, request.TrxInput.Signature);
            if (!trxValid)
            {
                return Task.FromResult(new TrxResponse
                {
                    Result = "Signature not valid"
                });
            }
            Transaction.AddToPool(newTrx);
            return Task.FromResult(new TrxResponse
            {
                Result = "Success"
            });
        }

        private static BlockModel ConvertBlock(Block block)
        {
            try
            {
                BlockModel mdl = new()
                {
                    Version = block.Version,
                    Height = block.Height,
                    Hash = block.Hash,
                    PrevHash = block.PrevHash,
                    TimeStamp = block.TimeStamp,
                    Transactions = JsonConvert.SerializeObject(block.Transactions),
                    MerkleRoot = block.MerkleRoot,
                    NumOfTx = block.NumOfTx,
                    TotalAmountLicenceToken = block.TotalAmountLicenceToken,
                    Difficulty = block.Difficulty,
                    Validator = block.Validator
                };
                return mdl;
            }
            catch
            {
                return null;
            }
        }

        private static TrxModel ConvertTrxModel(Transaction trx)
        {
            TrxModel mdl = new()
            {
                Hash = trx.Hash,
                TimeStamp = trx.TimeStamp,
                Sender = trx.Sender,
                Recipient = trx.Recipient,
                AmountLicenceToken = trx.AmountLicenceToken,
                Data = trx.Data,
            };
            return mdl;
        }
    }
}
