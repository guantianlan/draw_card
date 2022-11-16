using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace draw_crad
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            read();
            num1.Text = draw_card_data[0].num.ToString();
            num2.Text = draw_card_data[1].num.ToString();
            num3.Text = draw_card_data[2].num.ToString();
            num4.Text = draw_card_data[3].num.ToString();
            num5.Text = draw_card_data[4].num.ToString();
        }

        List<data> draw_card_data =new List<data>();
        List<string> studens = new List<string>();
        List<whitelist> whitelist_data = new List<whitelist>();
        public int prize_level;

        public bool select_id()
        {
            int a=0;
            for(int i=0;i<studens.Count;i++)
            {
                if (text_num.Text == studens[i])a= 1;
            }
            if (a==0)
            {
                return false;
            }
            else return true;
        }

        public void read()
        {
            try
            {
                StreamReader csv1 = new StreamReader("./UUID/draw_card.csv");
                StreamReader csv2 = new StreamReader("./UUID/studen.csv");
                StreamReader csv3 = new StreamReader("./UUID/whitelist.csv");
                string s;
                string[] s1;

                while ((s = csv1.ReadLine()) != null)
                {
                    
                    s1 = s.Split(new char[] { ','});
                    data draw_card1 = new data();

                    draw_card1.type = s1[0];
                    draw_card1.num = Convert.ToInt32(s1[1]);

                    draw_card_data.Add(draw_card1);

                }
                csv1.Close();

                while ((s = csv2.ReadLine()) != null)
                {                  

                    studens.Add(s);

                }
                csv2.Close();

                while ((s = csv3.ReadLine()) != null)
                {
                    s1 = s.Split(new char[] { ',' });
                    whitelist whitelist1 = new whitelist();

                    whitelist1.id = s1[0];
                    whitelist1.type = s1[1];

                    whitelist_data.Add(whitelist1);

                }
                csv3.Close();

            }
            catch
            {
                MessageBox.Show("出现了错误，请检查文件路径是否正确");
            }

        }//读取奖品的uuid


        void Updata_pool(int prize_leavel)
        {
            if(prize_leavel == 6)
            {
                bool a= false;
            }
            else
            {

                draw_card_data[prize_leavel].num--;
                studens.Add(text_num.Text);

                using (StreamWriter streamWriter = new StreamWriter("./UUID/draw_card.csv"))
                {
                    for (int i = 1; i < draw_card_data.Count; i++)
                    {
                        StringBuilder stringBuilder = new StringBuilder(draw_card_data[i].type);
                        stringBuilder.Append("," + draw_card_data[i].num);
                        streamWriter.WriteLine(stringBuilder.ToString());
                    }
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                using (StreamWriter streamWriter = new StreamWriter("./UUID/studen.csv"))
                {
                    for (int i = 0; i < studens.Count; i++)
                    {
                        StringBuilder stringBuilder = new StringBuilder(studens[i]);
                        streamWriter.WriteLine(stringBuilder.ToString());
                    }
                    streamWriter.Flush();
                    streamWriter.Close();
                }
            }
            
            
        }//更新奖品池


        void Prize_select()//抽奖
        {
            bool prize_num_nohave = true;
            
            Random random = new Random();
            int n ;
            int m=0;
            while (prize_num_nohave)
            {
                if(Select_way1.IsChecked==true)
                {
                    n = random.Next(0, 374);

                    if (n == 1) prize_level = 0;
                    else if (n == 2) prize_level = 1;
                    else if (n > 2 && n < 5) prize_level = 2;
                    else if (n >= 5 && n < 41) prize_level = 3;
                    else if (n >= 41 && n < 91) prize_level = 4;
                    else if (n >= 91 && n < 298) prize_level = 5;
                    else prize_level = 6;
                    if (prize_level == 6)
                    {
                        prize_num_nohave = false;
                    }
                    else if (draw_card_data[prize_level].num != 0)
                    {
                        prize_num_nohave = false;
                    }
                    else if (Convert.ToInt32(DateTime.Now.Hour.ToString()) <= 13 && draw_card_data[2].num == 1)
                    {
                        prize_num_nohave = false;
                    }
                }
                else if(Select_way2.IsChecked == true)
                {
                    n = random.Next(0, 374);

                    if (n >= 1 && n < 5) prize_level = 1;
                    else if (n == 2) prize_level = 2;
                    else if (n > 2 && n < 5) prize_level = 3;
                    else if (n >= 41 && n < 91) prize_level = 4;
                    else if (n >= 91 && n < 298) prize_level = 5;
                    else prize_level = 6;
                    if (prize_level == 6)
                    {
                        prize_num_nohave = false;
                    }
                    else if (draw_card_data[prize_level].num != 0)
                    {
                        prize_num_nohave = false;
                    }
                    else if(Convert.ToInt32(DateTime.Now.Hour.ToString())<=13 && draw_card_data[2].num==1)
                    {
                        prize_num_nohave = false;
                    }
                      
                }
                if (m == 0)
                {
                    for (int i = 0; i < whitelist_data.Count; i++)
                    {
                        if (text_num.Text == whitelist_data[i].id)
                        {
                            prize_level = Convert.ToInt32(whitelist_data[i].type);
                            if (draw_card_data[prize_level].num != 0)
                            {
                                prize_num_nohave = false;
                            }
                        }
                        
                        m = 1;
                    }
                }

            }
            if (prize_level == 0) text1.Text = "特等奖";
            else if (prize_level == 1) text1.Text = "一等奖";
            else if (prize_level == 2) text1.Text = "二等奖——斯卡蒂的红色虎鲸";
            else if (prize_level == 3) text1.Text = "二等奖——嘟嘟猫";
            else if (prize_level == 4) text1.Text = "三等奖";
            else if (prize_level == 5) text1.Text = "安慰奖";
            else text1.Text = "谢谢参与";
            Updata_pool(prize_level);

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(text_num.Text=="")
            {
                MessageBox.Show("请输入学号");
            }
            else
            {
                if(select_id())
                {
                    MessageBox.Show("您已经抽过了");
                }
                else
                {
                    Prize_select();
                    num1.Text = draw_card_data[0].num.ToString();
                    num2.Text = draw_card_data[1].num.ToString();
                    num3.Text = draw_card_data[2].num.ToString();
                    num4.Text = draw_card_data[3].num.ToString();
                    num5.Text = draw_card_data[4].num.ToString();
                }
            }
        }
    }
}
   