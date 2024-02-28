using FA.LVIS.Tower.Core;
using FA.LVIS.Tower.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography; 

namespace FA.LVIS.Tower.Data
{
    public enum AuditLogEventTypeEnum
    {
        Added = 0,
        Deleted,
        Modified,
        LogIn
    }

    public enum ExceptionStatusEnum
    {
        New = 201,
        Active,
        Hold,
        Resolved,
        Resubmitted
    }

    public enum Conditions
    {
        State = 701,
        county = 702
    }


    public enum ExceptionGroupEnum
    {
        BEQ = 1,
        TEQ
    }

    public enum ServiceEnum
    {
        CalculatorRequestInitial = 1,
        Escrow = 2,
        Title = 3,
        Signing = 4,
        EscrowTitle = 5,
        SubEscrow =6,
        TitleSubEscrow =7,
        EscrowSubEscrow =8,
        TitleReport = 10,
        ClosingDisclosure =11,
        TaxReport =12,
        EscrowDetails =13
    }

    public enum MessageStatusEnum
    {
        Inactive = 101,
        Active = 102,
        Started = 103,
        Completed = 104,
        Waived = 105,
        StartedAndCompleted = 106
    }

    public class Utilities
    {
        public static string ComputeName(string str)
        {
            return RemoveAllSpecialCharacters(str).ToUpper();
        }

