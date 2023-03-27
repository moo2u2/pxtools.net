using De.Hochstaetter.CommandLine;
using De.Hochstaetter.CommandLine.Attributes;
using De.Hochstaetter.CommandLine.Exceptions;
using De.Hochstaetter.CommandLine.Models;
using pxtools.Models;
using System.Text;

namespace pxtools
{
    public class PXSqlDump
    {

        private const int DB_UNSET = 0;
        private const int DB_MYSQL = 1;
        private const int DB_PGSQL = 2;
        private const int DB_OTHER = 3;

        private static int dbtype = DB_UNSET;

        //private const int name_quoting = -1;
        private const string OPT_MYSQL = "mysql";
        private const string OPT_PGSQL = "pgsql";

        //b:d:f:hxn:qQsV

        [GetOpt(LongName = "blobname", ShortName = 'b', ArgumentName = "<name>", Help = "Name of the .MB-file <name>")]
        public static string BlobName = string.Empty;

        [GetOpt(LongName = "database", ShortName = 'd', ArgumentName = "<mysql|pgsql>", Help = "Database compatible SQL")]
        public static string Database = string.Empty;

        [GetOpt(LongName = "filename", ShortName = 'f', ArgumentName = "<name>", Help = "Name of the .DB-file <name>")]
        public static string Filename = string.Empty;

        [GetOpt(LongName = "help", ShortName = 'h', HasArgument = false, Help = "Display this help and exit")]
        public static bool DisplayHelp;

        [GetOpt(LongName = "tablename", ShortName = 'n', ArgumentName = "<name>", Help = "Replace the tablename by <name>")]
        public static string TableName = string.Empty;

        [GetOpt(LongName = "no_namequoting", ShortName = 'q', HasArgument = false, Help = "Force name quoting deactivation")]
        public static bool NoNameQuoting;

        [GetOpt(LongName = "namequoting", ShortName = 'Q', HasArgument = false, Help = "Force name quoting activation")]
        public static int NameQuoting = -1;

        [GetOpt(LongName = "no_create", ShortName = 's', HasArgument = false, Help = "Skip table creation (insert data only)")]
        public static bool NoCreate;

        [GetOpt(LongName = "use_filename", ShortName = 'x', HasArgument = false, Help = "Use filename to determine tablename")]
        public static bool UseFilename;

        [GetOpt(LongName = "version", ShortName = 'V', HasArgument = false, Help = "Output version information and exit")]
        public static bool DisplayVersion;

        public static int create_sql_CREATE(px_header header, px_fieldInfo[] felder)
        {
            byte[] name = quote(PXConvert.PXNametoQuotedName(header.tableName), NameQuoting);
            Console.WriteLine($"CREATE TABLE {name.ToCleanString()} (");

            for (int i = 0; i < header.numFields; i++)
            {
                if (i > 0)
                    Console.WriteLine(",");
                name = quote(PXConvert.PXNametoQuotedName(felder[i].name), NameQuoting);
                Console.Write($"\t´{name.ToCleanString()}´ ");

                switch (felder[i].type)
                {
                    case PXTypes.PX_Field_Type_Alpha: Console.Write("VARCHAR"); break;
                    case PXTypes.PX_Field_Type_Date: Console.Write("DATE"); break;
                    case PXTypes.PX_Field_Type_ShortInt: Console.Write("INTEGER"); break;
                    case PXTypes.PX_Field_Type_LongInt: Console.Write("BIGINT"); break;
                    case PXTypes.PX_Field_Type_Currency: Console.Write("DECIMAL"); break;
                    case PXTypes.PX_Field_Type_Number: Console.Write("DOUBLE PRECISION"); break;
                    case PXTypes.PX_Field_Type_MemoBLOB: Console.Write("LONGTEXT"); break;
                    case PXTypes.PX_Field_Type_BinBLOB: Console.Write("BLOB"); break;
                    case PXTypes.PX_Field_Type_Graphic: Console.Write("BLOB"); break;
                    case PXTypes.PX_Field_Type_Logical: Console.Write("INTEGER"); break;
                    case PXTypes.PX_Field_Type_Time: Console.Write("TIME"); break;
                    case PXTypes.PX_Field_Type_Timestamp: Console.Write("TIMESTAMP"); break;
                    case PXTypes.PX_Field_Type_Incremental: Console.Write("INTEGER"); break;
                    case PXTypes.PX_Field_Type_BCD: Console.Write("INTEGER"); break;
                    case PXTypes.PX_Field_Type_DUNNO: Console.Write("VARCHAR(1)"); break;
                    default: Console.Write("Unknown: %02x", felder[i].type); break;
                }

                switch (felder[i].type)
                {
                    case PXTypes.PX_Field_Type_Logical:
                    case PXTypes.PX_Field_Type_Alpha:
                        if ((dbtype == DB_PGSQL) && (felder[i].size == 1))
                            break;
                        Console.Write($"({felder[i].size})");
                        break;
                    case PXTypes.PX_Field_Type_Currency:
                        Console.Write("(12,2)");
                        break;
                    case PXTypes.PX_Field_Type_Incremental:
                        Console.Write(" NOT NULL /* auto_increment */");
                        break;
                }

            }
            Console.WriteLine("\n);");

            return 0;
        }

