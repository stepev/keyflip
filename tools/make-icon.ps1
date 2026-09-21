# Генерирует assets\app.ico (16/32/48/64/256) — иконка приложения: буквы A и Я.
# Отрисовка и сборка ICO сделаны на C#, чтобы не мучиться с массивами байтов в PowerShell.
$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.Drawing

Add-Type -ReferencedAssemblies System.Drawing -TypeDefinition @'
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;

public static class IconMaker
{
    public static Bitmap Render(int size)
    {
        Bitmap bmp = new Bitmap(size, size, PixelFormat.Format32bppArgb);
        using (Graphics g = Graphics.FromImage(bmp))
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            g.Clear(Color.Transparent);

            int pad = Math.Max(1, (int)(size * 0.04));
            float r = Math.Max(2f, size * 0.22f);
            RectangleF rect = new RectangleF(pad, pad, size - 2 * pad, size - 2 * pad);
            using (GraphicsPath path = new GraphicsPath())
            {
                float d = r * 2;
                path.AddArc(rect.X, rect.Y, d, d, 180, 90);
                path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
                path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
                path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
                path.CloseFigure();
                using (LinearGradientBrush b = new LinearGradientBrush(rect,
                           Color.FromArgb(255, 38, 92, 158), Color.FromArgb(255, 18, 44, 90), 90f))
                    g.FillPath(b, path);
            }

            using (Font f = new Font("Segoe UI", size * 0.5f, FontStyle.Bold, GraphicsUnit.Pixel))
            using (SolidBrush white = new SolidBrush(Color.White))
            using (SolidBrush yellow = new SolidBrush(Color.FromArgb(255, 255, 208, 74)))
            {
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                g.DrawString("A", f, white, new RectangleF(0, size * 0.02f, size * 0.56f, size * 0.56f), sf);
                g.DrawString("Я", f, yellow, new RectangleF(size * 0.44f, size * 0.42f, size * 0.56f, size * 0.56f), sf);
            }
        }
        return bmp;
    }

    private static byte[] ToDib(Bitmap bmp)
    {
        int w = bmp.Width, h = bmp.Height;
        int maskStride = ((w + 31) / 32) * 4;
        using (MemoryStream ms = new MemoryStream())
        using (BinaryWriter bw = new BinaryWriter(ms))
        {
            bw.Write(40); bw.Write(w); bw.Write(h * 2);
            bw.Write((ushort)1); bw.Write((ushort)32); bw.Write(0);
            bw.Write(w * h * 4 + maskStride * h);
            bw.Write(0); bw.Write(0); bw.Write(0); bw.Write(0);
            for (int y = h - 1; y >= 0; y--)
                for (int x = 0; x < w; x++)
                {
                    Color c = bmp.GetPixel(x, y);
                    bw.Write(c.B); bw.Write(c.G); bw.Write(c.R); bw.Write(c.A);
                }
            bw.Write(new byte[maskStride * h]);
            bw.Flush();
            return ms.ToArray();
        }
    }

    private static byte[] ToPng(Bitmap bmp)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            bmp.Save(ms, ImageFormat.Png);
            return ms.ToArray();
        }
    }

    public static void Write(string path, int[] sizes)
    {
        byte[][] data = new byte[sizes.Length][];
        for (int i = 0; i < sizes.Length; i++)
            using (Bitmap bmp = Render(sizes[i]))
                data[i] = sizes[i] >= 256 ? ToPng(bmp) : ToDib(bmp);

        using (FileStream fs = File.Create(path))
        using (BinaryWriter bw = new BinaryWriter(fs))
        {
            bw.Write((ushort)0); bw.Write((ushort)1); bw.Write((ushort)sizes.Length);
            int offset = 6 + 16 * sizes.Length;
            for (int i = 0; i < sizes.Length; i++)
            {
                byte dim = sizes[i] >= 256 ? (byte)0 : (byte)sizes[i];
                bw.Write(dim); bw.Write(dim);
                bw.Write((byte)0); bw.Write((byte)0);
                bw.Write((ushort)1); bw.Write((ushort)32);
                bw.Write(data[i].Length);
                bw.Write(offset);
                offset += data[i].Length;
            }
            for (int i = 0; i < sizes.Length; i++) bw.Write(data[i]);
        }
    }
}
'@

$root = Split-Path -Parent (Split-Path -Parent $MyInvocation.MyCommand.Path)
$assets = Join-Path $root 'assets'
if (-not (Test-Path $assets)) { New-Item -ItemType Directory -Path $assets | Out-Null }
$icoPath = Join-Path $assets 'app.ico'

[IconMaker]::Write($icoPath, [int[]]@(16, 32, 48, 64, 256))
Write-Host ("Icon: {0} ({1} bytes)" -f $icoPath, (Get-Item $icoPath).Length)
