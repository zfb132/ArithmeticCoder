using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArithmeticCoder
{
    class ArithmeticCoder
    {
        //0的概率
        public const float p = 0.25f;
        public const double q = 1 - p;
        public static void Main(string[] args)
        {
            double probability = 1;

            string s = "11111100";
            int length = 0;
            probability = CalProbability(s);
            length = (int)Math.Ceiling(Math.Log(1.0 / probability) / Math.Log(2));
            double m = Code(s);
            ulong code = convert(m, length);
            Console.WriteLine("P(s)={0},length={1},CodeDeciaml={2},Code={3}", probability, length, m, code);
        }

        /// <summary>
        /// 进制转换,码的位数过多则无法转换
        /// </summary>
        /// <param name="m">将十进制浮点数转化为二进制数字</param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static ulong convert(double m, int len)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < len; i++)
            {
                sb.Append(Math.Floor(m * 2));
                m = m * 2 >= 1 ? m * 2 - 1 : m * 2;
            }
            //ulong temp=Convert.ToUInt64(sb.ToString(), 2)+1;
            //Convert.ToString(1111111111,2);
            //问题是如何判断是否要进行+1操作
            //二进制+1操作：从右向左找到string中第一个0（位置为i），将其置为1，将（i，end）置为0
            string s = sb.ToString();
            if (!m.Equals(0))
                s = binaryInc(s);
            return UInt64.Parse(s);
        }

        public static string binaryInc(string s)
        {
            char[] m = s.ToArray();
            for (int i = m.Length - 1; i >= 0; i--)
            {
                if (m[i].Equals('0'))
                {
                    m[i] = '1';
                    for (int t = i + 1; t < m.Length; t++)
                        m[t] = '0';
                    return new string(m);
                }
            }
            for (int t = 0; t < m.Length; t++)
                m[t] = '0';
            return new string(m);
        }

        public static double Code(string s)
        {
            double probability = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i].Equals('1'))
                    probability += CalProbability(s.Substring(0, i) + (Int16.Parse(s[i].ToString()) ^ 1).ToString());
            }
            return probability;
        }



        public static double CalProbability(string s)
        {
            double probability = 1;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i].Equals('1'))
                    probability *= q;
                else
                    probability *= p;
            }
            return probability;
        }
    }
}
