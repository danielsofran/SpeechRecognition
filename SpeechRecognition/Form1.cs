using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Windows.Media.SpeechRecognition;

namespace SpeechRecognition
{
    public partial class Form1 : Form
    {
        bool stop = false;
        Task task;
        CancellationTokenSource tokenSource = new CancellationTokenSource();

        public Form1()
        {
            InitializeComponent();
            
            var token = tokenSource.Token;
            task = new Task(async() => await Asculta(token), token);
        }

        private async Task Asculta(CancellationToken ct)
        {
            while (true)
            {
                if (ct.IsCancellationRequested)
                {
                    MessageBox.Show("canceled");
                    break;
                    ct.ThrowIfCancellationRequested();
                }
                try
                {
                    var speech = new Windows.Media.SpeechRecognition.SpeechRecognizer();
                    await speech.CompileConstraintsAsync();

                    SpeechRecognitionResult result = await speech.RecognizeAsync();
                    //MessageBox.Show(result.Text);
                    //textBox1.Text +=  + "\n";
                    textBox1.Invoke((MethodInvoker)delegate { textBox1.Text += result.Text + Environment.NewLine; });
                }
                catch (Exception e) { MessageBox.Show(e.Message); }
            }
        }

        private void button3_Click(object sender, EventArgs ev)
        {
            this.task.Start();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            tokenSource.Cancel();
        }
    }
}
