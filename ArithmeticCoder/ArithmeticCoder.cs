using System;
using System.Linq;
using System.Text;

namespace ArithmeticCoder
{
    class ArithmeticCoder
    {
        //0的概率
        public const float p = 0.25f;
        public const double q = 1 - p;
        public static void Main(string[] args)
        {
            string s = "11111100";
            int length = 0;
            double probability = 0, fs = 0;
            //计算信源的概率P(s=11111100)
            probability = CalProbability(s);
            //计算编码后的码长
            length = (int)Math.Ceiling(Math.Log(1.0 / probability) / Math.Log(2));
            //计算F(s)的概率
            fs = Encode(s);
            string code = Convert(fs, length);
            Console.WriteLine("s={0}", s);
            Console.WriteLine("P(s)={0},length={1}", probability, length);
            Console.WriteLine("F(s)={0},Code={1}", fs, code);

            //计算编码效率
            double hs = 0, averagelength = 0, efficiency = 0;
            hs = CalEntropy(new double[] { p, q });
            averagelength = AverageLength(s.Length, p, q);
            //扩展信源
            efficiency = hs * s.Length / averagelength;
            Console.WriteLine("H(s)={0},averageLength={1}", hs, averagelength);
            //ToString("P3") 转换为百分数形式，保留3位小数
            //ToString("0.000%") 转换为百分数形式，保留3位小数
            Console.WriteLine("n={0}", efficiency.ToString("P3"));
        }

        /// <summary>
        /// 进制转换，将十进制浮点数转换为二进制数
        /// </summary>
        /// <param name="m">待转换的十进制浮点数</param>
        /// <param name="len">要转换为二进制的位数</param>
        /// <returns>二进制字符串</returns>
        public static string Convert(double m, int len)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < len; i++)
            {
                sb.Append(Math.Floor(m * 2));
                m = m * 2 >= 1 ? m * 2 - 1 : m * 2;
            }
            string s = sb.ToString();
            if (!m.Equals(0))
                s = binaryInc(s);
            return s;
        }

        /// <summary>
        /// 实现二进制的加1操作
        /// </summary>
        /// <param name="s">要进行操作的字符串</param>
        /// <returns></returns>
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

        /// <summary>
        /// 将原码转换成F(s)的概率（十进制）
        /// </summary>
        /// <param name="s">原码</param>
        /// <returns></returns>
        public static double Encode(string s)
        {
            double probability = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i].Equals('1'))
                    probability += CalProbability(s.Substring(0, i) + (Int16.Parse(s[i].ToString()) ^ 1).ToString());
            }
            return probability;
        }


        /// <summary>
        /// 计算一个二进制字符串的概率
        /// </summary>
        /// <param name="s">二进制字符串</param>
        /// <returns>概率</returns>
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

        /// <summary>
        /// 计算任意信源的熵
        /// </summary>
        /// <param name="source">信源概率分布</param>
        /// <returns>信源的熵（平均信息量）</returns>
        public static double CalEntropy(double[] source)
        {
            double entropy = 0;
            for (int i = 0; i < source.Length; i++)
            {
                entropy += source[i] * (Math.Log(1.0 / source[i]) / Math.Log(2));
            }
            return entropy;
        }

        /// <summary>
        /// 计算平均码长
        /// <para>N=1时   1 1</para>
        /// <para>N=2时  1 2 1</para>
        /// <para>N=3时 1 3 3 1</para>
        /// <para>e.g.当信源长度n=3时，有四种情况：全0，全1，2X0+1,2X1+0</para>
        /// <para>分别计算出这四种情况下的概率返回</para>
        /// </summary>
        /// <param name="len">原码的长度</param>
        /// <param name="p0">发送0的概率</param>
        /// <param name="p1">发送1的概率</param>
        /// <returns></returns>
        public static double AverageLength(int len,double p0,double p1)
        {
            double[] lengths = new double[len + 1];
            double average = 0;
            for (int i = 0; i < len + 1; i++)
            {
                //计算各种组合的概率
                lengths[i] =  Math.Pow(p0, i) * Math.Pow(p1, len - i);
                //计算长度sigma(P(s)*l(s))
                lengths[i] = lengths[i] * Math.Ceiling(Math.Log(1.0 / lengths[i]) / Math.Log(2));
                average += CalGroup(i, len) * lengths[i];
            }
            return average;
        }

        /// <summary>
        /// 计算C(m,n)=n!/(m!*(n-m)!)
        /// <para>从n个元素中取出m个元素的组合数</para>
        /// </summary>
        /// <param name="m">取出元素个数</param>
        /// <param name="n">元素总个数</param>
        /// <returns></returns>
        public static double CalGroup(int m,int n)
        {
            double sa, sb, sc;
            sa = sb = sc = 0.0;
            for (int i = 2; i <= m; i++)
            {
                sa += Math.Log((double)i);
            }
            for (int i = 2; i <= n; i++)
            {
                sb += Math.Log((double)i);
            }
            for (int i = 2; i <= n - m; i++)
            {
                sc += Math.Log((double)i);
            }
            return Math.Exp(sb - sa - sc);
        }
    }
}