        public static IList<object?> create_table_data(px_header header, px_fieldInfo[] felder, byte[] block, string blobname)
        {
            IList<object?> data = new List<object?>(felder.Length);
            int block_index = 0;

            for (int i = 0; i < header.numFields; i++)
            {
                if (felder[i].type == PXTypes.PX_Field_Type_Alpha)
                {
                    byte[] str = new byte[felder[i].size + 1];
                    byte[] qstr = new byte[felder[i].size * 2 + 1];

                    Array.Copy(block[block_index..], str, felder[i].size);
                    block_index += felder[i].size;

                    str[felder[i].size] = (byte)'\0';

                    PXConvert.PXtoQuotedString(qstr, str, felder[i].type);
                    data.Add(str_to_sql(qstr).ToCleanString());
                }
                else if (felder[i].type == PXTypes.PX_Field_Type_BCD)
                {
                    byte[] str = new byte[felder[i].size + 1];

                    Array.Copy(block[block_index..], str, felder[i].size);
                    block_index += felder[i].size;

                    data.Add(str.ToCleanString());
                }
                else if (felder[i].type == PXTypes.PX_Field_Type_ShortInt)
                {
                    byte[] bytes = block[block_index..(block_index + felder[i].size)];
                    ushort s = BitConverter.ToUInt16(bytes);
                    ulong d = 0;

                    block_index += felder[i].size;

                    switch (PXConvert.PXtoLong(/*s*/ bytes, ref d, felder[i].type))
                    {
                        case Constants.VALUE_OK:
                            data.Add(d);
                            break;
                        case Constants.VALUE_IS_NULL:
                        case Constants.VALUE_ERROR:
                            data.Add(null);
                            break;
                        default:
                            break;
                    }
                }
                else if (felder[i].type == PXTypes.PX_Field_Type_Logical)
                {
                    if (felder[i].size != 1)
                        Console.Error.WriteLine("WHY??");

                    byte s = block[block_index];
                    ulong d = 0;
                    block_index += felder[i].size;

                    switch (PXConvert.PXtoLong(new byte[] { s }, ref d, felder[i].type))
                    {
                        case Constants.VALUE_OK:
                            data.Add(d);
                            break;
                        case Constants.VALUE_IS_NULL:
                        case Constants.VALUE_ERROR:
                            data.Add(null);
                            break;
                        default:
                            break;
                    }
                }
                else if (felder[i].type == PXTypes.PX_Field_Type_LongInt)
                {
                    byte[] bytes = block[block_index..(block_index + felder[i].size)];
                    ulong s = BitConverter.ToUInt64(bytes);
                    ulong d = 0;
                    block_index += felder[i].size;

                    switch (PXConvert.PXtoLong(/*s*/ bytes, ref d, felder[i].type))
                    {
                        case Constants.VALUE_OK:
                            data.Add(d);
                            break;
                        case Constants.VALUE_IS_NULL:
                        case Constants.VALUE_ERROR:
                            data.Add(null);
                            break;
                        default:
                            break;
                    }
                }
                else if (felder[i].type == PXTypes.PX_Field_Type_Incremental)
                {
                    byte[] bytes = block[block_index..(block_index + felder[i].size)];
                    ulong s = BitConverter.ToUInt64(bytes);
                    ulong d = 0;
                    block_index += felder[i].size;

                    switch (PXConvert.PXtoLong(/*s*/ bytes, ref d, felder[i].type))
                    {
                        case Constants.VALUE_OK:
                            data.Add(d);
                            break;
                        case Constants.VALUE_IS_NULL:
                        case Constants.VALUE_ERROR:
                            data.Add(null);
                            break;
                        default:
                            break;
                    }
                }
                else if (felder[i].type == PXTypes.PX_Field_Type_Number)
                {
                    byte[] bytes = block[block_index..(block_index + felder[i].size)];
                    ulong s = BitConverter.ToUInt64(bytes);
                    double d = 0;
                    block_index += felder[i].size;

                    switch (PXConvert.PXtoDouble(s, ref d, felder[i].type))
                    {
                        case Constants.VALUE_OK:
                            data.Add(d);
                            break;
                        case Constants.VALUE_IS_NULL:
                        case Constants.VALUE_ERROR:
                            data.Add(null);
                            break;
                        default:
                            break;
                    }
                }
                else if (felder[i].type == PXTypes.PX_Field_Type_Currency)
                {
                    byte[] bytes = block[block_index..(block_index + felder[i].size)];
                    ulong s = BitConverter.ToUInt64(bytes);
                    double d = 0;
                    block_index += felder[i].size;

                    switch (PXConvert.PXtoDouble(s, ref d, felder[i].type))
                    {
                        case Constants.VALUE_OK:
                            data.Add(d);
                            break;
                        case Constants.VALUE_IS_NULL:
                        case Constants.VALUE_ERROR:
                            data.Add(null);
                            break;
                        default:
                            break;
                    }
                }
                else if (felder[i].type == PXTypes.PX_Field_Type_Date)
                {
                    byte[] bytes = block[block_index..(block_index + felder[i].size)];
                    ulong s = BitConverter.ToUInt64(bytes);
                    block_index += felder[i].size;

                    tm _tm = new tm();

                    switch (PXConvert.PXtoTM(s, ref _tm, felder[i].type))
                    {
                        case Constants.VALUE_OK:
                            data.Add(string.Format("'%04i-%02i-%02i'", _tm.tm_year + 1900, _tm.tm_mon + 1, _tm.tm_mday));
                            break;
                        case Constants.VALUE_IS_NULL:
                        case Constants.VALUE_ERROR:
                            data.Add(null);
                            break;
                        default:
                            break;
                    }
                }
                else if (felder[i].type == PXTypes.PX_Field_Type_Time)
                {
                    ulong s = BitConverter.ToUInt64(block[block_index..(block_index + felder[i].size)]);
                    block_index += felder[i].size;

                    tm _tm = new tm();

                    switch (PXConvert.PXtoTM(s, ref _tm, felder[i].type))
                    {
                        case Constants.VALUE_OK:
                            data.Add(string.Format("'%02d:%02d:%02d'", _tm.tm_hour, _tm.tm_min, _tm.tm_sec));
                            break;
                        case Constants.VALUE_IS_NULL:
                        case Constants.VALUE_ERROR:
                            data.Add(null);
                            break;
                        default:
                            break;
                    }
                }
                else if (felder[i].type == PXTypes.PX_Field_Type_Timestamp)
                {
                    ulong s = BitConverter.ToUInt64(block[block_index..(block_index + felder[i].size)]);
                    block_index += felder[i].size;

                    tm _tm = new tm();

                    switch (PXConvert.PXtoTM(s, ref _tm, felder[i].type))
                    {
                        case Constants.VALUE_OK:
                            data.Add($"'{_tm.tm_year + 1900:d04}-{_tm.tm_mon + 1:d02}-{_tm.tm_mday:d02} {_tm.tm_hour:d02}:{_tm.tm_min:d02}:{_tm.tm_sec:d02}'");
                            break;
                        case Constants.VALUE_IS_NULL:
                        case Constants.VALUE_ERROR:
                            data.Add(null);
                            break;
                        default:
                            break;
                    }
                }
                else if (felder[i].type == PXTypes.PX_Field_Type_MemoBLOB)
                {
                    byte[] blob = block[block_index..(block_index + felder[i].size)];
                    byte[]? s = null;

                    block_index += felder[i].size;

                    s = PXConvert.PXMEMOtoString(blob, felder[i].size, blobname);

                    /* if the MEMO is in the external BLOB s will be set,
                     * otherwise we use blob from the .DB file
                     */
                    data.Add(str_to_sql(s != null ? s : blob).ToCleanString());
                }
                else if (felder[i].type == PXTypes.PX_Field_Type_Graphic || felder[i].type == PXTypes.PX_Field_Type_BinBLOB)
                {
                    byte[] blob = block[block_index..(block_index + felder[i].size)];
                    uint s_size = 0, d_size = 0;
                    byte[]? s = null;

                    block_index += felder[i].size;

                    PXConvert.PXBLOBtoBinary(blob, felder[i].size, blobname, ref s, ref s_size);

                    data.Add(binary_to_sql(s != null && s.Length > 0 ? s : blob, s_size, ref d_size).ToCleanString());
                }
                else if (felder[i].type == PXTypes.PX_Field_Type_DUNNO)
                {
                    block_index += felder[i].size;
                    data.Add(null);
                    Console.Error.WriteLine("Unknown type");
                }
                else
                {
                    Console.WriteLine($"\nUnknown: {felder[i].size}");
                    block_index += felder[i].size;
                    Console.WriteLine("Name: {felder[i].name}");
                }
            }

            return data;
        }

