using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPinyin;

namespace Piecework_wage_management_system
{
    public class BusinessLogicLayer
    {
        //权限处理
        public bool AuthorityProcessing(byte userAuthority, byte functionAuthority)
        {
            if (userAuthority >= functionAuthority)
                return true;
            else
                return false;
        }

        //拼音简写转换，通过NPinyin开源库实现
        public string PinyinAbbreviationConvert(string originalStr)
        {
            return Pinyin.ConvertEncoding(originalStr, Encoding.GetEncoding("GBK"), Encoding.GetEncoding("GBK"));
        }

    }
}
