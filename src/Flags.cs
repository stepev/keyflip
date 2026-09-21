using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace KeyFlip
{
    /// <summary>
    /// Рисование упрощённых флагов стран в маленьком размере (иконка трея).
    /// </summary>
    internal static class Flags
    {
        public static bool Draw(Graphics g, RectangleF r, string country)
        {
            if (string.IsNullOrEmpty(country)) return false;
            switch (country.ToUpperInvariant())
            {
                case "RU": H(g, r, C(0xFF, 0xFF, 0xFF), C(0x00, 0x39, 0xA6), C(0xD5, 0x2B, 0x1E)); return true;
                case "UA": H(g, r, C(0x00, 0x57, 0xB7), C(0xFF, 0xD7, 0x00)); return true;
                case "BY": H(g, r, C(0xCE, 0x11, 0x26), C(0x00, 0x9A, 0x44)); return true;
                case "KZ": Solid(g, r, C(0x00, 0xAF, 0xCA)); Circle(g, r, 0.5f, 0.45f, 0.26f, C(0xFF, 0xE4, 0x00)); return true;
                case "UZ": H(g, r, C(0x00, 0x99, 0xB5), C(0xFF, 0xFF, 0xFF), C(0x1E, 0xB5, 0x3A)); return true;
                case "KG": Solid(g, r, C(0xE8, 0x11, 0x2D)); Circle(g, r, 0.5f, 0.5f, 0.26f, C(0xFF, 0xEF, 0x00)); return true;
                case "AM": H(g, r, C(0xD9, 0x0C, 0x12), C(0x00, 0x33, 0xA0), C(0xF2, 0xA8, 0x00)); return true;
                case "AZ": H(g, r, C(0x00, 0x92, 0xBC), C(0xE4, 0x00, 0x2B), C(0x00, 0xAF, 0x66)); return true;
                case "GE": Solid(g, r, C(0xFF, 0xFF, 0xFF)); Cross(g, r, C(0xFF, 0x00, 0x00), 0.22f); return true;
                case "MD": V(g, r, C(0x00, 0x31, 0x9C), C(0xFF, 0xD2, 0x00), C(0xCC, 0x09, 0x2F)); return true;
                case "DE": H(g, r, C(0x00, 0x00, 0x00), C(0xDD, 0x00, 0x00), C(0xFF, 0xCE, 0x00)); return true;
                case "AT": H3(g, r, C(0xED, 0x29, 0x39), C(0xFF, 0xFF, 0xFF), C(0xED, 0x29, 0x39), 0.33f); return true;
                case "FR": V(g, r, C(0x00, 0x23, 0x95), C(0xFF, 0xFF, 0xFF), C(0xED, 0x29, 0x39)); return true;
                case "IT": V(g, r, C(0x00, 0x91, 0x46), C(0xFF, 0xFF, 0xFF), C(0xCE, 0x2B, 0x37)); return true;
                case "IE": V(g, r, C(0x16, 0x9B, 0x62), C(0xFF, 0xFF, 0xFF), C(0xFF, 0x88, 0x3E)); return true;
                case "RO": V(g, r, C(0x00, 0x2B, 0x7F), C(0xFC, 0xD1, 0x16), C(0xCE, 0x11, 0x26)); return true;
                case "BE": V(g, r, C(0x00, 0x00, 0x00), C(0xFA, 0xE0, 0x42), C(0xED, 0x29, 0x39)); return true;
                case "MX": V(g, r, C(0x00, 0x66, 0x47), C(0xFF, 0xFF, 0xFF), C(0xCE, 0x11, 0x26)); return true;
                case "NL": H(g, r, C(0xAE, 0x1C, 0x28), C(0xFF, 0xFF, 0xFF), C(0x21, 0x46, 0x8B)); return true;
                case "LU": H(g, r, C(0xED, 0x29, 0x39), C(0xFF, 0xFF, 0xFF), C(0x00, 0xA1, 0xDE)); return true;
                case "ES": H3(g, r, C(0xAA, 0x15, 0x1B), C(0xF1, 0xBF, 0x00), C(0xAA, 0x15, 0x1B), 0.25f); return true;
                case "PT": TwoV(g, r, C(0x00, 0x64, 0x36), C(0xDA, 0x20, 0x1E), 0.4f); Circle(g, r, 0.4f, 0.5f, 0.17f, C(0xFF, 0xE9, 0x00)); return true;
                case "PL": H(g, r, C(0xFF, 0xFF, 0xFF), C(0xDC, 0x14, 0x3C)); return true;
                case "CZ": Wedge(g, r, C(0xFF, 0xFF, 0xFF), C(0xD7, 0x14, 0x1A), C(0x11, 0x45, 0x7E)); return true;
                case "SK": H(g, r, C(0xFF, 0xFF, 0xFF), C(0x0B, 0x4E, 0xA2), C(0xEE, 0x1C, 0x25)); return true;
                case "SI": H(g, r, C(0xFF, 0xFF, 0xFF), C(0x00, 0x5D, 0xA4), C(0xED, 0x1C, 0x24)); return true;
                case "HR": H(g, r, C(0xFF, 0x00, 0x00), C(0xFF, 0xFF, 0xFF), C(0x17, 0x17, 0x96)); return true;
                case "RS": H(g, r, C(0xC6, 0x36, 0x3C), C(0x0C, 0x40, 0x76), C(0xFF, 0xFF, 0xFF)); return true;
                case "HU": H(g, r, C(0xCE, 0x25, 0x39), C(0xFF, 0xFF, 0xFF), C(0x47, 0x70, 0x50)); return true;
                case "BG": H(g, r, C(0xFF, 0xFF, 0xFF), C(0x00, 0x96, 0x6E), C(0xD6, 0x26, 0x12)); return true;
                case "LT": H(g, r, C(0xFD, 0xB9, 0x13), C(0x00, 0x6A, 0x44), C(0xC1, 0x27, 0x2D)); return true;
                case "LV": H3(g, r, C(0x9E, 0x31, 0x39), C(0xFF, 0xFF, 0xFF), C(0x9E, 0x31, 0x39), 0.4f); return true;
                case "EE": H(g, r, C(0x00, 0x72, 0xCE), C(0x00, 0x00, 0x00), C(0xFF, 0xFF, 0xFF)); return true;
                case "SE": Nordic(g, r, C(0x00, 0x6A, 0xA7), C(0xFE, 0xCC, 0x00)); return true;
                case "FI": Nordic(g, r, C(0xFF, 0xFF, 0xFF), C(0x00, 0x2F, 0x6C)); return true;
                case "NO": Nordic(g, r, C(0xBA, 0x0C, 0x2F), C(0xFF, 0xFF, 0xFF)); return true;
                case "DK": Nordic(g, r, C(0xC8, 0x10, 0x2E), C(0xFF, 0xFF, 0xFF)); return true;
                case "IS": Nordic(g, r, C(0x02, 0x52, 0x9C), C(0xFF, 0xFF, 0xFF)); return true;
                case "CH": Solid(g, r, C(0xD5, 0x2B, 0x1E)); Cross(g, r, C(0xFF, 0xFF, 0xFF), 0.2f, 0.62f); return true;
                case "GR": Greece(g, r); return true;
                case "TR": Solid(g, r, C(0xE3, 0x0A, 0x17)); Circle(g, r, 0.42f, 0.5f, 0.22f, C(0xFF, 0xFF, 0xFF)); Circle(g, r, 0.5f, 0.5f, 0.18f, C(0xE3, 0x0A, 0x17)); return true;
                case "JP": Solid(g, r, C(0xFF, 0xFF, 0xFF)); Circle(g, r, 0.5f, 0.5f, 0.28f, C(0xBC, 0x00, 0x2D)); return true;
                case "CN": Solid(g, r, C(0xEE, 0x1C, 0x25)); Star(g, r, 0.32f, 0.38f, 0.5f, C(0xFF, 0xFF, 0x00)); return true;
                case "VN": Solid(g, r, C(0xDA, 0x25, 0x1D)); Star(g, r, 0.5f, 0.5f, 0.62f, C(0xFF, 0xFF, 0x00)); return true;
                case "KR": Solid(g, r, C(0xFF, 0xFF, 0xFF)); Korea(g, r); return true;
                case "IN": H(g, r, C(0xFF, 0x99, 0x33), C(0xFF, 0xFF, 0xFF), C(0x13, 0x88, 0x08)); Circle(g, r, 0.5f, 0.5f, 0.13f, C(0x00, 0x00, 0x80)); return true;
                case "ID": H(g, r, C(0xFF, 0x00, 0x00), C(0xFF, 0xFF, 0xFF)); return true;
                case "TH": Thai(g, r); return true;
                case "IL": Israel(g, r); return true;
                case "BR": Solid(g, r, C(0x00, 0x9B, 0x3A)); Diamond(g, r, C(0xFE, 0xDF, 0x00)); Circle(g, r, 0.5f, 0.5f, 0.17f, C(0x00, 0x22, 0x76)); return true;
                case "AR": H3(g, r, C(0x74, 0xAC, 0xDF), C(0xFF, 0xFF, 0xFF), C(0x74, 0xAC, 0xDF), 0.33f); return true;
                case "CA": H3V(g, r, C(0xD8, 0x00, 0x21), C(0xFF, 0xFF, 0xFF), C(0xD8, 0x00, 0x21)); Circle(g, r, 0.5f, 0.5f, 0.16f, C(0xD8, 0x00, 0x21)); return true;
                case "US": Usa(g, r); return true;
                case "GB": UnionJack(g, r); return true;
                case "AU":
                case "NZ": Solid(g, r, C(0x00, 0x24, 0x7D)); UnionJack(g, new RectangleF(r.X, r.Y, r.Width * 0.5f, r.Height * 0.5f)); return true;
                case "EG":
                case "SY":
                case "IQ": H(g, r, C(0xCE, 0x11, 0x26), C(0xFF, 0xFF, 0xFF), C(0x00, 0x00, 0x00)); return true;
                case "SA": Solid(g, r, C(0x16, 0x5D, 0x31)); return true;
                default: return false;
            }
        }

        private static Color C(int r, int g, int b) { return Color.FromArgb(255, r, g, b); }

        private static void Solid(Graphics g, RectangleF r, Color c)
        {
            using (var b = new SolidBrush(c)) g.FillRectangle(b, r);
        }

        private static void H(Graphics g, RectangleF r, params Color[] colors)
        {
            float h = r.Height / colors.Length;
            for (int i = 0; i < colors.Length; i++)
            {
                float y = r.Y + i * h;
                float hh = (i == colors.Length - 1) ? r.Bottom - y : h + 0.5f;
                using (var b = new SolidBrush(colors[i])) g.FillRectangle(b, r.X, y, r.Width, hh);
            }
        }

        private static void V(Graphics g, RectangleF r, params Color[] colors)
        {
            float w = r.Width / colors.Length;
            for (int i = 0; i < colors.Length; i++)
            {
                float x = r.X + i * w;
                float ww = (i == colors.Length - 1) ? r.Right - x : w + 0.5f;
                using (var b = new SolidBrush(colors[i])) g.FillRectangle(b, x, r.Y, ww, r.Height);
            }
        }

        /// <summary>Три горизонтальные полосы, где верхняя и нижняя занимают долю edge.</summary>
        private static void H3(Graphics g, RectangleF r, Color top, Color mid, Color bottom, float edge)
        {
            float e = r.Height * edge;
            using (var b = new SolidBrush(mid)) g.FillRectangle(b, r);
            using (var b = new SolidBrush(top)) g.FillRectangle(b, r.X, r.Y, r.Width, e);
            using (var b = new SolidBrush(bottom)) g.FillRectangle(b, r.X, r.Bottom - e, r.Width, e);
        }

        private static void H3V(Graphics g, RectangleF r, Color left, Color mid, Color right)
        {
            using (var b = new SolidBrush(mid)) g.FillRectangle(b, r);
            using (var b = new SolidBrush(left)) g.FillRectangle(b, r.X, r.Y, r.Width * 0.25f, r.Height);
            using (var b = new SolidBrush(right)) g.FillRectangle(b, r.Right - r.Width * 0.25f, r.Y, r.Width * 0.25f, r.Height);
        }

        private static void TwoV(Graphics g, RectangleF r, Color left, Color right, float split)
        {
            using (var b = new SolidBrush(right)) g.FillRectangle(b, r);
            using (var b = new SolidBrush(left)) g.FillRectangle(b, r.X, r.Y, r.Width * split, r.Height);
        }

        private static void Circle(Graphics g, RectangleF r, float cx, float cy, float rad, Color c)
        {
            float d = Math.Min(r.Width, r.Height) * rad * 2f;
            using (var b = new SolidBrush(c))
                g.FillEllipse(b, r.X + r.Width * cx - d / 2f, r.Y + r.Height * cy - d / 2f, d, d);
        }

        private static void Cross(Graphics g, RectangleF r, Color c, float thickness)
        {
            Cross(g, r, c, thickness, 1f);
        }

        private static void Cross(Graphics g, RectangleF r, Color c, float thickness, float length)
        {
            float tw = r.Width * thickness, th = r.Height * thickness;
            using (var b = new SolidBrush(c))
            {
                g.FillRectangle(b, r.X + (r.Width - r.Width * length) / 2f, r.Y + (r.Height - th) / 2f, r.Width * length, th);
                g.FillRectangle(b, r.X + (r.Width - tw) / 2f, r.Y + (r.Height - r.Height * length) / 2f, tw, r.Height * length);
            }
        }

        private static void Nordic(Graphics g, RectangleF r, Color bg, Color cross)
        {
            Solid(g, r, bg);
            float th = r.Height * 0.22f;
            using (var b = new SolidBrush(cross))
            {
                g.FillRectangle(b, r.X, r.Y + (r.Height - th) / 2f, r.Width, th);
                g.FillRectangle(b, r.X + r.Width * 0.3f - th / 2f, r.Y, th, r.Height);
            }
        }

        private static void Wedge(Graphics g, RectangleF r, Color top, Color bottom, Color wedge)
        {
            using (var b = new SolidBrush(top)) g.FillRectangle(b, r.X, r.Y, r.Width, r.Height / 2f + 0.5f);
            using (var b = new SolidBrush(bottom)) g.FillRectangle(b, r.X, r.Y + r.Height / 2f, r.Width, r.Height / 2f);
            using (var b = new SolidBrush(wedge))
                g.FillPolygon(b, new[] { new PointF(r.X, r.Y), new PointF(r.X + r.Width * 0.5f, r.Y + r.Height / 2f), new PointF(r.X, r.Bottom) });
        }

        private static void Greece(Graphics g, RectangleF r)
        {
            Color blue = C(0x00, 0x5B, 0xAE);
            Solid(g, r, Color.White);
            float h = r.Height / 9f;
            using (var b = new SolidBrush(blue))
            {
                for (int i = 0; i < 9; i += 2) g.FillRectangle(b, r.X, r.Y + i * h, r.Width, h + 0.4f);
                g.FillRectangle(b, r.X, r.Y, h * 5, h * 5);
            }
            using (var b = new SolidBrush(Color.White))
            {
                g.FillRectangle(b, r.X, r.Y + h * 2, h * 5, h);
                g.FillRectangle(b, r.X + h * 2, r.Y, h, h * 5);
            }
        }

        private static void Israel(Graphics g, RectangleF r)
        {
            Color blue = C(0x00, 0x38, 0xB8);
            Solid(g, r, Color.White);
            float h = r.Height * 0.16f;
            using (var b = new SolidBrush(blue))
            {
                g.FillRectangle(b, r.X, r.Y + r.Height * 0.08f, r.Width, h);
                g.FillRectangle(b, r.X, r.Bottom - r.Height * 0.08f - h, r.Width, h);
            }
            using (var p = new Pen(blue, Math.Max(1f, r.Width * 0.07f)))
            {
                float cx = r.X + r.Width / 2f, cy = r.Y + r.Height / 2f, s = r.Height * 0.2f;
                g.DrawPolygon(p, new[] { new PointF(cx, cy - s), new PointF(cx + s * 0.87f, cy + s * 0.5f), new PointF(cx - s * 0.87f, cy + s * 0.5f) });
                g.DrawPolygon(p, new[] { new PointF(cx, cy + s), new PointF(cx + s * 0.87f, cy - s * 0.5f), new PointF(cx - s * 0.87f, cy - s * 0.5f) });
            }
        }

        private static void Thai(Graphics g, RectangleF r)
        {
            Solid(g, r, C(0xA5, 0x1E, 0x36));
            float h = r.Height / 6f;
            using (var b = new SolidBrush(Color.White))
            {
                g.FillRectangle(b, r.X, r.Y + h, r.Width, h + 0.4f);
                g.FillRectangle(b, r.X, r.Y + h * 4, r.Width, h);
            }
            using (var b = new SolidBrush(C(0x2D, 0x2A, 0x4A))) g.FillRectangle(b, r.X, r.Y + h * 2, r.Width, h * 2 + 0.4f);
        }

        private static void Korea(Graphics g, RectangleF r)
        {
            float d = Math.Min(r.Width, r.Height) * 0.5f;
            var box = new RectangleF(r.X + r.Width / 2f - d / 2f, r.Y + r.Height / 2f - d / 2f, d, d);
            using (var b = new SolidBrush(C(0xCD, 0x2E, 0x3A))) g.FillPie(b, box.X, box.Y, box.Width, box.Height, 180f, 180f);
            using (var b = new SolidBrush(C(0x00, 0x47, 0xA0))) g.FillPie(b, box.X, box.Y, box.Width, box.Height, 0f, 180f);
        }

        private static void Diamond(Graphics g, RectangleF r, Color c)
        {
            using (var b = new SolidBrush(c))
                g.FillPolygon(b, new[]
                {
                    new PointF(r.X + r.Width / 2f, r.Y + r.Height * 0.1f),
                    new PointF(r.Right - r.Width * 0.1f, r.Y + r.Height / 2f),
                    new PointF(r.X + r.Width / 2f, r.Bottom - r.Height * 0.1f),
                    new PointF(r.X + r.Width * 0.1f, r.Y + r.Height / 2f)
                });
        }

        private static void Star(Graphics g, RectangleF r, float cx, float cy, float size, Color c)
        {
            float R = Math.Min(r.Width, r.Height) * size / 2f;
            float x = r.X + r.Width * cx, y = r.Y + r.Height * cy;
            var pts = new PointF[10];
            for (int i = 0; i < 10; i++)
            {
                double ang = -Math.PI / 2 + i * Math.PI / 5;
                float rad = (i % 2 == 0) ? R : R * 0.42f;
                pts[i] = new PointF(x + (float)(Math.Cos(ang) * rad), y + (float)(Math.Sin(ang) * rad));
            }
            using (var b = new SolidBrush(c)) g.FillPolygon(b, pts);
        }

        private static void Usa(Graphics g, RectangleF r)
        {
            Color red = C(0xB2, 0x22, 0x34), blue = C(0x3C, 0x3B, 0x6E);
            Solid(g, r, Color.White);
            float h = r.Height / 7f;
            using (var b = new SolidBrush(red))
                for (int i = 0; i < 7; i += 2) g.FillRectangle(b, r.X, r.Y + i * h, r.Width, h + 0.4f);
            using (var b = new SolidBrush(blue)) g.FillRectangle(b, r.X, r.Y, r.Width * 0.42f, h * 4);
            using (var b = new SolidBrush(Color.White))
            {
                float dot = Math.Max(1f, r.Width * 0.06f);
                for (int row = 0; row < 3; row++)
                    for (int col = 0; col < 3; col++)
                        g.FillEllipse(b, r.X + r.Width * (0.06f + col * 0.13f), r.Y + h * (0.55f + row * 1.2f), dot, dot);
            }
        }

        private static void UnionJack(Graphics g, RectangleF r)
        {
            Color blue = C(0x01, 0x20, 0x69), red = C(0xC8, 0x10, 0x2E);
            Solid(g, r, blue);
            var old = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (var pw = new Pen(Color.White, Math.Max(1.5f, r.Width * 0.16f)))
            using (var pr = new Pen(red, Math.Max(1f, r.Width * 0.07f)))
            {
                g.DrawLine(pw, r.X, r.Y, r.Right, r.Bottom);
                g.DrawLine(pw, r.Right, r.Y, r.X, r.Bottom);
                g.DrawLine(pr, r.X, r.Y, r.Right, r.Bottom);
                g.DrawLine(pr, r.Right, r.Y, r.X, r.Bottom);
            }
            g.SmoothingMode = SmoothingMode.None;
            Cross(g, r, Color.White, 0.3f);
            Cross(g, r, red, 0.16f);
            g.SmoothingMode = old;
        }
    }
}