        public static int create_sql_INSERT(px_header header, px_fieldInfo[] felder, /*px_records*/ byte[] block, string blobname)
        {
            int block_index = 0;
            Console.Write("(");

            for (int i = 0; i < header.numFields; i++)
            {
                if (i > 0)
                    Console.Write(",");
                if (felder[i].type == PXTypes.PX_Field_Type_Alpha)
                {
                    byte[] str = new byte[felder[i].size + 1];
                    byte[] qstr = new byte[felder[i].size * 2 + 1];

                    Array.Copy(block[block_index..], str, felder[i].size);
                    block_index += felder[i].size;

                    str[felder[i].size] = (byte)'\0';

                    PXConvert.PXtoQuotedString(qstr, str, felder[i].type);
                    Console.Write($"\'{str_to_sql(qstr).ToCleanString()}\'");
                }
                else if (felder[i].type == PXTypes.PX_Field_Type_BCD)
                {
                    byte[] str = new byte[felder[i].size + 1];

                    Array.Copy(block[block_index..], str, felder[i].size);
                    block_index += felder[i].size;

                    Console.Write($"-\'{str.ToCleanString()}\'-");
                }
                else if (felder[i].type == PXTypes.PX_Field_Type_ShortInt)
                {
                    byte[] bytes = block[block_index..(block_index + felder[i].size)];
                    ushort s = BitConverter.ToUInt16(bytes);
                    ulong d = 0;

                    block_index += felder[i].size;

                    switch (PXConvert.PXtoLong(/*s*/ bytes, ref d, felder[i].type))
                    {
                        case Constants.VALUE_OK:
                            Console.Write(string.Format(" % Ld", d));
                            break;
                        case Constants.VALUE_IS_NULL:
                        case Constants.VALUE_ERROR:
                            Console.Write("NULL");
                            break;
                        default:
                            break;
                    }
                }
                else if (felder[i].type == PXTypes.PX_Field_Type_Logical)
                {
                    if (felder[i].size != 1)
                        Console.Error.WriteLine("WHY??");

                    byte s = block[block_index];
                    ulong d = 0;
                    block_index += felder[i].size;

                    switch (PXConvert.PXtoLong(/*s*/ new byte[] { s }, ref d, felder[i].type))
                    {
                        case Constants.VALUE_OK:
                            Console.Write(string.Format("%Lx", d));
                            break;
                        case Constants.VALUE_IS_NULL:
                        case Constants.VALUE_ERROR:
                            Console.Write("NULL");
                            break;
                        default:
                            break;
                    }
                }
                else if (felder[i].type == PXTypes.PX_Field_Type_LongInt)
                {
                    byte[] bytes = block[block_index..(block_index + felder[i].size)];
                    ulong s = BitConverter.ToUInt64(bytes);
                    ulong d = 0;
                    block_index += felder[i].size;

                    switch (PXConvert.PXtoLong(/*s*/ bytes, ref d, felder[i].type))
                    {
                        case Constants.VALUE_OK:
                            Console.Write(string.Format("%Ld", d));
                            break;
                        case Constants.VALUE_IS_NULL:
                        case Constants.VALUE_ERROR:
                            Console.Write("NULL");
                            break;
                        default:
                            break;
                    }
                }
                else if (felder[i].type == PXTypes.PX_Field_Type_Incremental)
                {
                    byte[] bytes = block[block_index..(block_index + felder[i].size)];
                    ulong s = BitConverter.ToUInt64(bytes);
                    ulong d = 0;
                    block_index += felder[i].size;

                    switch (PXConvert.PXtoLong(/*s*/ bytes, ref d, felder[i].type))
                    {
                        case Constants.VALUE_OK:
                            Console.Write(string.Format("%Ld", d));
                            break;
                        case Constants.VALUE_IS_NULL:
                        case Constants.VALUE_ERROR:
                            Console.Write("NULL");
                            break;
                        default:
                            break;
                    }
                }
                else if (felder[i].type == PXTypes.PX_Field_Type_Number)
                {
                    ulong s = BitConverter.ToUInt64(block[block_index..(block_index + felder[i].size)]);
                    double d = 0;
                    block_index += felder[i].size;

                    switch (PXConvert.PXtoDouble(s, ref d, felder[i].type))
                    {
                        case Constants.VALUE_OK:
                            Console.Write($"{d:f}");
                            break;
                        case Constants.VALUE_IS_NULL:
                        case Constants.VALUE_ERROR:
                            Console.Write("NULL");
                            break;
                        default:
                            break;
                    }

                }
                else if (felder[i].type == PXTypes.PX_Field_Type_Currency)
                {
                    ulong s = BitConverter.ToUInt64(block[block_index..(block_index + felder[i].size)]);
                    double d = 0;
                    block_index += felder[i].size;

                    switch (PXConvert.PXtoDouble(s, ref d, felder[i].type))
                    {
                        case Constants.VALUE_OK:
                            Console.Write($"{d:f}");
                            break;
                        case Constants.VALUE_IS_NULL:
                        case Constants.VALUE_ERROR:
                            Console.Write("NULL");
                            break;
                        default:
                            break;
                    }
                }
                else if (felder[i].type == PXTypes.PX_Field_Type_Date)
                {
                    ulong s = BitConverter.ToUInt64(block[block_index..(block_index + felder[i].size)]);
                    block_index += felder[i].size;

                    tm _tm = new tm();

                    switch (PXConvert.PXtoTM(s, ref _tm, felder[i].type))
                    {
                        case Constants.VALUE_OK:
                            Console.WriteLine(string.Format("'%04i-%02i-%02i'", _tm.tm_year + 1900, _tm.tm_mon + 1, _tm.tm_mday));
                            break;
                        case Constants.VALUE_IS_NULL:
                        case Constants.VALUE_ERROR:
                            Console.Write("NULL");
                            break;
                        default:
                            break;
                    }
                }
                else if (felder[i].type == PXTypes.PX_Field_Type_Time)
                {
                    ulong s = BitConverter.ToUInt64(block[block_index..(block_index + felder[i].size)]);
                    block_index += felder[i].size;

                    tm _tm = new tm();

                    switch (PXConvert.PXtoTM(s, ref _tm, felder[i].type))
                    {
                        case Constants.VALUE_OK:
                            Console.Write(string.Format("'%02d:%02d:%02d'", _tm.tm_hour, _tm.tm_min, _tm.tm_sec));
                            break;
                        case Constants.VALUE_IS_NULL:
                        case Constants.VALUE_ERROR:
                            Console.Write("NULL");
                            break;
                        default:
                            break;
                    }
                }
                else if (felder[i].type == PXTypes.PX_Field_Type_Timestamp)
                {
                    ulong s = BitConverter.ToUInt64(block[block_index..(block_index + felder[i].size)]);
                    block_index += felder[i].size;

                    tm _tm = new tm();

                    switch (PXConvert.PXtoTM(s, ref _tm, felder[i].type))
                    {
                        case Constants.VALUE_OK:
                            Console.Write(string.Format("'%04d-%02d-%02d ", _tm.tm_year + 1900, _tm.tm_mon + 1, _tm.tm_mday));
                            Console.Write(string.Format("%02d:%02d:%02d'", _tm.tm_hour, _tm.tm_min, _tm.tm_sec));
                            break;
                        case Constants.VALUE_IS_NULL:
                        case Constants.VALUE_ERROR:
                            Console.Write("NULL");
                            break;
                        default:
                            break;
                    }
                }
                else if (felder[i].type == PXTypes.PX_Field_Type_MemoBLOB)
                {
                    byte[] blob = block[block_index..(block_index + felder[i].size)];
                    byte[]? s = null;
                    //byte[]? qqstr = null;

                    block_index += felder[i].size;

                    s = PXConvert.PXMEMOtoString(blob, felder[i].size, blobname);

                    /* if the MEMO is in the external BLOB s will be set,
                     * otherwise we use blob from the .DB file
                     */
                    Console.Write($"\'{str_to_sql(s != null ? s : blob).ToCleanString()}\'");
                }
                else if (felder[i].type == PXTypes.PX_Field_Type_Graphic || felder[i].type == PXTypes.PX_Field_Type_BinBLOB)
                {
                    byte[] blob = block[block_index..(block_index + felder[i].size)];
                    uint s_size = 0, d_size = 0;
                    byte[]? s = null;
                    byte[] qqstr;

                    block_index += felder[i].size;

                    PXConvert.PXBLOBtoBinary(blob, felder[i].size, blobname, ref s, ref s_size);

                    qqstr = binary_to_sql(s != null && s.Length > 0 ? s : blob, s_size, ref d_size);
                    Console.Write($"'{qqstr[0..(int)d_size]}'");
                }
                else if (felder[i].type == PXTypes.PX_Field_Type_DUNNO)
                {
                    Console.Write("'?'");
                }
                else
                {
                    Console.WriteLine($"\nUnknown: {felder[i].size}");
                    block_index += felder[i].size;
                    Console.WriteLine("Name: {felder[i].name}");
                }
            }

            Console.Write(")");

            return 0;
        }

