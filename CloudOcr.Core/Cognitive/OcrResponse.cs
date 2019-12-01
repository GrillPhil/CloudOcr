using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CloudOcr.Core.Cognitive
{
    public class OCRResponse
    {
        public string Language { get; set; }
        public string TextAngle { get; set; }
        public string Orientation { get; set; }
        public List<Region> Regions { get; set; }

        public override string ToString()
        {
            return String.Join(" ", Regions.SelectMany(r => r.Lines).SelectMany(l => l.Words).Select(w => w.Text).ToArray());
        }
    }

    public class Word
    {
        public string BoundingBox { get; set; }
        public string Text { get; set; }
    }

    public class Line
    {
        public string BoundingBox { get; set; }
        public List<Word> Words { get; set; }
    }

    public class Region
    {
        public string BoundingBox { get; set; }
        public List<Line> Lines { get; set; }
    }
}
