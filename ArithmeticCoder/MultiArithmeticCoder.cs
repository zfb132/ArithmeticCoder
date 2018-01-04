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
    }
}