        public static string RemoveAllSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9]+", "", RegexOptions.Compiled);
        }
    }

    public class FIPSUtilities
    {
        public static List<StateMappingDTO> GetStatesList()
        {
            List<StateMappingDTO> StatesList = new List<StateMappingDTO>();

            using (DBEntities.FIPSEntities dbContext = new DBEntities.FIPSEntities())
            {
                var distinctstate = dbContext.FIPSCodes.OrderBy(x => x.State).Where(x => x.State != null && x.State != "")
                    .Select(se => new DataContracts.StateMappingDTO
                    {
                        StateFIPS = se.StateFIPS,
                        PreferenceState = se.State,
                        StateCodes = se.StateCode
                    });

                StatesList = distinctstate.DistinctBy(Sm => Sm.StateFIPS).ToList();
            }

            return StatesList;
        }

        public static List<CountyMappingDTO> GetCountyList(string StateFips)
        {
            List<CountyMappingDTO> CountyList = new List<CountyMappingDTO>();

            using (DBEntities.FIPSEntities dbContext = new DBEntities.FIPSEntities())
            {
                CountyList.Add(new CountyMappingDTO { county = "ALL", countyFIPS = "0" });

                var distinctstate = dbContext.FIPSCodes.Where(se => se.StateFIPS == StateFips && se.CountyFIPS != null).OrderBy(x => x.County)
                    .Select(se => new DataContracts.CountyMappingDTO
                    {
                        countyFIPS = se.CountyFIPS,
                        county = se.County
                    });
                CountyList.AddRange(distinctstate.DistinctBy(Sm => Sm.countyFIPS).OrderBy(sl => sl.county).ToList());
            }

            return CountyList;
        }

        
}
    public class FastServiceEnum
    {
        public const string TITLE_SERVICE_OBJ_CD = "TO";
        public const string ESCROW_SERVICE_OBJ_CD = "EO";
        public const string SUBESCROW_SERVICE_OBJ_CD = "SEO";
    }
    public class RandomDigitGenerator
    {
        private string CharacterSet { get
            {
                return "0123456789";
            }
        } 
        public string Generate(int lenght)
        {
            StringBuilder sb = new StringBuilder();            
            sb.Append(string.Join("", Enumerable.Range(0, lenght).Select(x => CharacterSet[this.Random((uint)CharacterSet.Length)]).ToList()));
            return sb.ToString();
        }
       
        private int Random(uint i)
        {
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            byte[] bytes = new byte[64];
            rng.GetBytes(bytes);
            ulong result = BitConverter.ToUInt64(bytes, 1);
            int intetger = (int)(result % i);
            return intetger;
        }        
    }
        public class PwdGenerator
    {
        public static string GetLVISHash(string plainText)
        {
            byte[] plainTextBytes = System.Text.Encoding.Default.GetBytes(plainText);
            byte[] salt = System.Text.Encoding.Default.GetBytes("665f4d7b-7ed1-48e0-b477-ec579ce8921b");
            
            return Convert.ToBase64String(new SHA256Managed().ComputeHash(plainTextBytes.Concat(salt).ToArray()));
        }
        public string Generate(GeneratorConfig generatorConfig)
        {
            string guid = Guid.NewGuid().ToString().Replace("-", "");
            StringBuilder sb = new StringBuilder(string.Join("", Enumerable.Range(0, 2).Select(x => guid[this.Random((uint)guid.Length)]).ToList()));
            foreach (KeyValuePair<string, CharListAndLength> kv in generatorConfig.CharDict)
            {
                sb.Append(string.Join("", Enumerable.Range(0, kv.Value.length).Select(x => kv.Value.charaters[this.Random((uint)kv.Value.charaters.Length)]).ToList()));
            }
            return this.Shuffle(sb.ToString());
        }
        private string Shuffle(string str)
        {
            char[] array = str.ToCharArray();
            uint n = (uint)array.Length;
            while (n > 1)
            {
                n--;
                int k = this.Random(n + 1);
                var value = array[k];
                array[k] = array[n];
                array[n] = value;
            }
            return new string(array);
        }
        private int Random(uint i)
        {
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            byte[] bytes = new byte[64];
            rng.GetBytes(bytes);
            ulong result = BitConverter.ToUInt64(bytes, 1);
            int intetger = (int)(result % i);
            return intetger;
        }
        public class CharListAndLength
        {
            public string charaters { get; set; }
            public int length { get; set; }

            public CharListAndLength(string charaters, int length)
            {
                this.charaters = charaters;
                this.length = length;
            }
        }
        public class GeneratorConfig
        {
            // minimum password strenght configuration
            const int MINNUMERAL = 1;
            const int MINALPHALOWER = 2;
            const int MINALPHACAPS = 2;
            const int MINSPECHARS = 1;

            const string NUMERALKEY = "numeral";
            const string ALPHALOWERKEY = "alphaLower";
            const string ALPHACAPSKEY = "alphaCaps";
            const string SPESYBKEY = "speSyb";

            public readonly Dictionary<string, CharListAndLength> CharDict = new Dictionary<string, CharListAndLength>() {
                { NUMERALKEY, new CharListAndLength("0123456789",MINNUMERAL) },
                { ALPHALOWERKEY , new CharListAndLength("abcdefghijklmnopqrstuvwxyz",MINALPHALOWER) },
                { ALPHACAPSKEY, new CharListAndLength("ABCDEFGHIJKLMNOPQRSTUVWXYZ",MINALPHACAPS) },
                { SPESYBKEY, new CharListAndLength("!#$%&=*+_",MINSPECHARS) } //-\'(),./:;<>?@[\\]^`{|}~", )
                };
            public int Numeral
            {
                get
                {
                    return CharDict[NUMERALKEY].length;
                }
                set
                {
                    if (value > MINNUMERAL)
                    {
                        CharDict[NUMERALKEY].length = value;
                    }
                }
            }
            public int AlphaLower
            {
                get
                {
                    return CharDict[ALPHALOWERKEY].length;
                }
                set
                {
                    if (value > MINALPHALOWER)
                    {
                        CharDict[ALPHALOWERKEY].length = value;
                    }
                }
            }
            public int AlphaCaps
            {
                get
                {
                    return CharDict[ALPHACAPSKEY].length;
                }
                set
                {
                    if (value > MINALPHACAPS)
                    {
                        CharDict[ALPHACAPSKEY].length = value;
                    }
                }
            }
            public int SpeSyb
            {
                get
                {
                    return CharDict[SPESYBKEY].length;
                }
                set
                {
                    if (value > MINSPECHARS)
                    {
                        CharDict[SPESYBKEY].length = value;
                    }
                }
            }
        }
    }

}
