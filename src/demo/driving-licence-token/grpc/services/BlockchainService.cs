using driving_licence_token;
using Grpc.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
            var validator = Validator.ValidatorList.Find(x => x.Address.Equals(request.SenderAddress));
            if (validator is null)
            {
                return Task.FromResult(new TrxResponse
                {
                    Result = "Sender not valid"
                });
            }

            List<Card> dataCard = new();
            try
            {
                dataCard = JsonConvert.DeserializeObject<List<Card>>(request.Data);
            }
            catch (Exception)
            {
                return Task.FromResult(new TrxResponse
                {
                    Result = "Data not valid"
                });
            }

            var trxIn = new TrxInput
            {
                SenderAddress = request.SenderAddress,
                TimeStamp = Utils.GetTime()
            };
            var trxOut = new TrxOutput
            {
                RecipientAddress = request.RecipientAddress,
                AmountLicenceToken = dataCard.Count,
                Data = JsonConvert.SerializeObject(dataCard),
            };
            var trxHash = Utils.GetTransactionHash(trxIn, trxOut);
            var signature = Validator.CreateSignature(trxHash, validator.PrivKey);
            trxIn.Signature = signature;

            var newTrx = new Transaction()
            {
                Hash = trxHash,
                TimeStamp = trxIn.TimeStamp,
                Sender = trxIn.SenderAddress,
                Recipient = trxOut.RecipientAddress,
                AmountLicenceToken = trxOut.AmountLicenceToken,
                Data = trxOut.Data,
            };
            // verify transaction ID
            var trxHashVerify = newTrx.GetHash();
            if (!trxHashVerify.Equals(trxHash))
            {
                return Task.FromResult(new TrxResponse
                {
                    Result = "Transaction ID not valid"
                });
            }
            // verify signature
            var trxValid = Transaction.VerifySignature(Validator.GetPubKeyHex(validator.PubKey), trxHash, trxIn.Signature);
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

        public override Task<BalanceResponse> GetBalance(AccountRequest request, ServerCallContext context)
        {
            var balance = Transaction.GetBalance(request.Address);
            return Task.FromResult(new BalanceResponse
            {
                Balance = balance
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

        public override Task<AccountResponse> CreateAccount(EmptyRequest request, ServerCallContext context)
        {
            AccountResponse response = new();
            var account = new Account();
            response.SecretNumber = account.SecretNumber.ToString();
            response.Address = account.GetAddress();
            response.PublicKey = account.GetPubKeyHex();
            return Task.FromResult(response);
        }
    }
}
