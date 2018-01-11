using System;
using System.Linq;

namespace ArithmeticCoder
{
    public class MultiArithmeticCoder
    {
        //symbol与probability按位对应
        public string symbol = "";
        public double[] probability;
        //存储每个符号对应的区间
        public double[] symbolInternal;

        /// <summary>
        /// 只输入信源符号，自动设置为等概率
        /// </summary>
        /// <param name="m">信源符号集合</param>
        public MultiArithmeticCoder(string m)
        {
            symbol = m;
            double p = 1.0 / m.Length;
            int len = m.Length;
            //实例化概率数组
            probability = new double[len];
            symbolInternal = new double[len + 1];
            symbolInternal[0] = 0;
            for (int i = 0; i < m.Length; i++)
            {
                probability[i] = p;
                symbolInternal[i + 1] = symbolInternal[i] + p;
            }
            symbolInternal[len] = 1;
        }

        /// <summary>
        /// 输入信源符号和对应概率
        /// </summary>
        /// <param name="m">信源符号集合</param>
        /// <param name="p">每个信源符号对应的概率</param>
        public MultiArithmeticCoder(string m,double[] p)
        {
            symbol = m;
            int len = m.Length;
            probability = new double[len];
            symbolInternal = new double[len + 1];
            symbolInternal[0] = 0;
            for (int i = 0; i < m.Length; i++)
            {
                probability[i] = p[i];
                symbolInternal[i + 1] = symbolInternal[i] + p[i];
            }
            symbolInternal[len] = 1;
        }

        /// <summary>
        /// 获得某个信源符号的概率区间端点
        /// </summary>
        /// <param name="c">信源符号</param>
        /// <param name="left">true表示left，false表示right</param>
        /// <returns></returns>
        public double GetLeft(char c,bool left)
        {
            if (symbol.Contains(c))
            {
                int i = symbol.IndexOf(c);
                return left ? symbolInternal[i] : symbolInternal[i + 1];
            }
            return -1;
        }

        /// <summary>
        /// 计算输入的信源序列的概率（取区间中点）
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public double Encode(string m)
        {
            //left、rigth、len依次为区间左端点、右端点、区间长度
            double left = 0.0, right = 1.0, len = 1.0;
            foreach(var c in m)
            {
                right = left + len * GetLeft(c,false);
                left = left + len * GetLeft(c,true);
                len = right - left;
            }
            //return (left + right) / 2;
            return left;
        }

        /// <summary>
        /// 返回二进制字符串转换成的十进制小数
        /// </summary>
        /// <param name="code">要转换的二进制小数的小数部分</param>
        /// <returns>十进制小数</returns>
        public double Bin2Dec(string code)
        {
            double temp = 0.0;
            for (int i = 0; i < code.Length; i++)
            {
                temp += Int16.Parse(code[i].ToString()) * Math.Pow(2, -(i + 1));
            }
            return temp;
        }

        /// <summary>
        /// 计算任意信源的熵
        /// </summary>
        /// <param name="source">信源概率分布</param>
        /// <param name="n">信源为n次扩展</param>
        /// <returns>信源的熵（平均信息量）</returns>
        public double CalEntropy(double[] source, int n)
        {
            double entropy = 0;
            for (int i = 0; i < source.Length; i++)
            {
                entropy += source[i] * (Math.Log(1.0 / source[i]) / Math.Log(2));
            }
            return entropy * n;
        }

        /// <summary>
        /// 计算一个多进制字符串的概率
        /// </summary>
        /// <param name="s">多进制字符串</param>
        /// <returns>概率</returns>
        private double CalStringProbability(string s)
        {
            double p = 1;
            for (int i = 0; i < s.Length; i++)
            {
                p *= probability[symbol.IndexOf(s[i])];
            }
            return p;
        }

        /// <summary>
        /// 返回某个概率对应的信源字符的索引
        /// </summary>
        /// <param name="p">概率</param>
        /// <returns></returns>
        public int GetRankIndex(double p)
        {
            double temp = 0;
            int i = 0;
            for (; i < symbol.Length; i++)
            {
                if (p >= temp)
                    temp += probability[i];
                else
                    return i - 1;
            }
            return i - 1;
        }

        public string Decode(string code, int n)
        {
            //符号概率
            double p = Bin2Dec(code);
            double low = 0, range = 1;
            string m = "";
            for (int i = 0; i < n; i++)
            {
                int index = 0;
                //获取概率所在区间的索引
                index = GetRankIndex((p - low) / range);
                //拼接输出字符串
                m += symbol[index];
                //当前区间左端点值
                low += range * symbolInternal[index];
                //当前区间长度
                range *= probability[index];
            }
            return m;
        }
    }
}
