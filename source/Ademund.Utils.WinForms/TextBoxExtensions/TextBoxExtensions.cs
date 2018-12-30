using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Ademund.Utils.TextBoxExtensions
{
    public static class TextBoxExtensions
	{
		public static void Flash(this TextBox textBox, int interval, Color color, int flashes)
		{
			new Thread(() => FlashInternal(textBox, interval, color, flashes)).Start();
		}

		private static void FlashInternal(TextBox textBox, int interval, Color flashColor, int flashes)
		{
			Color original = textBox.BackColor;
			for (int i = 0; i < flashes; i++)
			{
				UpdateTextbox(textBox, flashColor);
				Thread.Sleep(interval / 2);
				UpdateTextbox(textBox, original);
				Thread.Sleep(interval / 2);
			}
		}

		private delegate void UpdateTextboxDelegate(TextBox textBox, Color originalColor);

		public static void UpdateTextbox(this TextBox textBox, Color color)
		{
			if (textBox.InvokeRequired)
			{
				textBox.Invoke(new UpdateTextboxDelegate(UpdateTextbox), new object[] { textBox, color });
			}
			textBox.BackColor = color;
		}
	}
}