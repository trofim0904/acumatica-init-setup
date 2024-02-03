using System;
using System.Collections.Generic;
using System.Linq;
using TrialBalanceConversionTool.Entities;
using TrialBalanceConversionTool.Interfaces;

namespace TrialBalanceConversionTool.Services
{
    public class DataConverter: IDataConverter
    {
        private static OutputFileData OutputFileData { get; set; }

        public DataConverter()
        {
            OutputFileData = new OutputFileData();
        }

        public OutputFileData GetOutputFileData(InputFileData inputFileData, ConversionRules conversionRules)
        {
            InitializeOutputFileData(inputFileData, conversionRules);
            return OutputFileData;
        }

        private static void InitializeOutputFileData(InputFileData inputFileData, ConversionRules conversionRules)
        {
            OutputFileData.OutputRows = GetConvertedRows(inputFileData, conversionRules);
            InitializeTotals();
        }

        private static void InitializeTotals()
        {
            InitializeEquityGroupTotals();
            InitializeProfitGroupTotals();
            InitializeBalanceGroupTotals();
            var outputRows = OutputFileData.OutputRows.ToList();
            OutputFileData.DebitAmountTotal = outputRows.Where(x => !x.IsContainError).Sum(x => x.DebitAmount);
            OutputFileData.CreditAmountTotal = outputRows.Where(x => !x.IsContainError).Sum(x => x.CreditAmount);
        }

        private static void InitializeBalanceGroupTotals()
        {
            OutputFileData.Debits = OutputFileData.AssetsBalanceTotal +
                                             OutputFileData.ExpensesBalanceTotal;
            OutputFileData.Credits = OutputFileData.LiabilitiesBalanceTotal +
                                              OutputFileData.IncomeBalanceTotal;
            OutputFileData.BalanceTotal = OutputFileData.Debits - OutputFileData.Credits;
        }

        private static void InitializeEquityGroupTotals()
        {
            OutputFileData.AssetsBalanceTotal = GetBalanceTotalByAccountType(AccountTypes.Asset);
            OutputFileData.LiabilitiesBalanceTotal = GetBalanceTotalByAccountType(AccountTypes.Liability);
            OutputFileData.Equity = OutputFileData.AssetsBalanceTotal -
                                             OutputFileData.LiabilitiesBalanceTotal;
        }

        private static void InitializeProfitGroupTotals()
        {
            OutputFileData.IncomeBalanceTotal = GetBalanceTotalByAccountType(AccountTypes.Income);
            OutputFileData.ExpensesBalanceTotal = GetBalanceTotalByAccountType(AccountTypes.Expense);
            OutputFileData.Profit = OutputFileData.IncomeBalanceTotal -
                                             OutputFileData.ExpensesBalanceTotal;
        }

        private static IEnumerable<OutputRow> GetConvertedRows(InputFileData inputFileData, ConversionRules conversionRules)
        {
            var outputRows = new List<OutputRow>();
            inputFileData.InputRows.ToList().ForEach(inputRow =>
            {
                if (inputRow.Debit != 0.00 && inputRow.Credit != 0.00)
                {
                    IEnumerable<OutputRow> splitOutputRows = GetSplitOutputRows(inputRow, conversionRules);
                    outputRows.AddRange(splitOutputRows);
                }
                else if (!IsContainOnlyZeroValues(inputRow))
                {
                    OutputRow outputRow = GetOutputRow(inputRow, conversionRules);
                    outputRows.Add(outputRow);
                }
            });

            return outputRows;
        }

        private static bool IsContainOnlyZeroValues(InputRow inputRow)
        {
            return inputRow.BeginningBalance == 0.00 && inputRow.Debit == 0.00 &&
                   inputRow.Credit == 0.00 && inputRow.EndingBalance == 0.00;
        }

        private static double GetBalanceTotalByAccountType(Enum accountType)
        {
            var filteredRows = OutputFileData.OutputRows
                .Where(x => IsExpectedAccountType(x.Type, accountType) && !x.IsContainError);
            return filteredRows.Sum(x => x.EndingBalance);
        }

        private static OutputRow GetOutputRow(InputRow inputRow, ConversionRules conversionRules)
        {
            OutputRow outputRow = GetBaseConvertedRow(inputRow, conversionRules);
            string accountType = GetConvertedType(inputRow.AccountFirstNumber, conversionRules);
            SetBalancesDependingOnType(inputRow, outputRow, accountType);
            outputRow.DebitAmount = inputRow.Debit;
            outputRow.CreditAmount = inputRow.Credit;
            outputRow.IsSingleLineRow = true;
            return outputRow;
        }

        private static void SetBalancesDependingOnType(InputRow inputRow, OutputRow outputRow, string accountType)
        {
            if (IsExpectedAccountType(accountType, AccountTypes.Income) ||
                IsExpectedAccountType(accountType, AccountTypes.Liability))
            {
                outputRow.BeginningBalance = inputRow.BeginningBalance * -1;
                outputRow.EndingBalance = outputRow.BeginningBalance + inputRow.Credit - inputRow.Debit;
            }
            else
            {
                outputRow.BeginningBalance = inputRow.BeginningBalance;
                outputRow.EndingBalance = inputRow.EndingBalance;
            }
        }

        private static IEnumerable<OutputRow> GetSplitOutputRows(InputRow inputRow, ConversionRules conversionRules)
        {
            string accountType = GetConvertedType(inputRow.AccountFirstNumber, conversionRules);
            var isTransactionWithCreditIncrease =
                IsExpectedAccountType(accountType, AccountTypes.Income) ||
                IsExpectedAccountType(accountType, AccountTypes.Liability);
            return GetSplitRow(inputRow, conversionRules, isTransactionWithCreditIncrease);
        }