        public static void create_sql_dump(px_header header, px_fieldInfo[] felder, px_blocks[] blocks, string blobname, bool create_table)
        {
            int n, f, c = 0;

            if (create_table)
                create_sql_CREATE(header, felder);

            n = header.firstBlock - 1;
            while (n != -1)
            {
                if (c >= header.usedBlocks)
                {
                    Console.Error.Write($"PXSqlDump.{432}: Leaving here as are trying to use more blocks then registered in the header ({header.usedBlocks})\nTell me if I'm wrong");
                    throw new Exception("Trying to use more blocks then registered in the header");
                }

                if (blocks[n].numRecsInBlock > 0)
                {
                    byte[] name = quote(header.tableName, NameQuoting);
                    Console.Write($"INSERT INTO {name.ToCleanString()} VALUES ");
                }

                for (f = 0; f < blocks[n].numRecsInBlock; f++)
                {
                    if (f > 0)
                        Console.Write(",");
                    create_sql_INSERT(header, felder, blocks[n].records[f], blobname);
                }
                Console.WriteLine(";");
                n = blocks[n].nextBlock - 1;
                c++;
            }
        }

        private static Table create_sql_table(px_header header, px_fieldInfo[] felder, px_blocks[] blocks, string blobname)
        {
            int n, f, c = 0;
            Table table = new Table()
            {
                Name = header.tableName.ToCleanString(),
                Fields = new List<Field>(header.numFields)
            };
            for (int i = 0; i < header.numFields; i++)
            {
                StringBuilder tableName = new StringBuilder(felder[i].name.ToCleanString());
                tableName.Append(" (");

                switch (felder[i].type)
                {
                    case PXTypes.PX_Field_Type_Alpha:
                        tableName.Append("VARCHAR"); break;
                    case PXTypes.PX_Field_Type_Date:
                        tableName.Append("DATE"); break;
                    case PXTypes.PX_Field_Type_ShortInt:
                        tableName.Append("INTEGER"); break;
                    case PXTypes.PX_Field_Type_LongInt:
                        tableName.Append("BIGINT"); break;
                    case PXTypes.PX_Field_Type_Currency:
                        tableName.Append("DECIMAL"); break;
                    case PXTypes.PX_Field_Type_Number:
                        tableName.Append("DOUBLE PRECISION"); break;
                    case PXTypes.PX_Field_Type_MemoBLOB:
                        tableName.Append("LONGTEXT"); break;
                    case PXTypes.PX_Field_Type_BinBLOB:
                        tableName.Append("BLOB"); break;
                    case PXTypes.PX_Field_Type_Graphic:
                        tableName.Append("BLOB"); break;
                    case PXTypes.PX_Field_Type_Logical:
                        tableName.Append("INTEGER"); break;
                    case PXTypes.PX_Field_Type_Time:
                        tableName.Append("TIME"); break;
                    case PXTypes.PX_Field_Type_Timestamp:
                        tableName.Append("TIMESTAMP"); break;
                    case PXTypes.PX_Field_Type_Incremental:
                        tableName.Append("INTEGER"); break;
                    case PXTypes.PX_Field_Type_BCD:
                        tableName.Append("INTEGER"); break;
                    case PXTypes.PX_Field_Type_DUNNO:
                        tableName.Append("VARCHAR(1)"); break;
                    default:
                        tableName.Append($"Unknown: {felder[i].type:x02}"); break;
                }

                switch (felder[i].type)
                {
                    case PXTypes.PX_Field_Type_Logical:
                    case PXTypes.PX_Field_Type_Alpha:
                        if ((dbtype == DB_PGSQL) && (felder[i].size == 1))
                            break;
                        tableName.Append($"({felder[i].size})");
                        break;
                    case PXTypes.PX_Field_Type_Currency:
                        tableName.Append("(12,2)");
                        break;
                    case PXTypes.PX_Field_Type_Incremental:
                        tableName.Append(" NOT NULL /* auto_increment */");
                        break;
                }

                tableName.Append(")");
                table.Fields.Add(new Field(tableName.ToString()));
            }

            n = header.firstBlock - 1;
            table.Data = new List<IList<object?>>();
            while (n != -1)
            {
                if (c >= header.usedBlocks)
                {
                    Console.Error.Write($"PXSqlDump.{469}: Leaving here as are trying to use more blocks then registered in the header ({header.usedBlocks})\nTell me if I'm wrong");
                    throw new Exception($"Leaving here as are trying to use more blocks then registered in the header({header.usedBlocks})");
                }

                for (f = 0; f < blocks[n].numRecsInBlock; f++)
                {
                    table.Data.Add(create_table_data(header, felder, blocks[n].records[f], blobname));
                }

                n = blocks[n].nextBlock - 1;
                c++;
            }
            return table;
        }

