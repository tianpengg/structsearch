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

            }

            return resultList;
        }

        #endregion
        public List<int> SearchExactStruct(StructSearchExactDto model)
        {
            var dbType = DBTypeUtils.GetDBType(model.ChemContent);
            if (dbType == DBType.Molecule)
            {
                // 防止分子式报错
                model.ChemContent = ChemUtils.ReplaceChemContent(model.ChemContent);
            }
            Indigo indigo = new Indigo();

            List<int> resultList = new List<int>();

            Bingo bingo = null;

            if (DbPathExist(model.TableName, dbType))
            {
                bingo =IndigoUtils.GetReadOnlyBingoInstance(indigo, model.TableName, dbType);
            }

            if (bingo != null)
            {
                resultList = RunExactSearch(bingo, indigo, model.ChemContent, model.UseChirality, dbType);
                bingo.close();
            }

            return resultList;
        }

    }
}
