﻿using System.IO;
using System.Text;

namespace excel2json.GUI
{

    /// <summary>
    /// 为GUI模式提供的整体数据管理
    /// </summary>
    class DataManager
    {

        // 数据导入设置
        private Program.Options mOptions;
        private Encoding mEncoding;

        // 导出数据
        private JsonExporter mJson;
        private CSDefineGenerator mCSharp;

        /// <summary>
        /// 导出的Json文本
        /// </summary>
        public string JsonContext {
            get {
                if (mJson != null)
                    return mJson.context;
                else
                    return "";
            }
        }

        public string CSharpCode {
            get {
                if (mCSharp != null)
                    return mCSharp.code;
                else
                    return "";
            }
        }

        /// <summary>
        /// 保存Json
        /// </summary>
        /// <param name="filePath">保存路径</param>
        public void saveJson(string filePath)
        {
            if (mJson != null)
            {
                mJson.SaveToFile(filePath, mEncoding);
            }
        }

        public void saveCSharp(string filePath)
        {
            if (mCSharp != null)
                mCSharp.SaveToFile(filePath, mEncoding);
        }


        /// <summary>
        /// 加载Excel文件
        /// </summary>
        /// <param name="options">导入设置</param>
        public void loadExcel(Program.Options options)
        {
            mOptions = options;

            //-- Excel File
            string excelPath = options.ExcelPath;
            string excelName = Path.GetFileNameWithoutExtension(excelPath);

            //-- Header
            int header = options.HeaderRows;

            //-- Encoding
            Encoding cd = new UTF8Encoding(false);
            if (options.Encoding != "utf8-nobom")
            {
                foreach (EncodingInfo ei in Encoding.GetEncodings())
                {
                    Encoding e = ei.GetEncoding();
                    if (e.HeaderName == options.Encoding)
                    {
                        cd = e;
                        break;
                    }
                }
            }
            mEncoding = cd;

            //-- Load Excel
            ExcelLoader excel = new ExcelLoader(excelPath, header);

            //-- C# 结构体定义
            mCSharp = new CSDefineGenerator(excelPath, excel, options.ExcludePrefix);

            //-- 导出JSON
            mJson = new JsonExporter(excel, options.Lowcase, options.ExportArray, options.DateFormat, options.ForceSheetName, header, options.ExcludePrefix, options.CellJson);
        }
    }
}