        private static byte[] str_to_sql(byte[] src)
        {
            uint i, add;
            byte[] dst;

            /* count the numbers of ' */
            for (i = 0, add = 0; i < src.Length; i++)
            {
                if (src[i] == '\'' || src[i] == '\\')
                    add++;
            }

            dst = new byte[i + add + 1];

            for (i = 0; i < src.Length; i++)
            {
                char c = '\0';

                if (src[i] == '\'')
                {
                    switch (dbtype)
                    {
                        case DB_PGSQL: c = '\''; break;
                        case DB_MYSQL: c = '\\'; break;
                    }
                }
                if (src[i] == '\\')
                {
                    switch (dbtype)
                    {
                        case DB_PGSQL: c = '\\'; break;
                        case DB_MYSQL: c = '\\'; break;
                    }
                }

                if (c != '\0')
                    dst[i++] = Convert.ToByte(c);

                dst[i] = src[i];
            }
            dst[i - 1] = (byte)'\0';

            return dst;
        }

        private static byte[] binary_to_sql(byte[] src, uint src_len, ref uint dst_len)
        {
            uint i, add;
            byte[]? dst = null;

            /* count the numbers of ' */
            for (i = 0, add = 0; i < src_len; i++)
            {
                if (src[i] == '\'' ||
                    src[i] == '\\') add++;
            }

            dst = new byte[i + add];
            dst_len = i + add;

            for (i = 0; i < src_len; i++)
            {
                char c = '\0';

                if (src[i] == '\'')
                {
                    switch (dbtype)
                    {
                        case DB_PGSQL: c = '\''; break;
                        case DB_MYSQL: c = '\\'; break;
                    }
                }
                if (src[i] == '\\')
                {
                    switch (dbtype)
                    {
                        case DB_PGSQL: c = '\\'; break;
                        case DB_MYSQL: c = '\\'; break;
                    }
                }

                if (c != '\0')
                    dst[i++] = Convert.ToByte(c);

                dst[i++] = src[i];
            }

            return dst;
        }

