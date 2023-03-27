namespace pxtools
{
    /* https://www.cplusplus.com/reference/ctime/tm/ */
    public struct tm
    {
        public int tm_sec;
        public int tm_min;
        public int tm_hour;
        public int tm_mday;
        public int tm_mon;
        public int tm_year;
        public int tm_wday;
        public int tm_yday;
        public int tm_isdst;
    };

    /* void * are escaped for the 64bit machines 
     * otherwise they are too long (8bytes)
     */
    public struct px_header
    {
        public short recordSize;        /* 0x00 */
        public short headerSize;        /* 0x02 */
        public /*unsigned char*/ byte fileType;         /* 0x04 */
        public /*unsigned char*/ byte maxTableSize;     /* 0x05 */
        public uint numRecords;        /* 0x06 */
        public ushort usedBlocks;      /* 0x0a */
        public ushort fileBlocks;      /* 0x0c */
        public ushort firstBlock;      /* 0x0e */
        public ushort lastBlock;       /* 0x10 */
        public ushort dummy_1;         /* 0x12 */
        public /*unsigned char*/ byte  modifiedFlags1;       /* 0x14 */
        public /*unsigned char*/ byte IndexFieldNumber;     /* 0x15 */
        /* void * */
        public uint primaryIndexWorkspace;     /* 0x16 */
        /* void * */
        public uint dummy_2;               /* 0x1a */
        public ushort indexRootBlock;      /* 0x1e */
        public /*unsigned char*/ byte indexLevels;      /* 0x20 */
        public short numFields;         /* 0x21 */
        public short primaryKeyFields;      /* 0x23 */
        public uint encryption1;       /* 0x25 */
        public /*unsigned char*/ byte sortOrder;        /* 0x29 */
        public /*unsigned char*/ byte modifiedFlags2;       /* 0x2a */
        public ushort dummy_5;         /* 0x2b */
        public /*unsigned char*/ byte changeCount1;     /* 0x2d */
        public /*unsigned char*/ byte changeCount2;     /* 0x2e */
        public /*unsigned char*/ byte dummy_6;          /* 0x2f */
        /* char ** */
        public uint tableNamePtr;          /* 0x30 */
        /* void * */
        public uint fieldInfo;             /* 0x34 */
        public /*unsigned char*/ byte writeProtected;       /* 0x38 */
        public /*unsigned char*/ byte fileVersionID;        /* 0x39 */
        public ushort maxBlocks;       /* 0x3a */
        /*unsigned*/
        public byte dummy_7;          /* 0x3c */
        /*unsigned*/
        public byte auxPasswords;     /* 0x3d */
        public ushort dummy_8;         /* 0x3e */
        /* void * */
        public uint cryptInfoStart;            /* 0x40 */
        /* void * */
        public uint cryptInfoEnd;          /* 0x44 */
        /*unsigned*/
        public byte dummy_9;          /* 0x48 */
        public uint autoInc;           /* 0x49 */
        public ushort dummy_a;         /* 0x4d */
        /*unsigned*/
        public byte indexUpdateRequired;  /* 0x4f */
        public uint dummy_b;           /* 0x50 */
        /*unsigned*/
        public byte dummy_c;          /* 0x54 */
        /*unsigned*/
        public byte refIntegrity;     /* 0x55 */
        public ushort dummy_d;         /* 0x56 */
        public ushort fileVersionID2;      /* 0x58 */
        public ushort fileVersionID3;      /* 0x5a */
        public uint encryption2;       /* 0x5c */
        public uint fileUpdateTime;        /* 0x60 */
        public ushort hiFieldID;       /* 0x64 */
        public ushort hiFieldIDInfo;       /* 0x66 */
        public ushort sometimesNumFields;  /* 0x68 */
        public ushort dosGlobalCodePage;   /* 0x6a */
        public uint dummy_e;           /* 0x6c */
        public ushort changeCount4;        /* 0x70 */
        public uint dummy_f;           /* 0x72 */
        public ushort dummy_10;        /* 0x76 */

        public byte[] tableName;         /* ---- */
    };

    public struct px_fieldInfo
    {
        public byte[] name/*[80]*/;
        public int type;
        public int size;
    };

    public struct px_blocks
    {
        public int prevBlock;
        public int nextBlock;
        public int numRecsInBlock;
        public byte[][] records; /*px_records[]*/
    };

    public class PXTypes
    {
        public const byte PX_Field_Type_Alpha = 0x01;
        public const byte PX_Field_Type_Date = 0x02;
        public const byte PX_Field_Type_ShortInt = 0x03;
        public const byte PX_Field_Type_LongInt = 0x04;
        public const byte PX_Field_Type_Currency = 0x05;
        public const byte PX_Field_Type_Number = 0x06;
        public const byte PX_Field_Type_Logical = 0x09;
        public const byte PX_Field_Type_MemoBLOB = 0x0c;
        public const byte PX_Field_Type_BinBLOB = 0x0d;
        public const byte PX_Field_Type_DUNNO = 0x0e;
        public const byte PX_Field_Type_Graphic = 0x10;
        public const byte PX_Field_Type_Time = 0x14;
        public const byte PX_Field_Type_Timestamp = 0x15;
        public const byte PX_Field_Type_Incremental = 0x16;
        public const byte PX_Field_Type_BCD = 0x17;

        public const byte PX_Filetype_DB_Indexed = 0x00;
        public const byte PX_Filetype_PX = 0x01;
        public const byte PX_Filetype_DB_Not_indexed = 0x02;
        public const byte PX_Filetype_Xnn_NonInc = 0x03;
        public const byte PX_Filetype_Ynn = 0x04;
        public const byte PX_Filetype_Xnn_Inc = 0x05;
        public const byte PX_Filetype_XGn_NonInc = 0x06;
        public const byte PX_Filetype_YGn = 0x07;
        public const byte PX_Filetype_XGn_Inc = 0x08;

    }
}