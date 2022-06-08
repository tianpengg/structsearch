using com.epam.indigo;
using GraphMolWrap;
using Struct.Common.Utils;
using Struct.Models.Dto;
using Struct.Models.Help;
using Struct.Module.IBusiness;
using System;

namespace Struct.Module.Business
{
    public class StructStore : IStructStore
    {
        private static object _lock = new object();
        public bool AddStruct(AddOrUpdateStructDto model)
        {
            // 获取数据库类型
            var dbType = DBTypeUtils.GetDBType(model.ChemContent);

            if (dbType == DBType.Molecule)
            {
                // 防止分子式报错
                model.ChemContent = ChemUtils.ReplaceChemContent(model.ChemContent);
            }

            var result = false;

            lock (_lock)
            {
                Indigo indigo = new Indigo();

                Bingo bingo = IndigoUtils.GetWriteableBingoInstance(indigo, model.TableName, dbType);

                if (bingo != null)
                {
                    try
                    {
                        string smiles = model.ChemContent;
                        if (ChemUtils.IsMolContent(smiles))
                        {
                            string filterChemContent = ChemUtils.FilterHCount(smiles);
                            // 使用rdkit将mol转化为smiles
                            RWMol tmol = RWMol.MolFromMolBlock(filterChemContent, true, false);
                            smiles = tmol.MolToSmiles(true, true);
                        }
                        if (dbType == DBType.Reaction)
                        {
                            IndigoObject indigoObject = indigo.loadReaction(smiles);
                            result= bingo.insert(indigoObject, model.Id)>0;
                        }
                        else
                        {
                            IndigoObject indigoObject = indigo.loadMolecule(smiles);
                            result = bingo.insert(indigoObject, model.Id)>0;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        result = false;
                    }
                    finally
                    {
                        //释放连接
                        bingo.close();
                    }
                }
            }

            return result;
        }

        public bool UpdateStruct(AddOrUpdateStructDto model)
        {
            /*
             * 这里的更新操作，先删除再修改
             */
            //TODO

            return false;
        }
    }
}