        private static byte[] quote(byte[] src, int name_quoting)
        {
            return name_quoting == 1 ? $"\"{src}\"".ToCharArray().Select(c => Convert.ToByte(c)).ToArray() : src;
        }

        //private static char[] remove_ext(/*unsigned*/ string mystr, char dot, char sep)
        //{
        //    string retstr;
        //    char[] lastdot;
        //    char[] lastsep;

        //    // Error checks and allocate string.

        //    if (mystr == null)
        //        return null;
        //    //if ((retstr = malloc(mystr.Length + 1)) == null)
        //    //    return null;

        //    // Make a copy and find the relevant characters.

        //    retstr = mystr;
        //    lastdot = strrchr(retstr, dot);
        //    lastsep = (sep == 0) ? null : strrchr(retstr, sep);

        //    // If it has an extension separator.

        //    if (lastdot != null)
        //    {
        //        // and it's before the extenstion separator.

        //        if (lastsep != null)
        //        {
        //            if (lastsep < lastdot)
        //            {
        //                // then remove it.

        //                *lastdot = '\0';
        //            }
        //        }
        //        else
        //        {
        //            // Has extension separator with no path separator.

        //            *lastdot = '\0';
        //        }
        //    }

        //    // Return the modified string.

        //    return retstr;
        //}