        private static IEnumerable<OutputRow> GetSplitRow(InputRow inputRow, ConversionRules conversionRules,
            bool isTransactionWithCreditIncrease)
        {
            var splitTransactionRows = new List<OutputRow>();
            OutputRow firstLine = GetBaseConvertedRow(inputRow, conversionRules);
            OutputRow secondLine = GetBaseConvertedRow(inputRow, conversionRules);
            if (isTransactionWithCreditIncrease)
            {
                SetFieldsWithCreditIncrease(inputRow, firstLine, secondLine);
            }
            else
            {
                SetFieldsWithDebitIncrease(inputRow, firstLine, secondLine);
            }
            
            splitTransactionRows.Add(firstLine);
            splitTransactionRows.Add(secondLine);
            return splitTransactionRows;
        }

        private static void SetFieldsWithCreditIncrease(InputRow inputRow, OutputRow firstLine, OutputRow secondLine)
        {
            firstLine.BeginningBalance = inputRow.BeginningBalance * -1;
            firstLine.DebitAmount = inputRow.Debit;
            firstLine.IsFirstLineRow = true;
            secondLine.CreditAmount = inputRow.Credit;
            secondLine.EndingBalance = firstLine.BeginningBalance + inputRow.Credit - inputRow.Debit;
            secondLine.IsSecondLineRow = true;
        }

        private static void SetFieldsWithDebitIncrease(InputRow inputRow, OutputRow firstLine, OutputRow secondLine)
        {
            firstLine.BeginningBalance = inputRow.BeginningBalance;
            firstLine.DebitAmount = inputRow.Debit;
            firstLine.IsFirstLineRow = true;
            secondLine.CreditAmount = inputRow.Credit;
            secondLine.EndingBalance = inputRow.EndingBalance;
            secondLine.IsSecondLineRow = true;
        }

        private static OutputRow GetBaseConvertedRow(InputRow inputRow, ConversionRules conversionRules)
        {
            AccountConversionRow accountConversionRow = GetAccountConversionRow(inputRow, conversionRules);
            BranchConversionRow branchConversionRow = GetBranchConversionRow(inputRow, conversionRules);
            TypeConversionRow typeConversionRow = GetTypeConversionRow(inputRow.AccountFirstNumber, conversionRules);

            if (accountConversionRow == null)
            {
                return GetOutputRowWithError(Constants.ErrorMessages.AccountIsNotFound, inputRow);
            }

            if (branchConversionRow == null)
            {
                return GetOutputRowWithError(Constants.ErrorMessages.BranchIsNotFound, inputRow);
            }

            if (string.IsNullOrWhiteSpace(branchConversionRow.OutputDistrict))
            {
                return GetOutputRowWithError(Constants.ErrorMessages.DistrictIsNotFound, inputRow);
            }

            if (string.IsNullOrWhiteSpace(branchConversionRow.OutputRegion))
            {
                return GetOutputRowWithError(Constants.ErrorMessages.RegionIsNotFound, inputRow);
            }

            if (typeConversionRow == null)
            {
                return GetOutputRowWithError(Constants.ErrorMessages.TypeIsNotFound, inputRow);
            }

            var convertedRow = new OutputRow
            {
                Account = accountConversionRow.OutputAccount,
                Description = accountConversionRow.OutputDescription,
                Branch = branchConversionRow.OutputBranch,
                SubAccount = branchConversionRow.OutputSubAccount,
                Type = typeConversionRow.OutputType
            };

            return convertedRow;
        }

        private static BranchConversionRow GetBranchConversionRow(InputRow inputRow, ConversionRules conversionRules)
        {
            return conversionRules.BranchConversionDictionary
                .SingleOrDefault(x => x.InputBranch == inputRow.Branch);
        }

        private static AccountConversionRow GetAccountConversionRow(InputRow inputRow, ConversionRules conversionRules)
        {
            return conversionRules.AccountConversionDictionary
                .SingleOrDefault(x => x.InputAccount == inputRow.Account);
        }

        private static TypeConversionRow GetTypeConversionRow(string inputAccountFirstNumber, ConversionRules conversionRules)
        {
            return conversionRules.TypeConversionDictionary
                .SingleOrDefault(x => x.AccountFirstNumber == inputAccountFirstNumber);
        }

        private static string GetErrorMessage(string errorMessageType, InputRow inputRow)
        {
            return
                $"{errorMessageType} - {inputRow.Number} - {inputRow.BeginningBalance} / {inputRow.Debit} / {inputRow.Credit} / {inputRow.EndingBalance}";
        }

        private static OutputRow GetOutputRowWithError(string errorMessageType, InputRow inputRow)
        {
            return new OutputRow
            {
                Branch = GetErrorMessage(errorMessageType, inputRow),
                IsContainError = true
            };
        }

        private static string GetConvertedType(string inputAccountFirstNumber, ConversionRules conversionRules)
        {
            return conversionRules.TypeConversionDictionary
                .SingleOrDefault(x => x.AccountFirstNumber == inputAccountFirstNumber)?.OutputType;
        }

        private static bool IsExpectedAccountType(string currentAccountType, Enum expectedAccountType)
        {
            return currentAccountType != null && currentAccountType.Equals(expectedAccountType.ToString());
        }
    }
}
