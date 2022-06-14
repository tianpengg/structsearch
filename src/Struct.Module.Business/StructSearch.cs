using com.epam.indigo;
using GraphMolWrap;
using Struct.Common.Utils;
using Struct.Models.Dto;
using Struct.Models.Help;
using Struct.Module.IBusiness;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Struct.Module.Business
{
    public class StructSearch : IStructSearch
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        private static string _dbType = "";

        private static Indigo _indigo = new Indigo();

        private static string _tableName = "";

        #region 私有方法
        /// <summary>
        /// 检验db是否存在
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="tableIndex"></param>
        /// <returns></returns>
        private static bool DbPathExist(string tableName, string dbType)
        {
            string sdbPath = Path.Combine(AppConfig.SdbPath, tableName);

            if (dbType == DBType.Molecule)
            {
                return Directory.Exists(Path.Combine(sdbPath, AppConfig.Moleculedb));
            }
            else
            {
                return Directory.Exists(Path.Combine(sdbPath, AppConfig.Reactiondb));
            }
        }

        /// <summary>
        /// 获取Db实例
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="chemContent"></param>
        /// <returns></returns>
        private static (Bingo, string) GetBingo(string tableName,string chemContent)
        {
            var dbType = DBTypeUtils.GetDBType(chemContent);
            if (dbType == DBType.Molecule)
            {
                // 防止分子式报错
                chemContent = ChemUtils.ReplaceChemContent(chemContent);
            }
            _dbType = dbType;
            _tableName = tableName;

            Bingo bingo = IndigoUtils.GetReadOnlyBingoInstance(_indigo, tableName, dbType);

            return (bingo, chemContent);
        }

        /// <summary>
        /// 执行子结构搜索
        /// </summary>
        /// <param name="bingo"></param>
        /// <param name="indigo"></param>
        /// <param name="chemContent"></param>
        /// <param name="useChirality"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        private static List<int> RunExactSearch(Bingo bingo, Indigo indigo, string chemContent, bool useChirality, string dbType)
        {
            List<int> resultList = new List<int>();

            if (bingo == null)
            {
                return resultList;
            }

            try
            {
                string smiles = chemContent;

                if (ChemUtils.IsMolContent(chemContent))
                {
                    chemContent = ChemUtils.FilterHCount(chemContent);
                    RWMol tmol = RWMol.MolFromMolBlock(chemContent, true, false);
                    smiles = tmol.MolToSmiles(true, true);
                }

                if (dbType == DBType.Reaction)
                {
                    IndigoObject queryObject = indigo.loadReaction(smiles);
                    BingoObject bingoObject = bingo.searchExact(queryObject);
                    while (bingoObject.next())
                    {
                        resultList.Add(bingoObject.getCurrentId());
                    }
                }
                else
                {
                    IndigoObject queryObject = indigo.loadMolecule(smiles);
                    BingoObject bingoObject = bingo.searchExact(queryObject);
                    int resultCount = bingoObject.estimateRemainingResultsCount();
                    while (bingoObject.next())
                    {
                        int id = bingoObject.getCurrentId();
                        IndigoObject m1 = bingo.getRecordById(id);
                        string mol1Content = m1.canonicalSmiles();

                        #region 使用rdkit再次进行比较
                        RWMol mol1 = null;
                        if (ChemUtils.IsMolContent(mol1Content))
                        {
                            mol1 = RWMol.MolFromMolBlock(mol1Content);
                        }
                        else
                        {
                            mol1 = RWMol.MolFromSmiles(mol1Content);
                        }

                        RWMol mol2 = null;
                        if (ChemUtils.IsMolContent(chemContent))
                        {
                            mol2 = RWMol.MolFromMolBlock(chemContent);
                        }
                        else
                        {
                            mol2 = RWMol.MolFromSmiles(chemContent);
                        }

                        bool m1sm2 = mol1.hasSubstructMatch(mol2, useChirality);
                        bool m2sm1 = mol2.hasSubstructMatch(mol1, useChirality);

                        if (m1sm2 && m2sm1)
                        {
                            resultList.Add(bingoObject.getCurrentId());
                        }
                        #endregion
                    }
                }

            }
            catch (Exception ex)//无法获取结果时抛出异常
            {
                NLogHelper.Default.Error($"在执行[tableName={_tableName}]的[chemContent={chemContent}]的精确搜索时发生异常：{ex.Message}");
            }

            return resultList;
        }

        /// <summary>
        /// 执行子结构检索
        /// </summary>
        /// <param name="bingo"></param>
        /// <param name="indigo"></param>
        /// <param name="chemContent"></param>
        /// <returns></returns>
        private static List<int> RunSubSearch(Bingo bingo, Indigo indigo, string chemContent, string dbType)
        {
            List<int> resultList = new List<int>();
            string smiles = "";
            try
            {
                smiles = chemContent;
                if (ChemUtils.IsMolContent(chemContent))
                {
                    smiles = RWMol.MolFromMolBlock(chemContent).MolToSmiles(true, true);
                }

                if (dbType == DBType.Reaction)
                {
                    IndigoObject queryObject = indigo.loadReactionSmarts(smiles);
                    BingoObject bingoObject = bingo.searchSub(queryObject);
                    while (bingoObject.next())
                    {
                        resultList.Add(bingoObject.getCurrentId());
                    }
                }
                else
                {
                    IndigoObject queryObject = indigo.loadQueryMolecule(smiles);
                    BingoObject bingoObject = bingo.searchSub(queryObject);
                    while (bingoObject.next())
                    {
                        resultList.Add(bingoObject.getCurrentId());
                    }
                }


            }
            catch (Exception ex)//无法获取结果时抛出异常
            {
                NLogHelper.Default.Error($"在执行[tableName={_tableName}]的[chemContent={chemContent}]的子结构搜索时发生异常：{ex.Message}");
            }

            return resultList;
        }


        /// <summary>
        /// 执行相似性检索
        /// </summary>
        /// <param name="bingo"></param>
        /// <param name="indigo"></param>
        /// <param name="chemContent"></param>
        /// <param name="minPer"></param>
        /// <param name="maxPer"></param>
        /// <returns></returns>
        private static List<int> RunSmiSearch(Bingo bingo, Indigo indigo, string chemContent, float minPer, float maxPer, string dbType)
        {
            List<int> resultList = new List<int>();

            if (bingo == null)
            {
                return resultList;
            }

            try
            {
                string smiles = chemContent;

                if (ChemUtils.IsMolContent(chemContent))
                {
                    chemContent = ChemUtils.FilterHCount(chemContent);
                    RWMol tmol = RWMol.MolFromMolBlock(chemContent, true, false);
                    smiles = tmol.MolToSmiles(true, true);
                }

                if (dbType == DBType.Reaction)
                {
                    IndigoObject queryObject = indigo.loadReactionSmarts(smiles);
                    BingoObject bingoObject = bingo.searchSim(queryObject, minPer, maxPer);
                    while (bingoObject.next())
                    {
                        resultList.Add(bingoObject.getCurrentId());
                    }
                }
                else
                {
                    IndigoObject queryObject = indigo.loadMolecule(smiles);
                    BingoObject bingoObject = bingo.searchSim(queryObject, minPer, maxPer);
                    while (bingoObject.next())
                    {
                        resultList.Add(bingoObject.getCurrentId());
                    }
                }


            }
            catch (Exception ex)//无法获取结果时抛出异常
            {
                NLogHelper.Default.Error($"在执行[tableName={_tableName}]的[chemContent={chemContent},最小值={minPer},最大值={maxPer}]的子结构搜索时发生异常：{ex.Message}");
            }

            return resultList;
        }

        #endregion
        public List<int> SearchExactStruct(StructSearchExactDto model)
        {
            (Bingo bingo, string chemContent) = GetBingo(model.TableName, model.ChemContent);
            List<int> resultList = new List<int>();
            if (bingo != null)
            {
                resultList = RunExactSearch(bingo, _indigo, chemContent, model.UseChirality, _dbType);
                bingo.close();
            }

            return resultList;
        }

        public List<int> SearchSmiStruct(StructSearchSmiDto model)
        {
            (Bingo bingo,string chemContent) = GetBingo(model.TableName, model.ChemContent);
            List<int> resultList = new List<int>();
            if (bingo!= null)
            {
                resultList = RunSmiSearch(bingo, _indigo, chemContent, model.MinPer, model.MaxPer, _dbType);
                bingo.close();
            }

            return resultList;


        }

        public List<int> SearchSubStruct(StructSearchSubDto model)
        {
            (Bingo bingo, string chemContent) = GetBingo(model.TableName, model.ChemContent);
            List<int> resultList = new List<int>();
            if (bingo != null)
            {
                resultList = RunSubSearch(bingo, _indigo, model.ChemContent, _dbType);
                bingo.close();
            }

            return resultList;
        }
    }
}