        private static void display_help()
        {
            Console.Write(string.Format(
                   "PXSQLdump - (%s - %s)\n" +
                   "Usage: pxsqldump [OPTION]...\n" +
                   "\n" +
                   "Create a SQL-dump from a Paradox-database-file\n" +
                   "\n" +
                   "Options:\n" +
                   "  -b, --blobname=<name>        Name of the .MB-file <name>\n" +
                   "  -d, --database=<mysql|pgsql> Database compatible SQL\n" +
                   "  -f, --filename=<name>        Name of the .DB-file <name>\n" +
                   "  -h, --help                   Display this help and exit\n" +
                   "  -n, --tablename=<name>       Replace the tablename by <name>\n" +
                   "  -q, --no_namequoting         Force name quoting deactivation\n" +
                   "  -Q, --namequoting            Force name quoting activation\n" +
                   "  -s, --no_create              Skip table creation (insert data only)\n" +
                   "  -x, --use_filename           Use filename to determine tablename\n" +
                   "  -V, --version                Output version information and exit\n" +
                   "\n" +
                   "\n", "PACKAGE", "1.0.0"));
        }

        private static void display_version()
        {
            Console.Write(
                   "PXSQLdump - (%s - %s)\n" +
                   "Written by Jan Kneschke.\n" +
                   "\n" +
                   "\n", "PACKAGE", "1.0.0");
        }

        public static void Main(string[] args)
        {
            string tablename = "";
            var getOpt = new GetOpt(typeof(PXSqlDump));

            ParsedArguments parsedArguments;

            try
            {
                parsedArguments = getOpt.Parse(args);
                if (!string.IsNullOrEmpty(Database))
                    dbtype = Database == OPT_MYSQL ? DB_MYSQL : (Database == OPT_PGSQL ? DB_PGSQL : DB_OTHER);
            }
            catch (GetOptException ex)
            {
                Console.Error.WriteLine(ex.Message);
                throw;
            }

            if (DisplayVersion)
            {
                display_version();
                return;
            }

            if (DisplayHelp)
            {
                display_help();
                return;
            }

            if (tablename.Length > 75)
            {
                Console.Error.WriteLine("specified tablename is too long");
                throw new Exception();
            }

            switch (dbtype)
            {
                case DB_MYSQL:
                    if (NameQuoting == -1) NameQuoting = 0;
                    break;
                case DB_PGSQL:
                    if (NameQuoting == -1) NameQuoting = 1;
                    break;
                case DB_OTHER:
                    if (NameQuoting == -1) NameQuoting = 1;
                    break;
                default:
                    Console.Error.WriteLine("you have to specify a database . -d");
                    throw new Exception();
            }

            if (string.IsNullOrEmpty(Filename))
            {
                display_help();
                return;
            }

            ReadTable(Filename, BlobName, !NoCreate);
        }

        public static Table ReadTable(string filename, string? blobname = null, bool create_table = true)
        {
            px_fieldInfo[] felder;
            px_blocks[] blocks;

            px_header header = new px_header();
            header.tableName = new byte[79];

            try
            {
                using (FileStream f = File.Open(filename, FileMode.Open, FileAccess.Read))
                {
                    using (var r = new BinaryReader(f, Encoding.UTF8, false))
                    {
                        try
                        {
                            felder = PXParse.PXparseCompleteHeader(r, ref header);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"File '{filename}' is not a paradox-file: {e}");
                            throw new Exception("Invalid file");
                        }

                        blocks = PXParse.PXparseBlocks(f, header);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Can't open file {filename}: {ex.Message}\n");
                throw;
            }

            if (!string.IsNullOrEmpty(TableName) || UseFilename)
                header.tableName = TableName.ToCharArray().Select(c => Convert.ToByte(c)).ToArray();

            if (blocks == null || blocks.Length == 0)
            {
                Console.WriteLine($"No columns for {filename}");
                //throw new Exception();
                return new Table();
            }

            if (string.IsNullOrEmpty(blobname))
                blobname = $"{filename.Substring(0, filename.LastIndexOf('.'))}.MB";

            create_sql_dump(header, felder, blocks, blobname, create_table);
            Table table = create_sql_table(header, felder, blocks, blobname);

            return table;
        }
    }
}
